using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using TumoCommon;
using TumoCommon.Model;
using TumoCommon.Tools;
using UnityEngine;

public class InventoryItemDBController : ControllerBase
{
    public override OperationCode OpCode
    {
        get { return OperationCode.InventoryItemDB; }
    }

    public void GetInventoryItemDB()
    {
        PhotonEngine.Instance.SendRequest(OpCode,SubCode.GetInventoryItemDB,new Dictionary<byte, object>());
    }

    public void AddInventoryItemDB(InventoryItemDB itemDb)
    {
        itemDb.Role = null;
        Dictionary<byte,object> parameters=new Dictionary<byte, object>();
        ParameterTool.AddParmeter(parameters,ParameterCode.InventoryItemDB,itemDb);
        PhotonEngine.Instance.SendRequest(OpCode,SubCode.AddInventoryItemDB,parameters);
    }

    public void UpdateInventoryItemDB(InventoryItemDB itemDb)
    {
        itemDb.Role = null;
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParmeter(parameters, ParameterCode.InventoryItemDB, itemDb);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.UpdateInventoryItemDB, parameters);
    }

    public void UpgradeEquip(InventoryItemDB itemDb)
    {
        itemDb.Role = null;
        Role role = PhotonEngine.Instance.role;
        role.User = null;
        Dictionary<byte,object> parameters=new Dictionary<byte, object>();
        ParameterTool.AddParmeter(parameters,ParameterCode.InventoryItemDB,itemDb);
        ParameterTool.AddParmeter(parameters,ParameterCode.Role,role);
        PhotonEngine.Instance.SendRequest(OpCode,SubCode.UpgradeEquip,parameters);
    }

    public void UpdateInventoryItemDBList(InventoryItemDB itemDB1,InventoryItemDB itemDB2)
    {
        itemDB1.Role = null;
        itemDB2.Role = null;
        List<InventoryItemDB> list = new List<InventoryItemDB>();
        list.Add(itemDB1);
        list.Add(itemDB2);
        Dictionary<byte,object> parameters=new Dictionary<byte, object>();
        ParameterTool.AddParmeter(parameters,ParameterCode.InventoryItemDBList,list);
        PhotonEngine.Instance.SendRequest(OpCode,SubCode.UpdateInventoryItemDBList,parameters);
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        SubCode subCode = ParameterTool.GetParameter<SubCode>(response.Parameters, ParameterCode.SubCode, false);
        switch (subCode)
        {
            case SubCode.GetInventoryItemDB:
                List<InventoryItemDB> list =
                    ParameterTool.GetParameter<List<InventoryItemDB>>(response.Parameters,
                        ParameterCode.InventoryItemDBList);
                if (OnGetInventoryItemDBList != null)
                {
                    OnGetInventoryItemDBList(list);
                }
                break;
            case SubCode.AddInventoryItemDB:
                if (OnAddInventoryItemDB != null)
                {
                    InventoryItemDB itemDb =
                        ParameterTool.GetParameter<InventoryItemDB>(response.Parameters, ParameterCode.InventoryItemDB);
                    OnAddInventoryItemDB(itemDb);
                }
                break;
            case SubCode.UpdateInventoryItemDB:
                if (OnUpdateInventoryItemDB != null)
                {
                    OnUpdateInventoryItemDB();
                }
                break;
            case SubCode.UpdateInventoryItemDBList:
                if (OnUpdateInventoryItemDBList != null)
                {
                    OnUpdateInventoryItemDBList();
                }
                break;
            case SubCode.UpgradeEquip:
                if (OnUpgradeEquip != null)
                {
                    OnUpgradeEquip();
                }
                break;

        }
    }


    public event OnGetInventoryItemDBListEvent OnGetInventoryItemDBList;
    public event OnAddInventoryItemDBEvent OnAddInventoryItemDB;
    public event OnUpdateInventoryItemDBEvent OnUpdateInventoryItemDB;
    public event OnUpdateInventoryItemDBListEvent OnUpdateInventoryItemDBList;
    public event OnUpgradeEquipEvent OnUpgradeEquip;

}
