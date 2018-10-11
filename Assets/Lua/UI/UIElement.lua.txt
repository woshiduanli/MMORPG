
local Base = require "core.Object"

local UIElement = class(Base)

function UIElement:ctor(ui,link,path)
    Base.ctor(self,ui,link)
    self.Link = link
    self.NeedRefresh = true
    self.UI = ui
    self.Path = path
    self:Initialize()
end

function UIElement:Initialize()
end

function UIElement:UIEnable()
    self.IsActive = true
    self:OnUIEnable()
end

function UIElement:OnUIEnable()
end

function UIElement:UIDisable()
    self.IsActive = false
    self:OnUIDisable()
end

function UIElement:OnUIDisable()
end

function UIElement:Update()
end

function UIElement:IsActive()
    local active = CSGlobal.IsElementActive(self.UI.Name, self.Link)
    if active then
        return true
    end
    return false
end

function UIElement:IsGoActive(gid)
    local active = CSGlobal.IsGoActive(self.UI.Name, self.Link, gid)
    if active then
        return true
    end
    return false
end

function UIElement:SetActive(active)
    CSGlobal.SetElementActive(self.UI.Name, self.Link, active)
    if active == 1 then
        self:UIEnable()
    else
        self:UIDisable()
    end
end

function UIElement:SetGoActive(gid, active)
    CSGlobal.SetGoActive(self.UI.Name, self.Link, gid, active)
end

function UIElement:CreateEffect(filename, pid, x, y, z, scale)
    return CSGlobal.CreateEffect(self.UI.Name, self.Link, pid, filename, x, y, x, scale)
end

function UIElement:CreateNomalEffect(filename, pid)
    return CSGlobal.CreateNomalEffect(self.UI.Name, self.Link, pid, filename)
end

function UIElement:CreateSimpleUIObject(pid, apprid, equips, dress)
    return CSGlobal.CreateSimpleUIObject(self.UI.Name, self.Link, pid, apprid, equips, dress)
end

function UIElement:CreateMirrorUIObject(apprid, equips, dress, x, y, scale)
    return CSGlobal.CreateMirrorUIObject(self.UI.Name, apprid, equips, dress, x, y, scale)
end

function UIElement:SetEvent(eid, eventID, func)
    return CSGlobal.SetEvent(self.UI.Name, self.Link, eid, eventID, func)
end

function UIElement:DestroyEffect(hash)
    CSGlobal.DestroyEffect(self.UI.Name, hash)
end

function UIElement:DestroyRole(hash)
    CSGlobal.DestroyRole(self.UI.Name, hash)
end

function UIElement:CreateSprite(sid, spname, spritename)
    CSGlobal.CreateSprite(self.UI.Name, self.Link, sid, spname, spritename)
end

function UIElement:CreateImage(iid, filename)
    CSGlobal.CreateImage(self.UI.Name, self.Link, iid, filename)
end

function UIElement:CreateFont(cid, font)
    CSGlobal.CreateFont(self.UI.Name, self.Link, cid, font)
end

function UIElement:Clone()
    return self.UI:Clone(self.Link)
end

function UIElement:RegListView(vid)
    CSGlobal.RegListView(self.UI.Name, self.Link, vid)
 end

 function UIElement:OnListViewRefresh(dataIndex)
 end

return UIElement
