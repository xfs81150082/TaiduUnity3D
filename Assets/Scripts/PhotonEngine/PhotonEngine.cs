using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using TumoCommon;
using TumoCommon.Model;
using TumoCommon.Tools;
using UnityEngine;

public class PhotonEngine : MonoBehaviour, IPhotonPeerListener
{
    public ConnectionProtocol protocol = ConnectionProtocol.Tcp;
    public string serverAddress = "127.0.0.1:4530";
    public string applicationName = "TumoServer";

    //定义委托 和 事件
    public delegate void OnConnectedToServerEvent();
    public event OnConnectedToServerEvent OnConnectedToServer;

    private static PhotonEngine _instance;
    private Dictionary<byte, ControllerBase> controllers = new Dictionary<byte, ControllerBase>();

    private PhotonPeer peer;
    public bool isConnected = false;

    public static PhotonEngine Instance
    {
        get { return _instance; }
    }

    public Role role;

    private void Awake()
    {
        _instance = this;
        peer = new PhotonPeer(this, protocol);
        peer.Connect(serverAddress, applicationName);
        DontDestroyOnLoad(this.gameObject);
    }
   
    void Update()
    {
        if (peer != null)
        {
            peer.Service();
        }

    }

    void OnGui()
    {
        if (isConnected)
        {
            if (GUILayout.Button("Send a operation."))
            {
                Dictionary<byte, object> dict = new Dictionary<byte, object>();
                dict.Add(1, "username");
                dict.Add(2, "password");
                peer.OpCustom(1, dict, true);
            }
        }

    }

    public void RegisterController(OperationCode opCode, ControllerBase controller)
    {
        controllers.Add((byte)opCode, controller);
    }

    public void UnRegisterController(OperationCode opCode)
    {
        controllers.Remove((byte)opCode);
    }

    //向服务器发起请求的Peer方法
    public void SendRequest(OperationCode opCode, Dictionary<byte, object> parameters)
    {
        Debug.Log("到PhotonEngine的76行:: " + opCode);
        peer.OpCustom((byte)opCode, parameters, true);
    }

    public void SendRequest(OperationCode opCode,SubCode subCode, Dictionary<byte, object> parameters)
    {
        Debug.Log("到PhotonEngine的82行: " + opCode);
        parameters.Add((byte) ParameterCode.SubCode,subCode);
        peer.OpCustom((byte)opCode, parameters, true);
        Debug.Log("到PhotonEngine的85行: " + opCode);
    }


    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log(level + " : " + message);
    }

    public void OnEvent(EventData eventData)
    {
        ControllerBase controller;
        OperationCode opCode =
            ParameterTool.GetParameter<OperationCode>(eventData.Parameters, ParameterCode.OperationCode, false);
        controllers.TryGetValue((byte) opCode, out controller);
        if (controller!=null)
        {
            controller.OnEvent(eventData);
        }
        else
        {
            Debug.LogWarning("Receive a unknown event. OperationCode: "+opCode);
        }
    }

    //接收服务器反馈回来的信息
    public void OnOperationResponse(OperationResponse operationResponse)
    {
        //接收服务器反馈回来的信息
        ControllerBase controller;
        controllers.TryGetValue(operationResponse.OperationCode, out controller);
        if (controller != null)
        {
            controller.OnOperationResponse(operationResponse);
        }
        else 
        {
            Debug.Log("Receive a unknown response.OperationCode: " + operationResponse.OperationCode);
        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        Debug.Log("OnStatusChanged: " + statusCode);
        switch (statusCode)
        {
            case StatusCode.Connect:
                isConnected = true;
                //if (OnConnectedToServer != null) OnConnectedToServer();//当连结到服务器时，通知大家做后续工作,（自动生成的）
                OnConnectedToServer();  //当连结到服务器时，通知大家做后续工作
                break;
            default:
                isConnected = false;
                break;
        }
    }


}

