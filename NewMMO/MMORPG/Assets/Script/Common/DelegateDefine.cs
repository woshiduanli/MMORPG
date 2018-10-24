
using UnityEngine;
using System.Collections;
using System;
//using XLua;

/// <summary>
/// 这个类适合做一些通用委托
/// </summary>
public class DelegateDefine : Singleton<DelegateDefine>
{
    /// <summary>
    /// 场景加载完毕
    /// </summary>
    public Action OnSceneLoadOk;

    /// <summary>
    /// 渠道初始化完毕
    /// </summary>
    public Action ChannelInitOK;

    /// <summary>
    /// 窗口显示
    /// </summary>
    //[CSharpCallLua]/
    public delegate void OnViewShow();

    /// <summary>
    /// 窗口隐藏或者
    /// </summary>
    //[CSharpCallLua]
    public delegate void OnViewHide();

    /// <summary>
    /// 点击OK按钮
    /// </summary>
    //[CSharpCallLua]
    public delegate void OnOK();

    /// <summary>
    /// 点击取消按钮
    /// </summary>
    //[CSharpCallLua]
    public delegate void OnCancel();

    /// <summary>
    /// 点击确认选择数量按钮
    /// </summary>
    /// <param name="value"></param>
    //[CSharpCallLua]
    public delegate void OnChooseCount(int value);
}