
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class UIGameServerEnterView : UIWindowViewBase
{
    public Text lblDefaultGameServer;

    public void SetUI(string gameServerName)
    {
        lblDefaultGameServer.text = gameServerName;
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnSelectGameServer":
                UIDispatcher.Instance.Dispatch(ConstDefine.UIGmeServerEnterView_btnSelectGameServer);
                break;
            case "btnEnterGame":

//                Debug.LogError("进入2了");
//                GmeServerEnterViewBtnEnterGame(null);

               UIDispatcher.Instance.Dispatch(ConstDefine.UIGmeServerEnterView_btnEnterGame);
                break;
        }
    }

    private void GmeServerEnterViewBtnEnterGame(string[] param)
    {
        MyDebug.debug("点击了进入游戏");
        //UpdateLastLogOrServer(GlobalInit.Instance.CurAccount, GlobalInit.Instance.CurrSelectGameServer);
//        string str = "http://a769135040.gnway.cc";

        string str = "49.232.165.108";
//        string str = "123.206.56.9";
//        string str = "25.158.185.82";

//        string str = "192.168.0.103";

        //NetWorkSocket.Instance.Connect(GlobalInit.Instance.CurrSelectGameServer.Ip, GlobalInit.Instance.CurrSelectGameServer.Port);
        NetWorkSocket.Instance.Connect(str, 1011);
//        NetWorkSocket.Instance.Connect(str, 1011);


    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        lblDefaultGameServer = null;
    }

    internal void SetGameServerPageUI(List<RetGameServerPageEntity> list)
    {

    }
}