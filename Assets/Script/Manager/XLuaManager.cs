using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using LitJson;
using UniRx;
using UnityEngine;
using XLua;
using Action = System.Action;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using Object = UnityEngine.Object;

public class CSharpType
{
    public int HashID;
    public int TypeID;
    public object Value;
}

public class XLuaManager : CLoopObject
{
    public LuaEnv luaEnv { private set; get; }
    public LuaTable Reference { private set; get; }

    private System.Action<string, object[]> LuaOpenUI;

    [CSharpCallLua]
    private System.Action<float, float, float, uint> LuaUpdate;
    private Action LuaRateUpdate;
    private Action<string> LuaOnLevelWasLoaded;
    private Action<string> LuaDisposeEvent;
    private Action LuaNetDisConnect;
    private Action LuaNetConnect;

    [CSharpCallLua]
    public delegate int LuaHandleMsg(string msg);
    public LuaHandleMsg handleMsg;
    private Queue<string> MessageQueue = new Queue<string>();
    private Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
    private float ulimit_ms = 0f;
    private Map<string, string> LuaMap = new Map<string, string>();

    private static XLuaManager instance;
    protected override void Dispose(bool whatever)
    {
        instance = null;
        LuaUpdate = null;
        LuaRateUpdate = null;

        luaEnv.Dispose();
    }

    protected override void InitData()
    {
        InitLua();
        SetRepeat(1, 1);
    }

    private void InitLua()
    {
        instance = this;
        luaEnv = new LuaEnv();

        luaEnv.AddLoader((ref string filename) =>
        {
            //MyDebug.debug(filename + ":d");
            filename = filename.Replace(".", "/");
            //MyDebug.debug(filename + ":d2");
            //MyDebug.debug(Application.dataPath);

            string str = string.Empty;
            if (ConfigHelper.uiEditor_)
                str = File.ReadAllText(CString.Concat(Application.dataPath, CString.Format("/Lua/{0}.lua", filename)));
            //MyDebug.debug(CString.Concat(Application.dataPath, CString.Format("/Lua/{0}.lua", filename)) + ":d3");
            //MyDebug.debug(str + "d4"); 
            if (string.IsNullOrEmpty(str))
            {
                filename = filename.ToLower();
                LuaMap.TryGetValue(filename, out str);
                if (string.IsNullOrEmpty(str))
                {
                    AssetBundle assetBundle = AssetBundle.LoadFromFile(CDirectory.MakeFullMobilePath(CString.Format("res/lua/{0}.l", filename)));
                    UnityEngine.Object[] assets = assetBundle.LoadAllAssets();
                    TextAsset ta = assets[0] as TextAsset;
                    str = ta.text;
                    LuaMap[filename] = str;
                }
            }

            return Encoding.UTF8.GetBytes(str);
        });

        object[] luas = this.luaEnv.DoString(@"return require(""Manager.GameManager"")", "XLuaManager");
        LuaTable GlobalEnv = luas[0] as LuaTable;

        Action LuaInitData;
        GlobalEnv.Get("InitData", out LuaInitData);
        LuaInitData();
        GlobalEnv.Get("Update", out LuaUpdate);
        GlobalEnv.Get("RateUpdate", out LuaRateUpdate);

        LuaTable NetMgr = GlobalEnv.Get<LuaTable>("NetMgr");
        this.handleMsg = NetMgr.GetInPath<LuaHandleMsg>("HandleMsg");
        NetMgr.Dispose();

        LuaTable Global = GlobalEnv.Get<LuaTable>("Global");

        Global.Get("OnLevelWasLoaded", out LuaOnLevelWasLoaded);
        Global.Get("OpenUI", out LuaOpenUI);
        Global.Get("DisposeEvent", out LuaDisposeEvent);
        Global.Get("NetDisConnect", out LuaNetDisConnect);
        Global.Get("NetConnect", out LuaNetConnect);
        Global.Dispose();

        this.Reference = GlobalEnv.Get<LuaTable>("RefMgr");
        GlobalEnv.Dispose();
    }
    protected override void RegEvents()
    {
        // lua和c#必要的通信

        //RegEvent<CEvent.UI.DisposeEvent>((a, b) => { LuaDisposeEvent(b.sceneName); });
        //RegEvent<CEvent.UI.LuaOpenUI>((a, b) => { if (LuaOpenUI != null) { LuaOpenUI(b.ui, b.objs); } });
        //RegEvent<CEvent.UI.DisposeEvent>((a, b) => { LuaDisposeEvent(b.sceneName); });

        RegEvent<CEvent.Scene.LevelWasLoaded>((a, b) => { LuaOnLevelWasLoaded(SceneManager.GetActiveScene().name); });
        //RegEvent<CEvent.NetWork.NetDisconnect>((a, b) => { LuaNetDisConnect(); });
        //RegEvent<CEvent.NetWork.NetConnect>((a, b) => { LuaNetConnect(); });
    }

    protected override void OnUpdate()
    {
        if (LuaUpdate != null)
        {
            //LuaUpdate(Time.deltaTime, Time.realtimeSinceStartup, Time.unscaledDeltaTime, CTime.time_tick);
        }

        if (ulimit_ms == 0f)
            ulimit_ms = 1000F / Application.targetFrameRate;
        stopwatch.Start();

        for (int i = 0; i < MessageQueue.Count; i++)
        {
            if (this.stopwatch.ElapsedMilliseconds > ulimit_ms)
                break;

            if (MessageQueue.Count == 0)
                break;

            var message = MessageQueue.Dequeue();
            if (this.handleMsg != null)
                this.handleMsg(message);
        }

        stopwatch.Reset();
    }

    public override void RateUpdate()
    {
        if (LuaRateUpdate != null)
            LuaRateUpdate();
        luaEnv.Tick();
    }

    public void SendMessage(string msg)
    {
        MessageQueue.Enqueue(msg);
    }
}
