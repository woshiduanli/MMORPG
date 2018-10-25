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

    public void ClickLogin(string[] str)
    {
        MyDebug.debug("点击了登录");
        if (m_LogOnView.txtUserName.text == null || m_LogOnView.txtUserName.text == "")
        {
            this.ShowMessage("提示", "用户名空");
            return;
        }
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("Type", 1);
        dic.Add("UserName", m_LogOnView.txtUserName.text);
        dic.Add("Pwd", m_LogOnView.txtPwd.text);
        dic.Add("ChannelId", 0);
        NetWorkHttp.Instance.SendData("http://localhost:8080/api/account", OnLogOnCallBack, true, JsonMapper.ToJson(dic));
    }


    public void OpenRegView(string[] str)
    {
        MyDebug.debug("点击了去注册");
        m_LogOnView.Close(true);
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

    private void OnLogOnCallBack(NetWorkHttp.CallBackArgs obj)
    {
        if (obj.HasError)
        {
            MyDebug.debug(" 登录有错");

        }
        else
        {
            MyDebug.debug(obj.Value);
            Stat.LogOn(4, m_LogOnView.txtUserName.text);
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
        }

    }

    public void RegViewBtnToLogOnClick(string[] str)
    {
        MyDebug.debug("点击了去登录");
        m_RegView.Close(true);

    }

    public void OpenRegView()
    {
        MyDebug.debug("点击了去注册");
        m_RegView = UIViewUtil.Instance.OpenWindow(WindowUIType.Reg).GetComponent<UIRegView>();
        m_RegView.OnViewClose = () =>
        {
            OpenLogOnView();
        };
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
        m_LogOnView.OnViewClose = () =>
        {
            OpenRegView();
        };
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
