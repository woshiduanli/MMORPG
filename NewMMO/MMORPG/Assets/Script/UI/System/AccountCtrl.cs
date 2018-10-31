using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class AccountCtrl : SystemCtrlBase<AccountCtrl>, ISystemCtrl
{
    public AccountCtrl()
    {
        this.AddEventListener(ConstDefine.UILogOnView_btnLogOn, ClickLogin);
        this.AddEventListener(ConstDefine.UILogOnView_btnToReg, OpenRegView);
        this.AddEventListener(ConstDefine.UIRegView_btnReg, ClickReg);
        this.AddEventListener(ConstDefine.UIRegView_btnToLogOn, RegViewBtnToLogOnClick);
    }

    // 是否自动登录
    private bool m_IsAutoLogOn;

    // 快速登录
    public void QuickLogOn()
    {
        // 1 如果本地账号没有， 就注册， 否则马上登录

        if (!PlayerPrefs.HasKey(ConstDefine.LogOn_AccountID))
        {
            // 手动登录
            this.OpenView(WindowUIType.Reg);
        }
        else
        {
            m_IsAutoLogOn = true;
            // 自动登录
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Type", 1);
            dic.Add("UserName", PlayerPrefs.GetString(ConstDefine.LogOn_AccountUserName));
            dic.Add("Pwd", PlayerPrefs.GetString(ConstDefine.LogOn_AccountPwd));
            dic.Add("ChannelId", 0);
            NetWorkHttp.Instance.SendData(GlobalInit.Instance.WebAccountUrl + "api/account", OnLogOnCallBack, true, JsonMapper.ToJson(dic));
        }
    }

    public void ClickLogin(string[] str)
    {
        if (m_LogOnView == null)
        {
            MyDebug.debug("这里登录的问题, 我手动改了");
            m_LogOnView = UIViewUtil.Instance.OpenWindow(WindowUIType.LogOn).GetComponent<UILogOnView>();
        }
        if (m_LogOnView.txtUserName.text == null || m_LogOnView.txtUserName.text == "")
        {
            this.ShowMessage("提示", "用户名空");
            return;
        }

        m_IsAutoLogOn = false;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("Type", 1);
        dic.Add("UserName", m_LogOnView.txtUserName.text);
        dic.Add("Pwd", m_LogOnView.txtPwd.text);
        dic.Add("ChannelId", 0);
        NetWorkHttp.Instance.SendData(GlobalInit.Instance.WebAccountUrl + "api/account", OnLogOnCallBack, true, JsonMapper.ToJson(dic));
    }


    public void OpenRegView(string[] str)
    {
        MyDebug.debug("点击了去注册");
        m_LogOnView.CloseAndOpenNext(WindowUIType.Reg);
    }

    public void ClickReg(string[] str)
    {
        MyDebug.debug("点击了注册");
        if (m_RegView.txtUserName.text == null || m_RegView.txtUserName.text == "")
        {
            MyDebug.debug("请输入账户");
            return;
        }

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("Type", 0);

        dic.Add("UserName", m_RegView.txtUserName.text);
        dic.Add("Pwd", m_RegView.txtPwd.text);
        dic.Add("ChannelId", 0);

        NetWorkHttp.Instance.SendData("http://localhost:8080/api/account", OnRegCallBack, true, JsonMapper.ToJson(dic));

    }

    public class RetData
    {
        public int Id;
    }

    private void OnLogOnCallBack(NetWorkHttp.CallBackArgs obj)
    {
        if (obj.HasError)
        {
            MyDebug.debug(" 登录有错");

        }
        else
        {
            JsonData jsondata = JsonMapper.ToObject<JsonData>(obj.Value);
            if (!jsondata.ContainsKey("Value"))
            {
                return;
            }
            RetAccountEntity data = JsonMapper.ToObject<RetAccountEntity>(jsondata["objValue"].ToJson());
            if (!m_IsAutoLogOn)
            {
                // 不是自动登录
                if (data != null)
                {
                    PlayerPrefs.SetInt(ConstDefine.LogOn_AccountID, data.Id);
                    Stat.LogOn(data.Id, m_LogOnView.txtUserName.text);
                }
                PlayerPrefs.SetString(ConstDefine.LogOn_AccountUserName, m_LogOnView.txtUserName.text);
                PlayerPrefs.SetString(ConstDefine.LogOn_AccountPwd, m_LogOnView.txtPwd.text);
                m_LogOnView.CloseAndOpenNext(WindowUIType.GameServerEnter);
            }
            else
            {
                // 是自动登录
                Stat.LogOn(PlayerPrefs.GetInt(ConstDefine.LogOn_AccountID), PlayerPrefs.GetString(ConstDefine.LogOn_AccountUserName));
                GameServerCtrl.Instance.OpenView(WindowUIType.GameServerEnter);
            }

            if (data != null)
            {
                SetCurrentGameServer(data);
                GlobalInit.Instance.CurAccount = data;
            }
        }
    }

    private static void SetCurrentGameServer(RetAccountEntity data)
    {
        RetGameServerEntity curGameServerEntity = new RetGameServerEntity();
        curGameServerEntity.Id = data.LastServerId;
        curGameServerEntity.Name = data.LastServerName;
        curGameServerEntity.Ip = data.LastServerIP;
        curGameServerEntity.Port = data.LastServerPort;

        GlobalInit.Instance.CurrSelectGameServer = curGameServerEntity;
    }

    private void OnRegCallBack(NetWorkHttp.CallBackArgs obj)
    {
        if (obj.HasError)
        {
            MyDebug.debug(" 注册有错");

        }
        else
        {
            MyDebug.debug(obj.Value);
            RetAccountEntity data = JsonMapper.ToObject<RetAccountEntity>(obj.Value);
            if (data != null)
            {
                GlobalInit.Instance.CurAccount = data;
                SetCurrentGameServer(data);
                PlayerPrefs.SetInt(ConstDefine.LogOn_AccountID, data.Id);
                Stat.Reg(data.Id, m_RegView.txtUserName.text);
            }
            else
            {
                MyDebug.debug("-------------------空了");
            }
            //if (data.IsObject && data.ContainsKey("Value"))
            //{
            //    string str = ((JsonData)data["Value"]).ToString();
            //}

            PlayerPrefs.SetString(ConstDefine.LogOn_AccountUserName, m_RegView.txtUserName.text);
            PlayerPrefs.SetString(ConstDefine.LogOn_AccountPwd, m_RegView.txtPwd.text);
            m_RegView.CloseAndOpenNext(WindowUIType.GameServerEnter);
        }

    }

    private void RegOrLoginOK()
    {

    }

    public void RegViewBtnToLogOnClick(string[] str)
    {
        m_RegView.CloseAndOpenNext(WindowUIType.LogOn);

    }

    public void OpenRegView()
    {
        m_RegView = UIViewUtil.Instance.OpenWindow(WindowUIType.Reg).GetComponent<UIRegView>();
        //m_RegView.OnViewClose = (a) =>
        //{
        //    OpenLogOnView();
        //};
    }

    UILogOnView m_LogOnView;
    UIRegView m_RegView;

    public void OpenView(WindowUIType type)
    {
        switch (type)
        {
            case WindowUIType.None:
                break;
            case WindowUIType.LogOn:
                OpenLogOnView();
                break;
            case WindowUIType.Reg:
                OpenRegView();
                break;
            case WindowUIType.RoleInfo:
                break;
            default:
                break;
        }

    }

    public void OpenLogOnView()
    {
        m_LogOnView = UIViewUtil.Instance.OpenWindow(WindowUIType.LogOn).GetComponent<UILogOnView>();
        //m_LogOnView.OnViewClose = (a) =>
        //{
        //    OpenRegView();
        //};
    }

    public override void Dispose()
    {
        base.Dispose();
        RemoveEventListener(ConstDefine.UILogOnView_btnLogOn, ClickLogin);
        RemoveEventListener(ConstDefine.UILogOnView_btnToReg, OpenRegView);
        RemoveEventListener(ConstDefine.UILogOnView_btnLogOn, ClickReg);
        RemoveEventListener(ConstDefine.UILogOnView_btnToReg, RegViewBtnToLogOnClick);
    }


}
