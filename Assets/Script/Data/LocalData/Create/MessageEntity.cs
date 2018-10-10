
using UnityEngine;
using System.Collections;

/// <summary>
/// Message实体
/// </summary>
public partial class MessageEntity : AbstractEntity
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Msg { get; set; }

    /// <summary>
    /// 使用的功能模块
    /// </summary>
    public string Module { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

}
