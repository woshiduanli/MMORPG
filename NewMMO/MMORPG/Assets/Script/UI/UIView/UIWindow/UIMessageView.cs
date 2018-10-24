//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-05-11 22:32:41
//备    注：
//===================================================
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UIMessageView : MonoBehaviour
{
    /// <summary>
    /// 标题
    /// </summary>
    [SerializeField]
    private Text lblTitle;

    /// <summary>
    /// 内容
    /// </summary>
    [SerializeField]
    private Text lblMessage;

    /// <summary>
    /// 确定按钮
    /// </summary>
    [SerializeField]
    private Button btnOk;

    /// <summary>
    /// 取消按钮
    /// </summary>
    [SerializeField]
    private Button btnCancel;

    /// <summary>
    /// 确定按钮回调
    /// </summary>
    public Action OnOkClickHandler;

    /// <summary>
    /// 取消按钮回调
    /// </summary>
    public Action OnCancelHandler;

    void Awake()
    {
        EventTriggerListener.Get(btnOk.gameObject).onClick = BtnOkClickCallBack;
        EventTriggerListener.Get(btnCancel.gameObject).onClick = BtnCancelClickCallBack;
    }

    private void BtnOkClickCallBack(GameObject go)
    {
        if (OnOkClickHandler != null) OnOkClickHandler();
        Close();
    }

    private void BtnCancelClickCallBack(GameObject go)
    {
        if (OnCancelHandler != null) OnCancelHandler();
        Close();
    }

    private void Close()
    {
        gameObject.transform.localPosition = new Vector3(0, 5000, 0);
    }

    /// <summary>
    /// 显示窗口
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="message">内容</param>
    /// <param name="type">类型</param>
    /// <param name="okAction">确定回调</param>
    /// <param name="cancelAction">取消回调</param>
    public void Show(string title, string message, MessageViewType type = MessageViewType.Ok, Action okAction = null, Action cancelAction = null)
    {
        gameObject.transform.localPosition = Vector3.zero;
        lblTitle.text = title;
        lblMessage.text = message;

        switch (type)
        {
            case MessageViewType.Ok:
                btnOk.transform.localPosition = Vector3.zero;
                btnCancel.gameObject.SetActive(false);
                break;
            case MessageViewType.OkAndCancel:
                btnOk.transform.localPosition = new Vector3(-70, 0, 0);
                btnCancel.gameObject.SetActive(true);
                break;
        }

        OnOkClickHandler = okAction;
        OnCancelHandler = cancelAction;
    }

    void OnDestroy()
    {
        lblTitle = null;
        lblMessage = null;
        btnOk = null;
        btnCancel = null;
        OnOkClickHandler = null;
        OnCancelHandler = null;
    }
}