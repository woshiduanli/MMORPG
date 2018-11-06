

using System.Collections;

/// <summary>
/// GameLevelRegion实体
/// </summary>
public partial class GameLevelRegionEntity : AbstractEntity
{
    /// <summary>
    /// 游戏关卡Id
    /// </summary>
    public int GameLevelId { get; set; }

    /// <summary>
    /// 区域Id
    /// </summary>
    public int RegionId { get; set; }

    /// <summary>
    /// 初始化精灵
    /// </summary>
    public string InitSprite { get; set; }

}
