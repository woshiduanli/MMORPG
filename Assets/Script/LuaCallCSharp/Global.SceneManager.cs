
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;
using LitJson;
using XLua;
using UnityEngine.SceneManagement;


public static partial class Global
{

    public static void LoadLevel(string SceneName)
    {
        UIMgr.FireEvent(new CEvent.Scene.LoadLevel(SceneName));
    }

    // ∂¡»°≥°æ∞ÕÍ±œ
    public static void RegLevelWasLoaded(System.Action<string> SceneChange)
    {
        UIMgr.RegEvent<CEvent.Scene.LevelWasLoaded>((a, b) => { if (SceneChange != null) SceneChange(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name); });
    }


}