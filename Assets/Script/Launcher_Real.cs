using UnityEngine;
using System.Collections;




public class Launcher_Real : Launcher {


    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //加载UI
        GameObject launcherui = UnityEngine.Object.Instantiate(Resources.Load("UI/Login/UIPrefab/Launcher")) as GameObject;
        this.cLauncherUI = launcherui.AddComponent<CLauncherUI>();

        if (Application.isMobilePlatform)
        {
            GameObject exitui = UnityEngine.Object.Instantiate(Resources.Load("UI/Login/UIPrefab/ExitGame")) as GameObject;
            //exitui.AddComponent<CExitGameUI>();
            Object.DontDestroyOnLoad(exitui);
        }

        // 网络检查
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //this.cLauncherUI.SetAction(Application.Quit);
            //this.cLauncherUI.ShowMessagebox(ConstCopyFilePath.NoNetwork);
            return;
        }

        StartLaunche();
    }

    void StartLaunche()
    {
        this.state = LaunchState.Create(this);
        this.state.Enter();
    }

    void Update()
    {
        if (this.state == null)
            return;
        var s = this.state.CheckTransition();
        if (s != null)
        {
            if (s.GetType ().Name .Contains ("EnterGame"))
            {
                CGame game = new CGame();
                game.StartGame(); 
            }
            this.state.Exit();
            s.Enter();
            this.state = s;
            return;
        }
        this.state.Update();
    }

    void OnDestroy()
    {
        if (this.state != null)
        {
            this.state.Exit();
            this.state = null;
        }
    }





}
