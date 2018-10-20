
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;
using LitJson;
using XLua;


public static partial class Global
{

    public static void Connect()
    {
        if ("ER01ZXNIQGFET10" == System.Net.Dns.GetHostName())
        {
            NetWorkSocket.Instance.Connect("192.168.0.101", 1011);
        }
        else
        {
            NetWorkSocket.Instance.Connect("192.168.1.243", 1011);
        }
    }

    public static void RegProto(ushort protoCode, EventDispatcher.OnLuaActionHandler action)
    {
        MyDebug.debug("lua 注册了-----------："+ protoCode);
        //EventDispatcher.Instance.RecServer(protoCode); 
        EventDispatcher.Instance.RegProto(protoCode, action);
    }


    public static void RegProtoLua(System.Action<ushort, byte[]> RecServer)
    {
        MyDebug.debug("注册了--------RegProtoLua----------------------------------------------");
        EventDispatcher.Instance.RecServer(RecServer);
        //EventDispatcher.Instance.RecServer(protoCode, action);
        //EventDispatcher.Instance.RecServer(protoCode); 
        //EventDispatcher.Instance.RegProto(protoCode, action);
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
        MyDebug.debug("c#  发送了协议");
        NetWorkSocket.Instance.SendMsg(dataArr);
    }
}