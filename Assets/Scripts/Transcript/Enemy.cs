using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    private Transform player;
    private Transform transBloodPoint;
    private CharacterController cc;
    private Animation anim;

    public GameObject damageEffectPrefab;
    public int hp = 200;
    public int damage = 20;
    private float speed = 1;
    private float downSpeed = 1f;
    private float downDistance = 0f;
    private int attackRate = 2;
    private float attackDitance = 2;
    private float distance = 0;
    private float attackTimer = 0;
    private int hpTotal = 0;

    public string guid = "";      //这个是GUID是每一个敌人的唯一标识
    public int targetRoleId = -1; //表示这个敌人要攻击的目标
    private GameObject targetGo;  //表示要攻击的目标的游戏物体
 
    private Transform hpbarPoint;  
    private GameObject hpBarGo;
    private Slider hpBarSlider;
    private GameObject hudTextGo;
    public bl_HUDText hudText;

    //++++
    //用于攻击的参数   
    public float viewAngle = 50;
    public float maxDistance = 10f;
    public float attackDistance = 2;
    private float attack1Timer = 2f;
    public float timer = 0;
    public bool isAttacking = false;

    //用于小怪自动巡逻AI参数
    private float aiTimer = 2f;
    private bool idle = true;

    //同步位置
    private Vector3 lastPosition;
    private Vector3 lastEulerAngles;

    private bool lastIsIdle = true;
    private bool lastIsWalk = false;
    private bool lastIsAttack = false;
    private bool lastIsTakeDamage = false;
    private bool lastIsDie = false;

    private void Awake()
    {        
        
    }

    // Use this for initialization
    void Start ()
    {
        targetGo = GameController.Instance.GetPlayerByRoleID(targetRoleId);
        TranscriptManager.Instance.AddEnemy(this.gameObject);
        hpTotal = hp;
        transBloodPoint = transform.Find("BloodPoint").GetComponent<Transform>();
        cc = this.GetComponent<CharacterController>();
        InvokeRepeating("CalcDistance", 0, 0.1f);
        anim = this.GetComponent<Animation>();  
        hpbarPoint = transform.Find("HpBarPoint").GetComponent<Transform>();
        //if (TranscriptManager.Instance.player != null)
        //{
        //    player = TranscriptManager.Instance.player.transform;
        //}
        hpBarGo =  HpBarManager._instance.GetHpBar(hpbarPoint.gameObject);
        hpBarSlider = hpBarGo.GetComponent<Slider>();
        hudTextGo = HpBarManager._instance.GetHudText(hpbarPoint.gameObject);
        hudText = hudTextGo.GetComponent<bl_HUDText>();

        if (GameController.Instance.battleType == BattleType.Team && GameController.Instance.isMaster)
        {
            InvokeRepeating("CheckPositionAndRotation", 0, 1f / 30);
            InvokeRepeating("CheckAnimation", 0, 1 / 30f);
        }

    }
	
	// Update is called once per frame
	void Update () {
	    if (hp <= 0)
	    {
	        //移到地下
	        downDistance += downSpeed * Time.deltaTime;
	        transform.Translate( -transform.up * downSpeed * Time.deltaTime);
	        if (downDistance > 4)
	        {
	            Destroy(this.gameObject);
	        }
	        return;
	    }

        if (GameController.Instance.battleType==BattleType.Person||(GameController.Instance.battleType==BattleType.Team && GameController.Instance.isMaster))
	    {
            Transform player = targetGo.transform;
            Vector3 targetPos = player.position;
            targetPos.y = transform.position.y;
	        float distance = Vector3.Distance(transform.position, targetPos);  //小怪与玩家之间的距离
	        if (distance < attackDistance) //print("进入小怪攻击范围");  
	        {
                transform.LookAt(targetPos);
                //transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(targetPos - transform.position), speed * Time.deltaTime);
	            timer += Time.deltaTime;
	            if (timer >= attack1Timer)
	            {
                    anim.Play("attack01");    //进行攻击
	                isAttacking = true;
                    timer = 0;
	            }

	        }
            else //当 minDistance > attackDistance，则怪物追击走向攻击目标
            {
	            anim.Play("walk");
                isAttacking = false;
	            Move(); //小怪追击目标;    
	        }


	    }


    }

    
    private void Move()//小怪追击目标;
    {
        Transform player = targetGo.transform;
        Vector3 targetPos = player.position;
        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);
        cc.SimpleMove(transform.forward * speed );
    }

    void EnemyAutoMoveCCAI()
    {             
      
        if (player != null)
        {
            Vector3 playerPos = player.position;
            playerPos.y = transform.position.y;       
            float distance = Vector3.Distance(transform.position, playerPos);  //两怪之间的距离
            if (distance < maxDistance)                               //print("进入小怪攻击范围");  
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerPos - transform.position), speed * Time.deltaTime);
                if (distance <= attackDistance)
                {
                    //进行攻击
                    anim.Play("walk");
                    if (isAttacking == false)
                    {
                        timer += Time.deltaTime;
                        if (timer >= attack1Timer)
                        {
                            timer = 0;
                            Attack();
                        }
                    }
                }
                else //当 minDistance < dis < maxDistance 时，则怪物追击走向攻击目标
                {
                    anim.Play("walk");
                    Move();//小怪追击目标;       
                }
            }
            else                     //大于 maxDistance ，小怪脱离战斗，继续巡逻，不处理
            {
                AutoMoveCCAI();    //调用 小怪自动巡逻           
            }
        }
        else if (player == null)
        {
            AutoMoveCCAI();      //调用 小怪自动巡逻   
        }

    }

    void AutoMoveCCAI()   //小怪自动巡逻Void方法       
    {
        //用于小怪自动巡逻参数      
        float idleTimer = 2f;
        float walkTimer = 5f;
        float angle = 0;
        aiTimer -= Time.deltaTime;
        if (aiTimer <= 0)
        {
            if (idle)
            {
                idle = false;
                aiTimer = walkTimer;
                angle = Random.Range(-3600, 3600);   
                anim.Play("walk");  
            }           
            else
            {
                idle = true;
                aiTimer = idleTimer;
                anim.Play("idle");  
            }
        }
        
        if (!idle)
        {
            if (Mathf.Abs(angle) > 0.2f)
            {
                float temp = angle * 0.05f;
                transform.Rotate(0, temp, 0);
                angle -= temp;
            }
            cc.SimpleMove(transform.forward * speed );
        }
       

    }
   
    void Attack()
    {
        Transform player = targetGo.transform;
        distance = Vector3.Distance(player.position, transform.position);
        if (distance < attackDitance)
        {
            player.SendMessage("TakeDamage",damage,SendMessageOptions.DontRequireReceiver);
        }
    }
    
    //受到攻击调用的方法、、
    //0.收到多少伤害、、
    //1.后退的距离
    //2.浮空的高度
    //3.出血特效
    public void TakeDamage(string args)
    {
        if (hp <= 0)
        {
            Dead();
        }
        else
        {
            string[] proArray = args.Split(',');
            int damage = int.Parse(proArray[0]);
            hp -= damage;
            hudText.NewText("- " + damage.ToString(), hpbarPoint.transform, Color.red, 30, 0.1f, -1f, 1f, bl_Guidance.LeftUp);
            hpBarSlider.value = (float)this.hp / hpTotal;
            anim.Play("takedamage"); //受至攻击的动画
            float backDistance = float.Parse(proArray[1]); //后退距离
            float jumpDistance = float.Parse(proArray[2]); //浮空高度
            this.gameObject.transform.DOBlendableMoveBy(transform.InverseTransformDirection(TranscriptManager.Instance.player.transform.forward) * backDistance + Vector3.up * jumpDistance, 0.3f);
            GameObject.Instantiate(damageEffectPrefab, transBloodPoint.position, Quaternion.identity);
        }
    }   
    
    //当死亡时调用这个方法
    void Dead()
    {
        //1.第一种死亡方式是播放死亡动画
        //2.第二种破碎效果   
        TranscriptManager.Instance.RemoveEnemy(this.gameObject);        
        Destroy(hpBarGo);
        Destroy(hudTextGo);
        Destroy(this.gameObject, 2f);
        
        int random = Random.Range(0, 10);
        if (random < 5)
        {
            anim.Play("die");            
        }
        else
        {            
            this.GetComponentInChildren<MeshExploder>().Explode();
            this.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }         

    }

    void CheckPositionAndRotation()
    {
        Vector3 position = transform.position;
        Vector3 eulerAngles = transform.eulerAngles;
        if (position.x != lastPosition.x || position.y != lastPosition.y || position.z != lastPosition.z ||
            eulerAngles.x != lastEulerAngles.x || eulerAngles.y != lastEulerAngles.y ||
            eulerAngles.z != lastEulerAngles.z)
        {
            TranscriptManager.Instance.AddEnemyToSync(this);
            lastPosition = position;
            lastEulerAngles = eulerAngles;
        }

    }

    void CheckAnimation()
    {
        if (lastIsAttack != anim.IsPlaying("attack01") || lastIsDie != anim.IsPlaying("die") ||
            lastIsIdle != anim.IsPlaying("idle") || lastIsTakeDamage != anim.IsPlaying("takedamage") ||
            lastIsWalk != anim.IsPlaying("walk") )
        {
            TranscriptManager.Instance.AddEnemyToSyncAnimation(this); //把自身传递到管理器里，统一进行管理
            lastIsAttack = anim.IsPlaying("attack01");
            lastIsDie = anim.IsPlaying("die");
            lastIsIdle = anim.IsPlaying("idle");
            lastIsTakeDamage = anim.IsPlaying("takedamage");
            lastIsWalk = anim.IsPlaying("walk");
        }
    }

}
