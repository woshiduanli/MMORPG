--[[
	desc: 	定义LUA层的所有事件 注意事件代号不能出现相同的
	author: CJ
	create: 2018-06-16
]]

local EventTag = { }

EventTag.Activity = 
{ 
	Update = "activity:update_role_data", 
}

EventTag.Login = 
{
    PlayerDBList = "character:list",
    Login = "login",
    LoginInfo = "",
    EnterGame = "enter",
    GotoScene = "goto_scene",
}

EventTag.World = 
{
    EnterPlayer = "appear",
    EnterMonster = "appear_animal",
    DeleteRole = "disappear",
    RefreshMonster = "refresh_animal",
}

EventTag.Duplicate={
    BattleResult="instance_result",
    TargetChange="instance_target"
}

EventTag.Battleground = {
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
}

EventTag.ShortTreasure = {
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
}

EventTag.MindQuiz = {
    PlayerEnter = "mind_quiz:player_enter",
    MindQuizAnswer = "mind_quiz:player_answer",
}
return EventTag