--[[
	desc: 场景实体数据-幻兽
	author: CJ
	create: 2018-06-22
 ]]

 local PropertyType = GameDef.PropertyType
 local BuffSystem = require "GameSystem.BuffSystem"
 
 local Base = require "ClientRole.BaseData"
 local EudemonsData = class(Base)

 function EudemonsData:InitData(data)
	Base.InitData(self,data)
 end

function EudemonsData:OnDispose()
	Base.OnDispose(self)
end

return EudemonsData