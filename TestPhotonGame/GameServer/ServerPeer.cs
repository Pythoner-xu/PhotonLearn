using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using CommonProtocol;

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
                case (byte)OperationCode.Login:
                    // 构造响应
                    OperationResponse response = new OperationResponse();
                    response.OperationCode = operationRequest.OperationCode;
                    //
                    if (operationRequest.Parameters.Count < 2)
                    {
                        // 登录错误,告知Client
                        response.ReturnCode = (short)ErrorCode.InvalidParam;
                        response.DebugMessage = "Login Fail";
                    }
                    else
                    {
                        string account = (string)operationRequest.Parameters[(byte)LoginParamType.Account];
                        string password = (string)operationRequest.Parameters[(byte)LoginParamType.Password];
                        if (account == "test" && password == "1234")
                        {
                            // 登录成功
                            int Ret = 1;
                            Dictionary<byte, object> param = new Dictionary<byte, object>();
                            param.Add((byte)LoginParamType.Ret, Ret);
                            param.Add((byte)LoginParamType.Account, account);
                            param.Add((byte)LoginParamType.Password, password);
                            param.Add((byte)LoginParamType.NickName, "bighead");

                            response.ReturnCode = (short)ErrorCode.Ok;
                            response.DebugMessage = "";
                            response.Parameters = param;
                        }
                        else
                        {
                            response.ReturnCode = (short)ErrorCode.InvalidOperation;
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
