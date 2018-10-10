local base = World
local ShortTreasureWorld = class(base)
local CExitTimeUIData = require("UI.ExitTime.CExitTimeUIData")
local CCommonConfirmUIData   = require("UI.CommonConfirm.CCommonConfirmUIData")

function ShortTreasureWorld:InitData(mr)
    base.InitData(self, mr)
    NetManager.RegEvent(GameDef.NetEvent.Event, EventTag.ShortTreasure.ResTimeInfo, ShortTreasureWorld.ResTimeInfo)
end

function ShortTreasureWorld.ResTimeInfo(sn, t)
    local d = Json.encode(t)
    if t.end_time == "infinity" then
        Log.erro("[ShortTreasureWorld.ResTimeInfo] server data is error, time is infinity")
        return
    else
        if t.begin_time ~= nil and t.end_time ~= nil then
            CExitTimeUIData.endTime = t.end_time
            CExitTimeUIData.startTime = t.begin_time
        end
    end

    CExitTimeUIData.exitBtnClick = ShortTreasureWorld.ExitMap
    -- 打开结果ui
    ShortTreasureWorld:FireEvent(EventCode.UI.OpenUI, "ExitTime")
end

function ShortTreasureWorld.ExitMap()
    CCommonConfirmUIData.DefaultTipStr = "只要活动没有结束，退出后可再进入此活动"
    ShortTreasureWorld:FireEvent(EventCode.UI.OpenUI, "CommonConfirm")
end

function ShortTreasureWorld:CreateMainFace()
    ShortTreasureWorld:FireEvent(EventCode.UI.OpenUI, "ShortTreasureScene")
end



return ShortTreasureWorld
