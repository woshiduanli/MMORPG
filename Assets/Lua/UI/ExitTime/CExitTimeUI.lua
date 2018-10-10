local base = GameUI
local CExitTimeUI = class(base)
local thisData = require("UI.ExitTime.CExitTimeUIData")

function CExitTimeUI:ctor(ui)
    base.ctor(self, ui)
    self.IsFullScreen = false
    self.Layer = UIManager.Layer.MainFace
end

function CExitTimeUI.Initialize(self)
    self:SetEvent(self.HashIDTable.button_exit, UIManager.TriggerEventID.PointerClick, thisData.exitBtnClick)
end

function CExitTimeUI.UIEnable(self)
    self.Loop = true
    base.UIEnable(self)
    self:SetTimeString(self.HashIDTable.text_time, thisData.GetRemainTime())
end

-- 当前每秒执行一次
function CExitTimeUI:OnUpdate()
    if thisData.GetRemainTime() <0 then
        return
    end

    self:SetTimeString(self.HashIDTable.text_time, thisData.GetRemainTime())
end

function CExitTimeUI.OnDestroy(self)
    thisData.ClearData()
end

return CExitTimeUI
