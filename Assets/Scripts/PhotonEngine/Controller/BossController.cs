using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using TumoCommon;
using TumoCommon.Tools;
using UnityEngine;

public class BossController : ControllerBase
{
    public override OperationCode OpCode
    {
        get { return OperationCode.Boss; }
    }

    //发送同步Boss主角的战斗动画、死亡动画
    public void SyncBossAnimation(BossAnimationModel model)
    {
        Dictionary<byte,object> parsmeters = new Dictionary<byte, object>();
        ParameterTool.AddParmeter(parsmeters, ParameterCode.BossAnimationModel, model);
        PhotonEngine.Instance.SendRequest(OpCode,SubCode.SyncBossAnimation,parsmeters);
    }
    
    public override void OnEvent(EventData eventData)
    {
        SubCode subCode = ParameterTool.GetSubcode(eventData.Parameters);
        switch (subCode)
        {
            case SubCode.SyncBossAnimation:
                BossAnimationModel model =
                    ParameterTool.GetParameter<BossAnimationModel>(eventData.Parameters,
                        ParameterCode.BossAnimationModel);
                if (OnSyncBossAnimation != null)
                {
                    OnSyncBossAnimation(model);
                }
                break;
        }

    }

    public override void OnOperationResponse(OperationResponse response)
    {
        
    }

    public event OnSyncBossAnimationEvent OnSyncBossAnimation;

}
