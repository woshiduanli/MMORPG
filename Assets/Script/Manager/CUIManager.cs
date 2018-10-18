using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZCore;



public class CUIManager : CLoopObject
{
    // 访问的基本格式
    // http://localhost:8080/api/account/2

    private int MaxUICount = 4;
    private List<string> UIQueue = new List<string>();
    private List<string> idles = new List<string>();
    private UniqueIndex index_pool = new UniqueIndex(MAX_UI_COUNT);
    public const int MAX_UI_COUNT = 30;
    private CGameLuaUI[] uis = new CGameLuaUI[MAX_UI_COUNT];
    private Map<string, CGameUI> names = new Map<string, CGameUI>();
    public const int DISPOS_TIME = 30;
    public const int UPDATE_TIME = 5;
    public bool Hold { private set; get; }
    public AudioSource ButtomAudio { private set; get; }
    public AudioSource UIAudio { private set; get; }
    public GameObject UIRoot { private set; get; }
    public GameObject BlackBorder { private set; get; }
    public Camera UICamera { private set; get; }
    public Camera SenceCamera { private set; get; }
    private GameObject UI_BG;
    public CGameUI curFullWindow;
    public Camera SceneCamera { private set; get; }
    private Vector3 SceneCameraPos;
    public int GuideID;
    public NGUILink Link { private set; get; }
    private Light UILight;
    private Camera ScreenCamera;
    private RectTransform ScreenRect;
    private MonoBehaviour PostProcessing;
    private Map<CGameUI, bool> MirrRos = new Map<CGameUI, bool>();
    private Map<CGameUI, bool> SimpleRos = new Map<CGameUI, bool>();
    private string CacheUI;
    public Font uifont { private set; get; }
    public Font uifont_title { private set; get; }
    protected override void InitData()
    {
        LoadFont();
        //        this.SetRepeat(1, 1);
        UIRoot = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("UI/Login/UIPrefab/UIRoot") as GameObject);
        UnityEngine.Object.DontDestroyOnLoad(UIRoot);
        CClientCommon.SetActiveOverload(UIRoot, true);
        UIRoot.transform.position = Vector3.one * 1000;
        Link = UIRoot.GetComponent<NGUILink>();
        UICamera = Link.Get("UICamera_lk").GetComponent<Camera>();

        //#if UNITY_IPHONE || UNITY_IOS
        //        if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneX)
        //        {
        //            UICamera.rect = new Rect(0.04f, 0, 0.92f, 1);
        //            Link.Get("LeftBorder").SetActive(true);
        //            Link.Get("RightBorder").SetActive(true);
        //        }
        //#else
        //        bool bangs = CDeviceModel.CheckIsSpecialScreen();
        //        if (bangs)
        //        {
        //            UICamera.rect = new Rect(0.03f, 0, 0.94f, 1);
        //            Link.Get("LeftBorder").SetActive(bangs);
        //            Link.Get("RightBorder").SetActive(bangs);
        //        }
        //#endif
        //        SenceCamera = Link.Get("SceneCamera").GetComponent<Camera>();
        //        UICamera.cullingMask = CDefines.Layer.Mask.UI;

        //        MonoBehaviour mono = UICamera.GetComponent<MonoBehaviour>();
        //        if (mono)
        //            UnityEngine.Object.Destroy(mono);

        //        PostProcessing = SenceCamera.GetComponent<MonoBehaviour>();
        //        if (PostProcessing)
        //        {
        //            PostProcessing.enabled = false;
        //            UnityEngine.Object.Destroy(PostProcessing);
        //        }

        //        UI_BG = Link.Get("UI_BG");

        //        SceneCamera = Link.GetComponent<Camera>("SceneCamera");
        //        SceneCameraPos = SceneCamera.transform.localPosition;
        //        ScreenCamera = Link.GetComponent<Camera>("ScreenCamera");
        //        ScreenRect = Link.GetComponent<RectTransform>("CImage");
        //        UnityEngine.Object.DontDestroyOnLoad(UIRoot);

        //        LoadFont();
        //        this.acts = GetSingleT<CActivityManager>();
        //        this.Store = GetSingleT<StoreComponent>();
        //        this.setSystem = GetSingleT<GameSetSystem>();
        //        this.timelineSystem = GetSingleT<TimelineSystem>();

        //        GameObject go = new GameObject("UILight");
        //        go.transform.SetParent(UICamera.transform);
        //        go.transform.localEulerAngles = new Vector3(30, 0, 0);
        //        UILight = go.AddComponent<Light>();
        //        UILight.type = LightType.Directional;
        //        UILight.color = new Color32(255, 250, 248, 255);
        //        UILight.intensity = 0.91f;
        //        UILight.cullingMask = CDefines.Layer.Mask.UI | CDefines.Layer.Mask.UIPlayer;

        //        GameObject tempgo = new GameObject("UILight1");
        //        tempgo.transform.SetParent(UILight.transform);
        //        tempgo.transform.eulerAngles = new Vector3(20, -46, 50);
        //        Light templight = tempgo.AddComponent<Light>();
        //        templight.type = LightType.Directional;
        //        templight.color = new Color32(251, 199, 164, 255);
        //        templight.intensity = 0.33f;
        //        templight.cullingMask = CDefines.Layer.Mask.UI | CDefines.Layer.Mask.UIPlayer;
        //        UILight.gameObject.SetActive(false);
    }

    protected override void RegEvents()
    {
        RegEvent<CEvent.Scene.LevelWasLoaded>(OnLevelWasLoaded);
        RegEvent<CEvent.UI.ShowUI>(OnShowUIEvent);
        RegEvent<CEvent.UI.OpenUI>(OnCreateUIEvent);
        RegEvent<CEvent.UI.CloseUI>(OnCloseUI);
    }

    public void Add(CGameLuaUI ui, bool show)
    {
        if (!index_pool.CanAlloc())
        {
            LOG.TraceRed("ERROR: ui数量超出上限。ui name:{0}", ui.Name);
            return;
        }
        int index = index_pool.Alloc();

        if (uis[index] != null)
            throw new Exception(CString.Format("[CUIManager] ui index:{0} is already in use", index));
        if (names.ContainsKey(ui.Name))
            throw new Exception(CString.Format("[CUIManager] ui name:{0} is already exist", ui.Name));
        uis[index] = ui;
        names[ui.Name] = ui;
        ui.index = index;
        // 最后显示该ui
        ui.InitLua();
        if (show)
            ui.Show();
        else
        {
            if (ui.gameObject)
                ui.gameObject.SetActive(false);
        }
        idles.Add(ui.Name);
        if (ui.Layer == CUILayer.FullWindow && ui.isFullScreen)
        {
            if (UIQueue.Count >= MaxUICount)
                this.Remove(Get(UIQueue[0]));
            UIQueue.Add(ui.Name);
        }
        // CPROFILE.SAMPLE(false, "show ui");
    }

    public bool HasFullWindowUI()
    {
        for (int i = 0; i < uis.Length; i++)
        {
            CGameUI ui = uis[i];
            if (!ui)
                continue;
            if (CUILayer.FullWindow == ui.Layer && ui.IsShow())
                return true;
        }
        return false;
    }

    public void Remove(CGameUI ui)
    {
        if (!ui || ui.disposed)
            return;
        if (ui.index < 0)
            return;
        UIQueue.Remove(ui.Name);
        names.Remove(ui.Name);
        ui.ClosedDic.Clear();
        ui.enabled = false;
        uis[ui.index] = null;
        index_pool.Free(ui.index);
        ui.index = -1;

        if (loading.ContainsKey(ui.Name))
        {
            CGameUIAsset asset = loading[ui.Name];
            if (asset != null)
                asset.Destroy();
            loading.Remove(ui.Name);
        }
        ui.Dispose();
    }

    private void LoadFont()
    {
        uifont = Resources.Load<Font>("UI/Login/uifont");
        uifont_title = Resources.Load<Font>("UI/Login/uifont_title");
    }


    private void OnCloseUI(CObject sender, CEvent.UI.CloseUI e)
    {

        string uiname = rg.Match(e.UI).Value;
        CGameUI ui = Get(uiname);

        if (ui)
            ui.Close();

        if (loading.ContainsKey(uiname))
        {
            CGameUIAsset asset = loading[uiname];
            if (asset != null)
                asset.SetVisible(false);
        }
    }

    private void OnShowUIEvent(CObject sender, CEvent.UI.ShowUI e)
    {
        if (Hold)
            return;
        CGameUI exists = Get(e.UI) as CGameUI;
        if (exists)
            exists.Show();
    }
    private void OnCreateUIEvent(CObject sender, CEvent.UI.OpenUI e)
    {
        if (Hold)
            return;
        if (string.IsNullOrEmpty(e.UI))
            return;
        this.LoadUI(e.UI, e.Args);
    }
    public void CloseActiveUIs(CGameLuaUI other)
    {
        if (Hold)
            return;
        if (other.Layer == CUILayer.FullWindow)
        {
            for (int i = 0; i < uis.Length; i++)
            {
                CGameLuaUI ui = uis[i];
                if (!ui || !ui.IsShow() || other == ui)
                    continue;
                if (CUILayer.Tip == ui.Layer)
                    ui.Close();
                if (CUILayer.FullWindow == ui.Layer || CUILayer.MainFace == ui.Layer)
                    other.AddCloseUI(ui);
            }
            if (other.ClosedDic.Count == 0)
            {
                for (int i = 0; i < uis.Length; i++)
                {
                    CGameLuaUI ui = uis[i];
                    if (!ui || other == ui)
                        continue;
                    if (CUILayer.MainFace == ui.Layer)
                        other.AddCloseUI(ui);
                }
            }
        }
    }

    protected override void OnUpdate()
    {

        //if (Input.GetKeyDown (KeyCode.A))
        //{
        //  GameObject obj =   GameObject.Find("btn_denglu");
        //    obj.AddComponent<Button>().onClick.AddListener( ()=> { MyDebug.debug("11"); } );

        //}

        if (Hold)
            return;

        for (loading.Begin(); loading.Next();)
        {
            if (loading.Value != null)
                loading.Value.Update();
        }

        //if (idles.Count > 0)
        //{
        //    for (int i = 0; i < idles.Count; i++)
        //    {
        //        if (loading.ContainsKey(idles[i]))
        //            loading.Remove(idles[i]);
        //    }
        //    idles.Clear();
        //}

        //if (timelineSystem)
        //{
        //    bool isPlayTl = timelineSystem.IsPlaying();
        //    //UICamera.enabled = !isPlayTl;
        //    SenceCamera.enabled = !isPlayTl;
        //    UIRoot.SetActive(!isPlayTl);
        //}
        #region 回车键打开GM窗口
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                FireEvent(new CEvent.UI.OpenUI("CGmCmdUI"));
            }
        }
        #endregion
    }

    private void OnLevelWasLoaded(CObject sender, CEvent.Scene.LevelWasLoaded reale)
    {
        Hold = false;
        CheckGameState();
        string loadedLevelName = SceneManager.GetActiveScene().name;

        // 不等于的时候， 那么就显示出来， 如果等于就影藏
        UICamera.gameObject.SetActive(loadedLevelName != SceneName.ASYNC_LOADER_SCENE);
        // 过图清理资源
        //FireEvent(new CEvent.ResourceFactory.ClearResource(loadedLevelName));

        if (loadedLevelName == SceneName.ASYNC_LOADER_SCENE)
            CAsyncLevelLoaderUI.Create();
    }
    private Map<string, CGameUIAsset> loading = new Map<string, CGameUIAsset>();
    public Regex rg = new Regex("(?<=(" + "C" + "))[.\\s\\S]*?(?=(" + "UI" + "))", RegexOptions.Singleline);
    public void LoadUI(string ui_name, params object[] Args)
    {
        if (Hold)
            return;
        if (string.IsNullOrEmpty(ui_name))
            return;

        Type ui_type = Type.GetType(ui_name);
        if (ui_type == null)
            ui_type = typeof(CGameLuaUI);

        ui_name = rg.Match(ui_name).Value;
        CGameUI exists = Get(ui_name) as CGameUI;
        if (exists != null)
        {
            exists.ui_mgr = this;
            exists.args = Args; //在界面还没有show前赋值所传的参数
            if (exists.isActiveAndEnabled)
            {
                //exists.OnContextChange();
            }
            exists.Show();
            exists.LoadUICallback();
            return;
        }
        //防止重复加载
        if (loading.ContainsKey(ui_name))
            return;

        CGameUIAsset asset = CResourceFactory.CreateEmptyInstance<CGameUIAsset>(null, this, ui_name, ui_type, Args);
        loading.Add(ui_name, asset);
    }

    public CGameUI Get(string name)
    {
        CGameUI ui;
        if (names.TryGetValue(name, out ui))
            return ui;
        else
            return null;
    }

    public T Get<T>() where T : CGameUI
    {
        Type ui_type = typeof(T);
        string ui_name = rg.Match(ui_type.Name).Value;

        return Get(ui_name) as T;
    }
}

public class CUILayer
{
    /// <summary>
    /// 主界面(或者能和主界面共存的界面)
    /// </summary>
    public const int MainFace = 0;

    /// <summary>
    /// 全屏界面（系统界面 压屏....）
    /// </summary>
    public const int FullWindow = 1;

    /// <summary>
    /// 不受任何界面影响
    /// </summary>
    public const int Free = 2;
    /// <summary>
    /// TIPS界面有icon图标的建议使用。有其他全屏界面被打开会被清掉
    /// </summary>
    public const int Tip = 3;
}