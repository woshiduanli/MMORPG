
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UISelectRoleDeleteRoleView : UIViewBase
{
    /// <summary>
    /// 输入OK的框
    /// </summary>
    [SerializeField]
    private InputField txtOk;

    /// <summary>
    /// 提示信息
    /// </summary>
    [SerializeField]
    private Text lblTip;

    /// <summary>
    /// 移动的目标点
    /// </summary>
    private Vector3 m_MoveTargetPos;

    private Action m_OnBtnOkClick;

    protected override void OnAwake()
    {
        base.OnAwake();
        transform.localPosition = new Vector3(0, 1000, 0);
    }

    protected override void OnStart()
    {
        base.OnStart();
        transform.DOLocalMove(Vector3.zero, 0.2f).SetAutoKill(false).SetEase(GlobalInit.Instance.UIAnimationCurve).Pause();
    }

    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "btnClose":
                Close();
                break;
            case "btnOK":
                OnBtnOkClick();
                break;
        }
    }

    public void Show(string nickName, Action onBtnOkClick)
    {
        m_OnBtnOkClick = onBtnOkClick;

        lblTip.text = string.Format("您确定要删除<color=#ff0000ff>{0}</color>吗？", nickName);
        transform.DOPlayForward();
    }

    /// <summary>
    /// 关闭视图 这里不是销毁窗口 而是移动到上方
    /// </summary>
    public void Close()
    {
        transform.DOPlayBackwards();
        txtOk.text = "";
    }

    /// <summary>
    /// 确定按钮点击
    /// </summary>
    private void OnBtnOkClick()
    {
        if (string.IsNullOrEmpty(txtOk.text) || !txtOk.text.Equals("ok", StringComparison.CurrentCultureIgnoreCase))
        {
            MessageCtrl.Instance.Show("提示", "请输入OK删除角色");
            return;
        }

        if (m_OnBtnOkClick != null)
        {
            m_OnBtnOkClick();
        }
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();

        txtOk = null;
        lblTip = null;
        m_OnBtnOkClick = null;
    }
}