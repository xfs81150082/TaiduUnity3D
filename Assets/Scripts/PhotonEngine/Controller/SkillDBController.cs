using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using TumoCommon;
using TumoCommon.Model;
using TumoCommon.Tools;
using UnityEngine;

public class SkillDBController : ControllerBase
{
    public override OperationCode OpCode
    {
        get { return OperationCode.SkillDB; }
    }

    public void Get()
    {
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.Get, new Dictionary<byte, object>());
    }

    public void Add(SkillDB skillDb)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        skillDb.Role = null;
        ParameterTool.AddParmeter(parameters, ParameterCode.SkillDB, skillDb);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.Add, parameters);
    }

    public void UpdateSkillDB(SkillDB skillDb)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        skillDb.Role = null;
        ParameterTool.AddParmeter(parameters, ParameterCode.SkillDB, skillDb);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.Update, parameters);
    }

    public void Upgrade(SkillDB skillDb)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        Role role = PhotonEngine.Instance.role;
        role.User = null;
        ParameterTool.AddParmeter(parameters, ParameterCode.Role, role);
        skillDb.Role = null;
        ParameterTool.AddParmeter(parameters, ParameterCode.SkillDB, skillDb);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.Upgrade, parameters);
    }


    public override void OnOperationResponse(OperationResponse response)
    {
        SubCode subCode = ParameterTool.GetSubcode(response.Parameters);
        switch (subCode)
        {
            case SubCode.Add:
                SkillDB skillDb = ParameterTool.GetParameter<SkillDB>(response.Parameters, ParameterCode.SkillDB);
                if (OnAddSkillDB != null)
                {
                    OnAddSkillDB(skillDb);
                }
                break;
            case SubCode.Get:
                List<SkillDB> list =
                    ParameterTool.GetParameter<List<SkillDB>>(response.Parameters, ParameterCode.SkillDBList);
                if (OnGetSkillDBList != null)
                {
                    OnGetSkillDBList(list);
                }
                break;
            case SubCode.Update:
                if (OnUpdateSkillDB != null)
                {
                    OnUpdateSkillDB();
                }
                break;
            case SubCode.Upgrade:
                SkillDB skillDb2 = ParameterTool.GetParameter<SkillDB>(response.Parameters, ParameterCode.SkillDB);
                if (OnUpgradeSkillDB != null)
                {
                    OnUpgradeSkillDB(skillDb2);
                }
                break;


        }
    }



    public event OnGetSkillDBEvent OnGetSkillDBList;
    public event OnAddSkillDBEvent OnAddSkillDB;
    public event OnUpdateTaskDBEvent OnUpdateSkillDB;
    public event OnUpgrateSkillDBEvent OnUpgradeSkillDB;

}
