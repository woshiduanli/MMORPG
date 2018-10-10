---@type CGameLuaUI
local this
local Def = require "LuaHelper.LuaDef"
local Event = require "LuaHelper.LuaEventDef"
local CClientCommon = require "LuaHelper.CClientCommon"
local data = require "Activity.ShortTreasure"
local Helper = require "LuaHelper.Helper"
local refMgr = require "Manager.ReferenceManager"

local ShortTreasureScene = {isFullScreen = false, autoLoad = true, Layer = 0}

function ShortTreasureScene.New(root)
    o = {}
    o.root = root
    setmetatable(o, ShortTreasureScene)
    ShortTreasureScene.__index = ShortTreasureScene
    return o
end

function ShortTreasureScene.Start(self)
    this = self.root
    ShortTreasureScene.CurScore = this:Get("CurScore", typeof(Def.CommonClass.Text))
    ShortTreasureScene.CurLevel = this:Get("CurLevel", typeof(Def.CommonClass.Text))
    ShortTreasureScene.RewardGet_ = this:Get("RewardGet_ ", typeof(Def.CommonClass.Transfrom))

    ShortTreasureScene.RewardItem = this:AddBehaviour("RewardItem", typeof(Def.CommonClass.ItemWiget))

    CS.CClientCommon.SetActiveOverload(ShortTreasureScene.RewardItem.gameObject, false)
    CS.CClientCommon.SetActiveOverload(ShortTreasureScene.RewardGet_.gameObject, false)

    self.root:RegEventHandler(Event.ShortTreasure.UpdateCurScore, ShortTreasureScene.UpdateCurScore)
    self.root:RegEventHandler(Event.ShortTreasure.UpdateRewardLevel, ShortTreasureScene.UpdateRewardLevel)
end

function ShortTreasureScene.OnUIEnable(self)
    this:FireEvent(CS.CEvent.Auto.ActiveAttack())

    this:FireEvent(CS.CEvent.MainFace.ShowLeftObj(false))
    this:FireEvent(CS.CEvent.MainFace.BeastInstitute(false))
    ShortTreasureScene.UpdateRewardLevel(data.CurReward, true)
    ShortTreasureScene.UpdateCurScore()
end

function ShortTreasureScene.UpdateCurScore()
    ShortTreasureScene.CurScore.text = data.CurScore .. "/" .. data.GetUpScore()
end

function ShortTreasureScene.UpdateRewardLevel(RewardData, EnabelIt)
    ShortTreasureScene.CurLevel.text = CS.CString.Format(CS.Localization.Get("DRESS_RAISE_AIREN"), data.CurLevel)
    CS.CClientCommon.SetActiveOverload(
        ShortTreasureScene.RewardGet_,
        data.CurLevel == CClientCommon.GetTableLength(data.GetConfig())
    )

    if EnabelIt == nil then
        ShortTreasureScene.rankItemList = nil
    end
    if RewardData then
        local num =
            Helper.CClientCommon.GetTableLength(RewardData) -
            Helper.CClientCommon.GetTableLength(ShortTreasureScene.rankItemList)

        if EnabelIt then
            ShortTreasureScene.rankItemList =
                Helper.CClientCommon.CloneCUIElement(
                ShortTreasureScene.RewardItem,
                num,
                ShortTreasureScene.rankItemList
            )
        else
            ShortTreasureScene.rankItemList = Helper.CClientCommon.Clone(ShortTreasureScene.RewardItem, num)
        end
        local num2 = 0

        local refRoleLevel = refMgr:GetReference(Def.ReferenceDef.RoleLevel.reference, this.Mp.Level)
        -- local refRoleLevel = this.LuaMgr:GetReference(this.Mp.Level, Def.ReferenceDef.RoleLevel.reference)
        for k, v in pairs(RewardData) do
            if ShortTreasureScene.rankItemList[tonumber(k)] == nil then
                return
            end
            if v.Type == "expRatio" then
                num2 = tonumber((refRoleLevel.ActivityBaseExp * v.Amount) / 100)
                ShortTreasureScene.rankItemList[tonumber(k)]:SetItem(v.Id, tostring(math.ceil(num2)))
            else
                ShortTreasureScene.rankItemList[tonumber(k)]:SetItem(v.Id, tostring(math.ceil(v.Amount)))
            end
            ShortTreasureScene.rankItemList[tonumber(k)].tipsBtnsEnable = false
        end
    end
end

function ShortTreasureScene.OnDestroy(self)
    ShortTreasureScene.rankItemList = nil
end
--- testCode
function ShortTreasureScene.Update(self)
    local Input = CS.UnityEngine.Input
    local KeyCode = CS.UnityEngine.KeyCode

    if Input.GetKeyDown(KeyCode.F3) then
        data.ResFinish()
    end
end

return ShortTreasureScene
