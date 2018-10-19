using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;
using UnityEngine.UI;
using UniRx;
using UnityEngine.Events;
using Action = System.Action;

public class CGameLuaUI : CGameUI
{
    private bool isFullScreen_;
    public override bool isFullScreen { get { return isFullScreen_; } }

    private int Layer_;
    public override int Layer { get { return Layer_; } }

    private bool autoLoad_;
    public override bool autoLoad { get { return autoLoad_; } }
    public bool Loop;

    public LuaTable scriptEnv { private set; get; }
    private Action<LuaTable> luaEnable;
    private Action<LuaTable> luaDisable;
    private Action<LuaTable> luaLoadUICallback;
    private Action<LuaTable> luaWakeUp;
    private Action<LuaTable> luaOnDestroy;
    [CSharpCallLua]
    private Action<LuaTable, int, string> luaNewItem;
    [CSharpCallLua]
    private Action<LuaTable, int, int> luaRefreshItem;
    public XLuaManager LuaMgr { private set; get; }

    private Map<int, NGUILink> Elements = new Map<int, NGUILink>();
    private Map<int, int> ListViewIDs = new Map<int, int>();
    private int HashID;

    public override void SetAsset(CGameUIAsset asset)
    {
        this.Canvas = this.gameObject.GetComponent<Canvas>();
    }

    // 拿到资源以后的入口函数
    public override void InitData(CGameUIAsset asset)
    {
        // 此时初始化的当前对象就是 CGameLuaUI
        this.ui_mgr = asset.uiMgr;
        this.SetAsset(asset);
        this.context = asset.Args;
        this.autoCreate = this.Layer == CUILayer.FullWindow && this.isFullScreen && this.context == null;

        this.Initialize();
        this.SetName(asset.UIName);
        // asset.IsVisible() 决定是不是要显示出来当前的对象 ，此时默认就是true
        // 在add函数里面， 初始化lua ，然后显示ui 
        this.ui_mgr.Add(this, asset.IsVisible());
        this.LoadUICallback();
    }

    public override void Initialize()
    {
        this.HashID = nguiLink.HashID;
        NGUILink[] links = this.gameObject.GetComponentsInChildren<NGUILink>(true);
        if (links != null && links.Length > 0)
        {
            for (int i = 0; i < links.Length; i++)
            {
                if (links[i] == this.nguiLink)
                    continue;
                Elements.Add(links[i].HashID, links[i]);
            }
        }
    }

    protected void InitLuaAsset()
    {
        // 是否ui堆栈，是否主界面，是否和
        this.LuaMgr = this.ui_mgr.GetSingleT<XLuaManager>();

        this.scriptEnv = this.context[0] as LuaTable;
        this.isFullScreen_ = scriptEnv.Get<bool>("IsFullScreen");

        this.Layer_ = scriptEnv.Get<int>("Layer");

        //this.dontDestoryOnLoad = scriptEnv.Get<bool>("DontDestoryOnLoad");
        this.SortingOrder = scriptEnv.Get<int>("SortingOrder");
        this.planeDistance = scriptEnv.Get<int>("PlaneDistance");

        Action<LuaTable> luaInitialize;
        this.scriptEnv.Get("Initialize", out luaInitialize);
        this.scriptEnv.Get("LoadUICallback", out luaLoadUICallback);
        this.scriptEnv.Get("UIEnable", out luaEnable);
        this.scriptEnv.Get("UIDisable", out luaDisable);
        this.scriptEnv.Get("OnWakeUp", out luaWakeUp);
        this.scriptEnv.Get("OnDestroy", out luaOnDestroy);

        luaInitialize(this.scriptEnv);
    }

    public override void AddCloseUI(CGameLuaUI ui)
    {
        ui.SetActive(false);
        ClosedDic[ui.Name] = true; //ui.autoLoad;
    }

    public override void SimpleClose()
    {
        if (disposed)
            return;
        if (IsShow())
        {
            ClosedDic.Clear();
            SetActive(false);
            if (this.ui_mgr.Hold)
                return;

            //if (Layer == CUILayer.FullWindow)
            //{
            //    if (this.ui_mgr.Hold)
            //        return;
            //    this.ui_mgr.OpenMainFaceUI();
            //}
        }
    }

    public override void SetActive(bool active)
    {
        if (this.gameObject && !this.gameObject.activeSelf)
            this.gameObject.SetActive(true);
        this.enabled = active;
    }

    public void SetPlaneDistance(int planeDistance)
    {
        if (!Canvas) return;
        Canvas.planeDistance = planeDistance;
    }

    protected void OnEnable()
    {
        CloseTime = 0;
        SetPlaneDistance(this.planeDistance);
        if (Layer == CUILayer.FullWindow && this.isFullScreen)
            ui_mgr.curFullWindow = this;
        OnUIEnable();
        FireEvent(new CEvent.UI.UIEnableEvent(this));

        // 如果是全屏界面， 此时应该关闭，ui 摄像机的渲染
        //if (this.Layer == CUILayer.FullWindow && this.isFullScreen)
        //    FireEvent(new CEvent.CameraCtrl.CameraActive(false));
        //InputField d;
        //d.text = "sfsf"; 
        for (int i = 0; i < Childidles.Count; i++)
            ChildMap.Remove(Childidles[i]);
        Childidles.Clear();

        for (ChildMap.Begin(); ChildMap.Next(); )
        {
            if (ChildMap.Value)
                ChildMap.Value.enabled = true;
        }
    }

    void OnDisable()
    {
        SetPlaneDistance(Def.UIDisableDistance);
        OnUIDisable();
    }


    public void AddChild(CUIElement element)
    {
        if (ChildMap != null)
            ChildMap[element.HashID] = element;
    }

    public void RemoveChild(CUIElement element)
    {
        if (Childidles != null)
            Childidles.Add(element.HashID);
    }


    public override void SetName(string name)
    {
        this.Name = name;
        this.gameObject.name = name;

        this.gameObject.transform.SetParent(this.ui_mgr.UIRoot.transform);
        this.gameObject.transform.localPosition = Vector3.zero;
        this.gameObject.transform.localEulerAngles = Vector3.zero;
        if (this.Canvas)
        {
            this.Canvas.planeDistance = this.planeDistance;
            this.Canvas.renderMode = RenderMode.ScreenSpaceCamera;
            this.Canvas.worldCamera = this.ui_mgr.UICamera;
            SortingOrder = this.Canvas.sortingOrder;
        }
    }


    public override void InitLua()
    {
        InitLuaAsset();
    }

    public override void Show()
    {
        if (UIShow())
            // 当自己显示成功的时候， 就关闭其他全屏的界面
            this.ui_mgr.CloseActiveUIs(this);
    }




    #region C# 驱动 Lua




    protected override void OnWakeUp()
    {
        luaWakeUp(this.scriptEnv);
    }

    private NGUILink GetLink(int id)
    {
        if (id == this.HashID)
            return this.nguiLink;

        NGUILink element;
        Elements.TryGetValue(id, out element);
        return element;
    }

    public override void OnDestroy()
    {
        luaOnDestroy(this.scriptEnv);

        luaEnable = null;
        luaDisable = null;
        luaLoadUICallback = null;
        luaWakeUp = null;
        luaOnDestroy = null;

        scriptEnv.Dispose();
        scriptEnv = null;
    }
    #endregion

    public int Clone(int linkid)
    {
        if (linkid == this.HashID)
            return Def.INVALID_ID;
        NGUILink element;
        Elements.TryGetValue(linkid, out element);

        if (!element)
            return Def.INVALID_ID;

        GameObject go = CUIElement.AddChild(element.gameObject.transform.parent.gameObject, element.gameObject);
        if (!go)
            return Def.INVALID_ID;

        NGUILink link = go.GetComponent<NGUILink>();
        link.DoInitIfDont();
        link.HashID = link.GetHashCode();
        if (!Elements.ContainsKey(link.HashID))
            Elements.Add(link.HashID, link);
        return link.HashID;
    }

    public override void OnUIEnable()
    {
        if (luaEnable != null)
        {
            luaEnable(this.scriptEnv);
        }
    }

    public override void OnUIDisable()
    {
        if (luaDisable != null)
        {
            luaDisable(this.scriptEnv);
        }
    }

    public void SetGoActive(int linkid, int id, int active)
    {
        NGUILink link = GetLink(linkid);
        if (link)
        {
            GameObject go = link.Get(id);
            if (go)
                CClientCommon.SetUIActive(go, active == 1);
        }
    }

    public void SetElementActive(int linkid, int active)
    {
        NGUILink link = GetLink(linkid);
        if (link)
            link.gameObject.SetActive(active == 0 ? false : true);
    }

    public int IsElementActive(int linkid)
    {
        NGUILink link = GetLink(linkid);
        if (link)
            return link.gameObject.activeInHierarchy ? 1 : 0;
        return 0;
    }

    public override void WakeUp()
    {
        OnWakeUp();
        UIShow();
    }


    public override void Dispose()
    {
        if (disposed)
            return;
        FireEvent(new CEvent.UI.RemoveUI(this.Name));
        this.transform.SetParent(null);
        disposed = true;
        ChildMap.Clear();
        ChildMap = null;
        Childidles.Clear();
        Childidles = null;
        CDMap.Clear();
        CDMap = null;
        context = null;
        ClosedDic.Clear();
        ClosedDic = null;
        //for (int i = 0; i < this.event_handlers.Count; ++i)
        //    this.ui_mgr.obj_mgr.UnregEventHandler(this.event_handlers[i]);
        //event_handlers.Clear();
        OnUIDispose();

        //删除UI对象，必须放最后
        if (this.asset != null)
        {
            //this.asset.Destroy();
            this.asset = null;
        }
    }

    public void FireEvent(IEvent e)
    {
        this.ui_mgr.FireEvent(e);
    }

    public override void Close()
    {
        if (IsShow())
        {
            SetActive(false);
            if (this.Layer == CUILayer.Free) return;

            if (this.ui_mgr.Hold)
                return;
            for (ClosedDic.Begin(); ClosedDic.Next(); )
            {
                CGameUI ui = this.ui_mgr.Get(ClosedDic.Key);
                if (ui)
                    ui.WakeUp();
                else if (ClosedDic.Value)
                    this.ui_mgr.LoadUI(string.Concat("C", ClosedDic.Key, "UI"));
            }
            ClosedDic.Clear();
        }
    }

    public override bool UIShow()
    {
        if (disposed)
            return false;
        SetActive(true);
        return true;
    }

    public override bool IsShow()
    {
        if (disposed)
            return false;
        if (Canvas && Canvas.planeDistance == Def.UIDisableDistance)
            return false;
        return true;
    }



    public int IsGoActive(int linkid, int id)
    {
        NGUILink link = GetLink(linkid);
        if (link)
        {
            GameObject go = link.Get(id);
            if (go)
                return go.activeInHierarchy ? 1 : 0;
        }
        return 0;
    }

    public void SetText(int linkid, int tid, string text)
    {
        NGUILink link = GetLink(linkid);
        if (link)
        {
            CText cText = link.GetComponent<CText>(tid);
            if (cText)
                cText.text = text;
            else if (link.GetComponent<InputField>(tid))
            {
                InputField field = link.GetComponent<InputField>(tid);
                field.text = text;
            }
        }
    }

    public void SetInputField(int linkid, int tid, string text)
    {
        NGUILink link = GetLink(linkid);
        if (link)
        {
            InputField filed = link.GetComponent<InputField>(tid);
            if (filed)
            {
                filed.text = text;
            }
        }
    }

    public string GetText(int linkid, int tid)
    {
        NGUILink link = GetLink(linkid);
        if (link)
        {
            CText cText = link.GetComponent<CText>(tid);
            if (cText)
                return cText.text;
        }
        return "";
    }

    public void SetLocalPos(int linkid, int tid, int x, int y, int z)
    {
        NGUILink link = GetLink(linkid);
        if (link)
        {
            Transform tr = link.GetComponent<Transform>(tid);
            tr.localPosition = new Vector3(x, y, z);
        }
        else
        {
            LOG.Log("error not find link ,linkId is:" + tid);
        }
    }

    //public void CreateSprite(int linkid, int sid, string spname, string spritename)
    //{
    //    NGUILink link = GetLink(linkid);
    //    if (link)
    //        return;

    //    CImage image = link.GetComponent<CImage>(sid);
    //    if (!image)
    //        return;
    //    CreateSprite(image, spname, spritename);
    //}

    //public void CreateRawImage(int linkid, int iid, string filename)
    //{
    //    NGUILink link = GetLink(linkid);
    //    if (link)
    //        return;

    //    CRawImage rawImage = link.GetComponent<CRawImage>(iid);
    //    if (!rawImage)
    //        return;
    //    CreateRawImage(rawImage, filename);
    //}

    //public void CreateFont(int linkid, int tid, string font)
    //{
    //    NGUILink link = GetLink(linkid);
    //    if (link)
    //        return;

    //    CText text = link.GetComponent<CText>(tid);
    //    if (!text)
    //        return;
    //    CreateFont(text, font);
    //}

    //public int CreateEffect(int linkid, int pid, string filename, float x, float y, float z, float scale)
    //{
    //    NGUILink link = GetLink(linkid);

    //    if (!link)
    //        return Def.INVALID_ID;

    //    Transform parent = null;
    //    GameObject go = link.Get(pid);
    //    if (go)
    //        parent = go.transform;

    //    return CreateEffect(filename, parent, new Vector3(x, y, z), Vector3.one * scale, Vector3.zero).HashID;
    //}

    //public int CreateNomalEffect(int linkid, int pid, string filename)
    //{
    //    NGUILink link = GetLink(linkid);

    //    if (!link)
    //        return Def.INVALID_ID;

    //    Transform parent = null;
    //    GameObject go = link.Get(pid);
    //    if (go)
    //        parent = go.transform;

    //    return CreateEffect(filename, parent, Vector3.zero, Vector3.one, Vector3.zero).HashID;
    //}

    public float GetTransformSizeDelta(int linkid, int tid, int axis)
    {
        NGUILink link = GetLink(linkid);
        if (!link)
            return 0f;

        RectTransform rect = link.GetComponent<RectTransform>(tid);
        if (null == rect)
            return 0f;

        return axis == 0 ? rect.sizeDelta.x : rect.sizeDelta.y;
    }

    public float GetTransformLocalPosition(int linkid, int tid, int axis)
    {
        NGUILink link = GetLink(linkid);
        if (!link)
            return 0f;

        var trans = link.GetComponent<Transform>(tid);
        if (null == trans)
            return 0f;

        if (axis == 0)
            return trans.localPosition.x;
        else if (axis == 1)
            return trans.localPosition.y;
        else if (axis == 2)
            return trans.localPosition.z;
        else
            return 0f;
    }

    public int SetTransformLocalPosition(int linkid, int tid, float x, float y, float z)
    {
        NGUILink link = GetLink(linkid);
        if (!link)
            return 0;

        var trans = link.GetComponent<Transform>(tid);
        if (null == trans)
            return 0;

        trans.localPosition = new Vector3(x, y, z);
        return 1;
    }

    //public int CreateSimpleUIObject(int linkid, int pid, int apprid, int[] equips, int[] dress = null)
    //{
    //    NGUILink link = GetLink(linkid);

    //    if (!link)
    //        return Def.INVALID_ID;

    //    Transform parent = null;
    //    GameObject go = link.Get(pid);
    //    if (go)
    //        parent = go.transform;

    //    //ClientEquip[] equipArr = null;
    //    //if (equips != null && equips.Length > 0)
    //    //{
    //    //    equipArr = new ClientEquip[equips.Length];
    //    //    for (int i = 0; i < equips.Length; i++)
    //    //    {
    //    //        equipArr[i] = ClientEquip.Create(equips[i]);
    //    //        i++;
    //    //    }
    //    //}

    //    //ClientDress[] dressArr = null;
    //    //if (dress != null)
    //    //{
    //    //    dressArr = new ClientDress[dress.Length];
    //    //    for (int i = 0; i < dress.Length; i++)
    //    //    {
    //    //        dressArr[i] = ClientDress.Create(dress[i]);
    //    //        i++;
    //    //    }
    //    //}

    //    return CreateSimpleUIObject(apprid, equipArr, parent, dressArr).HashID;
    //}

    public int CreateMirrorUIObject(int apprid, int[] equips, int[] dress = null, float x = 0, float y = 0, float scale = 0.5f)
    {
        //ClientEquip[] equipArr = null;
        //if (equips != null && equips.Length > 0)
        //{
        //    equipArr = new ClientEquip[equips.Length];
        //    for (int i = 0; i < equips.Length; i++)
        //    {
        //        equipArr[i] = ClientEquip.Create(equips[i]);
        //        i++;
        //    }
        //}

        //ClientDress[] dressArr = null;
        //if (dress != null)
        //{
        //    dressArr = new ClientDress[dress.Length];
        //    for (int i = 0; i < dress.Length; i++)
        //    {
        //        dressArr[i] = ClientDress.Create(dress[i]);
        //        i++;
        //    }
        //}

        return 8;
    }

    public void SetEvent(int linkid, int eid, int eventID, System.Action callback)
    {
        NGUILink link = GetLink(linkid);
        if (link)
            link.SetEvent(eid, (EventTriggerType)eventID, (e) => { if (callback != null) callback(); });
        else
        {
            LOG.Log("error error link id not find : link id is  " + linkid);
        }
    }

    public void RegListView(int linkid, int vid, string path)
    {
        NGUILink link = GetLink(linkid);
        if (link)
        {
            CListView listView = link.GetComponent<CListView>(vid);
            if (listView)
                listView.OnActivation((index, item) =>
                {
                    NGUILink itemlink = item.GetComponent<NGUILink>();
                    itemlink.HashID = itemlink.GetHashCode();
                    ListViewIDs[index] = itemlink.HashID;
                    luaNewItem(this.scriptEnv, itemlink.HashID, path);
                },
                OnListViewRefreshItemHandler);
        }
    }

    public void CreateSprite(int linkid, int sid, string spname, string spritename)
    {
        NGUILink link = GetLink(linkid);
        if (link)
            return;

        CImage image = link.GetComponent<CImage>(sid);
        if (!image)
            return;
        CreateSprite(image, spname, spritename);
    }

    private void OnListViewRefreshItemHandler(int index, int dataIndex)
    {
        int lid;
        if (ListViewIDs.TryGetValue(index, out lid))
            luaRefreshItem(this.scriptEnv, lid, dataIndex);
    }
}