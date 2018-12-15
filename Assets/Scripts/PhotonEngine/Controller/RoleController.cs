using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using LitJson;
using TumoCommon;
using TumoCommon.Model;
using TumoCommon.Tools;
using UnityEngine;


public class RoleController : ControllerBase
{
    public override OperationCode OpCode
    {
        get { return OperationCode.Role; }
    }

    public void GetRole()
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters.Add((byte)ParameterCode.SubCode,SubCode.GetRole);
        PhotonEngine.Instance.SendRequest(OpCode,parameters);
    }

    public void AddRole(Role role)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters.Add((byte)ParameterCode.SubCode, SubCode.AddRole);
        parameters.Add((byte) ParameterCode.Role,JsonMapper.ToJson(role));
        PhotonEngine.Instance.SendRequest(OpCode, parameters);
    }

    public void SelectRole(Role role)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters.Add((byte) ParameterCode.Role, JsonMapper.ToJson(role));
        PhotonEngine.Instance.SendRequest(OpCode,SubCode.SelectRole,parameters);
    }

    public void UpdateRole(Role role)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters.Add((byte)ParameterCode.Role, JsonMapper.ToJson(role));
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.UpdateRole, parameters);
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        print("到RoleController的43行： " + response.ToString());
        SubCode subCode = ParameterTool.GetParameter<SubCode>(response.Parameters, ParameterCode.SubCode, false);
        switch (subCode)
        {
            case SubCode.GetRole:
                List<Role> list = ParameterTool.GetParameter<List<Role>>(response.Parameters, ParameterCode.RoleList);
                if (OnGetRole != null)
                {
                    OnGetRole(list);
                }
                break;
            case SubCode.AddRole:
                Role role = ParameterTool.GetParameter<Role>(response.Parameters, ParameterCode.Role);
                if (OnAddRole != null)
                {
                    OnAddRole(role);
                }
                break;
            case SubCode.SelectRole:
                print("到RoleController的66行: "+ SubCode.SelectRole);
                if (OnSelectRole != null)
                {
                    OnSelectRole();
                }
                break;

        }
    }

    public event OnGetRoleEvent OnGetRole;
    public event OnAddRoleEvent OnAddRole;
    public event OnSelectRoleEvent OnSelectRole;
}


