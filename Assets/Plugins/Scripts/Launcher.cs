using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using System;
using Object = UnityEngine.Object;
using System.Net;
using System.Threading;

public interface IGame
{
    void StartGame();
}

public class ConstCopyFilePath
{
    public const string language = "res/language/language.txt";
    public const string version = "res/version.txt";
    public const string _DllPath = "res/Game.dll";
    public const string launcher = "res/launcher.l";

    public const string LauncherTips = "游戏启动中";
    public const string VerSionTips = "获取更新内容";
    public const string DownLoadAssetsTips = "正在为您下载更新资源包";
    public const string DownLoadSizeTips = "本次下载{0}";
    public const string LoadAssetsTips = " 游戏启动中";
    public const string NoNetwork = "您的网络异常，请检查您的手机网络";
    public const string NetworkErro = "获取版本信息失败";
    public const string DownLoadErro = " 资源下载失败";
    public const string CheckVersionForceUpdate = "当前版本过低，请重新下载安装包";
    public const string CheckVersionFailure = "版本检查失败，检查网络是否异常";
    public const string DownErroMsgStr = "下载资源失败，是否重试?";
}

#if true
public class Launcher : MonoBehaviour
{
    protected internal CLauncherUI cLauncherUI = null;
    protected LaunchState state = null;

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

        if (this.state.pause)
            return;

        var s = this.state.CheckTransition();
        if (s != null)
        {
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

#else 

public class Launcher : MonoBehaviour
{
    public static string Version;
    public enum LState
    {
        Init = 0,
        VerSion,
        EnterGame,
        Over,
    }

#region 客户端启动状态基

    internal class LauncherState
    {
        protected Launcher launcher;
        public LauncherState(Launcher lau)
        {
            this.launcher = lau;
            Enter();
        }
        public virtual void Enter() { }
        public virtual void Update() { }
    }

    /// <summary>
    /// 初始化 拷贝txt和game.dll
    /// </summary>
    internal class InitState: LauncherState
    {
        private Queue<Progress.AssetProgress.ItemProgress> ProgressQueue = new Queue<Progress.AssetProgress.ItemProgress>();
        private Queue<string> CopyQueue = new Queue<string>();
        private const string _MDBPath = "res/Game.dll.mdb";

        public InitState(Launcher lau) : base(lau) { }
        private bool isDone;
        private void CheckCopyQueue(string file)
        {
            string filepath = CDirectory.MakeCachePath(file);
            if (File.Exists(filepath))
                return;
            CopyQueue.Enqueue(file);
        }

        public override void Enter()
        {
            if (Application.isEditor)
                CDirectory.ClearCachePath("res");
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            TextAsset asset = Resources.Load("config") as TextAsset;
            ConfigHelper.LoadConfig(asset);
            Resources.UnloadAsset(asset);
            ConfigHelper.SetFrameRate();

            Progress.Instance.SetProgressPercent(100, 0);

            //加载UI
            GameObject launcherui = UnityEngine.Object.Instantiate(Resources.Load("UI/Login/UIPrefab/Launcher")) as GameObject;
            this.launcher.cLauncherUI = launcherui.AddComponent<CLauncherUI>();

            if(Application.isMobilePlatform)
            {
                GameObject exitui = UnityEngine.Object.Instantiate(Resources.Load("UI/Login/UIPrefab/ExitGame")) as GameObject;
                exitui.AddComponent<CExitGameUI>();
                Object.DontDestroyOnLoad(exitui);
            }

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                this.launcher.cLauncherUI.SetOk(Application.Quit);
                this.launcher.cLauncherUI.ShowMessagebox(ConstCopyFilePath.NoNetwork);
                return;
            }

            Launcher.Version = Read("Version.txt");
            if (!string.IsNullOrEmpty(Launcher.Version))
            {
                if (VersionHelper.StrToInt(Launcher.Version) < VersionHelper.StrToInt(Application.version))
                {
                    CDirectory.ClearCachePath("");
                    CDirectory.ClearCachePath("res");
                }
            }
            Write("Version.txt", Application.version);
            Launcher.Version = Application.version;

            AssetBundle assetBundle = AssetBundle.LoadFromFile(CDirectory.MakeFullMobilePath(ConstCopyFilePath.launcher));
            UnityEngine.Object[] assets = assetBundle.LoadAllAssets();
            TextAsset lau = assets[0] as TextAsset;

            
            ByteReader reader = new ByteReader(System.Text.Encoding.UTF8.GetBytes(lau.text));
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                CheckCopyQueue(line);
            }

            for (int i = 0; i < CopyQueue.Count; i++)
                ProgressQueue.Enqueue(Progress.Instance.CreateItem(ConstCopyFilePath.LauncherTips));

            launcher.StartCoroutine(CopyFile());
        }

        IEnumerator CopyFile()
        {
            yield return null;
            if (CopyQueue.Count > 0)
                yield return CopyFile(CopyQueue.Dequeue());
            else
                isDone = true;
        }

        IEnumerator CopyFile(string file)
        {
            yield return null;
            string filepath = CDirectory.MakeCachePath(file);
            using (var www = new WWW(CDirectory.MakeWWWStreamPath(file)))
            {
                while (!www.isDone)
                    yield return null;
                if (!string.IsNullOrEmpty(www.error))
                {
                    LOG.LogError("WWW download:" + www.error + "  path :  " + www.url);
                    yield break;
                }
                string path = Path.GetDirectoryName(filepath);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                File.WriteAllBytes(filepath, www.bytes);
                ProgressQueue.Dequeue().Done();
                www.Dispose();
            }
            yield return launcher.StartCoroutine(CopyFile());
        }

        public override void Update()
        {
            if (!isDone)
                return;
            if (ConfigHelper.CheckVersion)
                this.launcher.ChangeState(LState.VerSion);
            else
                this.launcher.ChangeState(LState.EnterGame);
        }



        private void Write(string filename, string txt)
        {
            filename = CDirectory.MakeCachePath(filename);
            FileInfo t = new FileInfo(filename);
            if (t.Exists)
                t.Delete();
            StreamWriter sw = t.CreateText();
            if (sw != null)
            {
                sw.Write(txt);
                sw.Close();
                // 解锁
                sw.Dispose();
            }
        }

        private string Read(string filename)
        {
            filename = CDirectory.MakeCachePath(filename);
            FileInfo t = new FileInfo(filename);
            if (!t.Exists)
                return null;
            string txt = string.Empty;
            StreamReader sr = null;
            try
            {
                sr = File.OpenText(filename);
            }
            catch (Exception)
            {
                return null;
            }
            if (sr != null)
            {
                txt = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
            }
            return txt;
        }
    }

    /// <summary>
    /// 检查更新
    /// </summary>
    internal class CheckVerSionState: LauncherState
    {
        public CheckVerSionState(Launcher lau) : base(lau) { }
        private HttpHelper httpHelper;
        public override void Enter()
        {
            this.httpHelper = new HttpHelper();
        }

        public override void Update()
        {
            if (!string.IsNullOrEmpty(this.httpHelper.ErroTips))
            {
                this.httpHelper.Dispose();
                this.launcher.cLauncherUI.SetOk(() => { Enter(); });
                this.launcher.cLauncherUI.ShowMessagebox(this.httpHelper.ErroTips);
                return ;
            }

            if (this.httpHelper.isDone)
            {
                this.httpHelper.Dispose();
                this.httpHelper = null;

                Progress.Instance.AllDone();
                this.launcher.ChangeState(LState.EnterGame);
            }
        }
    }

    /// <summary>
    /// 进入游戏
    /// </summary>
    internal class EnterGame : LauncherState
    {
        private const string _MDBPath = "res/Game.dll.mdb";
        private IGame game;
        public EnterGame(Launcher lau) : base(lau) { }
        public override void Enter()
        {
            InitEventSystem();
            LoadGameData();
        }

        private void InitEventSystem()
        {
            GameObject eventsystem = new GameObject("EventSystem");
            Object.DontDestroyOnLoad(eventsystem);
            eventsystem.AddComponent<EventSystem>();
            StandaloneInputModule sim = eventsystem.AddComponent<StandaloneInputModule>();
            sim.forceModuleActive = true;
        }

        private void LoadGameData()
        {
            string path = CDirectory.MakeCachePath(ConstCopyFilePath.language);
            byte[] bytes = File.ReadAllBytes(path);
            Localization.language = "language";
            Localization.LoadCSV(bytes);

            if (Application.isEditor)
                PCLoadAssembly();
            else
                MobileLoadAssembly();
        }

#region PC环境下 加载DLL
        void PCLoadAssembly()
        {
            string mdb_path;
            string path = GetAssemblyPath(out mdb_path);
            System.Reflection.Assembly assembly = null;
            if (path.IndexOf(ConstCopyFilePath._DllPath) == -1)
            {
                FileStream stream = File.Open(path, FileMode.Open);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
                stream.Close();
                if (string.IsNullOrEmpty(mdb_path) && File.Exists(mdb_path))
                {
                    FileStream mdb_stream = File.Open(mdb_path, FileMode.Open);
                    byte[] mdb_buffer = new byte[mdb_stream.Length];
                    mdb_stream.Read(mdb_buffer, 0, (int)mdb_stream.Length);
                    mdb_stream.Close();
                    assembly = System.Reflection.Assembly.Load(buffer, mdb_buffer);
                }
                else
                {
                    assembly = System.Reflection.Assembly.Load(buffer);
                }
            }
            else
            {
#if ZTK
                assembly = ZToolKit.LoadAssembly(path);
#endif
            }
            this.game = (IGame)assembly.CreateInstance("CGame");
            this.game.StartGame();
        }

        //---------------------------------------------------
        static string GetAssemblyPath(out string mdb_path)
        {
            mdb_path = "";
            string path = CDirectory.MakeFilePath(ConstCopyFilePath._DllPath);
            if (!Application.isMobilePlatform)
            {
#if Build
                    path = CDirectory.MakeFilePath(ConstCopyFilePath._DllPath);
                    mdb_path = CDirectory.MakeFilePath(_MDBPath);
#else
                    path = Application.dataPath + "/../Library/ScriptAssemblies/" + "Assembly-CSharp.dll";
                    mdb_path = Application.dataPath + "/../Library/ScriptAssemblies/" + "Assembly-CSharp.dll.mdb";
#endif
            }
            return path;
        }
#endregion

#region 手机平台加载DLL
        void MobileLoadAssembly()
        {
            string path = CDirectory.MakeCachePath(ConstCopyFilePath._DllPath);
            System.Reflection.Assembly assembly = null;
#if ZTK
            assembly = ZToolKit.LoadAssembly(path);
#else
        FileStream stream = File.Open(path, FileMode.Open);
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, (int)stream.Length);
        stream.Close();
        assembly = System.Reflection.Assembly.Load(buffer);
#endif

            this.game = (IGame)assembly.CreateInstance("CGame");
            this.game.StartGame();
        }
#endregion
    }
#endregion

    private CLauncherUI cLauncherUI;
    private LauncherState launcherState;
    private LState lState = LState.Init;
    void Awake()
    {
        ChangeState(LState.Init);
    }

    private void ChangeState(LState state)
    {
        this.lState = state;
        switch (lState)
        {
            case LState.Init:
                if (!(launcherState is InitState))
                    launcherState = new InitState(this);
                break;
            case LState.VerSion:
                if (!(launcherState is CheckVerSionState))
                    launcherState = new CheckVerSionState(this);
                break;
            case LState.EnterGame:
                if (!(launcherState is EnterGame))
                    launcherState = new EnterGame(this);
                ChangeState(LState.Over);
                break;
            case LState.Over:
                launcherState = null;
                break;
        }
    }

    private void Update()
    {
        if (launcherState != null)
            launcherState.Update();
    }
}
#endif
