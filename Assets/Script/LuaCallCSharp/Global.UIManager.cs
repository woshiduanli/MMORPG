
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;
using LitJson;
using XLua;


public static partial class Global
{

    public static void CreateUIEvent(string ui, params object[] args)
    {
        if (UIMgr == null)
            return;
        UIMgr.FireEvent(new CEvent.UI.OpenUI(CString.Concat("C", ui, "UI"), args));
    }

}