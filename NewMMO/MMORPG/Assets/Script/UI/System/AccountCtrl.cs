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
            MyDebug.debug("点击了我这儿是自动登录");
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
        MyDebug.debug("点击了登录");

        //m_LogOnView =UIViewUtil.Instance.CloseWindow
        if (m_LogOnView == null)
        {
            MyDebug.debug("这里登录有问题, 登录没改"); 
            return; 
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

            if (!m_IsAutoLogOn)
            {
                // 不是自动登录
                JsonData data = JsonMapper.ToObject<JsonData>(obj.Value);
                string str = "";
                if (data.IsObject && data.ContainsKey("Value"))
                {
                    str = ((JsonData)data["Value"]).ToString();
                    PlayerPrefs.SetInt(ConstDefine.LogOn_AccountID, int.Parse(str));
                }
                Stat.LogOn(int.Parse(str), m_LogOnView.txtUserName.text);
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
        }
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


            JsonData data = JsonMapper.ToObject<JsonData>(obj.Value);
            if (data.IsObject && data.ContainsKey("Value"))
            {
                string str = ((JsonData)data["Value"]).ToString();
                PlayerPrefs.SetInt(ConstDefine.LogOn_AccountID, int.Parse(str));
            }

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
