using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//
using ExitGames.Client.Photon;

namespace GameClient
{
    public class Application : IPhotonPeerListener
    {
        private static Application instance;
        public static Application Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Application();
                }
                return instance;
            }
        }

        private readonly string serverAddress = "127.0.0.1:5055";
        private readonly string serverName = "GameServer";

        public bool m_ServerConnected;  // 已连接
        public bool m_OnLogin;          // 登陆中
        public PhotonPeer m_Peer;

        private Application()
        {
            this.m_ServerConnected = false;
            this.m_OnLogin = false;
            this.m_Peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        }

        /// <summary>
        /// 游戏主循环
        /// </summary>
        public void Run()
        {
            if (m_Peer.Connect(serverAddress, serverName))
            {
                // 连接成功
                do
                {
                    Console.WriteLine(">>>>>...");
                    // 启动点对点通信监听，当Server端有信息传过来的时候，根据信息的类型会触发OnEvent,OnOperationResponse
                    m_Peer.Service();

                    if (this.m_ServerConnected && !m_OnLogin)
                    {
                        Console.WriteLine("\n请输入账号：");
                        string account = Console.ReadLine();

                        Console.WriteLine("\n请输入密码：");
                        string password = Console.ReadLine();

                        Dictionary<byte, object> param = new Dictionary<byte, object>();
                        param.Add((byte)1, account.Trim());
                        param.Add((byte)2, password.Trim());

                        // 向Server端发送信息（命令代码暂定5）
                        this.m_Peer.OpCustom(5, param, true);
                        this.m_OnLogin = true;
                    }




                    // 挂起0.5s以卡死
                    System.Threading.Thread.Sleep(500);

                } while (true);
            }
            else
            {
                Console.WriteLine("Unknown hostname!");
            }
        }






        #region Client监听>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        public void DebugReturn(DebugLevel level, string message)
        {
            // 打印Debug的回传信息
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>DebugLevel:{0},Message:{1}", level, message);
        }

        public void OnEvent(EventData eventData)
        {
            // 监听Server端发送过来的事件信息
            throw new NotImplementedException();
        }

        public void OnMessage(object messages)
        {
            // 监听Server端发送过来的消息
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>Message:{0}", messages);
        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {
            // 监听Server端发送过来的操作命令信息
            switch (operationResponse.OperationCode)
            {
                case (byte)5:
                    switch (operationResponse.ReturnCode)
	                {
                        case (short)0: // 成功
                            int ret = Convert.ToInt16(operationResponse.Parameters[(byte)80]);
                            string account = Convert.ToString(operationResponse.Parameters[(byte)1]);
                            string password = Convert.ToString(operationResponse.Parameters[(byte)2]);
                            string nickName = Convert.ToString(operationResponse.Parameters[(byte)3]);

                            Console.WriteLine("Login Success \nRet={0} \nAccount={1} \nPassword={2} \nNickName-{3}", ret, account, password, nickName);
                            break;
                        case (short)1: // 账号或密码错误
                            Console.WriteLine(operationResponse.DebugMessage);
                            this.m_OnLogin = false;
                            break;
                        case (short)2: // 参数错误
                            Console.WriteLine(operationResponse.DebugMessage);
                            break;
                        default:
                            Console.WriteLine("未知ReturnCode:{0}", operationResponse.ReturnCode);
                            break;
	                }
                    break;
                default:
                    Console.WriteLine("未知OperationCode:{0}", operationResponse.OperationCode);
                    break;
            }
        }

        public void OnStatusChanged(StatusCode statusCode)
        {
            // 监听连接状态变更
            Console.WriteLine(">>>>>>>>>>>>>>>>>>PeerStatusCallback:" + statusCode.ToString());
            switch (statusCode)
            {
                case StatusCode.Connect:
                    Console.WriteLine("连线成功");
                    this.m_ServerConnected = true;
                    break;
                case StatusCode.Disconnect:
                    Console.WriteLine("连接断开");
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
                    Console.WriteLine("网络连接异常或远程服务器已关闭");
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
}
