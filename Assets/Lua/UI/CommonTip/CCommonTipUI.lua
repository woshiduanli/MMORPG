local CCommonTipUI = class(GameUI)

function CCommonTipUI:ctor(ui)
    GameUI.ctor(self, ui)
    self.isFullScreen = true
    self.Layer = UIManager.Layer.FullWindow

end

function CCommonTipUI.Awake(self)

end

function CCommonTipUI.UIEnable(self)

end

function CCommonTipUI.LoadUICallback(self)

end

function CCommonTipUI.OnDestroy(self)

end

return CCommonTipUI