using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System;
using MyGames;

public class GamesManager  {

    string fileDir;
    public delegate void ListenerHandler();
    public event ListenerHandler Listener;
    private Process curGame;
    private EventHandler exitGameHandler;
    List<GamePlayData> gamesList = new List<GamePlayData>();
    int curGameIndex = 0;
    GamePlayData curGameData;
    public void Init (List<GamePlayData> playList) {
        exitGameHandler = new EventHandler(NextProgress);
        for (int i = 0; i < playList.Count; i++)
        {
            gamesList.Add(playList[i]);
        }
        curGameIndex = 0;
        curGameData = null;
        fileDir = "Games/";
        //ShowWindow(GetForegroundWindow(), SW_SHOWMINIMIZED);
        SelectGame();
    }

    private static GamesManager _instance;
    private GamesManager()
    {

    }

    public static GamesManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new GamesManager();
            }
            return _instance;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    //operType参数如下：
    //0: 隐藏, 并且任务栏也没有最小化图标  
    //1: 用最近的大小和位置显示, 激活  
    //2: 最小化, 激活  
    //3: 最大化, 激活  
    //4: 用最近的大小和位置显示, 不激活  
    //5: 同 1  
    //6: 最小化, 不激活  
    //7: 同 3  
    //8: 同 3  
    //9: 同 1  
    //10: 同 1 
    [DllImport("kernel32.dll")]
    public static extern int WinExec(string exeName, int operType);

    [DllImport("user32.dll")]
    public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
    const int SW_SHOWMINIMIZED = 2; //{最小化, 激活}  
    const int SW_SHOWMAXIMIZED = 3;//最大化  
    const int SW_SHOWRESTORE = 1;//还原  
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    void SelectGame()
    {
        string gameName = "";
        if (curGameData == null)
        {
            curGameData = gamesList[0];
            gameName = curGameData.gameName;
        }
        else
        {
            gameName = curGameData.nextGameName;
            curGameData = Program.GamesPlay[gameName];
        }
        
        string exeName = fileDir + gameName + "/" + gameName + ".exe";
        StartProgress(exeName, gameName);
    }

    void StartProgress(string path, string gameName)
    {
        WinExec(path, 4);
        AddExitProcessFunc(gameName);
    }

    void AddExitProcessFunc(string gameName)
    {
        Process[] temp = Process.GetProcessesByName(gameName);
        if (temp.Length > 0)//如果查找到
        {
            //IntPtr handle = temp[0].MainWindowHandle;
            //SwitchToThisWindow(handle, true); // 激活，显示在最前
            curGame = temp[0];
            
            curGame.EnableRaisingEvents = true;
            curGame.Exited += exitGameHandler;
            if (curGame.HasExited == true) NextProgress();
            
            curGame.WaitForExit();
            NextProgress();
        }
        else
        {
            Process.Start(gameName + ".exe");//否则启动进程
            AddExitProcessFunc(gameName);
        }
    }

    void NextProgress(object obj, EventArgs e)
    {
        curGame.Exited -= exitGameHandler;
        //SelectGame();
    }
    void NextProgress()
    {
        curGame.Exited -= exitGameHandler;

        //SelectGame();
    }

    void Dispose()
    {
        if (curGame != null)
            curGame.Exited -= exitGameHandler;
    }

    ~GamesManager()
    {
        Dispose();
    }
}
