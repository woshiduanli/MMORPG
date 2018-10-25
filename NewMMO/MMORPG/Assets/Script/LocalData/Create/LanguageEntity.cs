
using System.Collections;

/// <summary>
/// Language实体
/// </summary>
public partial class LanguageEntity : AbstractEntity
{
    /// <summary>
    /// 模块
    /// </summary>
    public string Module { get; set; }

    /// <summary>
    /// Key
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Desc { get; set; }

    /// <summary>
    /// 中文
    /// </summary>
    public string CN { get; set; }

    /// <summary>
    /// 英文
    /// </summary>
    public string EN { get; set; }

}
