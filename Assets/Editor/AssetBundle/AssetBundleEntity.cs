
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// AssetBundle实体
/// </summary>
public class AssetBundleEntity
{
    /// <summary>
    /// 用于打包时候选定 唯一Key
    /// </summary>
    public string Key;

    /// <summary>
    /// 名称
    /// </summary>
    public string Name;

    /// <summary>
    /// 标记
    /// </summary>
    public string Tag;

    /// <summary>
    /// 是否文件夹
    /// </summary>
    public bool IsFolder;

    /// <summary>
    /// 是否初始资源
    /// </summary>
    public bool IsFirstData;

    /// <summary>
    /// 是否被选中
    /// </summary>
    public bool IsChecked;

    private List<string> m_PathList = new List<string>();


    //public 
    /// <summary>
    /// 路径集合
    /// </summary>
    public List<string> PathList
    {
        get { return m_PathList; }
    }
}