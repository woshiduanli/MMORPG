
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