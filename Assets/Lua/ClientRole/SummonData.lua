--[[
	desc: 场景实体对象-召唤物
	author: CJ
	create: 2018-06-22
 ]]
 local BuffSystem = require "GameSystem.BuffSystem"
 
 local Base = require "ClientRole.BaseData"
 local SummonData = class(Base)

 
 function SummonData:InitData(data)
	Base.InitData(self,data)
 end

function SummonData:OnDispose()
	Base.OnDispose(self)
end

return SummonData