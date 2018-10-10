--[[
	desc: lua 游戏相关常量定义
	author: CJ
	create: 2018-06-15
]]

local GameDef = {}

GameDef.RoleType = 
{
   Player = 0,
   Monster = 1,
   Eudemons = 2,
   NPC = 3,
   MapObject = 4,
   VirTual = 5,--离线玩家，虚拟玩家
   Summon = 6,--召唤物
   EudSoul = 7,--死亡幻兽
}

GameDef.MonsterType =
{
    Normal = 1,
    Elite = 2,
    Boss = 3,
    WorldBoss = 4,
}

GameDef.DunGeonType = {
    StarStep = 1,
    --"星级"
    MonsterSoul = 2,
    --"兽魂"
    ChapterDungeon = 3,
    --"章节副本"
    PVP = 4,
    --"pvp战场"
    WorldBoss = 5,
    --世界boss
    FieldHanGup = 6,
    --野外挂机
    WorldBossBattle = 7,
    --世界boss抢夺
    BraveTower = 8,
    -- 勇士之塔
    BattleFieldReady = 9,
    --战场准备
    BattleField = 10,
    --战场
    FameHall = 11,
    --名人堂
    WorldBoss1 = 12,
    ---世界BOSS
    PersonalBoss = 13,
    ---个人BOSS
    PrivilegeBoos = 14,
    ---特权BOSS
    WorldBossReal = 16,
    ---世界boss真
    PracticeAbyss = 17,
    --17-深渊迷宫地图组
    PracticeCorridors = 18,
    --18-冰封走廊地图组
    GroupStation = 19,
    --军团驻地
    DemonCity = 20,
    --恶魔塔
    FrozenCellar = 21,
    --冰封魔窟
    DragonTreasure = 22,
    --巨龙宝藏
    StormCellar = 23,
    --材料副本 风暴岩窟
    RoyalMine = 24,
    --金矿副本
    ThunderForbidden = 25,
    --金矿副本
    BossHome = 26,
    --BOSS之家副本
    --圣域战场
    Daily=27,
    --日常副本
    Battleground = {
        Id = 29,
        -- 脚本名
        WorldName = "BattlegroundWorld"
    },
    ShortTreasure = {
        Id = 28,
        -- 脚本名
        WorldName = "ShortTreasureWorld"
    }
}

GameDef.MapType = {
    CITY = 1,
    --"城市"
    FIELD = 2,
    --"野外"
    DUNGEON = 3,
    --"副本"
    WILDBOSS = 4,
    --"野外boss"
    PRACTICE = 6,
    --试炼之地
    BOSSHOME = 7
    --BOSS之家
}

GameDef.ActivityType = {
    None = 0,
    TeamFight = 1,
    BraveTower = 3,
    StarStep = 101,
    FieldPractice = 102,
    FameHall = 103,
    BossChallenges = 104,
    DemonCity = 105,
    PracticeLand = 106,
    GodTrial = 107,
    FrozenCellar = 108,
    DragonTreasure = 109,
    StormCellarLua = 110,
    DailyQuest = 111,
    Escort = 112,
    RoyalMine = 113,
    ThunderForbidden = 114,
    ---  boss 之家 115

    Battleground = {
        Id = 116,
        -- lua脚本名
        ActName = "Battleground"
    },
    --- 矮人宝库
    ShortTreasure = {
        Id = 117,
        -- lua脚本名
        ActName = "ShortTreasure"
    },
    MindQuiz = 119,
}

GameDef.LoadSceneType =
{
    Internal = 0, --当前场景直接切换
    AsyncLevel = 1, --通过过图场景切换
}

GameDef.ReferenceDef = {
    --- npc
    NPC = "npcrole",
    Monster = "monsterrole",
    Player = "mprole",
    RoleExp = "roleexp",
    EquipRef = "equip",
    DressRef = "newdress",
    MountRef = "mount",
    PropertyType = "propertytype",
    MapObject = "mapobject",
    Map = "map",
    --- 战场
    Battleground = {
        config = "activities",
        reference = "sacredbattlefield",
        view = "c_sacred_battlefield"
    },
    --- 矮人宝库
    ShortTreasure = {
        config = "activities",
        reference = "shorttreasure",
        view = "c_short_treasure"
    },
    --- 角色等级
    RoleLevel = {
        reference = "rolelevel"

        -- RoleLevel
        -- RoleLevlReferver
    },
    ---副本目标
    MapTarget = {
        reference = "maptarget"
    },
    ---活动外部界面
    ActivityExhibition = {
        reference = "activityexhibition"
    },
    ---风暴炎库
    StormCellar = {
        reference = "stormcellar"
    },
    --- 离散数据
    DiscreteReference = {
        --config = "discrete",
        reference = "discrete",
        --view = "c_all",
    },
    --- 智力问答问题配置
    MindQuizQuestion = {
        config = "activities",
        reference = "qa_quiz_lib",
        view = "c_qa_quiz_lib"
    },
    --- 智力问答奖励配置
    MindQuizReward = {
        config = "activities",
        reference = "qa_quiz_rewards",
        view = "c_qa_quiz_rewards"
    },

    -- 称号
    Appellation = {
        config = "actors",
        reference = "apellation",
        view = "c_appellation"
    },
}


GameDef.EventTag = {
    Battleground = {
        -- 新玩家数据
        ResNewOne = "battle_field:new_one",
        ReqLeftTime = "battle_field:get_time_info",
        ResTimeInfo = "battle_field:time_info",
        ReqPlayerEnter = "battle_field:player_enter",
        -- 活动没开启，进入时间已经过期
        ReqLevelIt = "battle_field:player_leave",
        -- 排名变化
        ResRankChange = "battle_field:rank_change",
        -- 结算消息， 空消息
        ResFinish = "battle_field:finished"
    },
    ShortTreasure = {
        --
        ResTimeInfo = "short_treasure:time_info",
        --- 层数获取
        ResPlayerInfo = "short_treasure:layer_info",
        ResPickUpScore = "short_treasure:pickup_a_score",
        --- 进入
        ReqPlayerEnter = "short_treasure:player_enter",
        -- 活动没开启，进入时间已经过期
        ReqLevelIt = "short_treasure:player_leave",
        -- 结算消息， 空消息
        ResFinish = "short_treasure:completed"
    },

    MindQuiz = {
        PlayerEnter = "mind_quiz:player_enter",
        MindQuizAnswer = "mind_quiz:player_answer",
    }, 

    ExitDuplicate = "exit_instance"


}


--- 游戏中的Font
GameDef.GameFont = {
    battleground1 = "battleground1",
    battleground2 = "battleground2",

}

--- 离散常量
GameDef.CommonPara = {
    --- 副本退出冷却时间
    CoolDownStrExitDuplication = 5
}

--- 副本结果枚举
GameDef.DuplicateResultType = {
    ShortTreasure = 0,
    Battleground = 1
}

GameDef.RedDotType = 
{
    Skill = 0,
    Equip = 1,
    Eudemon = 2,
    Mail = 3,
    Package = 4,
    Peek = 5,
    Quest = 6,
    Shop = 7,
    Auction = 8,
    Apeallation = 9,
    Chat = 10,
    Clothes = 11,
    Group = 12,
    Mount = 13,
    Compose = 14,
    ComposeItem = 15,
    ComposeGroup = 16,
    ComposeCategory = 17,
    Social = 18,
    SocialNewApply = 19,
    Equip_Enhance = 20,
    Equip_Forge = 21,
    Equip_Gem = 22,
    EudemonCollege_main = 23,
    Team_Apply = 24,
    Activity = 25,
}

GameDef.ConstFilePath = 
{
    QuestEmotionSignPath = "res/effect/ui/go_ui_gantanhao.go",
    QuestQuestionMarkSignPath = "res/effect/ui/go_ui_wenhao.go",
}

GameDef.PropertyType =
{
    NAME = "name",
    LEVEL = "level",
    HP = "hp",
    MaxHP = "health",
    XP = "xp",
    MAXXP = "maxXP",
    EXP = "exp",
    COIN = "coin",
    Ep = "ep",
    BEp = "bep",
    Et = "et",
    TITLE = "title",
    CAMP = "camp",
    PKMODE = "pk_mode",
    FIGHTVALUE = "battlePower",
    STRENGTH = "strength", --力量
    INTELLIGENCE = "intelligence", --智力
    ENDURANCE = "endurance", --耐力
    VITALITY = "vitality", --体质
    DEXTERITY = "dexterity", --敏捷
    PHYSICALATTACK = "physicalAttack", --物理攻击
    MINPHYSICALATTACK = "minPhysicalAttack", --物攻min
    MAXPHYSICALATTACK = "maxPhysicalAttack", --物攻max
    MAGICATTACK = "magicAttack", --魔法攻击	
    MINMAGICATTACK = "minMagicAttack", --魔攻min	
    MAXMAGICATTACK = "maxMagicAttack", --魔攻max	
    ARMOR = "armor", --物理防御	
    RESISTANCE = "resistance", --魔法防御	
    FP = "fp", --怒气值	
    XPRECORVERY = "xpRecovery", --XP回速
    HITRATING = "hitRating", --命中值	
    DODGERATING = "dodgeRating", --闪避值
    CRITICALCHANCE = "criticalChance", --暴击率
    CRITICALRATING = "criticalRating", --暴击值
    CRITICALDAMAGE = "criticalDamage", --暴击伤害
    CRITICALDAMAGERATING = "criticalDamageRating", --暴击伤害值
    CRITICALRESISTANCE = "criticalResistance", --抗暴击率
    CRITICALRESISTANCERATING = "criticalResistanceRating", --抗暴击率值
    CRITICALDAMAGEREDUCTION = "criticalDamageReduction", --抗暴击伤害
    CRITICALDAMAGEREDUCTIONRATING = "criticalDamageReductionRating", --抗暴击伤害值
    DAMAGEREDUCTION = "damageReduction", --伤害减免
    PHYSICALDAMAGEREDUCTION = "physicalDamageReduction", --物理伤害减免
    MAGICDAMAGEREDUCTION = "magicDamageReduction", --魔法伤害减免
    DAMAGEINCREASE = "damageIncrease", --伤害增加
    PYSICALDAMEGEINCREASE = "physicalDamageIncrease", --物理伤害增加
    MAGICDAMAGEINCREASE = "magicDamageIncrease", --魔法伤害增加
    TRUEDAMAGEINCREASE = "trueDamageIncrease", --最终伤害增加
    TRUEDAMAGEREDUCTIN = "trueDamageReduction", --最终伤害减免
    MOVESPEED = "moveSpeed", --移动速度
    HIT = "hit", --命中率
    DODGE = "dodge", --闪避率
    BODYSTATE = "body_state", --xp变身状态
    XPSTATE = "state", --xp变身状态
    XPDURATION = "duration", --xp持续时间
    MOUNTING = "mounting_id", --坐骑id
    APPELLATION = "appellation", --称号
    Prestige = "prestige_level", --威望
    Nobility = "nobility", --爵位
    TEAMID = "team_id", --当前队伍id
    GROUPID = "group_id", --军团id
    GROUPNAME = "group_name", --军团名字
    GROUPMEMBERTYPE = "member_type",
    GROUPSECONDMEMBERTYPE = "second_member_type",
    EudemonInBodyHp = "eudemon_inbody_hp", --合体当前HP
    EudemonInBodyMaxHp = "eudemon_inbody_health", --合体最大HP
    EudemonHelth = "health", --幻兽总血量
    EUDEMONLIMIT = "eudemon_limit",
    OFFLINEHOSTING = "offline_hosting",
    DEVILLAYER = "devil_tower_layer", --恶魔塔层数（挑战成功的）
    RANK = "rank", --兽兽进化
}

GameDef.MPSystem = 
{
    Equip = {Name = "Equip" , Path = "GameSystem.EquipSystem" ,}
}

GameDef.SceneName = {
     LOGIN = "Login"
}

GameDef.EventTriggerType =
{
    PointerEnter = 0,
    PointerExit = 1,
    PointerDown = 2,
    PointerUp = 3,
    PointerClick = 4,
    Drag = 5,
    Drop = 6,
    Scroll = 7,
    UpdateSelected = 8,
    Select = 9,
    Deselect = 10,
    Move = 11,
    InitializePotentialDrag = 12,
    BeginDrag = 13,
    EndDrag = 14,
    Submit = 15,
    Cancel = 16,
}

GameDef.UI = {
    ShortTreasure = {
        path = "",
        main = "",
    },

    Battleground = {
        path = "",
        main = "",
    },
    -- Battleground
}

GameDef.NetEvent = 
{
    Event = 0,
    Info = 1,
    Response = 2,
    Message = 3,
} 

return GameDef
