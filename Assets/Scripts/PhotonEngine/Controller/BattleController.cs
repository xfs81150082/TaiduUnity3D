using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using TumoCommon;
using TumoCommon.Model;
using TumoCommon.Tools;
using UnityEngine;

public class BattleController : ControllerBase
{
    public override OperationCode OpCode
    {
        get { return OperationCode.Battle; }
    }

    //发送同步游戏胜利状态
    public void SendGameState(GameStateModel model)
    {
        Dictionary<byte,object> parameters =new Dictionary<byte, object>();
        ParameterTool.AddParmeter(parameters,ParameterCode.GameSteteModel,model);
        PhotonEngine.Instance.SendRequest(OpCode,SubCode.SendGameState,parameters);
    }

    //发送同步主角的战斗动画、死亡动画
    public void SyncPlayerAnimation(PlayerAnimationModel model)
    {
        Dictionary<byte,object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParmeter(parameters, ParameterCode.PlayerAnimationModel
            , model);  //是model,不是parameters
        PhotonEngine.Instance.SendRequest(OpCode,SubCode.SyncAnimation,parameters);
    }

    //发送同步动画状态的请求
    public void SyncMoveAnimation(PlayerMoveAnimationModel model)
    {
        Dictionary<byte,object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParmeter(parameters,ParameterCode.PlayerMoveAnimationModel,model);
        PhotonEngine.Instance.SendRequest(OpCode,SubCode.SyncMoveAnimation,parameters);
    }

    //发起同步位置和旋转的请求
    public void SyncPositionAndRotion(Vector3 position,Vector3 eulerAngles)
    {
        Dictionary<Byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParmeter(parameters,ParameterCode.RoleId,PhotonEngine.Instance.role.Id,false);
        ParameterTool.AddParmeter(parameters,ParameterCode.Position,new Vector3Obj(position));
        ParameterTool.AddParmeter(parameters,ParameterCode.EulerAngles,new Vector3Obj(eulerAngles));
        PhotonEngine.Instance.SendRequest(OpCode,SubCode.SyncPositionAndRotation,parameters);
    }


    //发起组队的请求
    public void SendTeam()
    {
        PhotonEngine.Instance.SendRequest(OpCode,SubCode.SendTeam,new Dictionary<byte, object>());
    }

    public void CancelTeam()
    {
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.CancelTeam, new Dictionary<byte, object>());
    }

    //响应服务器转发的同步
    public override void OnEvent(EventData eventData)
    {
        SubCode subCode = ParameterTool.GetSubcode(eventData.Parameters);
        switch (subCode)
        {
            case SubCode.GetTeam:
                List<Role> roles = ParameterTool.GetParameter<List<Role>>(eventData.Parameters, ParameterCode.RoleList);
                int masterRoleId =
                    ParameterTool.GetParameter<int>(eventData.Parameters, ParameterCode.MasterRoleId, false);
                if (OnGetTeam != null)
                {
                    OnGetTeam(roles,masterRoleId);
                }
                break;
            case SubCode.SyncPositionAndRotation:
                int roleId = ParameterTool.GetParameter<int>(eventData.Parameters, ParameterCode.RoleId, false);
                Vector3 pos = ParameterTool.GetParameter<Vector3Obj>(eventData.Parameters, ParameterCode.Position).ToVector3();
                Vector3 eulerAngles =
                    ParameterTool.GetParameter<Vector3Obj>(eventData.Parameters, ParameterCode.EulerAngles).ToVector3();
                if (OnSyncPositionAndRotation != null)
                {
                    OnSyncPositionAndRotation(roleId, pos, eulerAngles);
                }
                break;
            case SubCode.SyncMoveAnimation:
                int roleId2 = ParameterTool.GetParameter<int>(eventData.Parameters, ParameterCode.RoleId, false);
                PlayerMoveAnimationModel model =
                    ParameterTool.GetParameter<PlayerMoveAnimationModel>(eventData.Parameters,
                        ParameterCode.PlayerMoveAnimationModel);
                if (OnSyncMoveAnimation != null)
                {
                    OnSyncMoveAnimation(roleId2,model);
                }
                break;
            case SubCode.SyncAnimation:
                int roleId3 = ParameterTool.GetParameter<int>(eventData.Parameters, ParameterCode.RoleId, false);
                PlayerAnimationModel model2 =
                    ParameterTool.GetParameter<PlayerAnimationModel>(eventData.Parameters,
                        ParameterCode.PlayerAnimationModel);
                if (OnSyncPlayerAnimation != null)
                {
                    OnSyncPlayerAnimation(roleId3, model2);
                }
                break;
            case SubCode.SendGameState:
                GameStateModel model3 =
                    ParameterTool.GetParameter<GameStateModel>(eventData.Parameters, ParameterCode.GameSteteModel);
                if (OnGameStateChange != null)
                {
                    OnGameStateChange(model3);
                }
                break;

        }
    }

    //响应服务器数据库的同步
    public override void OnOperationResponse(OperationResponse response)
    {
        SubCode subCode = ParameterTool.GetSubcode(response.Parameters);
        switch (subCode)
        {
            case SubCode.SendTeam:
                if (response.ReturnCode == (int) ReturnCode.GetTeam)
                {
                    List<Role> roles = ParameterTool.GetParameter<List<Role>>(response.Parameters,ParameterCode.RoleList);
                    int masterRoleId =
                        ParameterTool.GetParameter<int>(response.Parameters, ParameterCode.MasterRoleId, false);
                    if (OnGetTeam!=null)
                    {
                        OnGetTeam(roles,masterRoleId);
                    }
                }
                else if(response.ReturnCode==(int) ReturnCode.WartingTeam)
                {
                    if (OnWaitingTeam != null)
                    {
                        OnWaitingTeam();
                    }
                }
                break;
            case SubCode.CancelTeam:
                if (OnCancelTeam!=null)
                {
                    OnCancelTeam();
                }
                break;
           

        }
    }



    public event OnGetTeamEvent OnGetTeam;
    public event OnWaitingTeamEvent OnWaitingTeam;
    public event OnCancelTeamEvent OnCancelTeam;
    public event OnSyncPositionAndRotationEvent OnSyncPositionAndRotation;
    public event OnSyncMoveAnimationEvent OnSyncMoveAnimation;
    public event OnSyncPlayerAnimationEvent OnSyncPlayerAnimation;
    public event OnGameStateChangeEvent OnGameStateChange;
}
