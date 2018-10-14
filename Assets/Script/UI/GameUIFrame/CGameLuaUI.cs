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
    public override void InitLua()
    {
        InitLuaAsset();
    }

    protected void InitLuaAsset()
    {
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

    //public void SetGoActive(int linkid, int id, int active)
    //{
    //    NGUILink link = GetLink(linkid);
    //    if (link)
    //    {
    //        GameObject go = link.Get(id);
    //        if (go)
    //            CClientCommon.SetUIActive(go, active == 1);
    //    }
    //}

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