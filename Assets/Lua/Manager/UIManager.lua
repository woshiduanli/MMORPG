
local UIManager = {}

UIManager.Layer = {
    --主界面(或者能和主界面共存的界面)
    MainFace = 0,
    -- 全屏界面（系统界面 压屏....）
    FullWindow = 1,
    -- 不受任何界面影响
    Free = 2
}

UIManager.TriggerEventID = {
    PointerEnter = 0,
    PointerExit = 1,
    PointerDown = 2,
    PointerUp = 3,
    PointerClick = 4,
    Drag = 5,
    Drop = 6,
    Scroll = 7,
    UpdateSelected = 8,
    Select = 9,
    Deselect = 10,
    Move = 11,
    InitializePotentialDrag = 12,
    BeginDrag = 13,
    EndDrag = 14,
    Submit = 15,
    Cancel = 16
}

local UILayer = UIManager.Layer

function UIManager.InitData()
    UIManager.uis = {}
    UIManager.ButtomAudio = CSGlobal.ButtomAudioID
    UIManager.UIAudio = CSGlobal.UIAudioID
    UIManager.UIRoot = CSGlobal.UIRootID
    UIManager.UICamera = CSGlobal.UICameraID
    UIManager.UI_BG = CSGlobal.UI_BGID
    UIManager.ShadowTF = CSGlobal.ShadowTFID
    UIManager.ModelTF = CSGlobal.ModelTFID
    UIManager.uIShadow = CSGlobal.uIShadowID
    UIManager.uiFont = CSGlobal.uiFontID
    UIManager.uiTitleFont = CSGlobal.uiTitleFontID
    UIManager.Hold = false

    UIManager.RegEvents()
end

function UIManager.RegEvents()
    Global.RegEvent(EventCode.Scene.LevelWasLoaded, UIManager.OnLevelWasLoaded)
    Global.RegEvent(EventCode.UI.OpenUI, UIManager.OnCreateUIEvent)
    Global.RegEvent(EventCode.UI.CloseUI, UIManager.OnCloseUI)
    Global.RegEvent(EventCode.UI.DisposeEvent, UIManager.OnDisposeEvent)
end

function UIManager.Get(ui)
    return UIManager.uis[ui]
end

function UIManager.Remove(ui)
    UIManager.uis[ui] = nil
end

function UIManager.HasFullWindow()
    value = CSGlobal.HasFullWindow()
    if value == 1 then
        return true
    end
    return false
end

--事件相关-------------------------------------------------

-- 过图回调
function UIManager.OnLevelWasLoaded(scene)
    if (scene == GameDef.SceneName.LOGIN) then
        UIManager.OnCreateUIEvent("Login")
    end
end

-- 创建ui 
function UIManager.OnCreateUIEvent(ui, ...)

    local d = ...
    local window = UIManager.uis[ui]
    if window == nil or window == -1 then
        local path = string.format("UI.%s.C%sUI", ui, ui)
        local def = require(path)
        window = def.new(ui)
        UIManager.uis[ui] = window
    end
    CSGlobal.CreateUIEvent(ui, window, ...)
end

-- 关闭ui 
function UIManager.OnCloseUI(ui)
    CSGlobal.CloseUIEvent(ui)
end

function UIManager.OnDisposeEvent(scene)
    UIManager.Hold = true
    local forever = (scene == "Login" or scene == "SelecteRole")
    local temp = {}
    for _, ui in pairs(UIManager.uis) do
        if not ui.DontDestoryOnLoad or forever then
            table.insert(temp, ui)
        end
    end
    for _, ui in pairs(temp) do
        UIManager.Remove(ui.Name)
    end

    if forever then
        UIManager.uis = {}
    end
end

--UI相关的全局方法----------------------------------------------------

function UIManager.IsPointerOverUI(x, y)
    local value = CSGlobal.LuaIsPointerOverUI(x, y)
    if value == 1 then
        return true
    end
    return false
end

function UIManager.ActiveShahow(active)
    CSGlobal.LuaActiveShahow(active)
end

return UIManager
