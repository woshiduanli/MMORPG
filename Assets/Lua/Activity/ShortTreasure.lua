﻿-- local Global = require "LuaHelper.Global"
local ShortTreasure = class(Activity)

function ShortTreasure:ctor(at, name)
    Activity.ctor(self, at, name)
    self.AT = at
    self.Name = name

    -- this.LuaMgr:RegStringEvent(Def.EventTag.ShortTreasure.ResPickUpScore, ShortTreasure.ResPickUp)
    -- this.LuaMgr:RegStringEvent(Def.EventTag.ShortTreasure.ResPlayerInfo, ShortTreasure.ResPickUp)
    -- this.LuaMgr:RegStringEvent(Def.EventTag.ShortTreasure.ResFinish, ShortTreasure.ResFinish)
end

function ShortTreasure:RegEvents()
    NetManager.RegEvent(GameDef.NetEvent.Event, EventTag.ShortTreasure.ResPickUpScore, ShortTreasure.ResPickUp)
    NetManager.RegEvent(GameDef.NetEvent.Event, EventTag.ShortTreasure.ResPlayerInfo, ShortTreasure.ResPickUp)
    NetManager.RegEvent(GameDef.NetEvent.Event, EventTag.ShortTreasure.ResFinish, ShortTreasure.ResFinish)
end

function ShortTreasure.ResPlayerInfo(sn, obj)
    print("ResPlayerInfo:", Json.encode(obj))

    if obj.layer then
        ShortTreasure.CShortTreasureSceneUIData.CurLevel = obj.layer
    end

    if obj.score then
        ShortTreasure.CShortTreasureSceneUIData.CurScore = obj.score
    end

    Global.FireEvent(EventCode.ShortTreasure.UpdateCurScore)
end

-- 当前的层数
ShortTreasure.CurLevel = 1

-- 当前的分数
ShortTreasure.CurScore = 0

function ShortTreasure.ResPickUp(sn, obj)
    print("ResPickUp:", Json.encode(obj), Json.encode(sn))
    local t = obj
    if t.layer then
        ShortTreasure.CurLevel = t.layer
        ShortTreasure:FireEvent(EventCode.ShortTreasure.UpdateRewardLevel)
    end
    if t.score then
        ShortTreasure.CurScore = t.score
    end
    if ShortTreasure.CurLevel ~= ShortTreasure.FrontLevel then
    -- 以下内容更新奖励
    -- ShortTreasure.FrontLevel = ShortTreasure.CurLevel
    -- local curConfig = ReferenceManager:GetReference(Def.ReferenceDef.ShortTreasure.reference, ShortTreasure.CurLevel)
    -- ---@type LuaPropDefine
    -- ShortTreasure.UpScore = curConfig.LevelUpPoint

    -- CClientCommon.GetReward(curConfig.Rewards)
    -- for k1, v1 in pairs(curConfig.Rewards) do
    --     if v1.Type == "expRatio" then
    --         v1.Id = 299010004
    --     end
    -- end
    -- ShortTreasure.CurReward = nil
    -- ShortTreasure.CurReward = curConfig.Rewards
    -- this:FireEvent(Event.ShortTreasure.UpdateRewardLevel, ShortTreasure.CurReward)
    end
    ShortTreasure:FireEvent(EventCode.ShortTreasure.UpdateCurScore)
end

function ShortTreasure.GetUpScore()
    --- todo 也许层数降低
    local data = ReferenceManager.GetReference(GameDef.ReferenceDef.ShortTreasure.reference, ShortTreasure.CurLevel)
    if (data ~= nil) then
        ShortTreasure.UpScore = data.LevelUpPoint
    end
    return ShortTreasure.UpScore
end

--- 结算活动
function ShortTreasure.ResFinish(a, t)
    -- Main.Debug("结束:", t)
    -- local dataCur = Json.decode(t)
    -- ShortTreasure.CurLevel = dataCur[1]
    -- DuplicationResultData.Win = ShortTreasure.CurLevel > 0
    -- DuplicationResultData.ListRewardItemData = {}
    -- DuplicationResultData.MaxLevel = ShortTreasure.CurLevel
    -- DuplicationResultData.DuplicationType = Def.DuplicateResultType.ShortTreasure
    -- local i2 = 1
    -- for i = 1, ShortTreasure.CurLevel do
    --     local curConfig =  refMgr:GetReference(Def.ReferenceDef.ShortTreasure.reference, i)
    --     i2 = tostring(i2)
    --     local data = DuplicationResultData.RewardItem:New()
    --     CClientCommon.GetReward(curConfig.Rewards)
    --     --- 迭代出奖励
    --     for i, v in pairs(curConfig.Rewards) do
    --         local haveIt = false
    --         local v2Temp = nil
    --         for i2, v2 in pairs(DuplicationResultData.ListRewardItemData) do
    --             if v2.Id == v.Id then
    --                 haveIt = true
    --                 v2.num = v2.num + v.Amount
    --             end
    --         end
    --         if haveIt == false then
    --             local data = DuplicationResultData.RewardItem:New()
    --             data.type = v.Type
    --             data.Id = v.Id
    --             data.bind = v.Bind
    --             data.num = v.Amount
    --             table.insert(DuplicationResultData.ListRewardItemData, data)
    --         end
    --     end
    --     i2 = tonumber(i2) + 1
    -- end
    -- -- 这里改变倍率:::
    -- local refRoleLevel =   refMgr:GetReference(Def.ReferenceDef.RoleLevel.reference, this.Mp.Level)
    -- local allExpCount  = 0
    -- for k, v in pairs(DuplicationResultData.ListRewardItemData) do
    --     if v.type == "expRatio" then
    --         v.num = (refRoleLevel.ActivityBaseExp * v.num) / 100
    --         allExpCount = v.num + allExpCount
    --     end
    -- end
    -- DuplicationResultData.ExpCount = math.floor( allExpCount / 10000.0)
    -- this:FireEvent(CS.CEvent.UI.OpenUI("CDuplicationResultUI"))
    -- ShortTreasure.ClearData()
end

-- local Def = require "LuaHelper.LuaDef"
-- local Event = require "LuaHelper.LuaEventDef"
-- local Json = require "LuaHelper.JsonHelper"
-- local CClientCommon = require "LuaHelper.CClientCommon"
-- local DuplicationResultData = require "UI.DuplicationResult.DuplicationResultData"
-- local Main = require "Manager.Main"
-- local refMgr = require "Manager.ReferenceManager"
-- local this
-- local ShortTreasure = {}

-- function ShortTreasure.New(root)
--     o = {}
--     o.root = root
--     setmetatable(o, ShortTreasure)
--     ShortTreasure.__index = ShortTreasure
--     return o
-- end

-- function ShortTreasure.InitData(self)
--     this = self.root
-- end

-- function ShortTreasure.RegEvents(self)
--     ---- 排名改变
--     this.LuaMgr:RegStringEvent(Def.EventTag.ShortTreasure.ResPickUpScore, ShortTreasure.ResPickUp)

--     this.LuaMgr:RegStringEvent(Def.EventTag.ShortTreasure.ResPlayerInfo, ShortTreasure.ResPickUp)

--     ---- 活动结束
--     this.LuaMgr:RegStringEvent(Def.EventTag.ShortTreasure.ResFinish, ShortTreasure.ResFinish)
-- end

-- --- 上个层数
-- ShortTreasure.FrontLevel = 0

-- --- 当前的层数
-- ShortTreasure.CurLevel = 1

-- --- 当前的分数
-- ShortTreasure.CurScore = 0

-- --- 当前升级分数
-- ShortTreasure.UpScore = nil
-- function ShortTreasure.GetUpScore()
--     --   CS.CGameLuaUI
--     --- todo 也许层数降低
--     if ShortTreasure.UpScore == nil then
--         ShortTreasure.UpScore = 0
--         local data = refMgr:GetReference(Def.ReferenceDef.ShortTreasure.reference, ShortTreasure.CurLevel)
--         if (data ~= nil) then
--             ShortTreasure.UpScore = data.LevelUpPoint
--         end
--     end
--     return ShortTreasure.UpScore
-- end

-- --- 当前的奖励
-- ShortTreasure.CurReward = nil

-- function ShortTreasure.ResPickUp(a, jsonStr)
--     -- Main.Debug("ResPickUp:", t)
--     local t, a, error = Json.decode(jsonStr)
--     if (error == nil) then
--         if t.layer then
--             ShortTreasure.CurLevel = t.layer
--         end
--         if t.score then
--             ShortTreasure.CurScore = t.score
--         end
--         if ShortTreasure.CurLevel ~= ShortTreasure.FrontLevel then
--             ShortTreasure.FrontLevel = ShortTreasure.CurLevel
--             local curConfig = refMgr:GetReference(Def.ReferenceDef.ShortTreasure.reference, ShortTreasure.CurLevel)
--             ---@type LuaPropDefine
--             ShortTreasure.UpScore = curConfig.LevelUpPoint

--             CClientCommon.GetReward(curConfig.Rewards)
--             for k1, v1 in pairs(curConfig.Rewards) do
--                 if v1.Type == "expRatio" then
--                     v1.Id = 299010004
--                 end
--             end
--             ShortTreasure.CurReward = nil
--             ShortTreasure.CurReward = curConfig.Rewards
--             this:FireEvent(Event.ShortTreasure.UpdateRewardLevel, ShortTreasure.CurReward)
--         end
--         this:FireEvent(Event.ShortTreasure.UpdateCurScore)
--     end
-- end

-- function ShortTreasure.ClearData()
--     ShortTreasure.CurLevel = 0
-- end

-- --- 结算活动
-- function ShortTreasure.ResFinish(a, t)
--     Main.Debug("结束:", t)

--     local dataCur = Json.decode(t)

--     ShortTreasure.CurLevel = dataCur[1]
--     DuplicationResultData.Win = ShortTreasure.CurLevel > 0
--     DuplicationResultData.ListRewardItemData = {}
--     DuplicationResultData.MaxLevel = ShortTreasure.CurLevel
--     DuplicationResultData.DuplicationType = Def.DuplicateResultType.ShortTreasure

--     local i2 = 1
--     for i = 1, ShortTreasure.CurLevel do

--         local curConfig =  refMgr:GetReference(Def.ReferenceDef.ShortTreasure.reference, i)
--         i2 = tostring(i2)
--         local data = DuplicationResultData.RewardItem:New()
--         CClientCommon.GetReward(curConfig.Rewards)

--         --- 迭代出奖励
--         for i, v in pairs(curConfig.Rewards) do
--             local haveIt = false
--             local v2Temp = nil
--             for i2, v2 in pairs(DuplicationResultData.ListRewardItemData) do
--                 if v2.Id == v.Id then
--                     haveIt = true
--                     v2.num = v2.num + v.Amount
--                 end
--             end
--             if haveIt == false then
--                 local data = DuplicationResultData.RewardItem:New()
--                 data.type = v.Type

--                 data.Id = v.Id
--                 data.bind = v.Bind
--                 data.num = v.Amount
--                 table.insert(DuplicationResultData.ListRewardItemData, data)
--             end
--         end
--         i2 = tonumber(i2) + 1
--     end

--     -- 这里改变倍率:::
--     local refRoleLevel =   refMgr:GetReference(Def.ReferenceDef.RoleLevel.reference, this.Mp.Level)
--     local allExpCount  = 0
--     for k, v in pairs(DuplicationResultData.ListRewardItemData) do
--         if v.type == "expRatio" then
--             v.num = (refRoleLevel.ActivityBaseExp * v.num) / 100
--             allExpCount = v.num + allExpCount
--         end
--     end

--     DuplicationResultData.ExpCount = math.floor( allExpCount / 10000.0)
--     this:FireEvent(CS.CEvent.UI.OpenUI("CDuplicationResultUI"))
--     ShortTreasure.ClearData()
-- end

-- --- 获取配置
-- ShortTreasure.Config = nil
-- function ShortTreasure.GetConfig()
--     if ShortTreasure.Config == nil then
--         ShortTreasure.Config = refMgr:GetReferences(Def.ReferenceDef.ShortTreasure.reference)
--     end
--     return ShortTreasure.Config
-- end

-- function OnDestroy()
-- end

-- 活动开启时间
function ShortTreasure.GetActivityOpenTime(a, t)
end

ShortTreasure.CShortTreasureSceneUIData = {}
-- 当前层
ShortTreasure.CShortTreasureSceneUIData.CurLevel = 0
-- 当前分数

ShortTreasure.CShortTreasureSceneUIData.CurScore = 0
ShortTreasure.CShortTreasureSceneUIData.GetCurScoreStr =
    function()
    local data =
        ReferenceManager.GetReference(
        GameDef.ReferenceDef.ShortTreasure,
        ShortTreasure.CShortTreasureSceneUIData.CurScore
    )

    if (data == nil) then
        Log.error(
            "[shortTreasure] not find data   ShortTreasure.CShortTreasureSceneUIData.CurScore is :",
            ShortTreasure.CShortTreasureSceneUIData.CurScore
        )
        return "1/10"
    end

    return string.format("%s/%s", ShortTreasure.CShortTreasureSceneUIData.CurScore, data.LevelUpPoint)
end

ShortTreasure.CShortTreasureMainUIData = {}

return ShortTreasure
