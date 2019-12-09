using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SocketManager;

namespace MyGames
{
    class GameServer
    {
        #region 单例
        private static GameServer _instance;
        private GameServer() { }
        public static GameServer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameServer();
                    SocketManager.Instance.InitServer((msg)=> { _instance.ServerHandle(msg); });
                }
                return _instance;
            }
        }
        #endregion

        void ServerHandle(string msg)
        {

            Console.WriteLine(msg);
        }

        ~GameServer()
        {
            SocketManager.Instance.Dispose();
        }
    }
}
