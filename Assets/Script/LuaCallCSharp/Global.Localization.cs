
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;
using LitJson;
using XLua;


public static partial class Global
{

    public static string GetLan(string str)
    {
        return Localization.Get(str);
    }

}