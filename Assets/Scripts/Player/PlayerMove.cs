using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour {

    public float velocity = 5;
    public bool isCanControl = true; //表示是否可以用键盘控制
    private Rigidbody rigidbody;
    private NavMeshAgent agent;
    private Animator anim;
    private PlayerAttack playerAttack;
    private Vector3 lastPosition = Vector3.zero;
    private Vector3 lastEulerAngles = Vector3.zero;
    private bool isMove = false;
    private DateTime lastUpdateTime = DateTime.Now;
    private BattleController battleController;

    //public int myRoleId = -1;

    void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        playerAttack = this.GetComponent<PlayerAttack>();
    }

	// Use this for initialization
	void Start () {
	    if (GameController.Instance.battleType == BattleType.Team && isCanControl)
	    {
	        battleController = GameController.Instance.GetComponent<BattleController>();
	        InvokeRepeating("SyncPositonAndRotion",0,1f/30);
	        InvokeRepeating("SyncMoveAnimation", 0, 1f / 30); 

        }
	}

    // Update is called once per frame
    void Update()
    {
        if (isCanControl == false) return;
        if (playerAttack.hp <= 0) return;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 nowVel = rigidbody.velocity;
        if (Mathf.Abs(h) > 0.5f || Mathf.Abs(v) > 0.5f)
        {
            anim.SetBool("Move", true);
            if (anim.GetCurrentAnimatorStateInfo(1).IsName("EmptyState"))
            {
                rigidbody.velocity = new Vector3(h * velocity, nowVel.y , v * velocity);
                transform.LookAt(new Vector3(h, 0, v) + transform.position);
            }
            else
            {
                rigidbody.velocity = new Vector3(0, nowVel.y, 0);
            }

        }
        else
        {
            anim.SetBool("Move", false);
            rigidbody.velocity = new Vector3(0, nowVel.y, 0);
        }
    }

    GameObject FindNearestTargetGo()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
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


    void SyncPositonAndRotion()
    {
        Vector3 position = transform.position;
        Vector3 eulerAngles = transform.eulerAngles;
        if (position.x != lastPosition.x || position.y != lastPosition.y || position.z != lastPosition.z ||
            eulerAngles.x != lastEulerAngles.x || eulerAngles.y != lastEulerAngles.y || eulerAngles.z != lastEulerAngles.z)
        {
            //进行同步
            battleController.SyncPositionAndRotion(position,eulerAngles);
            lastPosition = position;
            lastEulerAngles = eulerAngles;
        }
    }

    public void SetPositionAndRotation(Vector3 position,Vector3 eulerAngles)
    {
        transform.position = position;
        transform.eulerAngles = eulerAngles;
    }

    void SyncMoveAnimation() //同步移动的动画
    {
        if (isMove!=anim.GetBool("Move"))  //当前动画状态发生了改变需要同步
        {
            //发送同步的请求
            PlayerMoveAnimationModel model = new PlayerMoveAnimationModel() {IsMove = anim.GetBool("Move") };
            model.SetTime(DateTime.Now);
            battleController.SyncMoveAnimation(model);
            isMove = anim.GetBool("Move");
        }
    }

    public void SetAnim(PlayerMoveAnimationModel model)
    {
        DateTime dt = model.GeTime();
        if (dt > lastUpdateTime)
        {
            anim.SetBool("Move", model.IsMove);
            lastUpdateTime = dt;
        }
    }


}
