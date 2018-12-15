using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAttack : MonoBehaviour {
        
    private Dictionary<string,PlayerEffect> effectDict = new Dictionary<string,PlayerEffect>();    
    public PlayerEffect[] effectArray;
    public GameObject attack026EffectPrefab;
    private Animator anim;
    private Player player;
    private BattleController battleController;
    private bool isSyncPlayerAnimation = false;//表示是否需要同步动画

    public float distanceAttackForward = 3;
    public float distanceAttackAround = 3;
    public int[] damageArray = new int[] { 20, 30, 30, 30 };
    public int hp = 1000;

    private Transform hpbarPoint;
    private GameObject hudTextGo;
    public bl_HUDText hudText;

    public enum AttackRange
    {
        Forward,
        Around
    }

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
        hpbarPoint = transform.Find("HpBarPoint").GetComponent<Transform>();
    }

    private void Start()
    {
        player = this.GetComponent<Player>();
        if (GameController.Instance.battleType == BattleType.Team && player.roleId==PhotonEngine.Instance.role.Id) //当前角色是团队战斗，且属于当前客户端
        {
            battleController = GameController.Instance.GetComponent<BattleController>();
            isSyncPlayerAnimation = true;
        }
        hp = PlayerInfo.Intance.HP;
        PlayerEffect[] peArray = this.GetComponentsInChildren<PlayerEffect>();
        foreach(PlayerEffect pe in peArray)
        {
            effectDict.Add(pe.gameObject.name, pe);
        }
        hudTextGo = HpBarManager._instance.GetHudText(hpbarPoint.gameObject);
        hudText = hudTextGo.GetComponent<bl_HUDText>();

    }

    //0 normal skill1 skill2 skill3 
    //1 effect name
    //2 sound nmae
    //3 move forward
    //4 unmp height
    public void Attack(string args)
    {    
        string[] proArray = args.Split(',');   // 1.拆分 接受到的数据 
        string effectName = proArray[1];        //2.得到特效的名字
        ShowPlayerEffect(effectName);             //调用特效,并播放
        string soundName = proArray[2];         //3.得到声音的名字
        SoundManager._instance.Play(soundName);    //播放声音
        float moveForward = float.Parse(proArray[3]);
        if (moveForward > 0.1f)
        { 
            //向前冲1M
            this.gameObject.transform.DOBlendableMoveBy(transform.forward * moveForward, 0.3f);         
        }

        string posType = proArray[0];//攻击类型        
        if (posType == "normal")
        {
            ArrayList array = GetEnemyInAttackRange(AttackRange.Forward);
            foreach (GameObject go in array)
            {
                go.SendMessage("TakeDamage", damageArray[0]+","+proArray[3]+","+proArray[4]);//ToTo 参数一会在写
            }
        }
    }

    void ShowEffectDeviHand(string effectName)
    {        
        //effectName = "attack026";        
        PlayerEffect pe;
        effectDict.TryGetValue(effectName, out pe);

        //print(pe.name);
        
        ArrayList array = GetEnemyInAttackRange(AttackRange.Forward);
        foreach(GameObject go in array)
        {
            GameObject.Instantiate(pe, go.transform.position, Quaternion.identity);
            pe.ShowTrue();
            
        }
    }

    void PlayAttack026Effect()
    {
        GameObject go = null;
        go = GameObject.Instantiate(attack026EffectPrefab, hpbarPoint.position, transform.rotation);
        go.transform.parent = hpbarPoint.transform;      
    }

    public void ShowPlayerEffect(string effectName)
    {
        PlayerEffect pe;
        if(effectDict.TryGetValue(effectName,out pe))
        {
            pe.ShowTrue();
        }        
    }

    //得到在攻击范围之内的敌人
    ArrayList GetEnemyInAttackRange(AttackRange attackRange)
    {
        ArrayList arrayList = new ArrayList();
        //1.得到前方向的
        if (attackRange == AttackRange.Forward)
        {
            foreach(GameObject go in TranscriptManager.Instance.GetEnemyList())
            {
                if (go != null)
                {
                    Vector3 pos = transform.InverseTransformPoint(go.transform.position);
                    if (pos.z > -0.5f)
                    {
                        float distance = Vector3.Distance(Vector3.zero, pos);
                        if (distance < distanceAttackForward)
                        {
                            arrayList.Add(go);
                        }
                    }
                }
                
            }
            
        }
        //2.得到周围的
        else 
        {
            foreach (GameObject go in TranscriptManager.Instance.GetEnemyList())
            {
               
                    float distance = Vector3.Distance(transform.position , go.transform.position);
                    if (distance < distanceAttackAround)
                    {
                        arrayList.Add(go);
                    }               
            }
        }
        return arrayList;
        
    }

    void TakeDamage(int damage)
    {
        if (this.hp <= 0) return;
        this.hp -= damage;
        if (OnPlayerHpChange != null)
        {
             OnPlayerHpChange(hp);
        }
        //1.播放受到攻击的动画
        int random = Random.Range(0, 100);
        if (random < damage)
        {
            anim.SetTrigger("TakeDamage");
            if (isSyncPlayerAnimation)
            {
                PlayerAnimationModel model = new PlayerAnimationModel() {takeDamage = true};
                battleController.SyncPlayerAnimation(model);
            }
        }
        //2.显示血量的减少
        hudText.NewText("- " + damage.ToString(), hpbarPoint.transform, Color.magenta, 30, 0.1f, -1f, 1f, bl_Guidance.LeftUp);

        //3.屏幕上血红特效的显示
        //4.死亡
        if (this.hp <= 0)
        {
            anim.SetTrigger("Die");
            if (isSyncPlayerAnimation)
            {
                PlayerAnimationModel model = new PlayerAnimationModel() { die = true };
                battleController.SyncPlayerAnimation(model);
            }
            hp = 0;
            GameController.Instance.OnPlayerDie(GetComponent<Player>().roleId);
        }
    }

    public event OnPlayerHpChangeEvent OnPlayerHpChange;

}
