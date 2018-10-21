
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 窗口UI管理器
/// </summary>
public class WindowUIMgr : Singleton<WindowUIMgr> 
{
    private Dictionary<WindowUIType, UIWindowBase> m_DicWindow = new Dictionary<WindowUIType, UIWindowBase>();

    /// <summary>
    /// 已经打开的窗口数量
    /// </summary>
    public int OpenWindowCount
    {
        get
        {
            return m_DicWindow.Count;
        }
    }

    #region OpenWindow 打开窗口
    /// <summary>
    /// 打开窗口
    /// </summary>
    /// <param name="type">窗口类型</param>
    /// <returns></returns>
    public GameObject OpenWindow(WindowUIType type)
    {
        if (type == WindowUIType.None) return null;

        GameObject obj = null;
        //如果窗口不存在 则
        if (!m_DicWindow.ContainsKey(type))
        {
            //枚举的名称要和预设的名称对应
            obj = ResourcesMgr.Instance.Load(ResourcesMgr.ResourceType.UIWindow, string.Format("pan{0}", type.ToString()), cache: true);
            if (obj == null) return null;
            UIWindowBase windowBase = obj.GetComponent<UIWindowBase>();
            if (windowBase == null) return null;

            m_DicWindow.Add(type, windowBase);

            windowBase.CurrentUIType = type;
            Transform transParent = null;

            switch (windowBase.containerType)
            {
                case WindowUIContainerType.Center:
                    transParent = SceneUIMgr.Instance.CurrentUIScene.Container_Center;
                    break;
            }

            obj.transform.parent = transParent;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            NGUITools.SetActive(obj, false);

            StartShowWindow(windowBase, true);
        }
        else
        {
            obj = m_DicWindow[type].gameObject;
        }
        //层级管理
        LayerUIMgr.Instance.SetLayer(obj);
        return obj;
    }
    #endregion

    #region CloseWindow 关闭窗口
    /// <summary>
    /// 关闭窗口
    /// </summary>
    /// <param name="type"></param>
    public void CloseWindow(WindowUIType type)
    {
        if (m_DicWindow.ContainsKey(type))
        {
            StartShowWindow(m_DicWindow[type], false);
        }
    }
    #endregion

    #region StartShowWindow 开始打开窗口
    /// <summary>
    /// 开始打开窗口
    /// </summary>
    /// <param name="windowBase"></param>
    /// <param name="isOpen">是否打开</param>
    private void StartShowWindow(UIWindowBase windowBase, bool isOpen)
    {
        switch (windowBase.showStyle)
        {
            case WindowShowStyle.Normal:
                ShowNormal(windowBase, isOpen);
                break;
            case WindowShowStyle.CenterToBig:
                ShowCenterToBig(windowBase, isOpen);
                break;
            case WindowShowStyle.FromTop:
                ShowFromDir(windowBase, 0, isOpen);
                break;
            case WindowShowStyle.FromDown:
                ShowFromDir(windowBase, 1, isOpen);
                break;
            case WindowShowStyle.FromLeft:
                ShowFromDir(windowBase, 2, isOpen);
                break;
            case WindowShowStyle.FromRight:
                ShowFromDir(windowBase, 3, isOpen);
                break;
        }
    }
    #endregion

    #region 各种打开效果

    /// <summary>
    /// 正常打开
    /// </summary>
    /// <param name="windowBase"></param>
    /// <param name="isOpen"></param>
    private void ShowNormal(UIWindowBase windowBase, bool isOpen)
    {
        if (isOpen)
        {
            NGUITools.SetActive(windowBase.gameObject, true);
        }
        else
        {
            DestroyWindow(windowBase);
        }
    }

    /// <summary>
    /// 中间变大
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="isOpen"></param>
    private void ShowCenterToBig(UIWindowBase windowBase, bool isOpen)
    {
        TweenScale ts = windowBase.gameObject.GetOrCreatComponent<TweenScale>();
        ts.animationCurve = GlobalInit.Instance.UIAnimationCurve;
        ts.from = Vector3.zero;
        ts.to = Vector3.one;
        ts.duration = windowBase.duration;
        ts.SetOnFinished(() => {
            if (!isOpen)
                DestroyWindow(windowBase);
        });
        NGUITools.SetActive(windowBase.gameObject, true);
        if (!isOpen) ts.Play(isOpen);
    }

    /// <summary>
    /// 从不同的方向加载
    /// </summary>
    /// <param name="windowBase"></param>
    /// <param name="dirType">0=从上 1=从下 2=从左 3=从右</param>
    /// <param name="isOpen"></param>
    private void ShowFromDir(UIWindowBase windowBase, int dirType, bool isOpen)
    {
        TweenPosition tp = windowBase.gameObject.GetOrCreatComponent<TweenPosition>();
        tp.animationCurve = GlobalInit.Instance.UIAnimationCurve;

        Vector3 from = Vector3.zero;
        switch (dirType)
        {
            case 0:
                from = new Vector3(0, 1000, 0);
                break;
            case 1:
                from = new Vector3(0, -1000, 0);
                break;
            case 2:
                from = new Vector3(-1400, 0, 0);
                break;
            case 3:
                from = new Vector3(1400, 0, 0);
                break;
        }

        tp.from = from;
        tp.to = Vector3.one;
        tp.duration = windowBase.duration;
        tp.SetOnFinished(() => {
            if (!isOpen)
                DestroyWindow(windowBase);
        });
        NGUITools.SetActive(windowBase.gameObject, true);
        if (!isOpen) tp.Play(isOpen);
    }

    #endregion

    #region DestroyWindow 销毁窗口
    /// <summary>
    /// 销毁窗口
    /// </summary>
    /// <param name="obj"></param>
    private void DestroyWindow(UIWindowBase windowBase)
    {
        m_DicWindow.Remove(windowBase.CurrentUIType);
        Object.Destroy(windowBase.gameObject);
    }
    #endregion
}