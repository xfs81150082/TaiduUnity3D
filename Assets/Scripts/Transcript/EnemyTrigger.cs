using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour {

    public GameObject[] enemyPrefabs;
    public Transform[] spawnPosArray;
    public float time = 0;  //表示多少秒后开始生成敌人
    public float repeateRate = 3;
    public int targetRoleId = -1;

    private bool isSpawned = false;
    private EnemyController enemyController;

	// Use this for initialization
	void Start () {
	    if (GameController.Instance.battleType==BattleType.Team && GameController.Instance.isMaster)
	    {
	        enemyController = TranscriptManager.Instance.GetComponent<EnemyController>();
	    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (GameController.Instance.battleType == BattleType.Person)
        {
            if (other.tag == "Player" && isSpawned==false )
            {
                isSpawned = true;
                StartCoroutine(SpawnEnemy());
            }
        }
        else if(GameController.Instance.battleType==BattleType.Team)
        {
            if (other.tag == "Player" && isSpawned==false && GameController.Instance.isMaster)  //当是团队战斗模式的时候，判断当前客户端是否是主机，是主机才会生产敌人
            {
                isSpawned = true;
                StartCoroutine(SpawnEnemy());
                
            }
            
        }

    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(time);
        //发送消息 让其他客户端产生敌人 ，包括数量和类型
        foreach (GameObject go in enemyPrefabs)
        {
            List<CreateEnemyProperty> propertylist = new List<CreateEnemyProperty>();
            foreach(Transform t in spawnPosArray)
            {
                GameObject temp = GameObject.Instantiate(go, t.position, Quaternion.identity) as GameObject;
                string guidStr = Guid.NewGuid().ToString();                //guidStr=GUID
                int targetRoleId = GameController.Instance.GetRandomRoleID();
                if (temp.GetComponent<Enemy>() != null)
                {
                    Enemy enemy = temp.GetComponent<Enemy>();
                    enemy.guid = guidStr;                                //为每一个新生产的敌人创建一个GUID
                    //temp.GetComponent<Enemy>().enabled = true;
                    enemy.targetRoleId = targetRoleId;
                }
                else
                {
                    BossOrc bossOrc = temp.GetComponent<BossOrc>();
                    bossOrc.guid = guidStr;                             //为新生产的Boss敌人创建一个GUID
                    bossOrc.targetRoleId = targetRoleId;
                }

                CreateEnemyProperty property = new CreateEnemyProperty()
                {
                    guid = guidStr,
                    position = new Vector3Obj(t.position),
                    prefabName = go.name,
                    targetRoleID = targetRoleId
                };
                propertylist.Add(property);
            }
            if (GameController.Instance.battleType == BattleType.Team && GameController.Instance.isMaster)
            {
                CreateEnemyModel model = new CreateEnemyModel() {List = propertylist};
                enemyController.SendCreateEnemy(model);
            }
            yield return new WaitForSeconds(repeateRate);
        }
    }

}
