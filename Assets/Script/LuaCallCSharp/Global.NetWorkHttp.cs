
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;
using LitJson;
using XLua;


public static partial class Global
{

    public static void SendData(string url, System.Action<string> callBack = null, bool isPost = false, string json = null)
    {
        NetWorkHttp.Instance.SendData(url, callBack, isPost, json);
    }

}