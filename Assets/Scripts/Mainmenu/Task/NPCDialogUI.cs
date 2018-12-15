using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class NPCDialogUI : MonoBehaviour {
    public static NPCDialogUI _instance;
    private DOTweenAnimation doAnim;
    public Text npcTalkText;



    private void Awake()
    {
        _instance = this;
        doAnim = this.GetComponent<DOTweenAnimation>();
        npcTalkText = transform.Find("Text").GetComponent<Text>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SHow(string npcTalk)
    {
        npcTalkText.text = npcTalk;
        doAnim.DOPlayForward();
    }
    public void Hide()
    {
        doAnim.DOPlayBackwards();
    }
    public void OnAccept()
    {
        //通知任务管理器已经接受
        TaskManager._inatance.OnAcceptTask();
        Hide();
    }


}
