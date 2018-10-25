
using UnityEngine;
using System.Collections;
using System;

public class UIWindowViewBase : UIViewBase
{
    private GameObject _gameObject;
    private Transform _transfrom;
    public new GameObject gameObject
    {
        get
        {
            if (_gameObject == null)
                _gameObject = base.gameObject;
            return _gameObject;
        }
    }
    public new Transform transform
    {
        get
        {
            if (_transfrom == null)
                _transfrom = base.transform;
            return _transfrom;
        }
    }

    /// <summary>
    /// 挂点类型
    /// </summary>
    [SerializeField]
    public WindowUIContainerType containerType = WindowUIContainerType.Center;

    /// <summary>
    /// 打开方式
    /// </summary>
    [SerializeField]
    public WindowShowStyle showStyle = WindowShowStyle.Normal;

    /// <summary>
    /// 打开或关闭动画效果持续时间
    /// </summary>
    [SerializeField]
    public float duration = 0.2f;
    public WindowUIType NextOpenWindow;

    public System.Action OnViewClose;

    public bool m_OpenNext;


    /// <summary>
    /// 视图名称
    /// </summary>
    [SerializeField]
    public string ViewName;

    public WindowUIType CurrentUIType;


    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        if (go.name.Equals("btnClose", StringComparison.CurrentCultureIgnoreCase))
        {
            Close();
        }
    }

    /// <summary>
    /// 关闭窗口
    /// </summary>
    public virtual void Close(bool m_openNext = false)
    {
        this.m_OpenNext = m_openNext; 
        UIViewUtil.Instance.CloseWindow(this.CurrentUIType);
    }

    /// <summary>
    /// 关闭并且打开下一个窗口
    /// </summary>
    /// <param name="nextType"></param>
    public virtual void CloseAndOpenNext(WindowUIType nextType)
    {
        this.Close();
    }

    /// <summary>
    /// 销毁之前执行
    /// </summary>
    protected override void BeforeOnDestroy()
    {
        LayerUIMgr.Instance.CheckOpenWindow();
        if (m_OpenNext)
        {

        if (OnViewClose != null) { OnViewClose(); }
        }
        //if (NextOpenWindow != WindowUIType.None)
        //{
        //    WindowUIMgr.Instance.OpenWindow(NextOpenWindow);
        //}
    }
}