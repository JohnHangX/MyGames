using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGames
{
    public class GamePlayData
    {
        public GamePlayData(string name, short time, string nextGame)
        {
            gameName = name;
            gameTime = time;
            nextGameName = nextGame;
        }
        /// <summary>
        /// 游戏名
        /// </summary>
        public string gameName;
        /// <summary>
        /// 游戏时间
        /// </summary>
        public short gameTime;
        /// <summary>
        /// 下一游戏名
        /// </summary>
        public string nextGameName;
    }
}
