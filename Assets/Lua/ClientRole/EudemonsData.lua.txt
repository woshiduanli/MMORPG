

 local PropertyType = GameDef.PropertyType
 local BuffSystem = require "GameData.BuffSystem"
 
 local Base = require "ClientRole.BaseData"
 local EudemonsData = class(Base)

 function EudemonsData:InitData(data)
	Base.InitData(self,data)
 end

function EudemonsData:OnDispose()
	Base.OnDispose(self)
end

return EudemonsData