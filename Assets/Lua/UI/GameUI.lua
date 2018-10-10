
local Base = require "core.Object"
local UILayer = UIManager.Layer
local Timer = require("util.Timer")

local GameUI = class(Base)

function GameUI:ctor(ui)
    Base.ctor(self, ui)
    -- 获取ID
    self.HashIDTable = require(string.format("UI.%s.C%sUIID", ui, ui))
    if self.HashIDTable == nil then
        Log.error("Error Error ", " not find your ID " .. "path is:", string.format("UI.%s.C%sUIID", ui, ui))
    end
    self.Link = self.HashIDTable.HashID
    self.Name = ui
    self.IsFullScreen = false
    self.DontDestoryOnLoad = false
    self.Layer = UILayer.Free
    self.LoopDuration = 1
    self.SortingOrder = 1
    self.PlaneDistance = 2
    self.NeedRefresh = true
    self.Loop = false
    self.Elements = {}
end

--unity驱动-----------------------------------------------------------------------

function GameUI:Initialize()
end

function GameUI:LoadUICallback()
end

function GameUI:UIEnable()
    self.IsActive = true
    self:OnUIEnable()
    for _, element in pairs(self.Elements) do
        if element:IsActive() then
            element:UIEnable()
        end
    end

    if self.Loop then
        if self.LoopTimer == nil then
            self.LoopTimer = Timer.new(self:GetSelfFunc(self.Update), self.LoopDuration, -1, true)
        end
        self.LoopTimer:Start()
    end
end

function GameUI:OnUIEnable()
end

function GameUI:UIDisable()
    self.IsActive = false
    self:OnUIDisable()
    for _, element in pairs(self.Elements) do
        element:UIDisable()
    end
    if self.LoopTimer then
        self.LoopTimer:Stop()
    end
end

function GameUI:OnUIDisable()
end

function GameUI:OnWakeUp()
end

function GameUI:OnDestroy()
    self:Dispose()
    UIManager.Remove(self.Name)
    self.LoopTimer = nil
    for _, element in pairs(self.Elements) do
        element:Dispose()
    end
    self.Elements = nil
end
--End-----------------------------------------------------------------------

function GameUI:IsShow()
    value = CSGlobal.IsUIShow(self.Name)
    if value == 1 then
        return true
    end
    return false
end

--会自动打开上次被顶掉的UI
function GameUI:Close()
    CSGlobal.UIClose(self.Name)
end

--单纯的关闭界面，不会显示被顶掉的UI ,如果当前UI是全屏，防止主界面丢失 ，会打开主界面
function GameUI:UISimpleClose()
    CSGlobal.UISimpleClose(self.Name)
end

function GameUI:Update()
    if self.Loop then
        self:UpdateChild()
        self:OnUpdate()
    end
end

function GameUI:UpdateChild()
    if not self.IsActive then
        return
    end
    for _, element in pairs(self.Elements) do
        element:Update()
    end
end

function GameUI:OnUpdate()
end

function GameUI:OnListViewInit(link, path)
    local def = require(path)
    local element = def.new(self, link, path)
    self.Elements[link] = element
end

function GameUI:OnListViewRefresh(link, dataIndex)
    local element = self:GetElement(link)
    if element then
        return
    end
    element:OnListViewRefresh(dataIndex)
end

---------Lua Call C#------------------------------------
function GameUI:SetGoActive(gid, active)
    CSGlobal.SetGoActive(self.Name, self.Link, gid, active)
end

function GameUI:CreateEffect(filename, pid, x, y, z, scale)
    return CSGlobal.CreateEffect(self.Name, self.Link, pid, filename, x, y, x, scale)
end

function GameUI:CreateNomalEffect(filename, pid)
    return CSGlobal.CreateNomalEffect(self.Name, self.Link, pid, filename)
end

function GameUI:CreateSimpleUIObject(pid, apprid, equips, dress)
    return CSGlobal.CreateSimpleUIObject(self.Name, self.Link, pid, apprid, equips, dress)
end

-- 设置时间字符串 时间字符串，返回格式：小时：分：秒
function GameUI:SetTimeHourString(textId, seconds)
    CSGlobal.SetTimeHourString(self.Name, self.Link, textId, seconds)
end

-- 时间字符串 返回格式：分：秒
function GameUI:SetTimeString(textId, seconds)
    CSGlobal.SetTimeString(self.Name, self.Link, textId, seconds)
end

function GameUI:SetCText(textId, text)
    CSGlobal.SetCText(self.Name, self.Link, textId, text)
end

function GameUI:CreateMirrorUIObject(apprid, equips, dress, x, y, scale)
    return CSGlobal.CreateMirrorUIObject(self.Name, apprid, equips, dress, x, y, scale)
end

function GameUI:SetEvent(eid, eventID, func)
    print("SetEvent.")
    return CSGlobal.SetEvent(self.Name, self.Link, eid, eventID, func)
end

function GameUI:SetGoActive(hash, active)
    if (active) then
        CSGlobal.SetGoActive(self.Name, self.Link, hash, 1)
    else
        CSGlobal.SetGoActive(self.Name, self.Link, hash, 0)
    end
end

function GameUI:DestroyEffect(hash)
    CSGlobal.DestroyEffect(self.Name, hash)
end

function GameUI:DestroyRole(hash)
    CSGlobal.DestroyRole(self.Name, hash)
end

function GameUI:SetCText(textId, text)
    CSGlobal.SetCText(self.Name, self.Link, textId, text)
end

function GameUI:GetCText(textId)
    return CSGlobal.GetCText(self.Name, self.Link, textId)
end

function GameUI:CreateSprite(sid, spname, spritename)
    CSGlobal.CreateSprite(self.Name, self.Link, sid, spname, spritename)
end

function GameUI:CreateImage(iid, filename)
    CSGlobal.CreateImage(self.Name, self.Link, iid, filename)
end

function GameUI:CreateFont(cid, font)
    CSGlobal.CreateFont(self.Name, self.Link, cid, font)
end

function GameUI:GetElement(link)
    return self.Elements[link]
end

function GameUI:AddElement(link, path)
    local element = self:GetElement(link)
    if element then
        return nil
    end
    local def = require(path)
    local element = def.new(self, link, path)
    self.Elements[link] = element
    return element
end

function GameUI:Clone(link)
    local copylink = CSGlobal.LuaClone(self.Name, link)
    if copylink == -1 then
        return nil
    end

    local element = self:GetElement(link)
    if element == nil then
        return nil
    end

    local def = require(element.Path)
    local element = def.new(self, copylink, element.Path)
    self.Elements[copylink] = element
    return element
end

function GameUI:RegListView(vid, path)
    CSGlobal.RegListView(self.Name, self.Link, vid, path)
end

return GameUI
