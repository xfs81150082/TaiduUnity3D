using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using ExitGames.Client.Photon;
using LitJson;
using TumoCommon;
using UnityEngine;
using UnityEngine.UI;

public class ServerController : ControllerBase  {

    public override OperationCode OpCode
    {
        get{return OperationCode.GetServer;}
    }
    
    private void Awake()
    {
     
    }

    // Use this for initialization
    public override void Start () {
		base.Start();
	    PhotonEngine.Instance.OnConnectedToServer += GetServerList;
        
	}
  
    // Update is called once per frame
    void Update () {
		
	}

    //调用方法  向服务器发送请求 得到服务器列表
    public void GetServerList()
    {
        PhotonEngine.Instance.SendRequest(OperationCode.GetServer,new Dictionary<byte, object>());
    }

    //接受服务器信息的方法
    public override void OnOperationResponse(OperationResponse response)
    {
        Dictionary<byte, object> parameters = response.Parameters;
        object jsonObject = null;
        parameters.TryGetValue((byte) ParameterCode.ServerList, out jsonObject);
        List<TumoCommon.Model.ServerProperty> serverList =
            JsonMapper.ToObject<List<TumoCommon.Model.ServerProperty>>(jsonObject.ToString());
        GetServerList(serverList);        
    }

    void GetServerList(List<TumoCommon.Model.ServerProperty> serverList)
    {
        int index = 0;
        ServerProperty spDefault = null;
        GameObject goDefault = null;
        foreach (TumoCommon.Model.ServerProperty spServer in serverList)
        {
            string ip = spServer.Ip + ":4530";
            string name = spServer.Name;
            int count = spServer.Count;
            GameObject go = null;
            if (count > 50)
            {
                //火爆
                go = Instantiate(StartMenuController._instance.serveritemRed) as GameObject;
            }
            else
            {
                //流畅 
                go = Instantiate(StartMenuController._instance.serveritemGreen) as GameObject;
            }
            go.transform.parent = StartMenuController._instance.content.transform;
            go.transform.localScale = new Vector3(1, 1, 1 ); //不知道什么原因，物体被放大513位，此处加此代码还原为1；
            ServerProperty sp = go.GetComponent<ServerProperty>();
            sp.ip = ip;
            sp.Name = name;
            sp.Count = count;
            if (index == 0)
            {
                spDefault = sp;
                goDefault = go;
            }
            index++;
        }
        StartMenuController.sp = spDefault;
        StartMenuController._instance.serverNameText.text = spDefault.Name;
        StartMenuController._instance.serverSelectedGo.GetComponentInChildren<Text>().text = spDefault.Name;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        PhotonEngine.Instance.OnConnectedToServer -= GetServerList;
    }

    

    

   


}
