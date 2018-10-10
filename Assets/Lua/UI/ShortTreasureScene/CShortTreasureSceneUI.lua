local base = GameUI
local CShortTreasureSceneUI = class(base)
local thisData = require("Activity.ShortTreasure")

function CShortTreasureSceneUI:ctor(ui)
    base.ctor(self, ui)
    self.IsFullScreen = false
    self.Layer = UIManager.Layer.MainFace
end

function CShortTreasureSceneUI.Initialize(self)
    self:RegEvent(EventCode.ShortTreasure.UpdateCurScore, self:GetSelfFunc(self.UpdateScore))
    self:RegEvent(EventCode.ShortTreasure.UpdateRewardLevel, self:GetSelfFunc(self.UpdateRewardLevel))
end

function CShortTreasureSceneUI:UpdateScore()
    local str = string.format("%s/%s", thisData.CurScore, thisData.GetUpScore())
    print("更新分数", str)
    self:SetCText(self.HashIDTable.CurScore, str)
end

function CShortTreasureSceneUI:UpdateRewardLevel()
    print("更新层数")
    self:SetCText(self.HashIDTable.CurLevel, thisData.CurLevel)
end

function CShortTreasureSceneUI.UIEnable(self)
    local str = string.format("%s/%s", thisData.CurScore, thisData.GetUpScore())
    self:SetCText(self.HashIDTable.CurScore, str)
    self:SetCText(self.HashIDTable.CurLevel, thisData.CurLevel)
end

function CShortTreasureSceneUI.OnDestroy(self)
    thisData.CurScore = 0 
end

return CShortTreasureSceneUI
