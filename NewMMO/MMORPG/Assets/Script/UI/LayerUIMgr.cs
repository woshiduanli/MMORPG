
using UnityEngine;
using System.Collections;

/// <summary>
/// UI层级管理器
/// </summary>
public class LayerUIMgr : Singleton<LayerUIMgr> 
{
    /// <summary>
    /// UIPanel层级深度
    /// </summary>
    private int m_UIPanelDepth = 50;

    /// <summary>
    /// 重置
    /// </summary>
    public void Reset()
    {
        m_UIPanelDepth = 50;
    }

    /// <summary>
    /// 检查窗口数量 如果没有打开的窗口 重置
    /// </summary>
    public void CheckOpenWindow()
    {
        m_UIPanelDepth--;
        if (WindowUIMgr.Instance.OpenWindowCount == 0)
        {
            Reset();
        }
    }

    /// <summary>
    /// 设置层级
    /// </summary>
    public void SetLayer(GameObject obj)
    {
        m_UIPanelDepth++;

        UIPanel[] panArr = obj.GetComponentsInChildren<UIPanel>();

        if (panArr.Length > 0)
        {
            for (int i = 0; i < panArr.Length; i++)
            {
                panArr[i].depth += m_UIPanelDepth;
            }
        }
    }
}