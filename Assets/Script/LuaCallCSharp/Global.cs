using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using XLua;

[GCOptimize]
[LuaCallCSharp]
public struct MapObjectData
{
    public int RefID;
    public float X;
    public float Y;
    public long SN;
    public float Dir_X;
    public float Dir_Z;
    public int LeftTime;
    public int MasterId;
    public string Owner_Name;
    public int OpenTimes;
}

[GCOptimize]
[LuaCallCSharp]
public struct CClientRoleData
{
    public int RefID;
    public float X;
    public float Y;
    public float SN;
    public float Dir_X;
    public float Dir_Z;
}
/// <summary>
/// lua调用C#函数类
/// </summary>
[LuaCallCSharp]
public static partial class Global
{
    #region lua的UI调用相关
    public static string MaskSp = "newpublic";
    public static string MaskSprite = "heisezhezhao";
    public static CUIManager UIMgr;
    public static NGUILink Link;

    public static void InitData(CUIManager uimgr)
    {
        UIMgr = uimgr;
        Link = uimgr.Link;
    }

    public static void AddEventListener(ushort protoCode, EventDispatcher.OnActionHandler handler, IProto iProto)
    {
        EventDispatcher.Instance.RegProto(protoCode, handler, iProto);
    }



    //public static void LuaActiveShahow(string ui, bool active)
    //{
    //    if (!UIMgr)
    //        return;
    //    CGameUI UI = UIMgr.Get(ui);
    //    if (UI)
    //        UIMgr.ActiveShahow(UI, active);
    //}

    /// <summary>
    /// 检查是否点击到UI上了
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    //public static int LuaIsPointerOverUI(float x, float y)
    //{
    //    if (UIMgr && UIMgr.IsPointerOverUI(new Vector2(x, y)))
    //        return 1;
    //    return 0;
    //}



    //public static void CloseUIEvent(string ui)
    //{
    //    if (!UIMgr)
    //        return;
    //    UIMgr.FireEvent(new CEvent.UI.CloseUI(CString.Concat("C", ui, "UI")));
    //}

   

    //public static void UISimpleClose(string ui)
    //{
    //    if (!UIMgr)
    //        return;
    //    CGameUI UI = UIMgr.Get(ui);
    //    if (UI)
    //        UI.SimpleClose();
    //}

    //public static int HasFullWindow()
    //{
    //    if (!UIMgr)
    //        return 0;
    //    return UIMgr.HasFullWindowUI() ? 1 : 0;
    //}

    //public static int LuaClone(string ui, int linkid)
    //{
    //    if (!UIMgr)
    //        return Def.INVALID_ID;
    //    CGameUI UI = UIMgr.Get(ui);
    //    if (UI)
    //        return (UI as CGameLuaUI).Clone(linkid);
    //    return Def.INVALID_ID;
    //}

    /// <summary>
    /// 设置指定UI的Active
    /// </summary>
    /// <param name="ui"></param>
    /// <param name="active"></param>
    //public static void SetUIActive(string ui, int active)
    //{
    //    if (!UIMgr)
    //        return;
    //    CGameUI UI = UIMgr.Get(ui);
    //    if (UI)
    //        UI.gameObject.SetActive(active == 0 ? false : true);
    //}

    /// <summary>
    /// 设置指定UI里面指定的Element的Active
    /// </summary>
    /// <param name="ui"></param>
    /// <param name="linkid"></param>
    /// <param name="active"></param>
    //public static void SetElementActive(string ui, int linkid, int active)
    //{
    //    if (!UIMgr)
    //        return;

    //    CGameUI UI = UIMgr.Get(ui);
    //    if (UI)
    //        (UI as CGameLuaUI).SetElementActive(linkid, active);
    //}

    /// <summary>
    /// 设置指定UI里面指定Element里面，资源的Active
    /// </summary>
    /// <param name="ui"></param>
    /// <param name="linkid"></param>
    /// <param name="gid"></param>
    /// <param name="active"></param>
   
    //public static void RemoveUIParent(string ui)
    //{
    //    if (!UIMgr)
    //        return;
    //    CGameUI UI = UIMgr.Get(ui);
    //    if (UI)
    //        UI.transform.SetParent(null);
    //}

    //public static int IsUIShow(string ui)
    //{
    //    if (!UIMgr)
    //        return 0;
    //    CGameUI UI = UIMgr.Get(ui);
    //    if (UI)
    //        return UI.IsShow() ? 1 : 0;
    //    return 0;
    //}

    //public static int IsElementActive(string ui, int linkid)
    //{
    //    if (!UIMgr)
    //        return 0;
    //    CGameUI UI = UIMgr.Get(ui);
    //    if (UI)
    //        return (UI as CGameLuaUI).IsElementActive(linkid);
    //    return 0;
    //}

    //public static int IsGoActive(string ui, int linkid, int gid)
    //{
    //    if (!UIMgr)
    //        return 0;
    //    CGameUI UI = UIMgr.Get(ui);
    //    if (UI)
    //        return (UI as CGameLuaUI).IsGoActive(linkid, gid);
    //    return 0;
    //}

    //public static int CreateEffect(string ui, int linkid, int pid, string filename, float x, float y, float z, float scale)
    //{
    //    if (!UIMgr)
    //        return Def.INVALID_ID;
    //    CGameUI UI = UIMgr.Get(ui);
    //    if (UI)
    //        return (UI as CGameLuaUI).CreateEffect(linkid, pid, filename, x, y, z, scale);
    //    return Def.INVALID_ID;
    //}

    //public static int CreateNomalEffect(string ui, int linkid, int pid, string filename)
    //{
    //    if (!UIMgr)
    //        return Def.INVALID_ID;
    //    CGameUI UI = UIMgr.Get(ui) as CGameUI;
    //    if (UI)
    //        return (UI as CGameLuaUI).CreateNomalEffect(linkid, pid, filename);
    //    return Def.INVALID_ID;
    //}

    //public static int CreateSimpleUIObject(string ui, int linkid, int pid, int apprid, int[] equips, int[] dress = null)
    //{
    //    if (!UIMgr)
    //        return Def.INVALID_ID;
    //    CGameUI UI = UIMgr.Get(ui) as CGameUI;
    //    if (UI )
    //        return (UI as CGameLuaUI).CreateSimpleUIObject(linkid, pid, apprid, equips, dress);
    //    return Def.INVALID_ID;

    //}

    //public static int CreateMirrorUIObject(string ui, int apprid, int[] equips, int[] dress, float x, float y, float scale)
    //{
    //    if (!UIMgr)
    //        return Def.INVALID_ID;
    //    CGameUI UI = UIMgr.Get(ui);
    //    if (UI)
    //        return (UI as CGameLuaUI).CreateMirrorUIObject(apprid, equips, dress, x, y, scale);
    //    return Def.INVALID_ID;
    //}

    //public static void DestroyEffect(string ui, int hash)
    //{
    //    if (!UIMgr)
    //        return;
    //    CGameUI UI = UIMgr.Get(ui);
    //    if (!UI || UI.asset == null)
    //        return;
    //    UI.asset.DestroyEffect(hash);
    //}

    //public static void DestroyRole(string ui, int hash)
    //{
    //    if (!UIMgr)
    //        return;
    //    CGameUI UI = UIMgr.Get(ui);
    //    if (!UI || UI.asset == null)
    //        return;
    //    UI.asset.DestroyRole(hash);
    //}

    //public static void SetCText(string ui, int linkid, int tid, string text)
    //{
    //    if (!UIMgr)
    //        return;
    //    CGameUI UI = UIMgr.Get(ui);
    //    if (UI)
    //        (UI as CGameLuaUI).SetText(linkid, tid, text);
    //}

    //public static void CreateSprite(string ui, int linkid, int sid, string spname, string spritename)
    //{
    //    if (!UIMgr)
    //        return;

    //    CGameUI UI = UIMgr.Get(ui);
    //    if (UI)
    //        (UI as CGameLuaUI).CreateSprite(linkid, sid, spname, spritename);
    //}

    //public static void CreateImage(string ui, int linkid, int iid, string filename)
    //{
    //    if (!UIMgr)
    //        return;

    //    CGameUI UI = UIMgr.Get(ui);
    //    if (UI)
    //        (UI as CGameLuaUI).CreateRawImage(linkid, iid, filename);
    //}

    //public static void CreateFont(string ui, int linkid, int tid, string font)
    //{
    //    if (!UIMgr)
    //        return;

    //    CGameUI UI = UIMgr.Get(ui);
    //    if (UI)
    //        (UI as CGameLuaUI).CreateFont(linkid, tid, font);
    //}

    //public static void ShowMessageBox(string title, string msg, int value, System.Action OK, System.Action Cancel)
    //{
    //    if (!UIMgr)
    //        return;
    //    UIMgr.FireEvent(new CEvent.UI.OpenUI("CMessageboxUI", title, msg, (Buttons)value, OK, Cancel));
    //}

    public static void CloseUIByTouch(System.Action action, System.Func<bool> checkFunc)
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            bool drag = Application.isMobilePlatform
                ? EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)
                : EventSystem.current.IsPointerOverGameObject();
            if (drag)
            {
                if (checkFunc() && action != null)
                    action();
            }
            else if (action != null)
                action();
        }
    }

    //public static void CreateLuaReference(string reference,string config,string view)
    //{
    //    if (!UIMgr)
    //        return;
    //    UIMgr.CreateObjectT<LuaReference>(reference, reference, config, view);
    //}

  

    /// <summary>
    /// 返回x或y单个方向的值
    /// </summary>
    /// <param name="ui"></param>
    /// <param name="linkid"></param>
    /// <param name="tid"></param>
    /// <param name="axis">0:x轴，1:y轴</param>
    /// <returns></returns>
    //public static float GetTransformSizeDelta(string ui, int linkid, int tid, int axis)
    //{
    //    if(!UIMgr)
    //        return 0f;

    //    CGameUI UI = UIMgr.Get(ui);
    //    if(!UI)
    //        return 0f;

    //    return (UI as CGameLuaUI).GetTransformSizeDelta(linkid,tid,axis);
    //}

    /// <summary>
    /// 获取单个方向坐标
    /// </summary>
    /// <param name="ui"></param>
    /// <param name="linkid"></param>
    /// <param name="tid"></param>
    /// <param name="axis">0:x轴 1:y轴 2:z轴</param>
    /// <returns></returns>
    //public static float GetTransformLocalPosition(string ui, int linkid, int tid, int axis)
    //{ 
    //    if(!UIMgr)
    //        return 0f;

    //    CGameUI UI = UIMgr.Get(ui);
    //    if(!UI)
    //        return 0f;

    //    return (UI as CGameLuaUI).GetTransformLocalPosition(linkid, tid, axis);
    //}

    //public static int SetTransformLocalPosition(string ui, int linkid, int tid, float x, float y, float z)
    //{
    //    if (!UIMgr)
    //        return 0;

    //    CGameUI UI = UIMgr.Get(ui);
    //    if (!UI)
    //        return 0;

    //    (UI as CGameLuaUI).SetTransformLocalPosition(linkid, tid, x, y, z);
    //    return 1;
    //}

    //public static void RegListView(string ui, int linkid, int vid,string path)
    //{
    //    if (!UIMgr)
    //        return;
    //    CGameUI UI = UIMgr.Get(ui);
    //    if (!UI)
    //        return;
    //    (UI as CGameLuaUI).RegListView(linkid, vid, path);
    //}
    #endregion

    #region 消息
    //public static void Action(string tag, params object[] args)
    //{
    //    Dispatcher.Action(tag, args);
    //}

    //public static void Request(string tag, params object[] args)
    //{
    //    Dispatcher.Request(tag, args);
    //}

    //public static void RequestChat(string tag, params object[] args)
    //{
    //    Dispatcher.RequestChat(tag, args);
    //}

    //public static void Response(string tag, params object[] args)
    //{
    //    Dispatcher.Response(tag, args);
    //}

    //public static void Event(string tag, long id, params object[] args)
    //{
    //    Dispatcher.Event(tag, id, args);
    //}

    //public static void Info(string tag, long id, object info)
    //{
    //    Dispatcher.Info(tag, id, info);
    //}

    //public static void Messasge(string channel, long id, object msg)
    //{
    //    Dispatcher.Messasge(channel, id, msg);
    //}
    //#endregion

    //#region 杂项
    //public static string GetLanguage(string key)
    //{
    //    return Localization.Get(key);
    //}

    //public static string FormatLanguage(string key, params object[] parameters)
    //{
    //    return Localization.Format(key, parameters);
    //}

    //public static void LoadLevel(string scene)
    //{
    //    if (UIMgr)
    //        UIMgr.FireEvent(new CEvent.Scene.LoadLevel(scene));
    //}

    //public static void ReturnLogin()
    //{
    //    if (UIMgr)
    //        UIMgr.FireEvent(new CEvent.GameState.ReturnLogin());
    //}

    //public static void FireSaveLoginDate(string account)
    //{
    //    if (!UIMgr)
    //        return;
    //    UIMgr.FireEvent(new CEvent.Login.SaveLoginDate(account));
    //}

    //public static void CreateEffect(string file, float x, float y, float scale)
    //{
    //    if (!UIMgr)
    //        return;
    //    UIMgr.FireEvent(new CEvent.World.Effect(file, new Vector3(x, 0, y), scale));
    //}

    //public static void CreateParentEffect(string file, long sn, float x, float y, float scale)
    //{
    //    if (!UIMgr)
    //        return;
    //    UIMgr.FireEvent(new CEvent.World.LuaEffect(file, sn, new Vector3(x, 0, y), scale));
    //}

    //public static void CreatePathEffect(string file, float bx, float by, float ex, float ey, float scale, float duration)
    //{
    //    if (!UIMgr)
    //        return;
    //    UIMgr.FireEvent(new CEvent.World.LuaPathEffect(file, new Vector3(bx, 0, by), new Vector3(ex, 0, ey), scale, duration));
    //}

    //public static void RemoveRole(long sn)
    //{
    //    if (!UIMgr || !UIMgr.World)
    //        return;
    //    UIMgr.World.RemoveRole(sn);
    //}

    //public static void RemoveMapObject(long sn)
    //{
    //    if (!UIMgr || !UIMgr.World)
    //        return;
    //    UIMgr.World.RemoveMapObject(sn);
    //}

    //public static void CreateMapObject( int RefID,
    //                                    float X,
    //                                    float Y,
    //                                    long SN,
    //                                    float Dir_X,
    //                                    float Dir_Z,
    //                                    int LeftTime,
    //                                    int MasterId,
    //                                    int OpenTimes,
    //                                    string owner)
    //{
    //    if (!UIMgr||!UIMgr.World)
    //        return;
    //    MapObjectData data = new MapObjectData();
    //    data.RefID = RefID;
    //    data.X = X;
    //    data.Y = Y;
    //    data.SN = SN;
    //    data.Dir_X = Dir_X;
    //    data.Dir_Z = Dir_Z;
    //    data.LeftTime = LeftTime;
    //    data.MasterId = MasterId;
    //    data.OpenTimes = OpenTimes;
    //    data.Owner_Name = owner;
    //    UIMgr.World.OnEnterMapObject(ref data);
    //}

    //public static void OnMapObjectPropChange(long sn,int OpenTimes)
    //{
    //    if (!UIMgr || !UIMgr.World)
    //        return;
    //    CMapObject mo = UIMgr.World.FindMoBySN(sn);
    //    if (mo)
    //        mo.OnPropChange(OpenTimes);
    //}
    #endregion
}
