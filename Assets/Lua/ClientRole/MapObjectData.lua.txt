

 local Base = require "core.Object"
 local CSGlobal = CS.Global
 local MapObjectData = class(Base)
 
 function MapObjectData:ctor(data,sn)
    Base.ctor(self,data)
    self.SN = sn
    self.RefID = data.templet_id
    self.reference = ReferenceManager.GetReference(GameDef.ReferenceDef.MapObject,self.RefID)
    self.Scale = 1
    if self.reference.Scale then
        self.Scale = self.reference.Scale / 100
    end
    self.x = data.pos.x
    self.z = data.pos.y
    self.Dirx = data.forward.x
    self.Dirz = data.forward.y
    self.Name = string.format( "XYMapObject%s_%s",self.SN,self.RefID)
    self.LeftTime = data.left_time
    self.MasterID = data.master_id
    self.OpenTimes = data.openTimes
    self.Owner_Name = data.owner_name

    CSGlobal.CreateMapObject(data.templet_id,data.pos.x,data.pos.y,sn,data.forward.x,data.forward.y,data.left_time,data.master_id,data.openTimes,data.owner_name)
 end

 function MapObjectData:OnPropChange(data)
    self.OpenTimes = data.openTimes
    CSGlobal.OnMapObjectPropChange(data.openTimes)
 end

function MapObjectData:OnDispose()
    CSGlobal.RemoveMapObject(self.SN)
    self.reference = nil
end

return MapObjectData