using System.Collections;
using System.Collections.Generic;
using TumoCommon.Model;
using UnityEngine;

public class TaskManager : MonoBehaviour {
    public static TaskManager _inatance;

    public TextAsset taskinfoText;
    private ArrayList taskList = new ArrayList();
    private IDictionary<int, Task> taskDict = new Dictionary<int, Task>();

    private Task currentTask;
    private PlayerAutoMove playerAutoMove;
    private PlayerAutoMove PlayerAutoMove
    {
        get
        {
            if (playerAutoMove == null)
            {
                playerAutoMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAutoMove>();
            }
            return playerAutoMove;
        }
        
    }

    public TaskDBController taskDbController;
    public event OnSyncTaskCompleteEvent OnSyncTaskComplete;



    private void Awake()
    {
        _inatance = this;
        taskDbController = this.GetComponent<TaskDBController>();

        taskDbController.OnGetTaskDBList += this.OnGetTaskDBList;
        taskDbController.OnAddTaskDB += this.OnAddTaskDB;
        taskDbController.OnUpdateTaskDB += this.OnUpdateTaskDB;

        InitTask();
        taskDbController.GetTaskDBList();
        
    }

    private void Start()
    {
        
    }

    public void OnGetTaskDBList(List<TaskDB> list)
    {
        if (list==null) return;
        foreach (var taskDb in list)
        {
            Task task = null;
            if (taskDict.TryGetValue(taskDb.TaskId,out task))
            {
                task.SyncTask(taskDb);
            }
        }
        if (this.OnSyncTaskComplete != null)
        {
            OnSyncTaskComplete();
        }

    }

    public void OnAddTaskDB(TaskDB taskDb)
    {
        Task task = null;
        if (taskDict.TryGetValue(taskDb.TaskId, out task))
        {
            task.SyncTask(taskDb);
        }


    }

    public void OnUpdateTaskDB()
    {
        
    }


    //初始化任务信息
    public void InitTask()
    {
        string[] taskinfoArray = taskinfoText.ToString().Split('\n');
        foreach(string str in taskinfoArray)
        {
            string[] proArray = str.Split('|');
            Task task = new Task();
            task.Id = int.Parse(proArray[0]);
            switch (proArray[1])
            {
                case "Main":
                    task.TaskType = TaskType.Main;
                    break;
                case "Reward":
                    task.TaskType = TaskType.Reward;
                    break;
                case "Daily":
                    task.TaskType = TaskType.Daily;
                    break;
            }
            task.Name = proArray[2];
            task.Icon = proArray[3];
            task.Does = proArray[4];            
            task.Cion = int.Parse(proArray[5]);
            task.Diamond = int.Parse(proArray[6]);
            task.TalkNpc = proArray[7];
            task.IdNpc = int.Parse(proArray[8]);
            task.IdTranscript = int.Parse(proArray[9]);
            taskList.Add(task);
            taskDict.Add(task.Id,task);
        }
            

    }


    public ArrayList  GetTaskList()
    {
        return taskList;
    }

    public void OnExcuteTask(Task task)
    {
        currentTask = task;
        if (task.TaskProgress == TaskProgress.NoStart)
        {//导航到npc那里，接受任务   

            PlayerAutoMove.SetDestin(NPCManager._instance.GetNpcById(task.IdNpc).transform.position);

        }else if (task.TaskProgress == TaskProgress.Accept)
        {
            PlayerAutoMove.SetDestin(NPCManager._instance.transcriptGO.GetComponent<Transform>().position);
        }
    }

    public void OnAcceptTask()
    {
        currentTask.TaskProgress = TaskProgress.Accept;
        currentTask.UpdateTask(this);//更新任务信息
        //TOTO 寻路至副本入口
        PlayerAutoMove.SetDestin(NPCManager._instance.transcriptGO.GetComponent<Transform>().position);
    }
    public void OnArriveDestion()
    {
        if (currentTask == null)
        {
            TranscriptMapUI._instance.Show();
        }
        else
        {
            if (currentTask.TaskProgress == TaskProgress.NoStart)
            {
                //NPC到达PNc位置
                NPCDialogUI._instance.SHow(currentTask.TalkNpc);
            }
            //到达副本入口
            TranscriptMapUI._instance.Show();
            TranscriptMapUI._instance.ShowTranscriptEnter(currentTask.IdTranscript);
        }

    }

}
