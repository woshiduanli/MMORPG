
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 登录窗口UI控制器
/// </summary>
public class UILogOnCtrl : UIWindowViewBase
{
    /// <summary>
    /// 昵称
    /// </summary>
    [SerializeField]
    private UIInput m_InputNickName;

    /// <summary>
    /// 密码
    /// </summary>
    [SerializeField]
    private UIInput m_InputPwd;

    /// <summary>
    /// 提示文字
    /// </summary>
    [SerializeField]
    private UILabel m_LblTip;

    #region OnBtnClick 重写基类OnBtnClick
    /// <summary>
    /// 重写基类OnBtnClick
    /// </summary>
    /// <param name="go"></param>
    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "btnLogOn":
                LogOn();
                break;
            case "btnToReg":
                BtnToReg();
                break;
        }
    }
    #endregion

    /// <summary>
    /// 我要注册按钮点击方法
    /// </summary>
    private void BtnToReg()
    {
        Close();
        //NextOpenWindow = WindowUIType.Reg;
    }

    private void LogOn()
    {
        string nickName = m_InputNickName.value.Trim();
        string pwd = m_InputPwd.value.Trim();

        if (string.IsNullOrEmpty(nickName))
        {
            m_LblTip.text = "请输入昵称";
            return;
        }

        if (string.IsNullOrEmpty(pwd))
        {
            m_LblTip.text = "请输入密码";
            return;
        }

        string oldNickName = PlayerPrefs.GetString(GlobalInit.MMO_NICKNAME);
        string oldPwd = PlayerPrefs.GetString(GlobalInit.MMO_PWD);

        if (oldNickName != nickName || oldPwd != pwd)
        {
            m_LblTip.text = "您输入的昵称或者密码错误";
            return;
        }

        GlobalInit.Instance.CurrRoleNickName = nickName;

        SceneMgr.Instance.LoadToCity();
    }
}