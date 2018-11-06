
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

interface ISystemCtrl
{
    void OpenView(WindowUIType type);
}
/// <summary>
/// UI窗口管理器
/// </summary>
public class UIViewMgr : Singleton<UIViewMgr>
{
    private Dictionary<WindowUIType, ISystemCtrl> m_SystemCtrlDic = new Dictionary<WindowUIType, ISystemCtrl>();

    public UIViewMgr()
    {
        //把窗口注册到 UI窗口管理器

        m_SystemCtrlDic.Add(WindowUIType.LogOn, AccountCtrl.Instance);
        m_SystemCtrlDic.Add(WindowUIType.Reg, AccountCtrl.Instance);
        m_SystemCtrlDic.Add(WindowUIType.GameServerEnter, GameServerCtrl.Instance);
        m_SystemCtrlDic.Add(WindowUIType.GameServerSelect, GameServerCtrl.Instance);
        m_SystemCtrlDic.Add(WindowUIType.RoleInfo, PlayerCtrl.Instance);

        m_SystemCtrlDic.Add(WindowUIType.GameLevelMap, GameLevelCtrl.Instance); //剧情关卡地图
        m_SystemCtrlDic.Add(WindowUIType.GameLevelDetail, GameLevelCtrl.Instance); //剧情关卡详情

        //m_SystemCtrlDic.Add(WindowUIType.WorldMap, WorldMapCtrl.Instance); //世界地图
    }

    /// <summary>
    /// 打开视图
    /// </summary>
    /// <param name="type"></param>
    //public void OpenWindow(WindowUIType type)
    //{
    //    m_SystemCtrlDic[type].OpenView(type);
    //}
    public void OpenWindow(WindowUIType type)
    {
        if (m_SystemCtrlDic.ContainsKey(type))
            m_SystemCtrlDic[type].OpenView(type);
    }
}