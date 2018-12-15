using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.PhotonEngine.Model;
using ExitGames.Client.Photon;
using TumoCommon;
using TumoCommon.Tools;
using UnityEngine;

public class EnemyController : ControllerBase
{
    public override OperationCode OpCode
    {
        get { return OperationCode.Enemy; }
    }

    //发起创建敌人的请求
    public void SendCreateEnemy(CreateEnemyModel model)
    {
        Dictionary<byte,object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParmeter(parameters,ParameterCode.CreateEnemyModel,model);
        PhotonEngine.Instance.SendRequest(OpCode,SubCode.CreateEnemy,parameters);
    }

    //同步敌人
    public void SyncEnemyPosition(EnemyPositionModel model)
    {
        Dictionary<byte,object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParmeter(parameters,ParameterCode.EnemyPositionModel,model);
        PhotonEngine.Instance.SendRequest(OpCode,SubCode.SyncPositionAndRotation,parameters);
    }

    public void SyncEnemyAnimation(EnemyAnimationModel model)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParmeter(parameters, ParameterCode.EnemyAnimationModel, model);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.SyncAnimation, parameters);
    }

    public override void OnEvent(EventData eventData)
    {
        SubCode subCode = ParameterTool.GetSubcode(eventData.Parameters);
        switch (subCode)
        {
            case SubCode.CreateEnemy:
                CreateEnemyModel model =
                    ParameterTool.GetParameter<CreateEnemyModel>(eventData.Parameters, ParameterCode.CreateEnemyModel);
                if (OnCreateEnemy != null)
                {
                    OnCreateEnemy(model);
                }
                break;
            case SubCode.SyncPositionAndRotation:
                EnemyPositionModel model1 =
                    ParameterTool.GetParameter<EnemyPositionModel>(eventData.Parameters,
                        ParameterCode.EnemyPositionModel);
                if (OnSyncEnemyPositionAndRotation != null)
                {
                    OnSyncEnemyPositionAndRotation(model1);
                }
                break;
            case SubCode.SyncAnimation:
                EnemyAnimationModel model2 =
                    ParameterTool.GetParameter<EnemyAnimationModel>(eventData.Parameters,
                        ParameterCode.EnemyAnimationModel);
                if (OnSyncEnemyAnimation != null)
                {
                    OnSyncEnemyAnimation(model2);
                }
                break;
        }
        
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        
    }

    public event OnCreateEnemyEvent OnCreateEnemy;
    public event OnSyncEnemyPositionAndRotationEvent OnSyncEnemyPositionAndRotation;
    public event OnSyncEnemyAnimationEvent OnSyncEnemyAnimation;


}