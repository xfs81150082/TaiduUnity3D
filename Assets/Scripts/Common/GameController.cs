using System.Collections;
using System.Collections.Generic;
using TumoCommon.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleType
{
    Person,
    Team,
    None
}

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController Instance
    {
        get { return _instance; }
    }
    private string playerPrefabName = "CostEngineer_boy";
    public BattleType battleType = BattleType.None;
    public List<Role> teamRoles = new List<Role>();
    public HashSet<int> dieRoleIdSet = new HashSet<int>(); //存已死亡的Id

    public int transcriptId = -1;
    public bool isMaster = false;

    private Dictionary<int,GameObject> playerDict = new Dictionary<int, GameObject>();
    private TaskDBController taskDbController;
    private BattleController battleController;

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        Transform posTransform = GameObject.Find("Player-pos").transform;
       
        if (PhotonEngine.Instance.role.IsMan)
        {
            playerPrefabName = "CostEngineer_boy";
        }
        else
        {
            playerPrefabName = "CostEngineer_girl";
        }
        GameObject playerGo = GameObject.Instantiate(Resources.Load("Player/"+playerPrefabName)) as GameObject;
        playerGo.transform.position = posTransform.position;
        playerGo.transform.parent = posTransform.transform;

        taskDbController = GetComponent<TaskDBController>();
        battleController = GetComponent<BattleController>();

        battleController.OnGetTeam += this.OnGetTeam;
        battleController.OnSyncPositionAndRotation += this.OnSyncPositionAndRotation;
        battleController.OnSyncMoveAnimation += this.OnSyncMoveAnimation;
        battleController.OnSyncPlayerAnimation += this.OnSyncPlayerAnimation;
        battleController.OnGameStateChange += this.OnGameStateChange;

    }


     public static int GetExpByLevel(int level)
    {
        return (int)((100f+(100f + (level - 1f) * 10f) )* (level-1) / 2);

    } //每级经验值

    public int GetRandomRoleID()//获取一个随机的角色id
    {
        if (battleType == BattleType.Team)
        {
            int index = Random.Range(0, teamRoles.Count);
            return teamRoles[index].Id;
        }
        else
        {
            return PhotonEngine.Instance.role.Id;
        }
    }

    public GameObject GetPlayerByRoleID(int roleId)
    {
        if (battleType == BattleType.Team)
        {
            GameObject go = null;
            playerDict.TryGetValue(roleId, out go);
            return go;
        }
        else
        {
            return TranscriptManager.Instance.player.gameObject;
        }
    }
    
    public void OnPlayerDie(int roleId)
    {
        if (battleType == BattleType.Person)
        {
            GameOverPanel.Instance.Show("游戏失败");
        }
        else
        {
            if (isMaster) //只在主机端做失败和胜利的检测
            {
                dieRoleIdSet.Add(roleId);
                if (dieRoleIdSet.Count == teamRoles.Count) 
                {
                    GameOverPanel.Instance.Show("游戏失败");
                    //向其他客户端发送游戏失败的状态
                    battleController.SendGameState(new GameStateModel(){isSuccess = false});
                }
            }
        }
        
    }

    public void OnBossDie()
    {
        if (battleType == BattleType.Person)
        {
            OnVictory();
        }
        else
        {
            OnVictory();
            //TOTO向其他客户端发达胜利的状态
            battleController.SendGameState(new GameStateModel() { isSuccess = true });
        }
        

    }

    void OnVictory()
    {
        GameOverPanel.Instance.Show("游戏胜利");
        foreach (Task task in TaskManager._inatance.GetTaskList())
        {
            if (task.TaskProgress == TaskProgress.Accept)
            {
                if (task.IdTranscript == transcriptId)
                {
                    task.TaskProgress = TaskProgress.Reward;//修改任务状态为领取奖励状态
                    TaskDB taskDb = task.TaskDB;
                    taskDb.State = (int) TaskState.Reward;
                    taskDbController.UpdateTaskDB(taskDb);
                }
            }
        }
    }

    void OnGameStateChange(GameStateModel model)
    {
        if (model.isSuccess)
        {
            OnVictory();
        }
        else
        {
            GameOverPanel.Instance.Show("游戏失败");
        }
    }

    public void OnGetTeam(List< Role> roles, int masterRoleId)
    {
        
    }

    void OnDestroy()
    {
        battleController.OnGetTeam -= this.OnGetTeam;
        battleController.OnSyncPositionAndRotation -= this.OnSyncPositionAndRotation;
        battleController.OnSyncMoveAnimation -= this.OnSyncMoveAnimation;
    }

    public void AddPlayer(int roleId, GameObject playerGamgObject)
    {
        playerDict.Add(roleId, playerGamgObject);
    }

    public void OnSyncPositionAndRotation(int roieId,Vector3 position , Vector3 eulerAngles)
    {
        GameObject go = null;
        bool isHave = playerDict.TryGetValue(roieId, out go);
        if (isHave)
        {
            go.GetComponent<PlayerMove>().SetPositionAndRotation(position,eulerAngles);
        }
        else
        {
            Debug.LogWarning("未找到对应的角色游戏物体进行更新位置");
        }
    }

    public void OnSyncMoveAnimation(int roleId, PlayerMoveAnimationModel model)
    {
        GameObject go = null;
        bool isHave = playerDict.TryGetValue(roleId, out go);
        if (isHave)
        {
            go.GetComponent<PlayerMove>().SetAnim(model);
        }
        else
        {
            Debug.LogWarning("未找到对应的角色游戏物体进行更新动画状态");
        }
    }

    public void OnSyncPlayerAnimation(int roleId,PlayerAnimationModel model)
    {
        GameObject go = null;
        bool isHave = playerDict.TryGetValue(roleId, out go);

        if (isHave)
        {
            go.GetComponent<PlayerAnimation>().SyncAnimation(model);
            if (model.die)
            {
                OnPlayerDie(roleId);
            }
        }
        else
        {
            Debug.LogWarning("未找到对应的角色游戏物体进行更新技能动画状态");
        }

    }

}
