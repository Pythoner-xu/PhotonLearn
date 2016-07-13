using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

// 
using ExitGames.Client.Photon;

public class PhotonClient : MonoBehaviour, IPhotonPeerListener
{
    public Text ui_InfoPanel;
    public InputField ui_AccountInput;
    public InputField ui_PasswordInput;


    private string serverAddress = "localhost:5055";
    private string serverApplication = "GameServer";
    private PhotonPeer m_Peer;
    private bool m_ConnectRet = false;   // 连接返回码
    private bool m_ServerConnected = false; // 连接状态

    void Awake()
    {
        this.m_Peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        this.m_ConnectRet = false;
        this.m_ServerConnected = false;
    }

    // Use this for initialization
    void Start()
    {
        bool ret = this.m_Peer.Connect(this.serverAddress, this.serverApplication);
        if (ret)
        {
            this.m_ConnectRet = true;
        }
        else
        {
            Debug.LogError("Unknown serverAddress!");
            this.ui_InfoPanel.text += string.Format("\n<color=red>{0}</color>", "Unknown serverAddress!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.m_ConnectRet)
        {
            // 启动点对点通信监听，当Server端有信息传过来的时候，根据信息的类型会触发OnEvent,OnOperationResponse
            this.m_Peer.Service();
            Debug.Log("<color=yellow>...</color>");
            //this.ui_InfoPanel.text += "<color=yellow>...</color>";
        }
    }



    public PhotonPeer GetClientPeer()
    {
        return this.m_Peer;
    }


    public void LoginClick()
    {
        if (this.ui_AccountInput.text != "" && this.ui_PasswordInput.text != "")
        {
            Dictionary<byte, object> param = new Dictionary<byte, object>();
            param.Add((byte)1, this.ui_AccountInput.text.Trim());
            param.Add((byte)2, this.ui_PasswordInput.text.Trim());

            // 向Server端发送信息（命令代码暂定5）
            this.m_Peer.OpCustom(5, param, true);
        }
    }



    #region >>>>>>>>>>实现IPhotonPeerListener接口中的方法<<<<<<<<<<
    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log("DebugReturn:" + level + "----" + message);
        this.ui_InfoPanel.text += string.Format("\n<color=green>{0}</color>", "DebugReturn:" + level + "----" + message);
    }

    public void OnEvent(EventData eventData)
    {
        Debug.Log("OnEvent:" + eventData.Code + "----" + eventData.Parameters);
        this.ui_InfoPanel.text += string.Format("\n<color=green>{0}</color>", "OnEvent:" + eventData.Code + "----" + eventData.Parameters);
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        Debug.Log("<color=green>" + operationResponse.OperationCode + "</color>");
        this.ui_InfoPanel.text += string.Format("\n<color=green>{0}</color>", "OnEvent:" + operationResponse.OperationCode);
        switch (operationResponse.OperationCode)
        {
            case (byte)5:
                switch (operationResponse.ReturnCode)
                {
                    case (short)0:
                        int ret = Convert.ToInt16(operationResponse.Parameters[(byte)80]);
                        string account = Convert.ToString(operationResponse.Parameters[(byte)1]);
                        string password = Convert.ToString(operationResponse.Parameters[(byte)2]);
                        string nickName = Convert.ToString(operationResponse.Parameters[(byte)3]);

                        Console.WriteLine("Login Success \nRet={0} \nAccount={1} \nPassword={2} \nNickName-{3}", ret, account, password, nickName);
                        this.ui_InfoPanel.text += string.Format("\nLogin Success \nRet={0} \nAccount={1} \nPassword={2} \nNickName-{3}", ret, account, password, nickName);
                        break;
                    case (short)1:
                        // 账号或密码错误
                        Debug.LogError(operationResponse.DebugMessage);
                        this.ui_InfoPanel.text += string.Format("\n<color=green>{0}</color>", operationResponse.DebugMessage);
                        break;
                    case (short)2:
                        // 参数错误
                        Debug.LogError(operationResponse.DebugMessage);
                        this.ui_InfoPanel.text += string.Format("\n<color=green>{0}</color>", operationResponse.DebugMessage);
                        break;
                    default:
                        Debug.LogError("未知operationResponse.ReturnCode" + operationResponse.ReturnCode);
                        break;
                }
                break;
            default:
                Debug.LogError("未知operationResponse.OperationCode" + operationResponse.OperationCode);
                break;
        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        switch (statusCode)
        {
            case StatusCode.Connect:
                Debug.Log("连接成功....");
                this.ui_InfoPanel.text += string.Format("\n<color=green>{0}</color>", "连接成功....");
                this.m_ServerConnected = true;
                break;
            case StatusCode.Disconnect:
                Debug.Log("连接失败....");
                this.ui_InfoPanel.text += string.Format("\n<color=green>{0}</color>", "连接失败....");
                this.m_ServerConnected = false;
                break;
            case StatusCode.DisconnectByServer:
                break;
            case StatusCode.DisconnectByServerLogic:
                break;
            case StatusCode.DisconnectByServerUserLimit:
                break;
            case StatusCode.EncryptionEstablished:
                break;
            case StatusCode.EncryptionFailedToEstablish:
                break;
            case StatusCode.Exception:
                break;
            case StatusCode.ExceptionOnConnect:
                break;
            case StatusCode.ExceptionOnReceive:
                break;
            case StatusCode.QueueIncomingReliableWarning:
                break;
            case StatusCode.QueueIncomingUnreliableWarning:
                break;
            case StatusCode.QueueOutgoingAcksWarning:
                break;
            case StatusCode.QueueOutgoingReliableWarning:
                break;
            case StatusCode.QueueOutgoingUnreliableWarning:
                break;
            case StatusCode.QueueSentWarning:
                break;
            case StatusCode.SecurityExceptionOnConnect:
                break;
            case StatusCode.SendError:
                break;
            case StatusCode.TimeoutDisconnect:
                break;
            default:
                break;
        }
    }
    #endregion
}
