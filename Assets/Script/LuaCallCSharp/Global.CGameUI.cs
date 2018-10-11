
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;
using LitJson;
using XLua;


public static partial class Global
{

    public static void SetEvent(string ui, int linkid, int eid, int eventID, System.Action callback)
    {
        CGameUI UI = UIMgr.Get(ui);
        if (UI)
            (UI as CGameLuaUI).SetEvent(linkid, eid, eventID, callback);
    }

    public static void SetCText(string ui, int linkid, int tid, string text)
    {
        if (UIMgr == null)
            return;
        CGameUI UI = UIMgr.Get(ui);
        if (UI)
            (UI as CGameLuaUI).SetText(linkid, tid, text);
    }

    public static string GetCText(string ui, int linkid, int tid)
    {
        if (UIMgr == null)
            return "";
        CGameUI UI = UIMgr.Get(ui);
        if (UI)
            return (UI as CGameLuaUI).GetText(linkid, tid);
        return ""; 
    }

    public static void CreateSprite(string ui, int linkid, int sid, string spname, string spritename)
    {
        if (UIMgr == null)
            return;

        CGameUI UI = UIMgr.Get(ui);
        if (UI)
            (UI as CGameLuaUI).CreateSprite(linkid, sid, spname, spritename);
    }

    public static void SetGoActive(string ui, int linkid, int gid, int active)
    {
        if (UIMgr == null)
            return;

        CGameUI UI = UIMgr.Get(ui);
        if (UI)
            (UI as CGameLuaUI).SetGoActive(linkid, gid, active);
    }

    public static void UIClose(string ui)
    {
        if (UIMgr == null)
            return;
        CGameUI UI = UIMgr.Get(ui);
        if (UI)
            UI.Close();
    }

}