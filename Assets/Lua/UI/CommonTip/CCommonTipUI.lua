local Base = GameUI
local CCommonTipUI = class(Base)

function CCommonTipUI:ctor(ui)
    Base.ctor(self, ui)
    self.isFullScreen = true
    self.Layer = UIManager.Layer.FullWindow

    print("CCommonTipUI:ct")

end

function CCommonTipUI.Initialize(self)

end

function CCommonTipUI.UIEnable(self)

end

function CCommonTipUI.LoadUICallback(self)

end

function CCommonTipUI.OnDestroy(self)

end

return CCommonTipUI