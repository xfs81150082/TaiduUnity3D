using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TaskUI : MonoBehaviour {
    public static TaskUI _instance;
    private GameObject taskListContent;
    public GameObject taskItemPrefabs;
    private DOTweenAnimation doAnim;

    private void Awake()
    {
        _instance = this;
        doAnim = this.GetComponent<DOTweenAnimation>();
        taskListContent = transform.Find("Scroll View/Viewport/Content").gameObject;

    }

    // Use this for initialization
    void Start () {
        //InitTaskList();

        TaskManager._inatance.OnSyncTaskComplete += this.OnSyncTaskComplete;

        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSyncTaskComplete()
    {
        InitTaskList();
    }

    //初始化任务列表信息
    void InitTaskList()
    {
        ArrayList taskList = TaskManager._inatance.GetTaskList(); 
        foreach(Task task in taskList)
        {            
            GameObject  go = Instantiate(taskItemPrefabs);
            go.transform.parent = taskListContent.transform;
            TaskItemUI it = go.GetComponent<TaskItemUI>();
            it.SetTask(task);
        }
    }
    public void Hide()
    {
        doAnim.DOPlayBackwards();
    }
    public void OnShowClick()
    {
        doAnim.DOPlayForward();
    }

}
