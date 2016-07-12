using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient
{
    class Program
    {
        /// <summary>
        /// 游戏入口
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Application app = Application.Instance;
            app.Run();
        }        
    }
}
