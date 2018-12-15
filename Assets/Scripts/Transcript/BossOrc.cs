using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

public class BossOrc : MonoBehaviour {

    private Transform player;
    private Animator anim;
    private Rigidbody rigidbody;
   


    //用于移动的参数
    public float moveSpeed = 2;
    public float rotateSpeed = 1;
    public float[] attackArray;

    //用于攻击的参数      
    public int hp = 2000;
    public int damage = 20;
    public float viewAngle = 50;
    public float maxDistance = 10f;
    public float attackDistance = 3;
    private  float attackTimer = 2f;
    public float timer = 0;
    public bool isAttacking = false;   
    private GameObject attack01Go;
    private GameObject attack02Go;
    private Transform attack03Point;
    public  GameObject attack03EffectPrefab;
    private Renderer renderer;
   
    //++++
    private int hpTotal = 0;
    private Transform transBloodPoint;   
    public GameObject damageEffectPrefab;
    public string guid;
    public int targetRoleId = -1; //表示这个敌人要攻击的目标
    private GameObject targetGo;  //表示要攻击的目标的游戏物体

    //同步位置
    private Vector3 lastPosition = Vector3.zero;
    private Vector3 lastEulerAngles = Vector3.zero;
    private bool isSyncBossAnimation = false;//表示是否需要同步动画
    private bool isWalk = false;
    private BossController bossController;

    private bool lastTakeDamage = false;
    private bool lastWalk = false;
    private bool lastDie = false;
    private bool lastAttack01 = false;
    private bool lastAttack02 = false;
    private bool lastAttack03 = false;
    private bool lastStand = false;


    private float speed = 1;
    private float downSpeed = 1f;
    private float downDistance = 0f;
    private int attackRate = 2;
    private float attackDitance = 2;
    private float distance = 0;
    private float attack1Timer = 0;

    private Transform hpbarPoint;
    //private GameObject hpBarGo;
    //private Slider hpBarSlider;
    private GameObject hudTextGo;
    public bl_HUDText hudText;


    //用于小怪自动巡逻AI参数
    float aiTimer = 2f;    
    bool idle = true;
       


    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {
        targetGo = GameController.Instance.GetPlayerByRoleID(targetRoleId);
        player = targetGo.transform;
        TranscriptManager.Instance.AddEnemy(this.gameObject);
        hpTotal = hp;
        anim = this.GetComponent<Animator>();     
        rigidbody = this.GetComponent<Rigidbody>();
        renderer = transform.Find("Object01").GetComponent<Renderer>();
        attack01Go = transform.Find("attack01").gameObject;
        attack02Go = transform.Find("attack02").gameObject;
        attack03Point = transform.Find("attack03Point").GetComponent<Transform>();
        hpbarPoint = transform.Find("HpBarPoint").GetComponent<Transform>();
        transBloodPoint = transform.Find("BloodPoint").GetComponent<Transform>();
        hudTextGo = HpBarManager._instance.GetHudText(hpbarPoint.gameObject);
        hudText = hudTextGo.GetComponent<bl_HUDText>();
        BossHpBar.Instance.Show(hp);

        bossController = GetComponent<BossController>();
        bossController.OnSyncBossAnimation += this.OnSyncBossAnimation;
        if (GameController.Instance.battleType == BattleType.Team && GameController.Instance.isMaster)
        {
            InvokeRepeating("CheckPositionAndRotation", 0, 1 / 30f);
            InvokeRepeating("CheckAnimation", 0, 1f / 30);
            
        }
        

    }
	
	// Update is called once per frame
	void Update ()
	{
	    //renderer.material.color = Color.Lerp(renderer.material.color, Color.white, Time.deltaTime);
	    if (GameController.Instance.battleType == BattleType.Person ||
	        (GameController.Instance.battleType == BattleType.Team && GameController.Instance.isMaster))
	    {
            //在攻击视野之内
	        Transform player = targetGo.transform;
	        Vector3 targetPos = player.position;
	        targetPos.y = transform.position.y;
            transform.LookAt(targetPos);
            float distance = Vector3.Distance(player.position, transform.position);
	        if (distance < attackDistance)
	        {
	            //进行攻击
	            anim.SetBool("walk", false);

	            if (isAttacking == false)
	            {
	                timer += Time.deltaTime;
	                if (timer > attackTimer)
	                {
	                    timer = 0;
	                    Attack();
	                }
	            }
	        }
	        else
	        {
	            //进行追击
	            anim.SetBool("walk", true);
	            rigidbody.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
	        }
        }
	}

    private int attackIndex = 0;
    void Attack()
    {
        isAttacking = true;
        attackIndex++;
        if (attackIndex == 4) attackIndex = 1;
        anim.SetTrigger("attack0"+attackIndex); 
    }

    void SetIsAttackToStand()
    {
        isAttacking = false;
    }

    void PlayAttack01Effect()
    {
        attack01Go.SetActive(true);
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < attackDistance)
        {
            player.SendMessage("TakeDamage", attackArray[0],SendMessageOptions.DontRequireReceiver);
        }
    }

    void PlayAttack02Effect()
    {
        attack02Go.SetActive(true);
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < attackDistance)
        {
            player.SendMessage("TakeDamage", attackArray[1], SendMessageOptions.DontRequireReceiver);
        }
    }

    void PlayAttack03Effect()
    {
        GameObject go = null;
        go = GameObject.Instantiate(attack03EffectPrefab, attack03Point.position, attack03Point.rotation);
        go.transform.parent = attack03Point.transform;
        go.GetComponent<BossBuliet>().Damage = attackArray[2];
    }

    void AutoMoveRBAI()   //小怪自动巡逻Void方法  
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
                angle = Random.Range(-180, 180);
                anim.SetBool("walk", true);
            }
            else
            {
                idle = true;
                aiTimer = idleTimer;
                anim.SetBool("walk", false);
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
            rigidbody.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
            //characterController.Move(transform.forward * moveSpeed * Time.deltaTime);
        }

    }

    void AutoAngleAttackAi()
    {
         if (hp <= 0) return;
            //if (animation.IsPlaying("hit")) return;
            //if (isAttacking == true) return;
	        Transform player = targetGo.transform;
            Vector3 playerPos = player.position;
	        playerPos.y = transform.position.y;   //保证夹角不受到y轴的影响
	        float angle = Vector3.Angle(playerPos - transform.position, transform.forward);
	        if (angle < viewAngle / 2)
	        {
	            //在攻击视野之内
	            float distance = Vector3.Distance(player.position, transform.position);
	            if (distance < attackDistance)
	            {
	                //进行攻击
                    transform.LookAt(playerPos);
                    anim.SetBool("walk", false);
	                if (isAttacking == false)
	                {
                        timer += Time.deltaTime;
	                    if (timer > attackTimer)
	                    {
	                        timer = 0;
	                        Attack();
	                    }
	                }
	            }
	            else
	            {
                    //进行追击
	                anim.SetBool("walk", true);
                    rigidbody.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
	            }
	        }
	        else
	        {
                //在攻击视野之外 进行转向
	            anim.SetBool("walk", true);
                Quaternion targetRotation = Quaternion.LookRotation(playerPos - transform.position);
	            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1 * Time.deltaTime);
	        }

    }

    //受到攻击调用的方法、、
    //0.收到多少伤害、、
    //1.后退的距离
    //2.浮空的高度
    //3.出血特效
    public void TakeDamage(string args)
    {
        if (hp < 0)
        {
            transform.Translate(-transform.up * downSpeed);
            return;
        }
        isAttacking = false;
        string[] proArray = args.Split(',');
        int damage = int.Parse(proArray[0]);
        hp -= damage;
        //更新Boss的血条
        BossHpBar.Instance.UpdateShow(hp);
        hudText.NewText("- " + damage.ToString(), hpbarPoint.transform, Color.red, 30, 0.1f, -1f, 1f, bl_Guidance.LeftUp);
        //hpBarSlider.value = (float)this.hp / 200;
        
        if (Random.Range(0, 10) == 9)
        {
            anim.SetTrigger("takedamage");
            //animation.Play("takedamage"); //受至攻击的动画
        }
        float backDistance = float.Parse(proArray[1]); //后退距离
        float jumpDistance = float.Parse(proArray[2]); //浮空高度
        this.gameObject.transform.DOBlendableMoveBy(
            transform.InverseTransformDirection(TranscriptManager.Instance.player.transform.forward) * backDistance +
            Vector3.up * jumpDistance, 0.3f);
        //出血效果
        GameObject.Instantiate(damageEffectPrefab, transBloodPoint.position, Quaternion.identity);
        //renderer.material.color = Color.red;
        if (hp <= 0)
        {
            Dead();
        }
    }

    //当死亡时调用这个方法
    void Dead()
    {
        //1.第一种死亡方式是播放死亡动画
        //2.第二种破碎效果   
        TranscriptManager.Instance.RemoveEnemy(this.gameObject);
        //Destroy(hpBarGo);
        //Destroy(hudTextGo);
        Destroy(this.gameObject, 3f);
        BossHpBar.Instance.Hide();
        int random = Random.Range(0, 10);
        if (random < 5)
        {
            anim.SetTrigger("die");         
            //this.GetComponentInChildren<MeshExploder>().Explode();
            //this.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }
        else
        {
            this.GetComponentInChildren<MeshExploder>().Explode();
            this.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;                  
        }
        GameController.Instance.OnBossDie();
    }

    GameObject FindNearestTargetGo()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        GameObject nearestGo = null;
        float distance = Mathf.Infinity;        
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - transform.position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                nearestGo = go;
                distance = curDistance;
            }
        }
        return nearestGo;
    } //最近的目标，通过数组查找



    void CheckPositionAndRotation()
    {
        Vector3 position = transform.position;
        Vector3 eulerAngles = transform.eulerAngles;
        if (position.x != lastPosition.x || position.y != lastPosition.y || position.z != lastPosition.z ||
            eulerAngles.x != lastEulerAngles.x || eulerAngles.y != lastEulerAngles.y ||
            eulerAngles.z != lastEulerAngles.z)
        {
            TranscriptManager.Instance.AddBossOrcToSync(this);
            lastPosition = position;
            lastEulerAngles = eulerAngles;
        }

    }

   

    void CheckAnimation()
    {
        if (lastWalk != anim.GetBool("walk") || lastDie != anim.GetBool("die") ||
            lastTakeDamage != anim.GetBool("TakeDamage") || lastAttack01 != anim.GetBool("attack01") ||
            lastAttack02 != anim.GetBool("attack02") || lastAttack03 != anim.GetBool("attack03"))
        {
            bossController.SyncBossAnimation(new BossAnimationModel()
            {
                walk = anim.GetBool("walk"),
                die = anim.GetBool("die"),
                TakeDamage = anim.GetBool("TakeDamage"),
                attack01 = anim.GetBool("attack01"),
                attack02 = anim.GetBool("attack02"),
                attack03 = anim.GetBool("attack03"),
            });
            lastWalk = anim.GetBool("walk");
            lastDie = anim.GetBool("die");
            lastTakeDamage = anim.GetBool("TakeDamage");
            lastAttack01 = anim.GetBool("attack01");
            lastAttack02 = anim.GetBool("attack02");
            lastAttack03 = anim.GetBool("attack03");
        }
        
    }

    public void OnSyncBossAnimation(BossAnimationModel model)
    {
        if (model.walk)
        {
            anim.SetBool("walk",true);
        }
        if (model.walk == false)
        {
            anim.SetBool("walk", false);
        }
        if (model.TakeDamage)
        {
            anim.SetBool("TakeDamage",true);
        }
        if (model.die)
        {
            anim.SetBool("die", true);
        }
        if (model.attack01)
        {
            anim.SetBool("attack01",true);
        }
        if (model.attack02)
        {
            anim.SetBool("attack02", true);
        }
        if (model.attack03)
        {
            anim.SetBool("attack03", true);
        }

    }


}
