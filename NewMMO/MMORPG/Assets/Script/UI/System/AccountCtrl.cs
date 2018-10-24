using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountCtrl : Singleton<AccountCtrl>
{
    public AccountCtrl()
    {
        UIDispatcher.Instance.AddEventListener(ConstDefine.UILogOnView_btnLogOn, ClickLogin);
        UIDispatcher.Instance.AddEventListener(ConstDefine.UILogOnView_btnToReg, ToReg);

        UIDispatcher.Instance.AddEventListener(ConstDefine.UIRegView_btnReg, ClickReg);
        UIDispatcher.Instance.AddEventListener(ConstDefine.UIRegView_btnToLogOn, ToLogin);
    }

    public void ClickLogin(string[] str)
    {
        MyDebug.debug("点击了");

    }

    public void ToReg(string[] str)
    {
        MyDebug.debug("点击了去注册");
        m_LogOnView.Close(true);

    }

    public void ClickReg(string[] str)
    {
        MyDebug.debug("点击了注册");
    }

    public void ToLogin(string[] str)
    {
        MyDebug.debug("点击了去登录");
        OpenLogOnView();

    }

    UILogOnView m_LogOnView;
    UIRegView m_RegView;

    public void OpenLogOnView()
    {
        m_LogOnView = WindowUIMgr.Instance.OpenWindow(WindowUIType.LogOn).GetComponent<UILogOnView>();
        m_LogOnView.OnViewClose = () =>
        {
            m_RegView = WindowUIMgr.Instance.OpenWindow(WindowUIType.Reg).GetComponent<UIRegView>();
        };
    }

    public override void Dispose()
    {
        base.Dispose();
        UIDispatcher.Instance.RemoveEventListener(ConstDefine.UILogOnView_btnLogOn, ClickLogin);
        UIDispatcher.Instance.RemoveEventListener(ConstDefine.UILogOnView_btnToReg, ToReg);

        UIDispatcher.Instance.RemoveEventListener(ConstDefine.UILogOnView_btnLogOn, ClickReg);
        UIDispatcher.Instance.RemoveEventListener(ConstDefine.UILogOnView_btnToReg, ToLogin);
    }


}
