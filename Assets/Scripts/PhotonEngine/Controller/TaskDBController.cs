﻿using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using TumoCommon;
using TumoCommon.Model;
using TumoCommon.Tools;
using UnityEngine;

public class TaskDBController : ControllerBase
{
    public override OperationCode OpCode
    {
        get { return OperationCode.TaskDB; }
    }

    public void GetTaskDBList()
    {
        PhotonEngine.Instance.SendRequest(OpCode,SubCode.GetTaskDB,new Dictionary<byte, object>());
    }

    public void AddTaskDB(TaskDB taskDB)
    {
        Dictionary<byte,object> parameters=new Dictionary<byte, object>();
        taskDB.Role = null;
        ParameterTool.AddParmeter(parameters,ParameterCode.TaskDB,taskDB);
        PhotonEngine.Instance.SendRequest(OpCode,SubCode.AddTaskDB,parameters);
    }

    public void UpdateTaskDB(TaskDB taskDB)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        taskDB.Role = null;
        ParameterTool.AddParmeter(parameters, ParameterCode.TaskDB, taskDB);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.UpdateTaskDB, parameters);
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        SubCode subCode = ParameterTool.GetParameter<SubCode>(response.Parameters, ParameterCode.SubCode, false);
        switch (subCode)
        {
            case SubCode.GetTaskDB:
                List<TaskDB> list =
                    ParameterTool.GetParameter<List<TaskDB>>(response.Parameters, ParameterCode.TaskDBList);
                if (OnGetTaskDBList != null)
                {
                    OnGetTaskDBList(list);
                }
                break;
                ;
            case SubCode.AddTaskDB:
                TaskDB taskDb = ParameterTool.GetParameter<TaskDB>(response.Parameters, ParameterCode.TaskDB);
                if (OnAddTaskDB != null)
                {
                    OnAddTaskDB(taskDb);
                }
                break;
                ;
            case SubCode.UpdateTaskDB:
                if (OnUpdateTaskDB != null)
                {
                    OnUpdateTaskDB();
                }
                break;
        }


    }

    public event OnGetTaskDBListEvent OnGetTaskDBList;
    public event OnAddTaskDBEvent OnAddTaskDB;
    public event OnUpdateTaskDBEvent OnUpdateTaskDB;


}
