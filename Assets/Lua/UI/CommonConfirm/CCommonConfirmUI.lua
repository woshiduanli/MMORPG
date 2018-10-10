local base = GameUI
local CCommonConfirmUI = class(base)
local thisData = require("UI.CommonConfirm.CCommonConfirmUIData")

function CCommonConfirmUI:ctor(ui)
    base.ctor(self, ui)
    self.IsFullScreen = false
    self.Layer = UIManager.Layer.Free
end

function CCommonConfirmUI.Initialize(self)
    self:SetEvent(self.HashIDTable.button_close, UIManager.TriggerEventID.PointerClick, self:GetSelfFunc(self.Close))

    self:SetEvent(self.HashIDTable.button_OkNode, UIManager.TriggerEventID.PointerClick, CCommonConfirmUI.ReqExit)

    --  self:SetEvent(self.HashIDTable.button_OkNode, UIManager.TriggerEventID.PointerClick,CCommonConfirmUIReqExit)
end

function CCommonConfirmUI.ReqExit()
    if (thisData.SureBtnCallBack == nil) then
        CSGlobal.Request(GameDef.EventTag.ExitDuplicate)
    else
        thisData.SureBtnCallBack()
    end
end

function CCommonConfirmUI.UIEnable(self)
    self:SetCText(self.HashIDTable.text_content, thisData.DefaultTipStr)
end

function CCommonConfirmUI.UIDisable(self)
    thisData.ReSetData()
end

function CCommonConfirmUI.OnDestroy(self)
end

return CCommonConfirmUI
