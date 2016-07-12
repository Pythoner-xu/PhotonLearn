using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace GameServer
{
    public class ServerPeer : ClientPeer
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="rpcProtocol"></param>
        /// <param name="nativePeer"></param>
        public ServerPeer(InitRequest initRequest)
            :base(initRequest)
        {

        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            // 断开连接监听
            //throw new NotImplementedException();
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            // 监听客户端发来的请求
            switch (operationRequest.OperationCode)
            {
                case (byte)5:
                    // 构造响应
                    OperationResponse response = new OperationResponse();
                    response.OperationCode = operationRequest.OperationCode;
                    //
                    if (operationRequest.Parameters.Count < 2)
                    {
                        // 登录错误,告知Client
                        response.ReturnCode = (short)2;
                        response.DebugMessage = "Login Fail";
                    }
                    else
                    {
                        string account = (string)operationRequest.Parameters[1];
                        string password = (string)operationRequest.Parameters[2];
                        if (account == "test" && password == "1234")
                        {
                            // 登录成功
                            int Ret = 1;
                            Dictionary<byte, object> param = new Dictionary<byte, object>();
                            param.Add((byte)80, Ret);
                            param.Add((byte)1, account);
                            param.Add((byte)2, password);
                            param.Add((byte)3, "bighead");

                            response.ReturnCode = (short)0;
                            response.DebugMessage = "";
                            response.Parameters = param;
                        }
                        else
                        {
                            response.ReturnCode = (short)1;
                            response.DebugMessage = "Wrong Account or Password!";
                        }
                    }

                    // 向Client发送消息，告知结果
                    SendOperationResponse(response, new SendParameters());
                    break;
                default:
                    break;
            }
        }
    }
}
