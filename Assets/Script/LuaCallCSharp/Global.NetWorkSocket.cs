
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;
using LitJson;
using XLua;


public static partial class Global
{

    public static void Connect(string ip, int port)
    {
        NetWorkSocket.Instance.Connect(ip, port);
    }

    public static void RegProto(ushort protoCode, EventDispatcher.OnLuaActionHandler action)
    {
        EventDispatcher.Instance.RegProto(protoCode, action);
    }

    public static MMO_MemoryStream CreateMemoryStream(byte[] buffer)
    {
        return new MMO_MemoryStream(buffer);
    }

    public static MMO_MemoryStream CreateMemoryStream()
    {
        return new MMO_MemoryStream();
    }

    public static void SendProto(byte[] dataArr)
    {
        NetWorkSocket.Instance.SendMsg(dataArr);
    }
}