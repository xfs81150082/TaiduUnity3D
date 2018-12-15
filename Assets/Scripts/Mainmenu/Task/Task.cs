using System;
using System.Collections;
using System.Collections.Generic;
using TumoCommon.Model;
using UnityEngine;

public enum TaskType
{
    Main=0,          //主线任务
    Reward=1,        //赏金任务
    Daily=2          //日常任务
}

public enum TaskProgress
{
    NoStart=0,   //没开始
    Accept=1,    //接受
    Complete=2,  //完成
    Reward=3     //奖金
}



public class Task
{
    #region(private)

    private int id;             //任务id
    private TaskType taskType;  //任务类型
    private string name;        //任务名称
    private string icon;        //任务图标
    private string does;        //任务描述
    private int cion;           //金币奖励
    private int diamond;        //钻石奖励
    private string talkNpc;     //跟N交谈的话语
    private int idNpc;          //Npc的id
    private int idTranscript;   //副本的id
    private TaskProgress taskProgress = TaskProgress.NoStart;   //任务进度，任务状态

    public TaskDB TaskDB
    {
        get; set;
    }

    #endregion ()
    public delegate void OnTaskChangeEvent();
    public event OnTaskChangeEvent OnTaskChange;

    //用来同步任务信息
    public void SyncTask(TaskDB taskDb)
    {
        TaskDB = taskDb;
        taskProgress = (TaskProgress) taskDb.State;
    }

    public void UpdateTask(TaskManager taskManager)
    {
        if (TaskDB==null)
        {
            TaskDB = new TaskDB();
            TaskDB.State = (int) taskProgress;
            TaskDB.TaskId = id;
            TaskDB.LastUpdateTime = new DateTime();
            TaskDB.Type = (int) taskType;   
            taskManager.taskDbController.AddTaskDB(TaskDB);
        }
        else
        {
            this.TaskDB.State = (int)taskProgress;
            taskManager.taskDbController.UpdateTaskDB(this.TaskDB);
        }
    }

    #region(get+set方法)

    public int Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public TaskType TaskType
    {
        get
        {
            return taskType;
        }

        set
        {
            taskType = value;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public string Icon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }

    public string Does
    {
        get
        {
            return does;
        }

        set
        {
            does = value;
        }
    }

    public int Cion
    {
        get
        {
            return cion;
        }

        set
        {
            cion = value;
        }
    }

    public int Diamond
    {
        get
        {
            return diamond;
        }

        set
        {
            diamond = value;
        }
    }

    public string TalkNpc
    {
        get
        {
            return talkNpc;
        }

        set
        {
            talkNpc = value;
        }
    }

    public int IdNpc
    {
        get
        {
            return idNpc;
        }

        set
        {
            idNpc = value;
        }
    }

    public int IdTranscript
    {
        get
        {
            return idTranscript;
        }

        set
        {
            idTranscript = value;
        }
    }

    public TaskProgress TaskProgress
    {
        get
        {
            return taskProgress;
        }

        set
        {
            if (taskProgress != value)
            {
                taskProgress = value;
                OnTaskChange();
            }
            
        }
    }

    #endregion()


}
