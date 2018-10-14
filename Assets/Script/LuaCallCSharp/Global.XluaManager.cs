
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;
using LitJson;
using XLua;

public static partial class Global
{

    public static void RegUpdate(System.Action<float, float, float, uint> LuaUpdate)
    {
        UIMgr.GetSingleT<XLuaManager>().LuaUpdate = LuaUpdate;
    }

    public static void RegLevelWasLoaded(System.Action<string> SceneChange)
    {
        UIMgr.RegEvent<CEvent.Scene.LevelWasLoaded>((a, b) => { if (SceneChange != null) SceneChange(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name); });
    }

}