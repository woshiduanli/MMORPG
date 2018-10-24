
//using UnityEngine;
//using System.Collections;

///// <summary>
///// 所有窗口UI的基类
///// </summary>
//public class UIWindowBase  
//{
//    /// <summary>
//    /// 挂点类型
//    /// </summary>
//    [SerializeField]
//    public WindowUIContainerType containerType = WindowUIContainerType.Center;

//    /// <summary>
//    /// 打开方式
//    /// </summary>
//    [SerializeField]
//    public WindowShowStyle showStyle = WindowShowStyle.Normal;

//    /// <summary>
//    /// 打开或关闭动画效果持续时间
//    /// </summary>
//    [SerializeField]
//    public float duration = 0.2f;

//    /// <summary>
//    /// 当前窗口类型
//    /// </summary>
//    [HideInInspector]
//    public WindowUIType CurrentUIType;

//    /// <summary>
//    /// 下一个要打开的窗口
//    /// </summary>
//    protected WindowUIType NextOpenWindow = WindowUIType.None;

//    /// <summary>
//    /// 关闭窗口
//    /// </summary>
//    protected virtual void Close()
//    {
//        WindowUIMgr.Instance.CloseWindow(CurrentUIType);
//    }

//    /// <summary>
//    /// 销毁之前执行
//    /// </summary>
//    protected override void BeforeOnDestroy()
//    {
//        LayerUIMgr.Instance.CheckOpenWindow();
//        if (NextOpenWindow == WindowUIType.None) return;
//        WindowUIMgr.Instance.OpenWindow(NextOpenWindow);
//    }
//}