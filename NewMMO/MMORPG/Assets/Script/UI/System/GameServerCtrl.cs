using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class GameServerCtrl : SystemCtrlBase<GameServerCtrl>, ISystemCtrl
{

    private UIGameServerEnterView m_GameServerEnterView;

    public GameServerCtrl()
    {
        this.AddEventListener(ConstDefine.UIGmeServerEnterView_btnSelectGameServer, GmeServerEnterViewzBtnSelectGameServer);
        this.AddEventListener(ConstDefine.UIGmeServerEnterView_btnEnterGame, GmeServerEnterViewBtnEnterGame);
        NetWorkSocket.Instance.OnConnectOk = OnConnectOkCallBack;
    }

    private void OnConnectOkCallBack()
    {
        MyDebug.debug("进入游戏连接成功");
        // 更新最后的登录服务器
    }

    public void OpenView(WindowUIType type)
    {
        switch (type)
        {
            case WindowUIType.GameServerEnter:
                OpenGameServerEnterView();
                break;
            case WindowUIType.GameServerSelect:
                OpenGameServerSelectView();
                break;
            default:
                break;
        }
    }

    private void OpenGameServerSelectView()
    {
    }

    private void OpenGameServerEnterView()
    {
        m_GameServerEnterView = UIViewUtil.Instance.OpenWindow(WindowUIType.GameServerEnter).GetComponent<UIGameServerEnterView>();
    }

    private void GmeServerEnterViewBtnEnterGame(string[] param)
    {
        MyDebug.debug("点击了进入游戏");
        NetWorkSocket.Instance.Connect();
    }

    private void GmeServerEnterViewzBtnSelectGameServer(string[] param)
    {
        MyDebug.debug("点击了选择区服功能暂时未开启"); return;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("Type", 0);
        //dic.Add("UserName", PlayerPrefs.GetString(ConstDefine.LogOn_AccountUserName));
        //dic.Add("Pwd", PlayerPrefs.GetString(ConstDefine.LogOn_AccountPwd));
        //dic.Add("ChannelId", 0);
        NetWorkHttp.Instance.SendData(GlobalInit.Instance.WebAccountUrl + "api/gameserver", OnGetGameServerPageCallBack, true, JsonMapper.ToJson(dic));
    }

    private void OnGetGameServerPageCallBack(NetWorkHttp.CallBackArgs obj)
    {
        if (obj.HasError)
        {
            MyDebug.debug(" 登录有错");

        }
        else
        {
            MyDebug.debug("服务端的返回值：" + obj.Value);
        }
    }


    public override void Dispose()
    {
        base.Dispose();
        RemoveEventListener(ConstDefine.UIGmeServerEnterView_btnSelectGameServer, GmeServerEnterViewzBtnSelectGameServer);
        RemoveEventListener(ConstDefine.UIGmeServerEnterView_btnEnterGame, GmeServerEnterViewBtnEnterGame);
    }


}
