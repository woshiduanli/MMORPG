using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRoleSceneCtrl : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        if (DelegateDefine.Instance.OnSceneLoadOk != null)
        {
            DelegateDefine.Instance.OnSceneLoadOk();
        }

        // 监听协议
        EventDispatcher.Instance.RegProto<RoleOperation_LogOnGameServerReturnProto>(ProtoCodeDef.RoleOperation_LogOnGameServerReturn, OnLogOnGameServerReturn);
        LogonGameServer(); 
    }

    void LogonGameServer()
    {
        RoleOperation_LogOnGameServerProto to =  new RoleOperation_LogOnGameServerProto();
        to.AccountId = GlobalInit.Instance.CurAccount.Id;

        NetWorkSocket.Instance.SendMsg(to.ToArray()); 
    }

    private void OnLogOnGameServerReturn(RoleOperation_LogOnGameServerReturnProto buffer)
    {
        MyDebug.debug(buffer.RoleList.Count);
        if (buffer.RoleList.Count == 0)
        {
            // 新建角色
            
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
