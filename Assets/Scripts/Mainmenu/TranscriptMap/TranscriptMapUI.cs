using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TumoCommon.Model;
using UnityEngine.SceneManagement;

public class TranscriptMapUI : MonoBehaviour {
    public static TranscriptMapUI _instance;

    private DOTweenAnimation tweenAnim;
    private TranscriptMapDilog dialog;
    private Dictionary<int, BtnTranscript> transcriptDict = new Dictionary<int, BtnTranscript>();
    private BtnTranscript btnTranscriptCurrent;
    private BattleController battleController;
    private TimerDialog timerDialog;
    //private TranscriptMapDilog transcriptMapDilog;

    private void Awake()
    {
        _instance = this;
        tweenAnim = this.GetComponent<DOTweenAnimation>();
        dialog = transform.Find("TranscriptMapDilogImage").GetComponent<TranscriptMapDilog>();
        timerDialog = transform.Find("TimeDialogImage").GetComponent<TimerDialog>();

        BtnTranscript[] transcripts = this.GetComponentsInChildren<BtnTranscript>();
        foreach (var temp in transcripts)
        {
            transcriptDict.Add(temp.id,temp);
        }
    }

    // Use this for initialization
    void Start () {
		battleController = GameController.Instance.GetComponent<BattleController>();
        battleController.OnWaitingTeam += this.OnWaitingTeam;
        battleController.OnCancelTeam += this.OnCancelTeamSuccess;
        battleController.OnGetTeam += this.OnGetTeam;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Show()
    {
        tweenAnim.DOPlayForward();
    }

    public void Hide()
    {
        tweenAnim.DOPlayBackwards();
    }

    public void OnBtnTranscriptClick(BtnTranscript transcript)
    {
        btnTranscriptCurrent = transcript;
        PlayerInfo info = PlayerInfo.Intance;
        if (info.Level >= transcript.needLevel)
        {
            dialog.ShowDialog(transcript);
        }
        else
        {
            dialog.ShowWarn();
        }
    }

    public void OnEnterClick()
    {
       
    }

    public void OnBack()
    {
        
    }

    public void ShowTranscriptEnter(int transcriptId)
    {
        BtnTranscript btnTranscript;
        transcriptDict.TryGetValue(transcriptId, out btnTranscript);
        OnBtnTranscriptClick(btnTranscript);

    }

    public void OnEnterPreson()
    {
        if (PlayerInfo.Intance.GetEnergy(btnTranscriptCurrent.needEnergy))
        {
            GameController.Instance.battleType = BattleType.Person;
            GameController.Instance.transcriptId = btnTranscriptCurrent.id;//保存当前进入的副本的Id，方便进行结束计算，和任务计算
            AsyncOperation operation = SceneManager.LoadSceneAsync(btnTranscriptCurrent.sceneName);
            LoadSceneProgressBar._instance.Show(operation);
        }
        else
        {
            MessageManger._instance.ShowMessage("体力不足，请稍后进入");
        }
        
    }

    public void OnEnterTeam()
    {
        battleController.SendTeam();//发起组队的请求
        timerDialog.StartTimer();
        dialog.Hide();
    }

    public void OnCancelTeam()
    {
        battleController.CancelTeam(); //发起取消组队的请求
        timerDialog.OnCancelButtonClick();
    }

    //响应服务器端组队成功
    public void OnGetTeam(List<Role> roles,int masterRoleId)
    {
        //toto
        GameController.Instance.battleType = BattleType.Team;  //当前战斗模式是否是团队战斗？
        GameController.Instance.teamRoles = roles;
        if (PhotonEngine.Instance.role.Id == masterRoleId)
        {
            GameController.Instance.isMaster = true;  //当前客户端是否是主机？
        }
        GameController.Instance.transcriptId = btnTranscriptCurrent.id;//保存当前进入的副本的Id，方便进行结束计算，和任务计算
        AsyncOperation operation = SceneManager.LoadSceneAsync(btnTranscriptCurrent.sceneName);
        LoadSceneProgressBar._instance.Show(operation);
    }

    //响应服务器端等待组队
    public void OnWaitingTeam()
    {
        print("OnWaitingTeam");
    }

    //响应服务器端取消组队
    public void OnCancelTeamSuccess()
    {
        print("OnCancelTeamSuccess");
    }

    public void ShowTranscriptMapDialog()
    {
        dialog.gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        if (battleController != null)
        {
            battleController.OnWaitingTeam -= this.OnWaitingTeam;
            battleController.OnCancelTeam -= this.OnCancelTeamSuccess;
            battleController.OnGetTeam -= this.OnGetTeam;
        }
    }


}
