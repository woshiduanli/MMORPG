using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class GameServerCtrl : SystemCtrlBase<GameServerCtrl>, ISystemCtrl
{

    private UIGameServerEnterView m_GameServerEnterView;
    private UIGameServerSelectView m_GameServerSelectView;

    public GameServerCtrl()
    {
        this.AddEventListener(ConstDefine.UIGmeServerEnterView_btnSelectGameServer, UIGameServerEnterViewzBtnSelectGameServer);
        this.AddEventListener(ConstDefine.UIGmeServerEnterView_btnEnterGame, GmeServerEnterViewBtnEnterGame);

        NetWorkSocket.Instance.OnConnectOk = OnConnectOkCallBack;
    }

    private void OnConnectOkCallBack()
    {
        MyDebug.debug("进入游戏连接成功");
        UpdateLastLogOrServer(GlobalInit.Instance.CurAccount, GlobalInit.Instance.CurrSelectGameServer);

        SceneMgr.Instance.LoadToSelectRole();

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
        m_GameServerEnterView = UIViewUtil.Instance.OpenWindow(WindowUIType.GameServerEnter, () =>
        {
            if (GlobalInit.Instance.CurrSelectGameServer != null)
                m_GameServerEnterView.SetUI(GlobalInit.Instance.CurrSelectGameServer.Name);
            else
            {
                MyDebug.debug("这里是空的");
            }
        }).GetComponent<UIGameServerEnterView>();
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

    private void UIGameServerEnterViewzBtnSelectGameServer(string[] param)
    {
        m_GameServerSelectView = UIViewUtil.Instance.OpenWindow(WindowUIType.GameServerSelect).GetComponent<UIGameServerSelectView>();

        m_GameServerSelectView.OnGameServerClick = OnGameServerClick;

        m_GameServerSelectView.OnShow = () =>
        {
            m_GameServerSelectView.SetSelectGameServerUI(GlobalInit.Instance.CurrSelectGameServer);
            MyDebug.debug("show了吗------");
            GetGameServerPage();
            //GetGameServer();
        };
        m_GameServerSelectView.OnPageClick = (a) =>
        {
            GetGameServer(a);
        };
    }

    void OnGameServerClick(RetGameServerEntity go)
    {
        m_GameServerSelectView.Close();
        GlobalInit.Instance.CurrSelectGameServer = go;
        if (m_GameServerEnterView != null)
        {
            m_GameServerEnterView.SetUI(go.Name);
        }
    }

    private void GetGameServerPage()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("Type", 0);
        dic["ChannelId"] = 0;
        dic["InnerVersion"] = 1001;
        NetWorkHttp.Instance.SendData(GlobalInit.Instance.WebAccountUrl + "api/gameserver", OnGetGameServerPageCallBack, true, JsonMapper.ToJson(dic));
    }

    private Dictionary<int, List<RetGameServerEntity>> m_gameServerDic = new Dictionary<int, List<RetGameServerEntity>>();
    private int m_currentClickPageIndex = 0;
    private bool m_IsBusy = false;

    private void GetGameServer(int pageIndex = 0)
    {
        m_currentClickPageIndex = pageIndex;
        if (m_gameServerDic.ContainsKey(pageIndex))
        {
            m_GameServerSelectView.SetGameServerUI(m_gameServerDic[pageIndex]);
            return;
        }
        if (m_IsBusy) return;
        m_IsBusy = true;
        Dictionary<string, object> dic1 = new Dictionary<string, object>();
        dic1.Add("Type", 1);
        dic1["ChannelId"] = 0;
        dic1["InnerVersion"] = 1001;
        dic1["pageIndex"] = pageIndex;
        NetWorkHttp.Instance.SendData(GlobalInit.Instance.WebAccountUrl + "api/gameserver", OnGetGameServerCallBack, true, JsonMapper.ToJson(dic1));
    }

    private void OnGetGameServerCallBack(NetWorkHttp.CallBackArgs obj)
    {
        if (obj.HasError)
        {
            MyDebug.debug(" -有错");
        }
        else
        {
            MyDebug.debug("服务端的返回值：" + obj.Value);
            List<RetGameServerEntity> list = JsonMapper.ToObject<List<RetGameServerEntity>>(obj.Value);
            if (m_GameServerSelectView != null && list != null)
            {
                m_gameServerDic[m_currentClickPageIndex] = list;
                m_GameServerSelectView.SetGameServerUI(list);
            }

            foreach (var item in list)
            {
                MyDebug.debug(item.Name + "  ---------   " + item.Id);
            }
            m_IsBusy = false;
        }
    }

    private void UpdateLastLogOrServer(RetAccountEntity curAccount, RetGameServerEntity curGameServer)
    {
        Dictionary<string, object> dic1 = new Dictionary<string, object>();
        dic1.Add("Type", 2);
        dic1["userId"] = curAccount.Id;
        dic1["lastServerId"] = curGameServer.Id;
        dic1["lastServerName"] = curGameServer.Name;
        NetWorkHttp.Instance.SendData("http://49.232.165.108:80/api/gameserver", OnUpdateLastLogOrServerCallBack, true, JsonMapper.ToJson(dic1));
    }


    void OnUpdateLastLogOrServerCallBack(NetWorkHttp.CallBackArgs arg)
    {
        if (arg.HasError)
        {
            MyDebug.debug(" -有错");

        }
        else
        {
        }

    }

    /// <summary>
    ///  获取区服页签
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetGameServerPageCallBack(NetWorkHttp.CallBackArgs obj)
    {
        if (obj.HasError)
        {
            MyDebug.debug(" -有错");

        }
        else
        {
            MyDebug.debug("服务端的返回值：" + obj.Value);
            List<RetGameServerPageEntity> list = JsonMapper.ToObject<List<RetGameServerPageEntity>>(obj.Value);

            if (m_GameServerSelectView != null && list != null)
            {
                RetGameServerPageEntity new2 = new RetGameServerPageEntity();
                new2.Name = "推荐区域";
                new2.PageIndex = 0;
                list.Insert(0, new2);
                m_GameServerSelectView.SetGameServerPageUI(list);
                GetGameServer();
            }
        }
    }


    public override void Dispose()
    {
        base.Dispose();
        RemoveEventListener(ConstDefine.UIGmeServerEnterView_btnSelectGameServer, UIGameServerEnterViewzBtnSelectGameServer);
        RemoveEventListener(ConstDefine.UIGmeServerEnterView_btnEnterGame, GmeServerEnterViewBtnEnterGame);
    }


}
