
using UnityEngine;
using System.Collections;

/// <summary>
/// Chapter实体
/// </summary>
public partial class ChapterEntity : AbstractEntity
{
    /// <summary>
    /// 章名称
    /// </summary>
    public string ChapterName { get; set; }

    /// <summary>
    /// 拥有关卡个数
    /// </summary>
    public int GameLevelCount { get; set; }

    /// <summary>
    /// 背景图
    /// </summary>
    public string BG_Pic { get; set; }

    /// <summary>
    /// Uvx
    /// </summary>
    public float Uvx { get; set; }

    /// <summary>
    /// Uvy
    /// </summary>
    public float Uvy { get; set; }

}
