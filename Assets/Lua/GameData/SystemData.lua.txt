 local Base = require "core.Object"
 local SystemData = class(Base)
 
 function SystemData:ctor(data)
	 Base:ctor (data)
	 
 end

function SystemData:ClearData()
	 
 end

 function SystemData:OnDispose()
	Base:OnDispose()
 end



return SystemData