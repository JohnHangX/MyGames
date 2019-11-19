using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyGames
{
    class Program
    {
        static string gameListPath = "gameList.txt";
        static string playListPath = "playList.txt";
        static string gamesPath = "Games";
        /// <summary>
        /// 游戏字典
        /// </summary>
        static Dictionary<string, string> Games = new Dictionary<string, string>();
        /// <summary>
        /// 游戏播放信息
        /// </summary>
        public static Dictionary<string, GamePlayData> GamesPlay = new Dictionary<string, GamePlayData>();
        public static List<GamePlayData> playList = new List<GamePlayData>();
        private static GamesManager mgr;
        static void Main(string[] args)
        {
            Console.WriteLine("启动程序");
            CheckConfigPath();
            ReadGameList();
            ReadPlayList();

            foreach (var item in playList)
            {
                Console.Write(string.Format("name:{0} ,time:{1} ,nextName:{2}\n", item.gameName, item.gameTime, item.nextGameName));
            }
            mgr = GamesManager.Instance;
            mgr.Init(playList);
            Console.ReadLine();

        }
        static void CheckConfigPath()
        {
            
            if (!File.Exists(gameListPath))
            {
                File.CreateText(gameListPath);
            }
            if (!File.Exists(playListPath))
            {
                File.CreateText(playListPath);
            }
            if (!Directory.Exists(gamesPath))
            {
                Directory.CreateDirectory(gamesPath);
            }
        }

        static string GetData(string line, string dataName)
        {
            int start = line.IndexOf(dataName);
            int dataLen = dataName.Length;
            int end = line.IndexOf("/", start + dataLen);
            if (end <= 0)
            {
                end = line.Length;
            }
            int len = end - (start + dataLen);
            string result = line.Substring(start + dataLen, len);
            return result;
        }
        static void ReadGameList()
        {            
            string[]lines = File.ReadAllLines(gameListPath);
            foreach (var data in lines)
            {
                try
                {                   
                    string guid = GetData(data, "GUID:");
                    string gameName = GetData(data, "NAME:");
                    Games[guid] = gameName;
                    
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        static void ReadPlayList()
        {
            string[] lines = File.ReadAllLines(playListPath);
            foreach (var data in lines)
            {
                try
                {
                    string gameName = GetData(data, "NAME:");
                    short gameTime = short.Parse(GetData(data, "TIME:"));
                    string nextGame = GetData(data, "NEXTNAME:");
                    GamePlayData playData = new GamePlayData(gameName, gameTime, nextGame);
                    GamesPlay[gameName] = playData;
                    playList.Add(playData);
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                
            }
        }

    }
}
