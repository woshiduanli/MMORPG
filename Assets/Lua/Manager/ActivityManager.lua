--[[
	desc: lua 活动管理器
	author: CJ
	create: 2018-06-20
]]
local ActDef = GameDef.ActivityType

local ActivityManager = {}

function ActivityManager.InitData()
    ActivityManager.Activitys = {}
    ActivityManager.AllActivitys = {}
    ActivityManager.ActLiveness = 0
    ActivityManager.ActRewarded = {}
    ActivityManager.LikedPlayer = {}
    ActivityManager.Liked = 0

    ActivityManager.InitActivitys()
    ActivityManager.RegEvents()
end

function ActivityManager.InitActivitys()
    --示例
    ActivityManager.Add(ActDef.ShortTreasure.Id,ActDef.ShortTreasure.ActName)
end

function ActivityManager.Add(at,path)
    local activity = ActivityManager.AllActivitys[at]
    if activity == nil then
        ActivityManager.AllActivitys[at] = string.format ("Activity.%s", path) 
    else
        Log.erro(string.format("AT = %s , Name = %s is RePeat", at , path))
    end
end

function ActivityManager.RegEvents()
    Global.RegEvent(EventCode.MainPlayer.ReadData , ActivityManager.OnReadActivityData)
    NetManager.RegEvent(GameDef.NetEvent.Event, EventTag.Activity.Update , ActivityManager.UpdateActivityData)
end

function ActivityManager.OnReadActivityData(data)
    ActivityManager.Activitys = {}
    for at, path in pairs(ActivityManager.AllActivitys) do
        local def = require(path)
        ActivityManager.Activitys[at] = def.new(at,path)
    end 

    local tempData = data.activity_data
    for at, value in pairs(tempData) do  
        local activity = ActivityManager.GetFromAT(at)
        if activity then
            activity:InitActData(value)
        end
    end 

    local liveness = data.liveness
    if data.liveness then
        ActivityManager.ActLiveness = liveness
    end

    local rewarded = data.rewarded
    if rewarded then
        ActivityManager.ActRewarded = rewarded
    end

    local likes = data.likes
    if likes then
       ActivityManager.Liked = likes.liked
       for ladderType, value in pairs(likes) do
         if ladderType ~= "liked" then
            local players = ActivityManager.LikedPlayer[ladderType]
            if players == nil then
                players = {}
                ActivityManager.LikedPlayer[ladderType] = players
            end
            for playerId, v in pairs(value) do
                players[playerId] = true
            end
         end
       end
    end
end

function ActivityManager.UpdateActivityData(data)
    if type(data) ~= "table" then 
        return 
    end 
    for at, value in pairs(data) do  
        local activity = ActivityManager.GetFromAT(at)
        if activity then
            activity:UpdateData(value)
        end
    end 

    local liveness = data.liveness
    if data.liveness then
        ActivityManager.ActLiveness = liveness
        ActivityManager.Event:FireEvent(EventCode.RedDot.Refresh , GameDef.RedDotType.Activity)
    end

    local rewarded = data.rewarded
    if rewarded then
        ActivityManager.ActRewarded = rewarded
        ActivityManager.Event:FireEvent(EventCode.Activity.RewardedDataChange)
        ActivityManager.Event:FireEvent(EventCode.RedDot.Refresh , GameDef.RedDotType.Activity)
    end

    local likes = data.likes
    if likes then
        local ladderType,playerId = table.unpack(likes)
        local players = ActivityManager.LikedPlayer[ladderType]
        if players == nil then
            players = {}
            ActivityManager.LikedPlayer[ladderType] = players
        end
        if players[playerId] == nil then
            ActivityManager.Liked = ActivityManager.Liked + 1
            players[playerId] = true
            ActivityManager.Event:FireEvent(EventCode.Activity.LikeDataUpdate , playerId)
        end
    end
    ActivityManager.Event:FireEvent(EventCode.SignIn.RetrieveActivityUpdateRoleData)
end

function ActivityManager.GetFromAT(at)
    return ActivityManager.Activitys[at]
end

--打开先前的活动ui
function ActivityManager.OpenPreActivityUI()
    for _, act in pairs(ActivityManager.Activitys) do  
       if act.PreLoadUI then
          act:OpenUI()
          act.PreLoadUI = false
       end
    end 
end

return ActivityManager
