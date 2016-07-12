using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 引入Photon命名空间
using Photon.SocketServer;

namespace GameServer
{
    public class ServerApplication : ApplicationBase
    {

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            // 建立点对点通信连接
            return new ServerPeer(initRequest);
        }

        protected override void Setup()
        {
            // 初始化Server
            //throw new NotImplementedException();
        }

        protected override void TearDown()
        {
            // 关闭Server并释放资源
            //throw new NotImplementedException();
        }
    }
}
