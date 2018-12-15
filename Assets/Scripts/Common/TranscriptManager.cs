using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.PhotonEngine.Model;
using UnityEngine;

public class TranscriptManager : MonoBehaviour {

    private static TranscriptManager instance;
    public static TranscriptManager Instance
    {
        get{  return instance; }
    }
    public GameObject player;     //当前主角

    private List<GameObject> enemyList = new List<GameObject>();
    private Dictionary<string,GameObject> enemyDict = new Dictionary<string, GameObject>();
    private List<Enemy> enemyToSyncList = new List<Enemy>();           //需要同步位置的敌人的集合
    private List<Enemy> enemyToSyncAnimationList = new List<Enemy>();  //需要同步动画的敌人的集合
    private BossOrc bossToSync = null;
    private EnemyController enemyController;

    private Animation enemyAnim;

    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        if (GameController.Instance.battleType == BattleType.Team)
        {
            enemyController = GetComponent<EnemyController>();
            enemyController.OnCreateEnemy += this.OnCreateEnemy;
            enemyController.OnSyncEnemyPositionAndRotation += this.OnSyncEnemyPositionAndRotation;
            enemyController.OnSyncEnemyAnimation += this.OnSyncEnemyAnimation;
        }

        if (GameController.Instance.battleType == BattleType.Team && GameController.Instance.isMaster)
        {
            InvokeRepeating("SyncEnemyPositionAndRotation", 0, 1 / 30f);
            InvokeRepeating("SyncEnemyAnimation", 0, 1 / 30f);
        }
    }

    void OnDestroy()
    {
        if (enemyController != null)
        {
             enemyController.OnCreateEnemy -= this.OnCreateEnemy;
        }
       
    }

    public void OnCreateEnemy(CreateEnemyModel model)
    {
        foreach (CreateEnemyProperty property in model.List)
        {
            GameObject enemyPrefab = Resources.Load("enemy/" + property.prefabName) as GameObject;
            GameObject go =
                GameObject.Instantiate(enemyPrefab, property.position.ToVector3(), Quaternion.identity) as GameObject;
            Enemy enemy = go.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.guid = property.guid;
                enemy.targetRoleId = property.targetRoleID;
            }
            else
            {
                BossOrc bossOrc = go.GetComponent<BossOrc>();
                bossOrc.guid = property.guid;
                bossOrc.targetRoleId = property.targetRoleID;
            }
        }
    }

    public void AddEnemy(GameObject enemyGo)
    {
        enemyList.Add(enemyGo);
        string guid = null;
        if (enemyGo.GetComponent<Enemy>() != null)
        {
            guid = enemyGo.GetComponent<Enemy>().guid;
        }
        else
        {
            guid = enemyGo.GetComponent<BossOrc>().guid;
        }
        enemyDict.Add(guid,enemyGo);
    }

    public void RemoveEnemy(GameObject enemyGo)
    {
        enemyList.Remove(enemyGo);
        string guid = null;
        if (enemyGo.GetComponent<Enemy>() != null)
        {
            guid = enemyGo.GetComponent<Enemy>().guid;
        }
        else
        {
            guid = enemyGo.GetComponent<BossOrc>().guid;
        }
        enemyDict.Remove(guid);
    }

    public List<GameObject> GetEnemyList()
    {
        return enemyList;
    }

    public Dictionary<string, GameObject> GetEnemyDict()
    {
        return enemyDict;
    }

    public void AddEnemyToSync(Enemy enemy)
    {
        enemyToSyncList.Add(enemy);
    }

    public void AddBossOrcToSync(BossOrc bossOrc)
    {
        this.bossToSync = bossOrc;
    }

    public void AddEnemyToSyncAnimation(Enemy enemy)
    {
        enemyToSyncAnimationList.Add(enemy);
    }

    void SyncEnemyPositionAndRotation()
    {
        if (enemyToSyncList != null && enemyToSyncList.Count > 0)
        {
            EnemyPositionModel model = new EnemyPositionModel();
            foreach (Enemy enemy in enemyToSyncList)
            {
                if (enemy != null)
                {
                    EnemyPositionProperty property = new EnemyPositionProperty()
                    {
                        guid = enemy.guid,
                        position = new Vector3Obj(enemy.transform.position),
                        eulerAngles = new Vector3Obj(enemy.transform.eulerAngles),
                    };
                    model.list.Add(property);
                }
            }
            if (bossToSync != null)
            {
                EnemyPositionProperty property = new EnemyPositionProperty()
                {
                    guid = bossToSync.guid,
                    position = new Vector3Obj(bossToSync.transform.position),
                    eulerAngles = new Vector3Obj(bossToSync.transform.eulerAngles),
                };
                model.list.Add(property);

            }
            bossToSync = null;
            enemyController.SyncEnemyPosition(model);
            enemyToSyncList.Clear();
        }

    }

    void OnSyncEnemyPositionAndRotation(EnemyPositionModel model)
    {
        foreach (EnemyPositionProperty property in model.list)
        {
            GameObject enemyGo;
            bool isGet = enemyDict.TryGetValue(property.guid, out enemyGo);
            if (isGet)
            {
                enemyGo.transform.position = property.position.ToVector3();
                enemyGo.transform.eulerAngles = property.eulerAngles.ToVector3();
            }
        }
    }


    //用来发起敌人动画同步的请求
    void SyncEnemyAnimation()
    {
        if (enemyToSyncAnimationList != null && enemyToSyncAnimationList.Count > 0)
        {
            EnemyAnimationModel model = new EnemyAnimationModel();
            foreach (Enemy enemy in enemyToSyncAnimationList)
            {
                Animation anim = enemy.GetComponent<Animation>();
                EnemyAnimtionProperty property = new EnemyAnimtionProperty()
                {
                    guid = enemy.guid,
                    isAttack = anim.IsPlaying("attack01"),
                    isDie = anim.IsPlaying("die"),
                    isTakeDamage = anim.IsPlaying("takedamage"),
                    isIdle = anim.IsPlaying("idle"),
                    isWalk = anim.IsPlaying("walk"),
                };
                model.list.Add(property);
            }
            enemyController.SyncEnemyAnimation(model);
            enemyToSyncAnimationList.Clear();
        }
    }

    public void OnSyncEnemyAnimation(EnemyAnimationModel model)
    {
        foreach (EnemyAnimtionProperty property in model.list)
        {
            GameObject enemyGo;

            bool isGet = enemyDict.TryGetValue(property.guid, out enemyGo);
            if (isGet)
            {
                Animation anim = enemyGo.GetComponent<Animation>();
                if (property.isAttack)
                {
                    anim.Play("attack01");
                }
                if (property.isDie)
                {
                    anim.Play("die");
                }
                if (property.isIdle)
                {
                    anim.Play("idle");
                }
                if (property.isTakeDamage)
                {
                    anim.Play("takedamage");
                }
                if (property.isWalk)
                {
                    anim.Play("walk");
                }
            }
        }
    }

    #region 没有引用

    List<GameObject> EnemyList()
    {       
        GameObject[] gos; 
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in gos)        
        {
            enemyList.Add(go);            
        }
        return enemyList;
    }//目标数组

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
 

        #endregion


}
