using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
public struct Def
{
    public const int UIDisableDistance = -100000; // 无效ID
    public const int INVALID_ID = -1; // 无效ID
    public const int MAX_MAP_STATE = 30; // 最大地图状态]
    public const int MaxLoginRoleCount = 4;
    public const int SKILL_MAX_PLAYER_SKILL = 7; //玩家最大技能个数
    public const int SKILL_NORMAL_ATTACK = 0; //普通攻击的技能INDEX
    public const int SKILL_XP_ATTACK = 6; //XP攻击的技能INDEX
    public const int SKILL_FINAL_ATTACK = 4; //终结技能(大招)INDEX
    public const int SKILL_MICRO_ATTACK = 5; //微操攻击的技能INDEX
    public const int COMBO_INTERVAL = 800; //分段技能间隔（MS）
    public const int NPC_CHAT_DISTANCE = 3; //NPC对话距离
    public const int SEARCH_DISTANCE = 2; //技能搜索距离
    public const int MAX_BAG_COUNT = 200; //玩家的背包格子
    public const int MAX_PLAYER_QUEST = 50; //玩家能接的最大任务数量
    public const int EQUIP_ID = 201; //装备ID字段区分
    public const int EUDEMON_SEAL_ID = 204; //封印道具
    public const int EUDEMON_EGG_GIFT_ID = 206; //蛋或者礼包
    public const int EUDEMON_EGG_ID = 2060201; //幻兽蛋id
    public const int EUDEMON_GIFT_ID = 2060202; //幻兽蛋礼包
    public const int GEM_ID = 205; //宝石IS字段区分
    public const float AUTO_ATK_DIS = 3; //自动战斗的 找怪范围
    public const int MAX_ENHANCE_LEVEL = 100; //装备最大追加等级
    public const int GEM_MAX_LEVEL = 9; //宝石最大等级
    public const int FAME_HALL_LOWEST_RANK = 2000;
    public const int SECOND_PER_MINUTE = 60;
    public const int SECOND_PER_HOUR = 3600;
    public const int SECOND_PER_DAY = 86400;
    public const int MAXDEALFRIEND = 10; //最大请求列表数量
    public const int MAINMAPID = 1010; //主城id
    public const int PERCENTDENOMINATOR = 10000;
    public const float UI_AUTO_SHOWTIMES = 5;
    public const float GROWTH_FACTOR = 2.0f / 3;
    public const int EUD_SOUL_APPID = 26000;
    public const int MIN_BAG_NUM_AUTO_SELL = 5; //背包小于5自动出售
    public const int PK_BUFF_ID = 400045000; //pkbuffid
    public const float CHARGE_DIS = 5; //冲锋距离
    public const int PkProtect = 60;//PK保护等级差
    public const int ITEM_BASE_DATA = 1000000;//道具基数
    public const string LOGIN_FAILED_WAIT = "login_wait_time";// 平台登陆游戏失败
    public const string ENTER_FAILED_WAIT = "enter_wait_time";// 进入游戏失败
    public const string FIRSET_QQ_FLAG = "qq_first";// QQ第一次进入游戏
    public const string FIRSET_WX_FLAG = "wx_first";// 微信第一次进入游戏
    public const string FIRSET_GUEST_FLAG = "guest_first";// 微信第一次进入游戏
    public const string LOGIN_BEFOR_NOTICE = "1";//登陆前公告
    public const string LOGIN_ARFER_NOTICE = "2";//登陆后公告
    public const string SCROLL_NOTICE = "804";//跑马灯公告
    public const int EUDEMON_SOUL_BASE_ID = 214000; //兽魂道具id前6位
}

public class PowerId
{
    int power1 = 10010;
    int power2 = 10020;
    int power3 = 10030;
    int power4 = 10040;
}

/// <summary>
/// 动态怪物类型
/// </summary>
public enum DynamicMonsterType
{
    Normal = 0,// 普通
    WorldLevel = 1,// 世界等级
    TeamLevel = 2, // 队伍等级
}


/// <summary>
///运营活动类型
/// </summary>
public enum OperateActivityType
{
    Normal = 0,// 普通
    SevenGift = 1//七日登陆活动
}

public enum RichListType
{
    LEVEL = 1,  //等级
    TALIMANS = 2,  //法宝
    PET = 3,      //幻兽
    RECHARGE = 4, //充值
    GEM = 5,    //宝石
    POWER = 6,   //战力
    KING = 7      //冲榜王
}


/// <summary>
/// 安全区类型
/// </summary>
public class SafeAreaType
{
    /// <summary>
    /// 圆型
    /// </summary>
    public const int Circle = 1;
    /// <summary>
    /// 矩形
    /// </summary>
    public const int Rectangle = 2;
    /// <summary>
    /// 多边形
    /// </summary>
    public const int Polygon = 3;
}

/// <summary>
/// 复活类型
/// </summary>
public class ReliveType
{
    /// <summary>
    /// 复活点
    /// </summary>
    public const int FixedPos = 0;
    /// <summary>
    /// 原地
    /// </summary>
    public const int CurrPos = 1;
}
public class BuffRemoveType
{
    public const int Never = 0; //不失效
    public const int TP = 1; //回城后消失
    public const int Dead = 2; //死亡后消失
    public const int Hit = 3; //被击后消失（昏睡
    public const int HitOver = 4; //被击效果完后消失（护盾）
    public const int OffLine = 5; //离线后消失
}

public class BuffType
{
    public const int Speed = 21; //速度
    public const int Dizzy = 28; //眩晕
    public const int Frozen = 29; //冰冻
    public const int Hide = 40; //隐身
    public const int ExpEffect = 41; //经验效果，经验加成
    public const int ClothesExp = 45; //外装经验效果
}

public enum ItemTagType
{
    EUDEMON = 1,
    TRANSMOGRIFICATION = 2,
    EVOLUTION = 3,
    KNIGHTS = 6,
    WAREHOUSE = 5,
    ILLUSTRATEDHANDBOOK = 7,
    EudemonSoul = 8,
}

public class MapType
{
    public const int CITY = 1; //"城市"
    public const int FIELD = 2; //"野外"
    public const int DUNGEON = 3; //"副本"
    public const int WILDBOSS = 4; //"野外boss"
    public const int PRACTICE = 6; //试炼之地
    public const int BOSSHOME = 7; //BOSS之家
    public const int STEELBOSS = 8; //钢翼
}

public class EudemonType
{
    public const int Eudemon = 1; //幻兽
    public const int Minor = 2; //副幻兽
}

public class EquipSuitType
{
    public const int PunishDeity = 1; //诛仙
    public const int PunishGod = 2; //诛神
}

public class DunGeonType
{
    public const int StarStep = 1; //"星级"
    public const int MonsterSoul = 2; //"兽魂"
    public const int ChapterDungeon = 3; //"章节副本"
    public const int PVP = 4; //"pvp战场"
    public const int WorldBoss = 5; //世界boss
    public const int FieldHanGup = 6; //野外挂机
    public const int WorldBossBattle = 7; //世界boss抢夺
    public const int BraveTower = 8; // 勇士之塔
    public const int BattleFieldReady = 9; //战场准备
    public const int BattleField = 10; //战场
    public const int FameHall = 11; //名人堂
    public const int WorldBoss1 = 12; //-世界BOSS
    public const int PersonalBoss = 13; //-个人BOSS
    public const int PrivilegeBoos = 14; //-特权BOSS
    //public const int         15//-英灵殿
    public const int WorldBossReal = 16; //-世界boss真
    public const int PracticeAbyss = 17; //17-深渊迷宫地图组
    public const int PracticeCorridors = 18; //18-冰封走廊地图组
    public const int GroupStation = 19; //军团驻地
    public const int DemonCity = 20; //恶魔塔
    public const int FrozenCellar = 21; //冰封魔窟
    public const int DragonTreasure = 22; //巨龙宝藏
    public const int StormCellar = 23; //材料副本 风暴岩窟
    public const int RoyalMine = 24; //金矿副本
    public const int ThunderForbidden = 25; //雷炎禁地
    public const int BossHome = 26; //BOSS之家副本
    public const int Daily = 27; //日常副本
    public const int ShortTreasure = 28; //矮人宝库
    public const int Battleground = 29; //圣域战场
    public const int SteelBoss = 30; //钢翼
    public const int TerritoryBattle = 31; //领地之争
    public const int PracticeIce = 32; //冰宫冒险区地图组
    public const int GroupHegemony = 33; //军团争霸
    public const int PracticeFrustrate = 34; //失落高地冒险区地图组
    public const int FoeAvenge = 35;//仇人复仇
}

public class MapObjetType
{
    public const int None = 0; //无效果
    public const int Collect = 1; //采集物品
    public const int TP = 2; //传送点
    public const int ItemBox = 3; //宝箱
    public const int AddBuff = 4; //添加BUFF
    public const int Obstacle = 5; //动态阻挡
    public const int Tombstone = 6; //墓碑
    public const int QuestCollect = 7; //任务采集
    public const int PathFinding = 8; //寻路点
    public const int Pyre = 9; //火堆
    public const int GroupBanquet = 10; //军团宴席
    public const int TerritoryStatue = 11;//领地城主雕像
    public const int HegemonyFlag = 12; //军团争霸旗帜
    public const int Survey = 13;   //调查物件
    public const int MageArae = 14; //法术场物件
    public const int EudemonSoulAltar = 15;
}

public class MapObjectTriggerType
{
    public const int CantTrigger = 0; //无触发
    public const int TriggerWhenApproach = 1; //靠近触发
    public const int TriggerWhenClick = 2; //主动触发
}

public class CoinType
{
    public const int Silver = 1; //银币
    public const int Gold = 2; //金币
    public const int Stone = 3; //魔石
    public const int Honor = 4; //荣誉
    public const int ET = 5; //
    public const int Exp = 6; //

    /// <summary>
    /// 没有经验id
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static int GetItemIdByCoinType(int type)
    {
        switch (type)
        {
            case Silver:    //金币
                return SpecialItemId.SilverId;
            case Gold:      //绑魔石
                return SpecialItemId.GoldId;
            case Stone:     //魔石
                return SpecialItemId.HonorId;
            case Honor:
                return SpecialItemId.HonorId;
            case ET:        //恶魔比
                return SpecialItemId.EtId;
        }
        return 0;
    }
}

public class CampType
{
    public const int PlayerCamp = 0; //玩家阵营
    public const int MonsterCamp = 1; //怪物阵营
    public const int AllFriendCamp = 100; //全部友好
    public const int PlayerFriendCamp = 101; //玩家友好
}

public class MonsterType
{
    public const int BOSS = 3; //BOSS,类型1、2的类型要用的时候再加
    public const int WORLDBOSS = 4; //世界BOSS
}

public class PropertyState
{
    // 角色状态
    public const int STATE_MOVE = 0x00000001; // 移动的状态,走,跑
    public const int STATE_JUMP = 0x00000002; // 跳跃状态
    public const int STATE_CAST = 0x00000004; // 施放技能
    public const int STATE_CAST_EX = 0x00000008; // 不能被打断的施放

    public const int STATE_FIGHT = 0x00000010; // 是否在战斗状态中
    public const int STATE_DIE = 0x00000020; // 死亡状态
    public const int STATE_NPC_RETURN = 0x00000040; // NPC离开战斗状态
    public const int STATE_HITBACK = 0x00000080; // 硬直状态

    public const int STATE_HP_SHIELD = 0x00000100; // 有血盾的状态
    public const int STATE_SOUL = 0x00000200; // 武魂变身状态
    public const int STATE_CRAWL = 0x00000400; // 爬行状态
    public const int STATE_FLY = 0x00000800; // 飞行状态

    //public const int RESTRICT_MOVE = 0x00001000;        // 禁止移动状态
    //public const int RESTRICT_SKILL = 0x00002000;       // 禁止使用技能状态
    //public const int RESTRICT_ITEM = 0x00004000;        // 禁止使用道具状态
    //public const int RESTRICT_ATTACK = 0x00008000;      // 禁止攻击状态

    //public const int IMMU_RESTRICT_MOVE = 0x00010000;   // 免疫禁止移动
    //public const int IMMU_RESTRICT_SKILL = 0x00020000;  // 免疫禁止使用技能
    //public const int IMMU_RESTRICT_ITEM = 0x00040000;   // 免疫禁止使用道具
    //public const int IMMU_RESTRICT_ATTACK = 0x00080000; // 免疫禁止攻击
    //public const int IMMU_DAMAGE = 0x00100000;          // 免疫伤害

    public const int STATE_HANG = 0x01000000; // 离线挂机状态
    public const int STATE_PROTECT = 0x02000000; // 保护模式
}

public class PlayerState
{
    public const int STATE_NORMAL = 0; // 普通状态
    public const int STATE_BATTLE = 1; // 战斗
    public const int STATE_GROUP = 2; // 组队
}

public class EquipKind
{
    //1-头盔；2-护肩；3-衣服；4-护腕；5-鞋子；6-武器；7-项链；8-手镯；9-耳环；10-戒指
    public const int EQUIP_PROTECTION_HEAD = 1; //"头盔"
    public const int EQUIP_PROTECTION_SHOULDER = 2; //"肩膀"
    public const int EQUIP_PROTECTION_BODY = 3; // "衣服"
    public const int EQUIP_PROTECTION_FOREARM = 4; //"护腕"
    public const int EQUIP_PROTECTION_CRUS = 5; //"鞋子"
    public const int EQUIP_ARMS_WEAPON = 6; // "武器"
    public const int EQUIP_PROTECTION_NECK = 7; // "项链"
    public const int EQUIP_PROTECTION_BRACELET = 8; //"手镯"
    // TODO: 改名
    public const int EQUIP_PROTECTION_BELT = 9; //"耳环"
    public const int EQUIP_PROTECTION_RING = 10; //"戒指"

    public const int MAX_EQUIP = 10; //最大装备数量
}

public class DiamondSlot
{
    public const int DIAMOND_FIRST = 0;
    public const int DIAMOND_SECOND = 1;
    public const int DIAMOND_THIRD = 2;
    public const int DIAMOND_FOURTH = 3;
    public const int DIAMOND_FIFTH = 4;
}

public class RoleOccupation
{
    public const int Warrior = 1; //战士：10001
    public const int Mage = 2; //法师：10011
    public const int Vampire = 3; //血族 10021
}

public class RoleGender
{
    public const int Male = 1; //男
    public const int FeMale = 2; //女
}

public class EquipMask
{
    public const int Body = 0; //身体
    public const int None = 1; //备用部位
    public const int Weapon = 2; //武器
    public const int Face = 3; //脸
    public const int Hair = 4; //头发
    public const int Wing = 5; //翅膀
}

public class EmojiType
{
    public const string EmojiNormal = "EmojiNormal"; //普通表情
    public const string EmojiWord = "EmojiWord"; // 文字表情
    public const string EmojiHistory = "EmojiHistory"; //输入历史
    public const string EmojiProp = "EmojiProp"; //道具
    public const string EmojiEudemon = "EmojiEudemon"; //幻兽
    public const string EmojiAchieveHelp = "EmojiAchieveHelp"; //任务求助
    public const string EmojiAchievement = "EmojiAchievement"; //成就
    public const string EmojiSocilAction = "EmojiSocilAction"; //社交动作
}

public class StandType
{
    public const int Upright = 1; //直立
    public const int Crawl = 2; //爬行
    public const int Fly = 3; //飞行
}

public class Numerical
{
    public const int EnhanceNum = 10000; //装备强化基数
}

//聊天频道
//public enum ChatChannel
//{
//    SystemChannel,//系统
//    WorldChannel,//世界
//    TeamChannel,//队伍
//    GroupChannel,//军团
//    PrivateChannel,//私聊
//    OrganizeChannel,//组队
//    FriendChannel//好友
//}

public class GameHintType
{
    public const int OtherHint = 0; //其他
    public const int MarqueeHint = 1; //跑马灯
    public const int SrollHint = 2; //滚屏
}

public enum SocialType
{
    Normal = 1,
    Friend = 2,
    Black = 3,
    Foe = 4,
    FriendToGift = 5,
}

public enum BattleResultType
{
    Neutrality = 0,
    Success = 1,
    TimeOutFail = 2,
}

enum EudemonQuality
{
    EudemonQuality_C = 1,
    EudemonQuality_B = 2,
    EudemonQuality_A = 3,
    EudemonQuality_S = 4,
    EudemonQuality_SS = 5,
    EudemonQuality_SSS = 6,
}

/// <summary>
/// 藏宝图效果类型
/// </summary>
enum TreasureType
{
    AddRes = 1, //增加资源 如金币
    AddItem = 2, //增加道具
    CreateMonster = 3, //召怪
    EnterInstance = 4 //进入副本
}

/// <summary>
/// 红点提示类型
/// </summary>
public enum RedDotType
{
    Skill = 0,
    Equip,
    Eudemon,
    EudemonFight,
    EudemonFightButton,
    EudemonFeeding,
    EudemonToggle,
    EudemonToggleEudemonItem,
    EudemonExpItemEnough,
    EudemonAppointToggle,
    EudemonAppointItem,
    EudemonAppointEudemonItem,
    EudemonSoul,
    EudemonSoulEudemon,
    EudemonSoulLattice,
    Mail,
    Package,
    Peek,
    Quest,
    Shop,
    Auction,
    Apeallation,
    Chat,
    Clothes,
    Group,
    GroupBuildingUpgrade,
    GroupBuildingInstanceCanUpgrade,
    GroupHoonShopCanBuy,
    GroupHoonShopItemCanBuy,
    GroupRedPacket,
    GroupApply,
    Mount,
    Compose,
    ComposeItem,
    ComposeGroup,
    ComposeCategory,
    Social,
    SocialNewApply,
    Equip_Enhance,
    Equip_Forge,
    Equip_Gem,
    Equip_Suit,
    Equip_MainUI,
    EudemonCollege_main,
    Team_Apply,
    Activity,
    Welfare,


    Welfare_Grow,
    Welfare_Lottery,
    TerritoryReward,
    FirstRechargeReward,
    QuestionnaireSurvey,
    FateContractRed1,
    FateContractRed2,
    FateContractRed3,
    FateContractRed4,
    FateContractRed5,
    TempleAltar,

    SignIn,
    ResourceRetrieve,

    FateAllOut,

    OperateActivityButton,
    OperateActivityItem,

    RichList_Lev,
    RichList_Tal,
    RichList_Pet,
    RichList_Rec,
    RichList_Gem,
    RichList_Pow,

    TreasureHunt_WareHouse_Btn,
    TreasureHunt_WareHouse_Equip,
    TreasureHunt_WareHouse_Monster,
    TreasureHunt_Main_UI,

    Talismans,
    Tal_List_Prop,
    Tal_List_Appr,
    Tal_Prop_Upgard,
    Tal_Prop_Upstar,
    Tal_Prop_Power,

    Achievement_Main_UI,

    DressToggle,
    AppellationToggle,
    AppellationItem,

    MountToggle,

    PrestigeToken,
    PrestigeCanImprove,
    PrestigeToggle,
    CharacterHead,

    Stall,
    GroupHegemony,

    AllOutRight,
    AllOutTopRight,

}

/// <summary>
/// 帮助按钮类型
/// </summary>
public enum HelpBtnType
{

}
public enum UINameInInt
{
    CDuplicateUI = 102,     //星级副本
    CFrozenCellarUI = 103,  //冰封魔窖
    CFieldLeaderUI = 104,   //世界BOSS
    CDragonTreasureUI = 105,//炎龙宝藏
    CRoyalMineMainUI = 106, //皇家矿洞
    CThunderForbiddenMainUI = 107,//雷炎禁地
    CPracticeLandUI = 108,        //星际怪挑战
}


public class GuideMFArea
{
    public const int Nomal = 0;
    [EditorEnum("打开主界面功能菜单操作区域")]
    public const int BR_System = 1;
    [EditorEnum("打开主界面技能操作区域")]
    public const int BR_Skill = 2;
    public const int BT_Activity = 3;
    public const int L_Quest = 4;
}

public class FreshGuideCondType
{
    [EditorEnum("功能解锁ID")]
    public const int SystemID = 1;
    [EditorEnum("接取任务")]
    public const int AccQuest = 2;
    [EditorEnum("完成提交任务")]
    public const int QuestSubmit = 3;
    [EditorEnum("剧情动画结束")]
    public const int TimeLineEnd = 4;
}

[System.AttributeUsage(System.AttributeTargets.Field)]
public class EditorEnumAttribute : System.Attribute
{
    public EditorEnumAttribute(string name) : this(Def.INVALID_ID, name, String.Empty)
    { }

    public EditorEnumAttribute(int id, string name) : this(id, name, String.Empty)
    { }

    public EditorEnumAttribute(string name, string display) : this(Def.INVALID_ID, name, display)
    { }

    public EditorEnumAttribute(int id, string name, string display)
    {
        this.id = id;
        this.name = name;
        this.display = display;
    }

    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }
    public string Comment
    {
        get
        {
            return name;
        }
    }
    public string Display
    {
        get
        {
            return display;
        }
        set
        {
            display = value;
        }
    }
    public int ID
    {
        get
        {
            return id;
        }
    }

    private int id;
    private string name;
    private string display; //客户端用于显示的自定义字符串
}

public class ETeamFightLevel
{
    [EditorEnum("黑铁")]
    public const int TFL_IRON = 1;
    [EditorEnum("青铜")]
    public const int TFL_BRONZE = 2;
    [EditorEnum("白银")]
    public const int TFL_SILVER = 3;
    [EditorEnum("黄金")]
    public const int TFL_GLOD = 4;
    [EditorEnum("白金")]
    public const int TFL_PLATINUM = 5;
}

public enum PKTYPE
{
    PEACE = 0, //和平（不能攻击玩家）
    KILLING = 1, //自由（不能攻击队友） 全体
    LEGION = 2, //军团（不能攻击团友和队友）强制
    TEAM = 3, //队伍
    CAMP = 4, //阵营（活动模式不能攻击同阵营玩家）
    ALL = 5 //全体（除NPC都能攻击）
}

public class EudemonState
{
    [EditorEnum("召回")]
    public const int BACK_FIGHT = 0;
    [EditorEnum("出战未合体")]
    public const int GO_FIGHT = 1;
    [EditorEnum("合体")]
    public const int FIT = 2;
}

public class PropertyType
{
    public const string NAME = "name";
    public const string LEVEL = "level";
    public const string HP = "hp";
    public const string MaxHP = "health";
    public const string XP = "xp";
    public const string MAXXP = "maxXP";
    public const string EXP = "exp";
    public const string COIN = "coin";
    [Obsolete("此货币已移除")]
    public const string Gold = "gold";
    public const string Ep = "ep";
    public const string BEp = "bep";
    public const string Et = "et";
    public const string Honor = "honor";
    public const string TITLE = "title";
    public const string CAMP = "camp";
    public const string PKMODE = "pk_mode";
    public const string FIGHTVALUE = "battlePower";
    public const string BP = "bp";//其他玩家战斗力
    public const string MAXFIGHTVALUE = "max_battle_power";
    public const string STRENGTH = "strength"; //力量
    public const string INTELLIGENCE = "intelligence"; //智力
    public const string ENDURANCE = "endurance"; //耐力
    public const string VITALITY = "vitality"; //体质
    public const string DEXTERITY = "dexterity"; //敏捷
    public const string PHYSICALATTACK = "physicalAttack"; //物理攻击
    public const string MINPHYSICALATTACK = "minPhysicalAttack"; //物攻min
    public const string MAXPHYSICALATTACK = "maxPhysicalAttack"; //物攻max
    public const string MAGICATTACK = "magicAttack"; //魔法攻击
    public const string MINMAGICATTACK = "minMagicAttack"; //魔攻min
    public const string MAXMAGICATTACK = "maxMagicAttack"; //魔攻max
    public const string ARMOR = "armor"; //物理防御
    public const string RESISTANCE = "resistance"; //魔法防御
    public const string FP = "fp"; //怒气值
    public const string XPRECORVERY = "xpRecovery"; //XP回速
    public const string HITRATING = "hitRating"; //命中值
    public const string DODGERATING = "dodgeRating"; //闪避值
    public const string CRITICALCHANCE = "criticalChance"; //暴击率
    public const string CRITICALRATING = "criticalRating"; //暴击值
    public const string CRITICALDAMAGE = "criticalDamage"; //暴击伤害
    public const string CRITICALDAMAGERATING = "criticalDamageRating"; //暴击伤害值
    public const string CRITICALRESISTANCE = "criticalResistance"; //抗暴击率
    public const string CRITICALRESISTANCERATING = "criticalResistanceRating"; //抗暴击率值
    public const string CRITICALDAMAGEREDUCTION = "criticalDamageReduction"; //抗暴击伤害
    public const string CRITICALDAMAGEREDUCTIONRATING = "criticalDamageReductionRating"; //抗暴击伤害值
    public const string DAMAGEREDUCTION = "damageReduction"; //伤害减免
    public const string PHYSICALDAMAGEREDUCTION = "physicalDamageReduction"; //物理伤害减免
    public const string MAGICDAMAGEREDUCTION = "magicDamageReduction"; //魔法伤害减免
    public const string DAMAGEINCREASE = "damageIncrease"; //伤害增加
    public const string PYSICALDAMEGEINCREASE = "physicalDamageIncrease"; //物理伤害增加
    public const string MAGICDAMAGEINCREASE = "magicDamageIncrease"; //魔法伤害增加
    public const string TRUEDAMAGEINCREASE = "trueDamageIncrease"; //最终伤害增加
    public const string TRUEDAMAGEREDUCTIN = "trueDamageReduction"; //最终伤害减免
    public const string MOVESPEED = "moveSpeed"; //移动速度
    public const string HIT = "hitChance"; //命中率
    public const string DODGE = "dodgeChance"; //闪避率
    public const string BODYSTATE = "body_state"; //xp变身状态
    public const string XPSTATE = "state"; //xp变身状态
    public const string XPDURATION = "duration"; //xp持续时间
    public const string MOUNTING = "mounting_id"; //坐骑id
    public const string APPELLATION = "appellation"; //称号
    public const string Prestige = "prestige_level"; //威望
    public const string Nobility = "nobility"; //爵位
    public const string TEAMID = "team_id"; //当前队伍id
    public const string GROUPID = "group_id"; //军团id
    public const string GROUPNAME = "group_name"; //军团名字
    public const string GROUPMEMBERTYPE = "member_type";
    public const string GROUPSECONDMEMBERTYPE = "second_member_type";
    public const string EudemonInBodyHp = "eudemon_inbody_hp"; //合体当前HP
    public const string EudemonInBodyMaxHp = "eudemon_inbody_health"; //合体最大HP
    public const string EudemonHelth = "health"; //幻兽总血量
    public const string EUDEMONLIMIT = "eudemon_limit";
    public const string OFFLINEHOSTING = "offline_hosting";
    public const string DEVILLAYER = "devil_tower_layer"; //恶魔塔层数（挑战成功的）
    public const string RANK = "rank"; //兽兽进化
    public const string BLOCKPIERCE = "blockPierce"; //格挡穿透率
    public const string BLOCKDAMAGEPENETRATION = "blockDamagePenetration"; //格挡减伤穿透
    public const string BLOCK = "block"; //格挡率
    public const string BLOCKDAMAGE = "blockDamage"; //格挡减伤
    public const string SentGroupMailTimes = "sent_mail_times";
    public const string DonateTimes = "bep_donate_times";
    public const string LastRefreshTimes = "last_refresh_time";
    public const string BASE_SCORE = "base_score";//属性评分
    public const string ACHIEVEMENT = "achievement";//成就
    public const string STALLERR = "stall_err";//交易行种更新玩家是否可以操作需要密码的物品
    public const string TotalRechargeValue = "total_recharge_val";
    public const string TOTAL_RECHARGE = "total_recharge_val";//玩家累冲
    public const string GET_RECHARGE_FLAGS = "open_acc_pay_flags";//玩家累冲领取的礼包列表
    public const string P2P_TRAD_QUOTA = "trade_quota";//P2P，iOS当天交易额度
    public const string RedPackets = "red_packets"; //红包列表
}

#region 活动相关

//活动分类
public class ActivityKind
{
    [EditorEnum("日常活动")]
    public const int COMMON = 0;
    [EditorEnum("运营活动")]
    public const int OPERATE = 1;
    [EditorEnum("奖励活动")]
    public const int AWARD = 2;
    [EditorEnum("目标性活动")]
    public const int TARGET = 3;
    [EditorEnum("帮会活动")]
    public const int GROUP = 4;
    [EditorEnum("独立显示活动")]
    public const int SINGLE = 5;
    [EditorEnum("开服活动")]
    public const int SERVEROPEN = 6;
    [EditorEnum("日常限时活动")]
    public const int COMMONTIMELIMIT = 7;

}

public class ActivityUnlockType
{
    [EditorEnum("日常活动")]
    public const int COMMON = 0;
    /// <summary>
    /// 每日
    /// </summary>
    [EditorEnum("每日")]
    public const int DAY = 1;
    [EditorEnum("每周")]
    public const int WEEK = 2;
    /// <summary>
    /// 开服活动
    /// </summary>
    [EditorEnum("开服活动")]
    public const int OPEN = 3;
}

//活动类型
public class ActivityType
{
    [EditorEnum("无")]
    public const int None = 0;
    [EditorEnum("team_fight")]
    public const int TeamFight = 1;
    //[EditorEnum("world_boss")]
    //public const int WorldBoss = 2;
    [EditorEnum("brave_tower")]
    public const int BraveTower = 3;
    [EditorEnum("legendary_challeng")]
    public const int LegendaryChalleng = 5;
    //[EditorEnum("wild_boss")]
    //public const int WildBoss = 7;

    [EditorEnum("star_step")]
    public const int StarStep = 101;
    [EditorEnum("fieldPractice")]
    public const int FieldPractice = 102;

    [EditorEnum("fame_hall")]
    public const int FameHall = 103;

    [EditorEnum("boss_challenges")]
    public const int BossChallenges = 104;



    [EditorEnum("demon_city")]
    public const int DemonCity = 105;
    [EditorEnum("practice_land")]
    public const int PracticeLand = 106;
    [EditorEnum("god_trial")]
    public const int GodTrial = 2;
    [EditorEnum("quiz")]
    public const int Quiz = 107;
    [EditorEnum("mindquiz")]
    public const int MindQuiz = 119;
    [EditorEnum("frozen_ceallar")]
    public const int FrozenCellar = 108;

    [EditorEnum("dagon_treasure")]
    public const int DragonTreasure = 109;

    [EditorEnum("material_duplication")]
    public const int StormCellar = 110;

    [EditorEnum("daily_quest")]
    public const int DailyQuest = 111;
    [EditorEnum("escort")]
    public const int Escort = 112;
    [EditorEnum("royal_mine")]
    public const int RoyalMine = 113;
    [EditorEnum("thunder_forbidden")]
    public const int ThunderForbidden = 114;
    [EditorEnum("boss_LordFortres")]
    public const int LordFortres = 115;
    [EditorEnum("sy_battle_ground")]
    public const int Battleground = 116;
    [EditorEnum("short_treasure")]
    public const int ShortTreasure = 117;

    [EditorEnum("group_banquet")]
    public const int GroupBanquet = 118;

    [EditorEnum("steel_boss")]
    public const int SteelBoss = 123;
    [EditorEnum("group_hegemony")]
    public const int GroupHegemony = 124;

    [EditorEnum("territory_warfare")]
    public const int TerritoryBattle = 125;


    [EditorEnum("fate_contract")]
    public const int FateContract = 126;

    #region [开服活动]
    [EditorEnum("open_acc_recharge")]
    public const int OpenAccRecharge = 302;
    #endregion

    #region [腾讯平台活动]
    [EditorEnum("tencent_platform")]
    public const int TencentPlatform = 303;
    #endregion
}
public struct TimePeriodConfig
{
    // [FieldDef("[开-月", 50, "TimeMonth")]
    public short OpenMonth;
    // [FieldDef("开-日", 50, "TimeDay")]
    public short OpenDay;
    //[FieldDef("开-周", 50, "TimeWeekDay")]
    public short OpenWeekDay;
    //[FieldDef("开-时", 50, "TimeHour")]
    public short OpenHour;
    // [FieldDef("开-分]", 50, "TimeMinute")]
    public short OpenMinute;
    // [FieldDef("[关-月", 50, "TimeMonth")]
    public short CloseMonth;
    //[FieldDef("关-日", 50, "TimeDay")]
    public short CloseDay;
    // [FieldDef("关-周", 50, "TimeWeekDay")]
    public short CloseWeekDay;
    // [FieldDef("关-时", 50, "TimeHour")]
    public short CloseHour;
    //  [FieldDef("关-分]", 50, "TimeMinute")]
    public short CloseMinute;
}

//活动进入结果
public enum ActvEnterResult
{
    [Describe("进入成功")]
    SUCCESS = 0, [Describe("未到等级")]
    LEVEL_LIMIT = 1, [Describe("等级过高")]
    LEVEL_OVER = 2, [Describe("次数限制")]
    COUNT_LIMIT = 3, [Describe("未开放")]
    TIME_LIMIT = 4, [Describe("人数限制")]
    PLAYER_COUNT_LIMIT = 5, [Describe("没有权限")]
    NO_ACCESS = 6, [Describe("提前结束")]
    PRE_OVER = 7, [Describe("活动结束")]
    FINISHED = 8, [Describe("不在安全区，无法进入")]
    NOT_IN_SAFEZONE = 9, [Describe("该活动系统暂未开启")]
    NOT_OPEN = 10, [Describe("内部错误")]
    INTERNAL_ERROR = 100,
}

/// <summary>
/// 活动失败时，结算界面选择的提升类型
/// </summary>
public enum ActivityFailPromoteType
{
    None = 0,
    Eudemon = 1,
    Equip = 2,
    FightValue = 3,
}

#endregion

#region 网络事件相关
public class EventTag
{
    public const string BootupTime = "bootup_time";
    public const string Login = "login";
    public const string Account = "account:auth";
    public const string ChatLogin = "chat:auth";
    public const string Create = "character:create";
    public const string PlayerDBList = "character:list";
    public const string BackSelectRole = "logout";
    public const string BackLogin = "backLogin";
    public const string EnterGame = "enter";
    public const string EnterWorld = "enterworld";
    public const string EnterWorldSuccess = "enter_success";
    public const string GotoScene = "goto_scene";
    public const string ReviveEvent = "revive";
    public const string InstanceTagetChange = "instance_target";
    public const string LoginOrder = "refresh_login_order";
    public const string loginduplicat = "login_duplicate";
    public const string LoginFinish = "login_finish";
    public const string PlatRelations = "account:plat_relations";
    public const string ReplyPlatRelations = "plat_relations";
    #region 移动同步
    public const string Move = "move";
    public const string StopMove = "stop";
    public const string Postion = "mov";
    public const string EndPostion = "stop";
    public const string TpPos = "fly_to_pos";
    public const string Transposition = "trans"; // 微操技能移动同步协议
    public const string JumpTo = "jump_to"; // 强行设置幻兽位置协议
    public const string AsynPos = "asyn_pos"; //同步位置
    public const string MPJump = "skill:jump";
    public const string RoleJump = "jump";
    public const string FlyShoes = "skill:fly_to_pos_with_shoes";
    #endregion

    #region 技能相关
    public const string fireskill = "cast";

    public const string fireball = "missile";
    //public const string skillresult = "cast_result";
    public const string damage = "dmg";
    public const string skillhit = "be_hit";
    public const string FireBallHit = "cast_done";
    public const string SpellLevelup = "skill:level_up";
    public const string SpellLevelupEvent = "spell_level_up";
    public const string ChangeBody = "skill:change_body";
    public const string CastPetSpell = "skill:cast_pet_spell";
    #endregion

    public const string propschange = "synchro_props";
    public const string EnterPlayer = "in";
    public const string EnterMonster = "mob_in";
    public const string RefreshMonster = "refresh_animal";
    public const string DeleteRole = "out";
    public const string DeleteDrop = "dp_out";
    public const string BagSell = "bag:sell";
    public const string BagMultiSell = "bag:multi_sell";
    public const string BagUseDevil = "bag:use_devil";
    public const string BagLost = "bag:lost";
    public const string BagGain = "bag:gain";
    public const string BagStore = "bag:store";
    public const string BagClear = "bag:clear";
    public const string BagUse = "bag:use";
    public const string BagCompose = "bag:compose";
    public const string PropChange = "prop_changed";
    public const string HpChange = "pt_hp";
    public const string Equip = "equipments:equip";
    public const string UnEquip = "equipments:unequip";
    public const string EnhanceEquip = "equipments:enhance";
    public const string EquipFuse = "equipments:fuse";
    public const string EquipFused = "equipments:fused";
    public const string EquipComposed = "equipments:composed";
    public const string EquipCompose = "equipments:compose";
    public const string GemEmbed = "equipments:embed";
    public const string GemUnEmbed = "equipments:unembed";
    public const string GemUp = "equipments:gemup";
    public const string EquipmentRefine = "equipments:refine";
    public const string EquipmentRefineSave = "equipments:refined";
    public const string EquipmentSuitForge = "equipments:make_set";
    public const string EquipmentSuitForgeSet = "equipments:set";
    public const string AppearTalismans = "appear_talismans";// 法宝技能释放时的模型以及效果

    public const string AddFriend = "relation:add_friend_apply";
    public const string RecAddFriend = "relation:recv_add_friend_apply";
    public const string DealFriend = "relation:dealwith_friend_apply";
    public const string FindFriend = "relation:get_role_info";
    public const string AddBlack = "relation:add_black_apply";
    public const string DelFriend = "relation:delete_friend_apply";
    public const string DelBlack = "relation:delete_black_apply";
    public const string FriendGift = "relation:use_friendly_value_item";
    public const string RecFriendGift = "relation:add_friendly_value";
    public const string RecRecommendFriends = "relation:friends_by_random";
    public const string FriendGroup = "relation:into_group";
    public const string ChangeGroup = "relation:change_group";
    public const string RecFriendsOnlineStatus = "relation:status_of_friends";
    public const string FriendNameChanged = "relation:friend_name_changed";
    public const string RemoveFriendGroup = "relation:remove_group";
    public const string FriendLevelChanged = "friend_level_changed";
    public const string FriendApplyFailed = "relation:dealwith_friend_apply_failed";
    public const string FindFriendsFailed = "relation:get_role_info_faied";
    public const string FoeUpdate = "relation:foe_update";
    public const string FoeRelease = "relation:foe_release";

    public const string GainEudemon = "eudemons:gain";
    public const string ComposeEudemon = "eudemons:compose";
    public const string LostEudemon = "eudemons:lost";
    public const string FormatEudemon = "eudemons:formation";
    public const string FitEudemon = "eudemons:in_body";
    public const string UnFitEudemon = "eudemons:out_body";
    public const string RecieveFitEudemon = "eudemons:eudemons_to_body";
    public const string FormatEudemonChanged = "eudemons:formation_changed";
    public const string EvolveEudemon = "eudemons:evolve";
    public const string Eudemonevolved = "eudemons:evolved";
    public const string EudemonLevelUp = "eudemons:levelup";
    public const string EudemonRelive = "eudemons:relive";
    public const string EudemonRankUp = "eudemons:rankup";
    public const string EudemonMyRanks = "eudemons:ranks";
    public const string EudemonRanking = "eudemons:ladder";
    public const string EudemonRankUpdata = "eudemons:ranking";
    public const string EudemonPeek = "eudemons:peek";
    public const string EudemonPeekResult = "eudemons:peek_result";
    public const string EudemonDismiss = "eudemons:dismiss";
    public const string EudemonDismissed = "eudemons:dismissed";
    public const string EudemonHpChange = "pt_ehp";
    public const string EudemonRename = "eudemons:rename";
    public const string EudemonTrans = "eudemons:transfer"; //仓库与幻兽列表操作
    public const string EudemonSeal = "eudemons:seal"; //封印
    public const string EudemonRerace = "eudemons:rerace"; //相性变化
    public const string EudemonInherit = "eudemons:inherit"; //幻兽传承
    public const string EudemonKnights = "eudemons:knight"; //幻兽骑士
    public const string OpenThirdEudemon = "eudemons:open_third";
    public const string EudemonComposeSN = "eudemons:compose_sn"; //幻兽身份牌幻化
    public const string EudemonTalent = "eudemons:talent"; //幻兽天赋
    public const string EudemonSpell = "eudemons:spell"; //幻兽技能
    public const string EudemonOwned = "eudemons:owned"; //幻兽拥有，包括曾经拥有
    public const string EudemonRedeem = "eudemons:redeem"; //幻兽图鉴兑换
    public const string RecieveOpenThirdEudemon = "eudemons:eudemon_limit_changed";
    public const string EudemonCollege = "college:list";
    public const string EudemonCollegeGeted = "college";
    public const string EudemonCollegeChanged = "college:changed";
    public const string EudemonCollegeReward = "college:claim";
    public const string EudemonCollegeRewardGeted = "college:claimed";
    public const string UpdateDeadPetNum = "update_dead_pet_num"; //更新玩家死亡幻兽数量
    public const string EudemonGiftChoose = "bag:choose"; //幻兽礼包选择
    public const string LadderEudemonData = "eudemons:peek_in_rank";//排行榜幻兽信息
    public const string EudemonSoulEmbed = "eudemon_soul:embed";
    public const string EudemonSoulDigout = "eudemon_soul:digout";
    public const string RecieveEudemonSoulsChange = "eudemon:souls";
    public const string EudemonSoulBuild = "eudemon_soul:build";
    public const string SoulBuildResult = "soul_build_result";
    public const string EudemonSoulSplit = "eudemon_soul:split";
    public const string SoulSplitResult = "soul_split_result";

    public const string GroupCreate = "rolegroup:create_group";
    public const string GroupCreateSucess = "rolegroup:create_group_sucess";
    public const string GroupCreateFail = "rolegroup:create_group_fail";
    public const string GroupAddToGroup = "rolegroup:add_to_group";
    public const string GroupListRequest = "groups:group_list_to_client";
    public const string GroupApply = "rolegroup:join_request";
    public const string GroupInfoRequest = "group:group_info";
    public const string GroupInvite = "group:invite_to";
    public const string GroupInviteFrom = "group:invite_from";
    public const string GroupInviteFailed = "group:invite_failed";
    public const string GroupInviteAgree = "rolegroup:join_request";
    public const string GroupInviteRefuse = "";
    public const string GroupQuit = "group:remove_member";
    public const string GroupAgree = "group:handle_join_request";
    public const string GroupMemberListRequest = "group:member_list";
    public const string GroupApplyListRequest = "group:join_request_list";
    public const string GroupDeclaration = "group:modify_declaration";
    public const string GroupNotice = "group:modify_notice";
    public const string GroupChangeName = "rolegroup:modify_groupname";
    public const string GroupChangeQQ = "group:modify_qq_group";
    public const string GroupChangeNameSucess = "group:modify_group_name_success";
    public const string GroupChangeNameFail = "rolegroup:modify_group_name_fail";
    public const string GroupMail = "rolegroup:send_mail";
    public const string GroupSendMailFail = "rolegroup:send_mail_fail";
    public const string GroupMailSent = "group_times:sent_mail_times";
    public const string GroupMemberAdd = "group:add_member";
    public const string GroupMemberUpdate = "";
    public const string GroupMemberRemove = "rolegroup:remove_from_group";
    public const string GroupAppoint = "group:handle_member_type";
    public const string GroupAppointChief = "group:handle_leader";
    public const string GroupTransfer = "group:handle_transfer";
    public const string GroupRecycleChief = "group:recycle_chief";
    public const string GroupAppointLeader = "group:handle_chief";
    public const string GroupAutoJoin = "group:make_auto_join";
    public const string GroupMemberNameChanged = "group:member_name_changed";
    public const string GroupCaptionNameChanged = "groups:caption_name_changed";
    public const string GroupStationTP = "group:enter_home";
    public const string GroupDonateLogs = "group:donate_logs";
    public const string GroupDonate = "group:donate";
    public const string GroupTradeGoodsBox = "group:buy_gift";
    public const string GroupBuildingUpgrade = "group:upgrade";
    public const string GroupPropChanged = "group:prop_changed";
    public const string GroupCurrencyChanged = "group:currency_changed";
    public const string GroupHoonShopGoodsListRequest = "group:shop_list";
    public const string GroupHoonShopGoodsList = "group:shop";
    public const string GroupHoonShopBuyGood = "group:buy";
    public const string GroupRedPacketsList = "group:red_packets";
    public const string GroupSendRedPacket = "rolegroup:send_packet";
    public const string GroupSendTempRedPacket = "rolegroup:send_template_packet";
    public const string GroupGetNewRedPacket = "group:new_packet";
    public const string GroupGainPacket = "group:gain_packet";
    public const string OpenGroupRedPacket = "group:open_packet";
    public const string ViewGroupRedPacket = "group:view_packet";
    public const string GroupRedPacketInfo = "group:red_packet";
    public const string GetGroupBaseInfo = "groups:get_base_info";
    public const string SaveJoinSetting = "group:save_join_setting";
    public const string JoinSettingChanged = "group:join_setting_change";
    public const string OweCurrencyChange = "group:owe_currency_change";
    public const string GroupBuildingChange = "group:building_change";
    public const string GetGroupBuildingData = "group:get_building_data";
    public const string GetSelfEquipScoreData = "equipments:peek_self_score";

    #region 图腾相关
    public const string UpgradeTotemEnhance = "group:upgrade_totem_enhance";
    public const string UnlockTotem = "rolegroup:unlock_totem";
    public const string MountTotemEudemon = "rolegroup:mount_totem_eudemon";
    public const string MountTotem = "eudemons:mount_totem";
    public const string UnmountTotemEudemon = "rolegroup:unmount_totem_eudemon";
    public const string GroupTotemUnlock = "group:unlock_totem";
    #endregion

    #region 军团宴会
    public const string GroupBanquetPlayerEnter = "group_party:player_enter";
    public const string GroupBanquetTreatDine = "group_party:treat_dine";
    public const string GroupBanquetEatDine = "group_party:eat_dine";
    public const string GroupBanquetQuizAnswer = "group_party:quiz_answer";
    public const string GroupBanquetRollDice = "group_party:roll_begin";
    public const string GroupBanquetAvatarInfo = "group_party:avatar_info";
    public const string GroupBanquetQuizStart = "group_party:quiz_start";
    public const string GroupBanquetQuizRESP = "group_party:quiz_answer_resp";
    public const string GroupBanquetQuizNext = "group_party:quiz_next";
    public const string GroupBanquetQuizEnd = "group_party:quiz_end";
    public const string GroupBanquetQuizInfo = "group_party:quiz_info";
    public const string GroupBanquetQuizRanking = "group_party:quiz_rank";
    public const string GroupBanquetQuizPoint = "group_party:group_quiz_point";
    public const string GroupBnaquetRollRESP = "group_party:roll_resp";
    public const string GroupBanquetReadyInfo = "group_party:ready_info";
    public const string GroupBanquetFirstAnswer = "group_party:quiz_answer_first";
    public const string GroupBanquetRollEnd = "group_party:roll_end";
    #endregion

    #region 军团领地

    public const string TerritoryAskEnter = "territory_warfare:player_enter"; //请求进入某个领地:
    public const string TerritoryResponEnter = "territory_warfare:player_enter_failed"; //回应进入失败
    public const string TerritoryAskLeave = "territory_warfare:player_leave"; //请求离开领地战地图
    public const string TerritoryAskInfo = "territory_warfare:get_info"; //请求领地战数据
    public const string TerritoryResInfo = "territory_warfare:get_info"; //回应领地战数据
    public const string TerritoryPointRank = "territory_warfare:territory_point_rank"; //事件: 单个领地积分排行
    public const string TerritoryPersonKill = "territory_warfare:self_kill_cnt"; //事件: 个人击杀数据更新
    public const string TerritoryHurtRank = "territory_warfare:territory_hurt_rank"; //事件: 单个领地伤害排行
    public const string TerritoryAskGroupPointRank = "territory_warfare:group_point_rank"; //请求：军团积分排行榜
    public const string TerritoryResGroupPointRank = "territory_warfare:group_point_rank"; //回复：军团积分排行榜
    public const string TerritoryAskAvatarRank = "territory_warfare:avatar_rank"; //请求：个人击杀排行榜
    public const string TerritoryResAvatarRank = "territory_warfare:avatar_rank"; //回复：个人击杀排行榜
    public const string TerritoryAskAssignInfo = "territory_warfare:territory_assign_info"; //请求：领地分配信息
    public const string TerritoryResAssignInfo = "territory_warfare:territory_assign_info"; //回复：领地分配信息
    public const string TerritoryEnd = "territory_warfare:warfare_result"; //领地战结束
    public const string TerritoryRecvWages = "territory_warfare:recv_wages"; //请求领取工资？
    public const string TerritoryRecvWagesOk = "territory_warfare:recv_wages"; //领取成功
    public const string TerritoryRecvWagesFaile = "territory_warfare:recv_wages_failed"; //领取失败
    public const string TerritoryBossBlood = "territory_warfare:territory_change";//据点血条刷新
    public const string TerritoryResultTimeEvent = "group_station:exp_welfare";//活动结果按钮

    #endregion

    public const string TitleLevelUp = "titles:levelup";

    public const string GemGain = "gems:gain";
    public const string GemLost = "gems:lost";
    public const string GemStore = "gems:store";
    public const string ReviveRequest = "skill:revive";

    public const string Drops = "drops";
    public const string ChangeLine = "scene_line:change_line";
    public const string ReqLines = "scene_line:get_scene_lines";
    public const string RepLines = "scene_lines";
    public const string TitleChanged = "title_changed";
    public const string GainExp = "gain_exp";
    public const string KillExp = "kill_exp";
    public const string Death = "be_killed";
    public const string AIMoveToPoint = "ai_move_to_point";

    public const string PopoverMonsterPush = "monster_say_on_head";

    #region 军团争霸
    public const string GroupBattlePlayerEnter = "group_battle:player_enter";
    public const string GroupBattleGetInfo = "group_battle:get_info";
    public const string GroupBattleEnd = "group_battle:battle_end";
    public const string GroupBattleInfo = "group_battle:battle_info";
    public const string GroupBattleChange = "group_battle:battle_change";
    public const string GroupBattleRecvWages = "group_battle:recv_wages";
    public const string GroupBattleRecvWagesFailed = "group_battle:recv_wages_failed";
    public const string GroupBattleAssignSweepReward = "group_battle:assign_sweep_reward";
    public const string GroupBattleAssignSweepRewardFailed = "group_battle:assign_sweep_reward_failed";
    #endregion

    #region 任务
    public const string QuestAccept = "task_msg:accept";
    public const string QuestSubmit = "task_msg:submit";
    public const string QuestCancel = "task_msg:give_up";
    public const string Questdialogue = "task_msg:dialogue_over";
    public const string DialyAccept = "task_msg:accept_daily";

    public const string QuestUpdate = "update_task";
    public const string QuestDeleteAccepted = "del_accepted";
    public const string QuestAddSubmitted = "add_submitted";
    public const string QuestAddAcceptable = "add_acceptable";
    public const string DialyDataUpdate = "task_daily_data";
    public const string DialyExtraAward = "task_daily_awardex";

    public const string EscortFail = "task_escort_failed";
    public const string EscortAccept = "task_msg:accept_escort";
    public const string EscortUpdate = "task_escort_data";
    public const string EscortExtraAward = "task_escort_awards";

    public const string TriggerQuest = "task_msg:trigger";
    public const string ArrivePoint = "quest:arrive_point";
    public const string PlayPlot = "play_plot";
    public const string PlayerTrigger = "skill:trigger_object";
    public const string BattleResult = "instance_result";
    public const string ExitDuplicate = "exit_instance";

    public const string Hint = "hint"; //公用提示，两参数 第一个类型，第二个提示ID;
    public const string HintWithArgs = "hint_with_args"; //带多个替换元素的消息;
    #endregion

    #region 时装坐骑
    public const string ClothesExpire = "dresses:expired";
    public const string MountExpire = "mounts:expired";
    public const string ConfirmClothesExpire = "dresses:confirm";
    public const string ConfirmMountExpire = "mounts:confirm";

    public const string SetMount = "skill:set_mount"; //上下马
    public const string UnEquipMount = "mounts:unmount";
    public const string EquipMount = "mounts:mount"; //装备坐骑
    public const string DischargeClothes = "dresses:undress";
    public const string Wear = "dresses:wear";
    public const string ShowDressBp = "dresses:dress_bp";
    public const string ClothesRaise = "dresses:raise_stars"; //升星
    public const string ClothesActive = "dresses:active";
    public const string MountActive = "mounts:active";
    public const string MountUseProps = "mounts:use_attr"; //使用属性
    //public const string MountRenewal = "mounts:renewal";//续期
    public const string ExteriorChanged = "exterior_changed";
    public const string DressGain = "dress:gain";
    public const string MountGain = "mount:gain";
    public const string ShapeGain = "shape:gain";
    public const string MountCheck = "mounts:check";
    #endregion
    public const string AddSceneEffect = "add_scene_effect";
    public const string AddBuff = "buff";
    public const string UpdateBuff = "update_buff";
    public const string RemoveBuff = "del_buff";

    #region 摆摊
    public const string StallsSubmit = "stalls:submit";
    public const string StallsSubmitting = "stalls:submitting";
    public const string StallsSubmited = "stalls:submitted";

    public const string StallsCancel = "stalls:cancel";
    public const string StallsCancelled = "stalls:cancelled";

    public const string StallsRenew = "stalls:renew";
    public const string StallsRenewed = "stalls:renewed";

    public const string StallsRefresh = "stalls:refresh";
    public const string StallsRefreshed = "stalls:refreshed";

    public const string Stalls = "stalls";
    public const string StallsList = "stalls:list";

    public const string StallsBuy = "stalls:buy";
    public const string StallsSold = "stalls:sold";

    public const string StallsUpdate = "stalls:update";
    public const string StallsClaim = "stalls:claim";
    public const string StallsClaimed = "stalls:claimed";
    public const string StallsExpired = "stalls:expired";
    public const string StallsPrice = "stalls:price"; //查询同类商品
    #endregion

    #region 拍卖
    public const string AuctionsListRqst = "auctions:list"; //1、获取正在拍卖的物品列表
    public const string AuctionsListRsp = "auctions";
    public const string AuctionNewPrice = "auctions:refresh"; //2、获取单个物品最新价格
    public const string AuctionAddPrice = "auctions:bid"; //3、竞拍
    public const string AuctionNewBidder = "bid:fail"; //4、竞拍失败
    public const string AuctionRecords = "auctions:records"; //5、拍出记录
    public const string AuctionsMineRsp = "auctions:my_bids"; //我的竞拍

    #endregion

    #region VIP
    public const string VIPChange = "vip:changed";
    public const string BuyVIPGift = "vip:buy_gift";
    public const string ActiveVip = "vip:active";
    public const string RenewVip = "vip:renewal";
    public const string VIPEnd = "vip:expired";
    public const string VIPConsume = "vip:gain_exp";
    public const string VIPBuyTimes = "vip:purchase";
    /// <summary>
    /// VIP最大等级
    /// </summary>
    public const int VIPMaxLevel = 9;
    /// <summary>
    /// 体验VIP等级
    /// </summary>
    public const int VIPTasteLevel = -1;
    #endregion

    #region 商城
    public const string ShopList = "shop:list";
    public const string quote = "goods_quote";
    public const string ShopGoods = "shop_goods";
    public const string ShopBuy = "shop:buy";
    public const string DemonShopGoodsBuy = "devil_shop:buy";
    public const string RepDemonShopGoodsBuy = "devil_shop:buy";

    #endregion

    #region 签到
    public const string CheckInClaim = "checkin:claim"; //签到
    public const string CheckInChanged = "checkin:changed"; //变化时间
    public const string CheckinClaimPeriod = "checkin:claim_period"; //连续签到奖励
    #endregion

    #region 世界等级
    public const string WorldLevelRqst = "realm:world_level";
    public const string WorldLevel = "world_level";
    #endregion

    #region 排行榜
    public const string Ladders = "ladders";
    public const string LaddersGet = "ladders:get";
    public const string Peek = "peek";
    public const string PeekResult = "peek_result";
    public const string SelfLaddersGet = "ladders:get_with_self";
    public const string SelfLadders = "ladders_with_self";
    //}
    public const string LikeInfo = "ladders:like_info";
    public const string LikeInfoReceive = "ladders_like_info";
    public const string Like = "ladders:like";
    #endregion

    #region     智力竞赛

    public const string QuizStart = "quiz:activity_start";
    public const string QuizBegin = "quiz:begin";
    public const string QuizPlayerEnter = "quiz:player_enter";
    public const string QuizInitData = "quiz:now_info";
    public const string QuizAnswer = "quiz:player_answer";
    public const string QuizAnswerRSP = "quiz:answer_rsp";
    public const string QuizAnswerReward = "quiz:answer_reward";
    public const string QuizRewards = "quiz:activity_reward";

    #endregion

    #region     智力问答

    public const string MindQuizStart = "mind_quiz:activity_start";
    public const string MindQuizBegin = "mind_quiz:begin";
    public const string MindQuizPlayerEnter = "mind_quiz:player_enter";
    public const string MindQuizPlayerLeave = "mind_quiz:player_leave";
    public const string MindQuizInitData = "mind_quiz:now_info";
    public const string MindQuizInitDataDuringAnnouncing = "mind_quiz:announce_period_info";
    public const string MindQuizAnswer = "mind_quiz:player_answer";
    public const string MindQuizAnswerRSP = "mind_quiz:answer_rsp";
    public const string MindQuizAnswerReward = "mind_quiz:answer_reward";
    public const string MindQuizRewards = "mind_quiz:activity_reward";
    public const string OtherAnswerBarrage = "mind_quiz:choose";
    public const string MindQuizTopThree = "mind_quiz:rank_self";
    public const string MindQuizGetEndTopThree = "mind_quiz:first_three";

    #endregion

    #region 活动
    public const string ActivityBuyTime = "activities:buy_times";
    public const string FuncListAll = "func_list:all";
    public const string FuncListUpdate = "func_list:update";

    public const string Activity = "activity:";
    public const string ActivityUpdateRoleData = "activity:update_role_data";
    public const string ResourcesRetrieve = "activities:res_back";
    public const string OfflineHosting = "activities:offline_hosting";
    public const string OfflineHostingInfo = "offline_hosting:info";
    public const string SupplyHosting = "activities:supply_hosting";
    public const string EnterPrepareScene = "enter_prepare_scene";
    public const string EnterPrepareSceneSuccess = "team_fight:enter_prepare_scene_success";
    public const string EnterPrepareSceneFaild = "team_fight:enter_prepare_scene_failed";
    public const string EnterPrepareSceneReset = "team_fight:auto_pass";
    public const string BackPrepareScene = "return_prepare_scene";

    public const string FightEndTime = "team_fight:fight_end_time";
    public const string FightKillNotify = "team_fight:kill_notify";
    public const string FightResult = "team_fight:fight_result";
    public const string FightLevel = "team_fight:start_level";
    public const string FightFinalResult = "team_fight:final_result";
    public const string TreasurList = "update_treasure_list"; //藏宝图位置

    #region worldboss
    /// <summary>
    /// 请求世界boss 信息
    /// </summary>
    public const string WorldBossBaseInfo = "request_base_info";
    public const string WorldBossResponseBaseInfo = "world_boss:response_base_info";
    /// <summary>
    /// 请求世界boss 排行信息
    /// </summary>
    public const string WorldBossRequestRank = "request_rank";
    public const string WorldBossResponse_rank = "world_boss:response_rank";

    // 打击boss
    public const string WorldBossAttack_boss = "attack_boss";
    public const string WorldBossBoss_fight_result = "world_boss:boss_fight_result";

    // 打击player
    public const string WorldBossAttack_player = "attack_player";
    public const string WorldBoss_attack_player_result = "world_boss:attack_player_result";

    // 左小角的记录
    public const string WorldBossReqGrabInfo = "request_fight_record";
    public const string WorldBossResGrabInfo = "world_boss:response_fight_record";

    //public const string WorldBossAttack_boss = "world_boss:boss_fight_result";

    // 请求抢夺信息
    public const string WorldBossResponseGrabPlayerInfo = "world_boss:response_attack_list";
    public const string WorldBossRequestGrabPlayerInfo = "request_attack_list";

    // 最终的结算界面
    public const string WorldBossReqFinishRank = "request_final_result";
    public const string WorldBossResFinishRank = "world_boss:response_final_result";

    // 世界boss 的战斗时间
    public const string WorldBossAttackTime = "world_boss:fight_time";

    #endregion

    #region 勇士之塔

    // 请求攻击
    public const string BraveTowerReqAttack = "brave_tower:challenge";
    // 回复攻击成功
    public const string BraveTowerAttackSuccess = "brave_tower:result";
    // 勇士之塔 的战斗时间
    public const string BraveTowerAttackTime = "brave_tower:fight";
    //放弃
    public const string BraveTowerGiveUp = "brave_tower:give_up";
    public const string BraveTowerInfo = "brave_tower:tower";
    public const string BraveTowerUpdateInfo = "brave_tower:tower_update";
    #endregion

    #region 经验副本：冰封魔窟
    public const string FrozenCellarTime = "frozen_cellar:fight";
    public const string FrozenCellarResult = "frozen_cellar:result";
    public const string FrozenCellarEnhance = "frozen_cellar_instance:enhance";
    public const string FrozenCellarEnhanceInfo = "frozen_cellar:enhanced";
    public const string FrozenCellarRequstEnhanceInfo = "frozen_cellar_instance:enhance_info";
    #endregion

    #region 金币副本：巨龙宝藏
    public const string DragonTreasureTime = "dragon_treasure:fight";
    public const string DragonTreasureResult = "dragon_treasure:result";
    public const string DragonTreasureResolveDie = "dragon_treasure:resolve_die";
    #endregion

    #region 战场
    public const string BattleGroundPrepareTime = "battle_ground:prepare_time";
    public const string BattleGroundPrepareCount = "battle_ground:prepare_count";
    public const string BattleGroundFightInitInfo = "battle_ground:fight_init_info";
    public const string BattleGroundResourceGetScore = "battle_ground:resource_get_score";
    public const string BattleGroundKillGetScore = "battle_ground:kill_role_get_score";
    public const string BattleGroundCampScore = "battle_ground:camp_score";
    public const string BattleGroundRefreshResourceId = "battle_ground:refresh_resource_id";
    public const string BattleGroundApplyBattleground = "apply_battleground";
    public const string BattleGroundFightRank = "battle_ground:fight_rank";
    public const string BattleGroundRequestFightRank = "battle_ground:request_fight_rank";
    public const string BattleGroundFightResult = "battle_ground:fight_result";
    #endregion
    #region 风暴岩库
    public const string StormCellarFight = "storm_cellar:fight";
    public const string StormCellarChallenge = "storm_cellar:challenge";
    public const string StormCellarResult = "storm_cellar:result";
    public const string StormCellarSweep = "storm_cellar:sweep";
    public const string StormCellarGetPoint = "storm_cellar:get_point";
    public const string StormCellarSlowDown = "storm_cellar_instance:slow_down";
    public const string StormCellarExit = "storm_cellar_instance:leave";

    #endregion

    #region [WildPractice]
    public const string KillMonsterNum = "kill_monster_num";
    #endregion
    #region 金矿副本
    public const string RoyalMineExit = "royal_mine_instance:leave";
    public const string RoyalMineRecords = "royal_mine:records";
    public const string RoyalMineChallenge = "royal_mine:challenge";
    public const string RoyalMineSweep = "royal_mine:sweep";
    public const string RoyalMineKillNum = "royal_mine:kill_num";
    public const string RoyalMineFightTime = "royal_mine:fight";
    public const string RoyalMineResult = "royal_mine:result";
    #endregion
    #region 雷岩禁地

    public const string ThunderForbiddenExit = "thunder_forbidden_instance:leave";
    public const string ThunderForbiddenRecords = "thunder_forbidden:records";
    public const string ThunderForbiddenChallenge = "thunder_forbidden:challenge";
    public const string ThunderForbiddenSweep = "thunder_forbidden:sweep";
    public const string ThunderForbiddenKillNum = "thunder_forbidden:kill_num";
    public const string ThunderForbiddenFightTime = "thunder_forbidden:fight";
    public const string ThunderForbiddenResult = "thunder_forbidden:result";
    #endregion

    #endregion

    #region 邮件
    public const string MailRequestList = "mail:list";
    public const string MailListRsp = "mail:list";
    public const string MailReadRqst = "mail:read";
    public const string MailClaimRqst = "mail:get_attachment";
    public const string MailClaimRsp = "mail:get_attachment";
    public const string MailDeleteRqst = "mail:delete";
    public const string MailDeleteRsp = "mail:delete";
    public const string MailClaimAllRqst = "mail:get_all_attachment";
    public const string MailClaimAllRsp = "mail:get_all_attachment";
    public const string MailReceived = "mail:received";

    #endregion

    #region 副本
    public const string AskChangeMap = "instance";
    public const string ExitMap = "exit_instance";
    public const string OnTouchObject = "skill:touch_obj";
    public const string PlayStory = "play_movie";
    public const string InstanceStart = "instance:time";
    public const string MonsterSay = "monster_say";
    public const string FireScene = "fire_scene_event";
    public const string SceneGapTime = "scene:gap_time";
    public const string RemoveSceneGap = "base_instance:clear_scene_cd";
    public const string RemoveSceneGapSuccess = "clear_scene_cd_success";
    public const string ClienSceneEvent = "client_scene_event";
    public const string PlayClientAi = "trigger_client_ai";
    public const string ClinetAiOver = "skill:client_ai_over";
    #endregion

    #region 心态包
    public const string Ping = "ping";
    public const string Pong = "pong";
    #endregion

    #region 组队
    public const string TeamAddMember = "team:add_team_member";
    public const string TeamAutoMatch = "team:set_auto_match";
    public const string TeamCreate = "team:create_team";
    public const string TeamCreateSuccess = "create_team_success";
    public const string TeamGetList = "team:get_team_list";
    public const string TeamGetInfo = "team:get_team_info";
    public const string TeamInviteJoin = "team:invite";
    public const string TeamApplyJoin = "team:apply_join";
    public const string TeamTargetChange = "team:change_team_target";
    public const string TeamInviteResult = "team:commit_invite";
    public const string TeamApplyResult = "team:commit_apply";
    public const string TeamGetApply = "team:look_apply_list";
    public const string TeamClearApply = "team:clear_apply_list";
    public const string TeamFired = "team:fired";
    public const string TeamQuit = "team:quit";
    public const string TeamAutoRatify = "team:set_approved";
    public const string TeamTurnCaptain = "team:transfer_leader";
    public const string TeamAskChangeAssist = "team:change_assist";
    public const string TeamAssistChanged = "team:assist_changed";

    public const string TeamAutoMatchEvent = "auto_match";
    public const string TeamList = "team_list";
    public const string TeamInfo = "team_info";
    public const string TeamInvite = "invite_team";
    public const string TeamJoin = "join_team";
    public const string TeamTargetChanged = "team_target_changed";
    public const string TeamMemberJoin = "add_team_member";
    public const string TeamApplyList = "apply_list";
    public const string TeamMemberLeave = "leave_team";
    public const string TeamCaptainChanged = "leader_changed";
    public const string TeamApproveChanged = "team_approve_changed";
    public const string TeamPropsChanged = "team_member_props_changed";
    public const string TeamApplyCleared = "clear_team_apply";
    public const string TeamApplyAll = "team:one_key_apply"; //一键申请
    public const string TeamAskInstance = "ask_goto_instance"; //进入副本准备通知
    public const string TeamCommitInstance = "team:commit_enter_instance"; //准备拒绝副本
    public const string RefuseInstance = "refuse_goto_instance"; // 队员拒绝进入
    public const string ReadyInstance = "ready_goto_instance"; //队员准备
    public const string RefuseInvite = "reject_invite";
    public const string GotoInstanceFailed = "team_mem_goto_instance_failed";
    public const string RefuseTeamApply = "reject_apply";
    public const string TeamApplyExpired = "team_apply_expired";
    public const string TeamJoinFailed = "team_join_fail";
    public const string ChangeTeamLevel = "team:change_team_level";
    public const string TeamLevelChanged = "team_level_changed";
    public const string TeamNewApplyJoin = "team_apply_join";
    public const string TeamApplyFail = "team_apply_fail";
    public const string TeamDealApplyFail = "team_deal_apply_fail";
    public const string TeamInviteFail = "team_invite_fail";
    public const string TeamDealInviteFail = "team_deal_invite_fail";
    public const string AllApplyAlready = "already_applied";

    #endregion

    #region 称号
    public const string AppellationEquip = "appellation:equip";
    public const string AppellationUnEquip = "appellation:unequip";
    public const string AppellationGain = "appellation:gain";
    public const string AppellationExpiration = "appellation:expiration";
    public const string AppellationLost = "appellation:lost";
    #endregion

    #region 系统开放

    public const string SystemUnlocked = "functions:unlocked";
    public const string ActivityNotOpen = "activity";
    public const string ActivityNotOpenIt = "activity:not_open";
    #endregion

    #region 威望/爵位
    public const string ImprovePrestige = "nobility:levelup";
    public const string ImprovePrestigeLevel = "nobility:improve_evaluation";
    #endregion

    #region 活动界面
    public const string ActivityList = "activities:lists";
    public const string ActivityListInfo = "activities";
    public const string ValalityBoxRewarded = "activities:rewarded";
    public const string ValalityBoxReward = "activities:reward";
    #endregion

    #region 野外修炼
    public const string ActiveExpPool = "active_exp_pool";
    public const string UpdateExpPool = "update_exp_pool";
    public const string InjectExp = "exp_pool:inject_exp";
    public const string LookPool = "exp_pool:look_pool";
    public const string ExpPoolInfo = "exp_pool_info";
    public const string ResetExpPool = "reset_exp_pool";
    #endregion

    #region BOSS挑战
    public const string PersonalBossRecords = "personal_boss:records";
    public const string PersonalBossChallenge = "personal_boss:challenge";
    public const string PersonalBossResult = "personal_boss:result";
    public const string LookWildInfo = "wild_leader:look_wild_info";
    public const string RecievedWildInfo = "wild_leader_info";
    public const string AddWildLeaderBelong = "add_wild_leader_belong";
    public const string RemoveWildLeaderBelong = "remove_wild_leader_belong";
    public const string VipBossRecords = "vip_boss:records";
    public const string VipBossChallenge = "vip_boss:challenge";
    public const string VipBossResult = "vip_boss:result";
    public const string PersonalBossFight = "personal_boss:fight";
    public const string VipBossFight = "vip_boss:fight";
    public const string LookWildBoss = "wild_boss:look_wildboss_info";
    public const string ReplyWildBossInfo = "wild_boss_info";
    public const string WildFocusSubscribe = "wild_boss:subscribe";
    public const string WildBossRelive = "boss_will_relive";
    public const string LookWildFocusedBoss = "wild_boss:look_subscribe";
    public const string ReplyWildFocusedBoss = "subscribe_info";
    public const string WillBossKillNumInfo = "kill_wild_boss";
    public const string BossHomeLookInfo = "boss_home:look_bosshome_info";
    public const string ReplyBossHomeInfo = "boss_home_info";
    public const string FocusOnBossHome = "boss_home:subscribe";
    public const string LookBossHomeFocusInfo = "boss_home:look_subscribe";
    public const string ReplyBossHomeFocusInfo = "bosshome_subscribe_info";
    public const string BossHomeRelive = "bosshome_boss_will_relive";
    public const string BossKillRecords = "boss_home:kill_records";
    public const string ReplyBossKillRecords = "boss_home:kill_records";
    public const string Drop_Records = "drop_records:get_drop_records";
    public const string ReplyDrop_Records = "drop_records:get_drop_records";
    public const string WorldBossKillRecords = "wild_boss:kill_records";
    public const string SteelBossKillRecords = "steel_wing:kill_records";
    public const string ReplySteelBossKillRecords = "steel_wing:kill_records";
    public const string ReplyWorldBossKillRecords = "wild_boss:kill_records";

    public const string FocusOnSteelBoss = "steel_wing:subscribe";
    public const string LookSteelBossFocusInfo = "steel_wing:look_subscribe";
    public const string ReplySteelBossFocusInfo = "steelwing_subscribe_info";
    public const string SteelBossLookInfo = "steel_wing:look_steelwing_info";
    public const string ReplySteelBossInfo = "steel_wing_info";
    public const string SteelWingAnger = "steel_wing:anger";
    public const string SteelWingAngerMax = "steel_wing:anger_max";
    public const string SteelBossRelive = "steelwing_boss_will_relive";
    #endregion

    #region 名人堂

    public const string FameHallOptions = "fame_hall:options";
    public const string FameHallSelect = "fame_hall:select";
    public const string FameHallResult = "fame_hall:result";
    public const string FameHallFight = "fame_hall:fight";
    public const string FameHallGetReward = "fame_hall:reward";
    public const string FameHallGetRewardSuccess = "famehall:rewards";
    public const string FameHallGiveUp = "fame_hall:give_up";
    public const string FameHallVitualId = "fame_hall:virtual_id";
    public const string FaneHallTimeReward = "fame_hall:times_reward";
    #endregion

    #region 改名
    public const string RenameMatch = "rename:match";
    public const string RenameChange = "rename:change";
    #endregion

    #region 恶魔城
    public const string DemonCityStage = "devil_city:stage";
    public const string DemonCityChallange = "devil_city:challenge";
    public const string DemonCityResult = "devil_city:result";
    public const string DemonCityGiveUp = "devil_city:give_up";
    public const string DemonCityFightTime = "devil_city:fight";
    public const string DemonCityDailyAward = "devil_city:daily_rewards";
    #endregion

    #region 仓库
    public const string WarehouseStore = "warehouse:store"; //存入仓库
    public const string WarehouseFetch = "warehouse:fetch"; //从仓库取出
    public const string WarehouseExtend = "warehouse:extend"; //扩展仓库
    #endregion

    #region 矮人宝库
    public const string ShortTreasureResTimeInfo = "short_treasure:time_info";
    public const string ShortTreasureResPlayerInfo = "short_treasure:layer_info";
    public const string ShortTreasureResWaitTime = "short_treasure:wait_for_exit";
    public const string ShortTreasureResPickUpScore = "short_treasure:pickup_a_score";
    public const string ShortTreasureReqPlayerEnter = "short_treasure:player_enter";
    public const string ShortTreasureReqLevelIt = "short_treasure:player_leave";
    public const string ShortTreasureResEnterFail = "short_treasure:enter_failed";
    public const string ShortTreasureResFinish = "short_treasure:completed";
    public const string ShortTreasureResCanNotAttack = "short_treasure:cannot_attack";
    #endregion

    #region 圣域战场
    public const string BattlegroundNotJoinReason = "battle_field:enter_failed";
    public const string BattlegroundResNewOne = "battle_field:new_one";
    public const string BattlegroundReqLeftTime = "battle_field:get_time_info";
    public const string BattlegroundResTimeInfo = "battle_field:time_info";
    public const string BattlegroundReqPlayerEnter = "battle_field:player_enter";
    public const string BattlegroundReqLevelIt = "battle_field:player_leave";
    public const string BattlegroundRResRankChange = "battle_field:rank_change";
    public const string BattlegroundResFinish = "battle_field:finished";
    public const string BattlegroundUpdateLvAndPowerValue = "player_lv_bp_changed";
    #endregion


    #region 命运契约
    // 更新
    public const string FateContractUpdateData = "fate_contract:update";
    // 领取
    public const string FateContractReqGet = "fate_contract:recv_target_reward";
    // 总的奖励
    public const string FateContractReqGetContract = "fate_contract:recv_type_reward";

    #endregion


    #region 神之试炼
    public const string GodTrialRecordsOfGuild = "god_trial:records_of_guild";
    public const string GodTrialRecordsOfAvatar = "god_trial:records_of_avatar";
    public const string GodTrialChanllenge = "god_trial:chanllenge";
    public const string GodTrialTimeLeft = "god_trial:time_left";
    public const string GodTrialListsOfRob = "god_trial:lists_of_rob";
    public const string GodTrialFightTime = "god_trial:fight_time";
    public const string GodTrialResult = "god_trial:result";
    public const string GodTrialZhanXun = "god_trial:award";
    public const string GodTrialRob = "god_trial:rob";
    // 玩家自己的消息
    public const string GodTrialSelfBeGod = "god_trial:robbed";
    //
    public const string GodTrialUnsubscribe = "god_trial:unsubscribe";

    public const string GodTrialEnd = "god_trial:activity_end";

    #endregion

    #region 成就
    public const string AchievementUpdateInfo = "achievements:update";
    public const string AchievementClaimInfo = "achievements:claim";
    #endregion

    #region 战斗力
    public const string FightValueDetails = "battlepower:details";
    #endregion

    #region 剧情
    public const string StoryOver = "skill:story_end";
    #endregion

    //装备锻造
    public const string EquipmentsForge = "equipments:forge";
    //装备传承
    public const string EquipmentsInherit = "equipments:inherit";
    //装备熔炼
    public const string EquipmentsSmelt = "equipments:smelt";

    public const string EquipmentsUnlock = "equipments:unlock";

    #region [法宝]

    //法宝升级
    public const string TalimansUpClass = "talismans:class";
    //法宝升阶升星
    public const string TalimansUpStar = "talismans:rank";
    //法宝外观
    public const string TalimansShape = "talismans:shape";
    //法宝威能
    public const string TalimansPower = "talismans:power";
    //法宝的属性
    public const string TalimansProp = "talismans:props";
    //法宝外观
    public const string TalimansShow = "talismans:show";
    //外观佩戴
    public const string TalimansWearing = "talismans:wear";
    //外观取下
    public const string TalimansUnWear = "talismans:unwear";
    //法宝吞噬面板刷新
    public const string TalimansUpdata = "talismans:class_update";
    #endregion
    //七日登录礼包
    public const string SevenGifts = "week_package:get";
    public const string SevenGiftsPush = "week_package:changed";
    public const string Noviceguide = "novice_guide:achieve";
    public const string NoviceguideFinsh = "novice_guide:finish";

    #region 首冲奖励
    public const string FRReceiveReward = "first_pay_reward:get_rewards";
    public const string FRReceiveListUpdate = "first_pay_reward:update";
    #endregion

    #region 寻宝
    public const string RequestTreasureHuntClickXunBaoEvent = "treasure_hunt:hunt";
    public const string RequestTreasureHuntCloseUI = "treasure_hunt:close";
    public const string RequestTreasureHuntOpenUI = "treasure_hunt:open";
    public const string RequestTreasureHuntTemporaryWarehouseTakeOutItem = "treasure_hunt:get";
    public const string RequestTreasureHuntTemporaryWarehouseTakeOutAllItem = "treasure_hunt:all_get";
    public const string RequestTreasureHuntSelfRecord = "treasure_hunt:self_show_update";
    public const string RequestTreasureHuntAllRecord = "treasure_hunt:show_update";
    public const string RequestTreasureHuntIntegralMallExchange = "treasure_hunt:exchange";
    #endregion


    #region [掉落]
    public const string AppearDropList = "appear_drop_list";
    public const string AppearDrop = "appear_drop";
    public const string PickupDrop = "skill:pickup_drop";
    public const string PickupSuccess = "pickup_drop";
    #endregion

    #region [游戏公告跑马灯]
    public const string GameHint = "msg:hint";
    #endregion

    #region [设置]
    public const string SaveSetting = "setting:save";
    public const string FetchStting = "setting:fetch";
    public const string Settings = "settings";
    #endregion

    #region [PK模式]
    public const string SetPKMode = "skill:set_pk_mode";
    public const string UpdatePkInfo = "update_pk_info";
    public const string UpdateHateList = "update_hate_list";
    public const string KillRedName = "kill_bad";
    #endregion

    #region [充值回调]
    public const string PaySuccessCallBack = "get_balance";
    #endregion

    #region [敏感字检测]
    public const string BadWordCheckReq = "skill:bad_word";
    public const string BadWordCheckResp = "badword";
    #endregion

    #region 成长礼包
    public const string RequestLeftGrowGift = "growth_package:refresh";
    public const string UpdateLeftGrowGift = "growth_package:update";
    public const string RequestGetGrowGift = "growth_package:get";
    #endregion
    #region 服务器提示
    // 服务器提示
    public const string ServerTip = "idip:popup_msg";
    // 禁止消息
    public const string ServerBanMsg = "idip:ban_msg";
    //禁止指定玩法列表
    public const string ServerBanPlayerThingMsg = "ban_play_method:list";
    // 禁止的玩法更新
    public const string ServerBanPlayerUpdateThing = "ban_play_method:update";

    #endregion

    #region 每日礼盒
    public const string RequestUpdateLucky = "daily_gift:player_enter";
    public const string ResponseInitLucky = "daily_gift:ladder_init";
    public const string ResponseUpdateLucky = "daily_gift:ladder_update";
    public const string StopUpdateLucky = "daily_gift:player_leave";
    public const string RefreshDailyLucky = "daily_gift:reset";
    public const string RequestResponseDailyBoxLottery = "daily_gift:lottery";
    public const string RequestResponseGetDailyBoxLottery = "daily_gift:reason";
    #endregion

    #region 开服累冲
    public const string RechargeAsk = "open_acc_pay:get_rewards";
    public const string RechargeReturn = "open_acc_pay:update";
    #endregion

    #region [开服冲榜]
    public const string DayAchieved = "hit_list:day_achieved";//获取某一天达到的目标及领取状态 请求 
    public const string CurTop = "hit_list:cur_top";//当前榜第一名
    public const string GetRank = "hit_list:get_rank";//获得某天排行榜 请求
    public const string DaysLadder = "seven_days_ladder";//获得某天排行榜相应 info
    public const string GetBtn = "hit_list:click_get_btn";//玩家点击领取按钮 请求
    public const string Gained = "hit_list:gained";//玩家点击领取按钮 相应
    public const string LastDayBalance = "hit_list:last_day_balance";
    public const string BalanceRank = "hit_list:balance_rank";//结算后服务器通知
    public const string RedDot = "hit_list:red_dot";//请求查看是否有奖励
    public const string OpenNext = "hit_list:open_next";//有榜开启
    #endregion

    #region 登录基金
    public const string BuyLoginFund = "login_fund:fund";
    public const string GetBasisFund = "login_fund:get_basis";
    public const string GetLoginFund = "login_fund:get";
    public const string RefreshFund = "login_fund:changed";
    #endregion

    #region 成长基金
    public const string InvestmentFund = "growth_fund:fund";//投资购买
    public const string GetSelffund = "growth_fund:get_basis";//领取本金
    public const string GetOtherFund = "growth_fund:get";//领取其他可领取
    public const string PushAllFund = "growth_fund:update";//刷新可领取的基金
    #endregion

    #region P2P
    public const string ReqP2PStar = "player_trade:start";   //请求：发送交易申请
    public const string ReqP2PAccept = "player_trade:accept";//请求：接收交易申请
    public const string ReqP2PRefuse = "player_trade:reject";//请求：拒绝交易
    public const string ReqP2PLock = "player_trade:lock";    //请求：锁定交易物品和魔石
    public const string ReqP2PSure = "player_trade:trade";   //请求：我方确认交易
    public const string ReqP2PCancel = "player_trade:cancel";//请求：对方取消交易
    public const string RecP2P = "player_trade:message";     //响应：所有请求返回同一个响应，根据返回数据中的message枚举值区分。
    #endregion
}

public class ActionTag
{
    public const string Login = "Login";
    public const string CreateRole = "createRole";
    public const string LoginCreatRole = "2createRole";
    public const string LoginSelectedRole = "2selectRole";
    public const string LoginRoleSelected = "LoginRoleSelected";
    public const string StartGame = "StartGame";
    public const string SellItem = "SellItem";
    public const string EquipItem = "EquipItem";
    public const string UnEquipItem = "UnEquipItem";

    public const string HpChanged = "hp_changed";
    public const string LevelChanged = "level_changed";
    public const string ExpChanged = "exp_changed";
    public const string xpChanged = "xp_changed";
    public const string CoinChanged = "coin_changed";
    public const string OpenSendGift = "openSendGift";

}

public class ChannelTag
{
    public const string Channel_Private = "chat:private";

    public const string MsgAuth = "msg:auth";
    public const string MsgPoint = "msg:point";
    public const string MsgWorld = "msg:world";
    public const string MsgSystem = "msg:system";
    public const string MsgDove = "msg:dove";
    public const string MsgGroup = "msg:group"; //军团
    public const string MsgTeam = "msg:team"; //队伍
    public const string MsgFriends = "msg:friends";
    public const string MsgInvite = "msg:invite"; //组队
    public const string MsgSetChannels = "msg:set_channels"; //组队
    public const string ClearChat = "idip:clear_chat";//发言清除
}

public class GMTag
{
    #region netString
    public static string gm_vip = "gm:vip";
    public static string gm_level = "gm:level";
    public static string gm_exp = "gm:exp";
    public static string gm_addItem = "gm:addItem";
    public static string gm_coin = "gm:coin";
    public static string gm_ep = "gm:ep";
    public static string change_scene = "change_scene";
    public static string addEudemon = "gm:addEudemon";
    public static string addGem = "gm:addGem";
    public static string gold = "gm:gold";
    public static string eexp = "gm:eexp";
    public static string open_act = "gm:open_act";
    public static string close_act = "gm:close_act";
    public static string checkin_reset = "checkin:reset";
    public static string gm_appellation = "gm:addAppellation";
    public static string resetExpPool = "gm:reset_exp_pool";
    public static string guildAuction = "gm:auction_guild";
    public static string change_map = "change_map";
    public static string gm_monster = "gm:monster";
    public static string gm_clear = "gm:clear";
    public static string gm_remove = "gm:remove";
    #endregion
}

public class BackGroundName
{
    public const string UIBG = "backGroud";
    public const string RADERBG = "ditu";
    public const string CUP = "jiangbei";
}
#endregion

#region 安全SDK
public class TssSdkType
{
    public const string ReportData = "word_check:anti";
}
#endregion

#region 军团相关
public enum GroupRight
{
    /// <summary>
    /// 领袖
    /// </summary>
    GR_CHIEF = 1,
    /// <summary>
    /// 领袖伴侣
    /// </summary>
    GR_CHIEF_MATE,
    /// <summary>
    /// 军团长伴侣
    /// </summary>
    GR_LEADER_MATE,
    /// <summary>
    /// 副团长伴侣
    /// </summary>
    GR_VICELEADER_MATE,
    /// <summary>
    /// 元老伴侣
    /// </summary>
    GR_FOUNDING_MATE,
    /// <summary>
    /// 军团长
    /// </summary>
    GR_LEADER,
    /// <summary>
    /// 副军团长
    /// </summary>
    GR_VICELEADER,
    /// <summary>
    /// 元老
    /// </summary>
    GR_FOUNDING,
    /// <summary>
    /// 玫瑰女神
    /// </summary>
    GR_ROSE,
    /// <summary>
    /// 团员
    /// </summary>
    GR_MEMBER,
    GR_MAX
}
public enum GroupTitleClass
{
    GTC_Family,
    GTC_Team,
    GTC_Group,
    GTC_Default,
    GTC_Max
}
#endregion

#region 任务相关
public class QuestType
{
    public const int Main = 1; //主线
    public const int Branch = 2; //支线
    public const int Daily = 3; //日常
    public const int Escort = 5; //跑商
    public const int Guide = 6; //引导任务
}

public class QuestCompleteConditionType
{
    public const int NoCompleteCondition = 0; // 无完成条件
    public const int KillMonster = 11001; // 杀怪

    public const int KillMonsterWithSpecifiedLevel = 11002; //杀指定等级区间的怪
    public const int KillWildLeader = 11003; //击杀世界boss、
    public const int CollectByKillMonster = 13001; //杀怪收集道具
    public const int CollectByPickUp = 13002; // 收集
    public const int CollectByBuy = 13003;
    public const int SurVey = 13004;    //调查

    public const int UseItemAnyWhere = 14001; //任意地点使用道具
    public const int UseItemInAssignedPlace = 14002; //指定地点使用道具

    public const int ExecuteAction = 15001; // 执行指定动作
    public const int TalkWithNpc = 15002; // 与NPC进行对话
    public const int ExecuteActionToObject = 15003; //对指定物件执行指定动作
    public const int ArriveAssignedPlace = 15004; //到达指定地点
    public const int GuideToAcceptQuest = 15005; //引导去完成对应任务

    public const int ReachLiveness = 16002; //达到活跃度

    public const int CompleteAct = 17001; //完成活动
    public const int CompleteFameHall = 17002; //完成名人堂
    public const int PassDemonCityLayer = 18001; //通关恶魔塔指定层数

    public const int WearEquip = 19001; //穿戴装备
    public const int EquipAddition = 19002; //装备追加
    public const int EquipForge = 19004;//装备锻造
    public const int EquipWake = 19005;//装备觉醒
    public const int CompoundItem = 19100; //合成道具

    public const int SkillLevel = 20002; //技能等级

    public const int AcquireEudemon = 21001; //获取幻兽
    public const int EudemonLevelUpTime = 21003; //升级幻兽次数
    public const int EudemonsFight = 21005;//出战幻兽
    public const int EudeomnCompose = 21006; //幻兽幻化
    public const int EudemonsKnights = 21007;//圆桌骑士

    public const int JoinGroup = 80001; //加入军团

    public const int AddFriends = 80006; //添加好友

    public const int PassDuplicate = 80002; //通关副本

    public const int Escort = 80007; //跑商


    public const int SingleBoss = 18002;//传奇挑战


    public const int TalimansUpLevel = 31001;//法宝升级
    public const int TalimansUpStar = 31002;//法宝升星
    public const int TalimansPower = 31003;//法宝威能

    public const int HorseActive = 41001;//激活坐骑
    public const int EvilShop = 80008;//恶魔商店
}

//接取或完成时的行为
public class QuestActionType
{
    public const int PlayTimeline = 5;//播放timeline
}

public class DescribeAttribute : Attribute
{
    private string name;

    public DescribeAttribute(string name)
    {
        this.Name = name;
    }

    public readonly string Data;
    public readonly int Tag;
    public static readonly DescribeAttribute Empty = new DescribeAttribute(string.Empty, string.Empty, 0);
    public DescribeAttribute(string name, string data) : this(name)
    {
        this.Data = data;
    }
    public DescribeAttribute(string name, int tag) : this(name)
    {
        this.Tag = tag;
    }
    public DescribeAttribute(string name, string data, int tag) : this(name, data)
    {
        this.Tag = tag;
    }
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }
}

public static class EnumDescribe<T>
{
    private static readonly Dictionary<T, DescribeAttribute> dict_ = new Dictionary<T, DescribeAttribute>();

    static EnumDescribe()
    {
        FieldInfo[] fiList = typeof(T).GetFields();
        foreach (FieldInfo fi in fiList)
        {
            object[] attrList = fi.GetCustomAttributes(typeof(DescribeAttribute), true);
            if (attrList.Length > 0)
            {
                var da = attrList[0] as DescribeAttribute;
                if (da != null)
                {
                    var t = (T)fi.GetValue(typeof(T));
                    dict_[t] = da;

                    //add by cdh 20161229
                    if (!typeof(T).IsEnum)
                        da.Name = Localization.Get(CString.Format("{0}_{1}", typeof(T).FullName, fi.GetValue(null)));
                    else
                    {
                        if (fi.FieldType.Name == typeof(T).Name)
                        {
                            da.Name = Localization.Get(CString.Format("{0}_{1}", typeof(T).FullName, Convert.ToInt32(t)));
                        }
                    }
                }
            }
        }
    }

    public static DescribeAttribute Get(T t)
    {
        DescribeAttribute result;
        if (dict_.TryGetValue(t, out result))
        {
            return result;
        }
        else
        {
            return new DescribeAttribute(t.ToString());
        }
    }

    public static void SetName(T t, string name)
    {
        DescribeAttribute attr;
        if (dict_.TryGetValue(t, out attr))
            attr.Name = name;
    }
}
#endregion

#region 离散数据id
public class DiscreteTag
{
    public const int CloverId = 208030201; //四叶草ID
    public const int EudemonBeadsId = 208030303; //圣兽魔晶
    public const int EudemonEnergyId = 299010008; //幻兽能晶ID
    public const long FriendGift = 799000044;
    public const long ImprovePrestige = 799000120;
    public const long NobilityCanonizedRole = 799000136;
    public const long GetMapIDList = 799000083;
    public const long FieldExpend = 799000087;
    public const long FieldExpendRat = 799000086;
    public const long ChangePlayerName = 799000090;
    public const long EudemonDismissReturnPercentQuality = 799000077;
    public const long EudemonDismissReturnPercentExp = 799000078;
    public const long EudemonDismissReturnPercentMinor = 799000079;
    public const long EudemonDismissReturnPercentEvolved = 799000080;
    public const long EudemonDismissReturnPercentRank = 799000081;
    public const long EudemonScorePerStar = 799000001;
    public const long TemplateEudemon = 799000082;
    public const long PerfectRetrieveFactor = 799000129;
    public const long NormalRetrieveFactor = 799000130;
    public const long RetrieveTimesLimit = 799000131;
    public const long HositingPropData = 799000143;
    public const long MaxHostingTime = 799000132;
    public const long HostingPropRecommendTime = 799000149;
    public const long QuickBuyHostingPropId = 799000152;
    public const int FrozenCellarMapId = 799000185;
    public const int DragonTreasureMapId = 799000186;
    public const int DragonStarTime = 799000187;
    public const int QuizTime = 799000178;
    public const int OfflineHositingUnlockedLevel = 799000190;
    public const int DragonTreasureWaveMax = 799000237;

    public const long PriceChangePerClick = 799000033;
    public const long AuctionPricePerAdd = 799000026;
    public const long EudemonExpItem = 799000073;
    public const long PkModeCd = 799000100;

    public const int ExpId = 799000102;
    public const int FameHallAddScoreInterval = 799000097;
    public const int FameHallBuyTimePrice = 799000098;
    //public const int FameHallAddScoreTime = 799000099;
    public const int FameHallScoreItemId = 799000112;
    public const long FameHallResetAward = 799000099;

    public const long WorldChatLevel = 799000107;
    public const long WorldChatSecond = 799000108;
    public const long PigeonStayTime = 799000111;
    public const long TeamChatLevel = 799000295;
    public const long GroupChatLevel = 799000296;
    public const long FriendChatLevel = 799000297;
    public const long AreaChatLevel = 799000298;

    public const long AppellationUnlockLevel = 799000076;

    public const long ChangeNameCost = 799000117;
    public const long ChangeNameLevel = 799000118;

    //装备锻造成功率
    public const long EquipComposeRate = 799000113;
    public const long EquipRefineTimes = 799000114;
    public const long EquipRefineMaterial = 799000115;
    public const long EquipRefineCost = 799000116;

    public const long MaxWarehouseSpace = 799000126;
    public const long WarehouseExtend = 799000127;
    public const long WarehouseShopItem = 799000128; //背包扩充道具商城ID
    public const long EudemonRefineBattlePower = 799000012; //洗炼战斗力

    public const long EquipComposeRecomendTip1 = 899000193; //锻造可提升装备相性属性
    public const long EquipComposeRecomendTip2 = 899000194; //相性属性可增加幻兽攻击

    //装备锻造消耗物品ID
    public const long EquipDaunZhaoCostItem = 799000091;
    //装备锻造消耗货币(数组，对应品质)
    public const long EquipDuanZhaoCostCoin = 799000093;

    //装备传承消耗(数组，对应品质)
    public const long EquipInheritCostCoin = 799000113;

    /// <summary>
    /// 装备开孔消耗道具（0表示只判断等级，不需要额外道具）
    /// </summary>
    public const long GemSocketUnlockItem = 799000122;

    /// <summary>
    /// 装备锻造最大次数，离散表ID
    /// </summary>
    public const long EquipMaxForgeNumID = 799000092;

    /// <summary>
    /// 法宝追加消耗道具以及成长值
    /// </summary>
    public const long TalismansEnhanceCost = 799000176;

    //幻兽相关
    public const long EudemonSealItem = 799000133;
    public const long EudemonRaceChange = 799000135;
    public const long EudemonWarehouseCapacity = 799000144;
    public const long EudemonColumnCapacity = 799000145;
    public const long EudemonRaceStarLimit = 799000146;
    public const long EudemonIdentityComposeCost = 799000166; //
    public const long EudemonRareFightLimit = 799000165; //珍兽出战数量限制

    public const long PayForCheckIn = 799000119; //补签花费

    public const long WorldLevel = 799000089; //世界等级
    public const long TeamExpAddition = 799000147; //组队经验加成
    public const long VIPExpAddition = 799000148; //VIP贵族经验加成

    public const long EudemonSealCost = 799000167; //封印消耗魔石
    public const long EudemonDismiss = 799000077; //幻兽放生返还
    public const long EudemonTalentCost = 799000172; //幻兽天赋领悟消耗
    public const long EudemonTalentScore = 799000173; //幻兽天赋评分
    public const long EudemonTalentComprehendCost = 799000174; //幻兽天赋领悟消耗
    public const long EudemonRaceScore = 799000018; //雷相性评分
    public const long EudemonBaseMaxValue = 799000133; //幻兽7条属性最大值

    //军团相关
    public const long ChangeGroupName = 799000045;
    public const long CreateGroup = 799000070;
    public const long GroupStationMapId = 799000150;
    public const long GroupDonateTypes = 799000036;
    public const long GroupDonateTypeOneCost = 799000037;
    public const long GroupDonateTypeTwoCost = 799000038;
    public const long GetHoonValueWithDonate = 799000039;
    public const long HoonTradeGoodsBoxCost = 799000040;
    public const long HoonTradeGoodsBoxDuration = 799000041;
    public const long HoonTradeGoodsBoxNum = 799000042;
    public const long DonateGetGroupMoney = 799000043;
    public const long GroupBuildingUpgradeDiscount = 799000046;
    public const long GroupRedPacketAmountMaxPercent = 799000154;
    public const long GroupRedPacketAmountAddPercent = 799000155;
    public const long GroupRedPacketStayTime = 799000157;

    public const long DropPos = 799000153; //掉落点

    //装备获得提示时间
    public const long QuickEquipmentTipTime = 799000189;
    //装备强制推送阶数
    public const long EquipRecommandRank = 799000251;

    //宝石秀
    public const long GemShowPer = 799000195; //宝石秀出现概率

    //冰封魔窟伤害鼓舞
    public const long FrozenCellarHarmPromoteByCoin = 799000193;
    public const long FrozenCellarHarmPromoteByStone = 799000194;

    //跑商，每日
    public const long DialyQuestNum = 799000198;
    public const long DialyQuestExtraAward = 799000199;
    //public const long DialyQuestNpcId= 799000200;
    public const long EscortTpPoints = 799000201;
    public const long EscortQuestId = 799000203;
    public const long EscortDoubleAwardTime = 799000204;

    //副本
    public const long RatingStar = 799000187; //副本评星

    public const long PickupRange = 799000207; //拾取范围
    public const long SeekRange = 799000208; //搜索范围
    public const long DemoncityTreasureBox = 799000209; //恶魔宝箱层数对应宝箱配置

    // 经验id
    public const int ExpIdItem = 299010004;

    public const int LadderLikeNum = 799000232;
    public const int MaxPKValue = 799000247; //pk值上限
    //法宝经验丹
    public const int TalismanExpItem = 799000256;
    //法宝祝福丹
    public const int TalismanStarItem = 799000257;
    //交易行上架金币消耗
    public const int StallSubGoldId = 799000255;
    // 不使用小飞鞋距离
    public const int FlyShoeLimit = 799000271;

    // VIP卡道具
    public const int VIPCard = 799000276;
    //交易行购买商品，输错密码的CD时间配置
    public const int Stall_BuyItem_InputPwd = 799000275;
    // 每日登陆VIP经验增加N点
    public const int VIPDayExp = 799000272;
    // 消费非绑定魔石100=1点VIP经验
    public const int VIPConsume = 799000273;
    // VIP到期后再上线提示到期的最小VIP等级
    public const int VIPMinLevel = 799000277;

    //MSDK 特权相关 - 炎龙宝藏金币加成
    public const int LoginTeQuan = 799000300;
    public const int QQVip = 799000301;
    public const int QQSvip = 799000302;

}
#endregion

#region [离散文本]
public class DiscreteTextTag
{
    public const long ExpLiquid15 = 899000100; //1级经验药水(1.5倍)
    public const long ExpLiquid20 = 899000101; //2级经验药水(2倍)
    public const long ExpLiquid30 = 899000102; //3级经验药水(3倍)
    public const long ExpAttenuation = 899000103; //经验衰减
    public const long GroupHallUpgradeInfo = 899000104; //军团大厅升级介绍
    public const long GroupHoonShopUpgradeInfo = 899000105; //战勋商店升级介绍
    public const long GroupDonateUpgradeInfo = 899000106; //军需库升级介绍
    public const long LineSameTips = 899000135; // 已在当前线路
    public const long Vip1Tile = 899000215;// VIP：星之祝福标题
    public const long Vip1Content = 899000216;// VIP：星之祝福内容
    public const long Vip2Tile = 899000217;//VIP：月之祝福标题
    public const long Vip2Content = 899000218;//VIP：月之祝福内容
    public const long Vip3Tile = 899000219;//VIP：神之祝福标题
    public const long Vip3Content = 899000220;//VIP：神之祝福内容
    public const long Vip_test = 899000221;//VIP：体验卡内容

}
#endregion

#region [道具类型]
public class ItemType
{
    public const short EquipGrowthMaterial = 1; //1-装备养成材料
    public const short Consumable = 2; //2-角色消耗
    public const short EudemonsGrowthMaterial = 3; //3-幻兽养成材料
    public const short TreasureBox = 4; //4-宝箱
    public const short EquipChip = 5; //5-装备碎片
    public const short Diamond = 6; //6-通用装备
    public const short EudemonsEgg = 11; //11-幻兽蛋
    public const short EudemonsSkillBook = 12; //12-幻兽技能书
    public const short FashionableDress = 13; //13-时装
    public const short Mount = 14; //14-坐骑
    public const short TaskItems = 15; //15-任务物品
    //预留，1-装备养成材料；
    //    2-角色消耗；
    //    3-幻兽养成材料；
    //    4-宝箱；
    //    5-未鉴定装备；
    //    6-宝石；
    //    7-装备锻造图纸；
    //    11-幻兽蛋；
    //    12-幻兽技能书；
    //    13-时装；
    //    14-坐骑；
    //    15-任务物品；
    //    16-随机卷轴；
    //    99-图标显示
}
#endregion

#region [幻兽技能天赋操作类型]
public class EudemonSkillOperatType
{
    public const short Study = 0; //学习
    public const short Up = 1; //洗或者升级
    public const short Forget = 2; //遗忘
}
#endregion

#region [技能类型]
public class SkillType
{
    public const short Unknown = -1; //未知
    public const short Skill = 0; //普通技能
    public const short Talent = 1; //天赋
    public const short Exclusive = 2; //专属技能
}
#endregion

#region [道具使用类型]
public class ItemUseType
{
    public const short UISkip = 1; //1-装备养成材料
    public const short ItemUseType2 = 2; //预留
    public const short ItemUseType3 = 3; //预留
    public const short ItemUseType4 = 4; //预留
    public const short AdditionBuff = 5; //添加Buff
    public const short ItemUseType6 = 6; //预留
    public const short Compose = 7; //7-装备碎片合成
    public const short LimitTimeDress = 11; //7-限时时装激活道具
    public const short SommonBoss = 14; //召唤boss，召唤怪物
    public const short Treasure = 15; //藏宝图
    public const short QuickUse = 16; //快速使用（1个直接使用，多个跳批量使用界面）
    public const short EudemonGift = 20; //幻兽礼包选择幻兽
    public const short DemonBox = 21; //幻兽礼包选择幻兽
    public const short PayGift = 22; //付费礼包
}
#endregion

#region 金币银币等特殊物品ID
public class SpecialItemId
{
    public const int SilverId = 299010001; //现在的金币
    public const int GoldId = 299010002; //绑定魔石
    public const int HonorId = 299010003; //魔石
    public const int EtId = 299010008;
    #region [经验药水ID]
    public const long EXP_LIQUID_15 = 208021101;
    public const long EXP_LIQUID_20 = 208021102;
    public const long EXP_LIQUID_30 = 208021103;
    #endregion
    public const long RandomTicket = 208021301; //随机券
    public const int ReliveWearyBuffId = 417000301; //复活疲劳
    public const long SmallFlyShoe = 208070101; //小飞鞋
    //boss令牌
    public static readonly List<int> BossToken = new List<int>
    {
        208021201,
        208021202,
        208021203,
        208021204,
        208021205,
        208021206
    };

    public static readonly List<int> TalismanExpItem = new List<int>
    {
        208010511,
        208010512,
        208010513
    };

    public static readonly List<int> TalismanStarItem = new List<int>
    {
       208010521,
       208010522
    };

    public static readonly List<int> TalismansPowerItem = new List<int>()
    {
        208010541,
        208010542,
        208010543
    };

    public static readonly List<int> TalismansLookItem = new List<int>()
    {
        208010531,
        208010532,
        208010533,
        208010534,
        208010535
    };
}
#endregion

#region 聊天按钮
public enum Buttons
{
    OK = 0, //消息框包含"确定”按钮。
    Cancel = 1, //消息框包含"取消”按钮。
    OKCancel = 2, //消息框包含"确定”和"取消”按钮。
}
#endregion

#region 章节副本

public class ClientSecneEventType
{
    public const int MonsterWave = 1;
    public const int AddEffect = 2;
    public const int RemoveEffect = 3;
}

public class ChapterClientAiType
{
    public const int MoverAndHurtToucher = 2;
    public const int Tp = 1;
}

public class ChapterClientAiTriggerType
{
    public const int Dialogue = 1;
    public const int OtherAi = 2;
    public const int EnterDuplicate = 3;
}

public class ChapterClientAiStopTriggerType
{
    public const int Tp = 1;
    public const int OtherAi = 2;
    public const int Timeline = 3;
    public const int ServerAi = 4;
}
#endregion

#region NPC功能类型
public class NpcFunctionType
{
    public const int OpenUI = 1;
}
#endregion

public enum ComposeMinorResultType
{
    OK = 0,
    NO_ENOUGH_MINOR,
    NO_ENOUGH_CLOVER,
    NO_ENOUGH_MASTER_LEVEL,
    USE_HIGH_LEVEL_MINOR,
    ALREADY_MAX_LEVEL,
    NONE,
}

#region [UI Name]
public class UIName
{
    /// <summary>
    /// 确认框
    /// </summary>
    public const string CommonConfirmUI = "CCommonConfirmUI";
    /// <summary>
    /// 用户信息及背包
    /// </summary>
    public const string PlayerInfoUI = "CPlayerInfoUI";
    /// <summary>
    /// 组队
    /// </summary>
    public const string TeamUI = "CTeamUI";
    /// <summary>
    /// 队伍进入活动确认框
    /// </summary>
    public const string TeamReadyConfirmUI = "CTeamReadyConfirmUI";
    /// <summary>
    /// 技能
    /// </summary>
    public const string SkillUI = "CSkillUI";
    /// <summary>
    /// 野外修炼经验加成
    /// </summary>
    public const string ExpAdditionPanelsUI = "CExpAdditionPanelsUI";
    /// <summary>
    /// 试炼之地地图
    /// </summary>
    public const string PracticeLandHUDUI = "CPracticeLandHUDUI";
    /// <summary>
    /// 试炼之地活动
    /// </summary>
    public const string PracticeLandUI = "CPracticeLandUI";
}
#endregion

#region 系统/活动ID

public class FunctionAndActivityID
{
    public const int Social = 300101;
    public const int Eudemon = 100301;
    public const int FitEudemon = 100302;
    public const int SkillBtn = 100101;
    public const int Skill2 = 100102;
    public const int Skill3 = 100103;
    public const int Equip = 100201;
    public const int EudemonCollege = 100308;
    public const int FieldPractice = 201101;
    public const int Compose = 300501;
    public const int DareBoss = 200002;
    public const int FieldBoss = 201701;
    public const int Activity = 200001;
    public const int DemonCity = 201001;
    public const int Nobility = 100501;
    public const int DragonTreasure = 200401;
    public const int FrozenCellar = 200301;
    public const int Group = 300201;
    public const int Auction = 300401;
    public const int Stall = 300301;
    public const int FameHall = 200501;
    public const int WorldBoss = 200701;
    public const int Duplicate = 200201;
    public const int PersonalBoss = 200801;
    public const int PrivilegedBoss = 200901;
    public const int GloryAbattoir = 201601;
    public const int Talisman = 100401;
    public const int Quiz = 201301;
    public const int FiendChallenge = 201401;
    public const int BattleGround = 201501;
    public const int BraveTower = 201201;
}

#endregion

#region [经验来源]
public enum EXPSource
{
    KILL_MONSTER = 0,
    OTHER
}
#endregion

public class FreshGuideType
{
    /// <summary>
    ///  幻兽出战
    /// </summary>
    public const int EudemonFight = 1;
    /// <summary>
    ///  幻兽合体
    /// </summary>
    public const int EudemonCombine = 5;

    /// <summary>
    ///  装备推荐
    /// </summary>
    public const int RecommendOperate = 2;
}

public enum P2P_PlayerType
{
    My,
    Other
}

public class RoleBodyType
{
    /// <summary>
    ///  大体型
    /// </summary>
    public const int Big = 1;
    /// <summary>
    ///   中等体型
    /// </summary>
    public const int Middle = 2;

    /// <summary>
    ///   小体型
    /// </summary>
    public const int Small = 3;
}

public class CDefines {

    public static class Layer
    {
        public const int Default = 0;
        public const int IgnoreRaycast = 2;
        public const int Water = 4;
        public const int UI = 5;
        public const int Player = 8;
        public const int MainPlayer = 9;
        public const int Effect = 10;
        public const int UIPlayer = 11;
        public const int Jump = 12;
        public const int Terrain = 13;
        public const int Fly = 14;
        public const int Prop = 15;
        public const int MainBuilding = 16;
        public const int Distortion = 17;
        public const int TimelineUI = 20;
        public const int Npc = 21;
        public const int UITerrain = 22;
        public const int Dialogue = 23;
        public const int Probe = 29;
        public const int SafeArea = 30;
        public const int SkyBox = 31;

        public static class Mask
        {
            public const int Default = 1 << Layer.Default;
            public const int IgnoreRaycast = 1 << Layer.IgnoreRaycast;
            public const int Water = 1 << Layer.Water;
            public const int UI = 1 << Layer.UI;
            public const int Player = 1 << Layer.Player;
            public const int MainPlayer = 1 << Layer.MainPlayer;
            public const int Effect = 1 << Layer.Effect;
            public const int UIPlayer = 1 << Layer.UIPlayer;
            public const int Jump = 1 << Layer.Jump;
            public const int Terrain = 1 << Layer.Terrain;
            public const int Fly = 1 << Layer.Fly;
            public const int Prop = 1 << Layer.Prop;
            public const int MainBuilding = 1 << Layer.MainBuilding;
            public const int Distortion = 1 << Layer.Distortion;
            public const int TimelineUI = 1 << Layer.TimelineUI;
            public const int Npc = 1 << Layer.Npc;
            public const int UITerrain = 1 << Layer.UITerrain;
            public const int Dialogue = 1 << Layer.Dialogue;
            public const int Probe = 1 << Layer.Probe;
            public const int SafeArea = 1 << Layer.SafeArea;
            public const int SkyBox = 1 << Layer.SkyBox;
        }
    }


    public enum NavLayerMask
    {
        EveryThing = -1,
        DEFAULT = 1,
        NOTWALKABLE = 2,
        JUMP = 4,
        AI = 8,
        PLAYER2 = 16,
        PLAYER3 = 32,
        PLAYER4 = 64,
        PLAYER5 = 128,
        PLAYER6 = 256,
        PLAYER7 = 512,
        PLAYER8 = 1024,
        PLAYER9 = 2048,
        PLAYER10 = 4096,
        PLAYER11 = 8192,
        PLAYER12 = 16384,
        PLAYER13 = 32768,
        PLAYER14 = 65536,
        PLAYER15 = 131072,
        PLAYER16 = 262144,
        PLAYER17 = 524288,
        PLAYER18 = 1048576,
        PLAYER19 = 2097152,
        PLAYER20 = 4194304,
        PLAYER21 = 8388608,
        PLAYER22 = 16777216,
        PLAYER23 = 33554432,
        PLAYER24 = 67108864,
        PLAYER25 = 134217728,
        PLAYER26 = 268435456,
        PLAYER27 = 536870912,
        PLAYER28 = 1073741824
    }
}
