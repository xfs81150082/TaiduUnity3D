using System.Collections;
using System.Collections.Generic;
using TumoCommon.Model;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{

    public Transform[] positonTransformArray;


    void Awake()
    {
        SpawnPlayer();

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SpawnPlayer()
    {
        if (GameController.Instance.battleType==BattleType.Person)
        {
            //个人战斗角色加载
            Role role = PhotonEngine.Instance.role;
            GameObject playerPrefab;
            if (role.IsMan)
            {
                playerPrefab = Resources.Load("Player-battle/Player-boy_Transcript") as GameObject;
            }
            else
            {
                playerPrefab = Resources.Load("Player-battle/Player-girl_Transcript") as GameObject;
            }
            GameObject go = GameObject.Instantiate(playerPrefab, positonTransformArray[0].position, Quaternion.identity) as GameObject;
            TranscriptManager.Instance.player = go;  //指定player 替代transcriptManager中的player
            go.GetComponent<Player>().roleId = role.Id;
        }
        else if(GameController.Instance.battleType==BattleType.Team)
        {
            //团队战斗角色加载 : 2017/10/10团队加载不成功，移动和PK 控制不了，出现空指针，主要是：TranscriptManager.Instance.player = go; //空指针？取值不成功
            for (int i = 0; i < 3; i++)
            {
                Role role0 = GameController.Instance.teamRoles[i];
                Vector3 pos = positonTransformArray[i].position;
                GameObject playerPrefab0;
                if (role0.IsMan)
                {
                    playerPrefab0 = Resources.Load("Player-battle/Player-boy_Transcript") as GameObject;
                }
                else
                {
                    playerPrefab0 = Resources.Load("Player-battle/Player-girl_Transcript") as GameObject;
                }
                GameObject go0 =
                    GameObject.Instantiate(playerPrefab0, pos, Quaternion.identity) as
                        GameObject;
                go0.GetComponent<Player>().roleId = role0.Id;
                GameController.Instance.AddPlayer(role0.Id,go0);
                if (role0.Id == PhotonEngine.Instance.role.Id)
                {
                    //当前创建的角色为当前客户端控制的，角色攻击
                    TranscriptManager.Instance.player = go0; //空指针？
                }
                else
                {
                    //这个角色是其他客户端控制的，本客户端修改为移动不可控
                    go0.GetComponent<PlayerMove>().isCanControl = false; //空指针？
                    
                }
            }
            
        }
    }

}
