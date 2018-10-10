local base = GameUI
local CBattlegroundResultUI = class(base)

function CBattlegroundResultUI:ctor(ui)
    base.ctor(self, ui)
    self.isFullScreen = true
    self.Layer = UIManager.Layer.FullWindow

end

function CBattlegroundResultUI.Initialize(self)

end

function CBattlegroundResultUI.UIEnable(self)

end

function CBattlegroundResultUI.OnDestroy(self)

end

return CBattlegroundResultUI