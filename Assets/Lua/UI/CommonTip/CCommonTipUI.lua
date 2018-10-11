local Base = GameUI
local CCommonTipUI = class(Base)

function CCommonTipUI:ctor(ui, data)
    Base.ctor(self, ui, data)
    self.isFullScreen = true
    self.Layer = UIManager.Layer.FullWindow
end

function CCommonTipUI.Initialize(self)
    self:SetEvent(
        self.HashIDTable.btn_sure,
        UIManager.TriggerEventID.PointerClick,
        self:GetSelfFunc(CCommonTipUI.OnClickSure)
    )
    self:SetEvent(
        self.HashIDTable.btn_cancel,
        UIManager.TriggerEventID.PointerClick,
        self:GetSelfFunc(CCommonTipUI.OnClickCancel)
    )
end

function CCommonTipUI:OnClickSure()
    self.thisData.SureCallBack()
end

function CCommonTipUI:OnClickCancel()
    self.thisData.CancelCallBack()
end

function CCommonTipUI.UIEnable(self)
    -- self:SetCText(self.HashIDTable.Desc,self.thisData.SubJect)
end

function CCommonTipUI.LoadUICallback(self)
end

function CCommonTipUI.OnDestroy(self)
end

return CCommonTipUI
