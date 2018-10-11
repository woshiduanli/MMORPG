
 local Base = require "core.Object"
 local PropertyType = GameDef.PropertyType
 local RoleType = GameDef.RoleType
 local Vec3 = require "LuaHelper.VectorHelper"
 local BaseData = class(Base)
 
 function BaseData:ctor(data,sn)
    self.SN = sn
    self.DelayLoad = false
    self.Scale = 1
    self.Datas = {}
    Base.ctor(self,data)
 end

 function BaseData:Update()
 end

 function BaseData:InitData(data)
    self.x = data.pos.x
    self.z = data.pos.y
    self.Dirx = data.forward.x
    self.Dirz = data.forward.y
    self.ObjectType = data.type
    self:SetProperty(data.points)
    self:SetProperty(data.stats)
    local speed = self:GetData(PropertyType.MOVESPEED)
    if speed == nil or speed == 0 then
        self.IsNeverMove = true
    else
        self.IsNeverMove = false
    end
 end

 function BaseData:GetData(pname)
     return self.Datas[pname]
 end

 function BaseData:SetData(pname,value)
    if value then
       self.Datas[pname] = value
    end
end

function BaseData:PropertyChange(data)
    for key,value in pairs(data) do
        if type(value) == "table" then
            self:SetProperty(value)
        else
            self:SetData(key,value)
            self:OnPropertyChange(key,data)
        end
    end
end

function BaseData:SetProperty(data)
    for key,value in pairs(data) do
        self:SetData(key,value)
    end
end

function BaseData:OnPropertyChange(key,data)
end

function BaseData:OnDispose()
    CSGlobal.RemoveRole(self.SN)
    self.Datas = nil
end

return BaseData