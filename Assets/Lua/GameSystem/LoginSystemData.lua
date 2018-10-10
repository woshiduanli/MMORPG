

 local Base = require "GameSystem.SystemData"
 local LoginSystemData = class(Base)
 
 function LoginSystemData:ctor(data)
	Base:ctor(data)
 end 

 function LoginSystemData:RegEvents()
	 self:RegEvent( 3, "" )
 end

 function LoginSystemData.CallBack(json  )
	print("josn:".. json)
end


 function LoginSystemData:OnDispose()
	Base:OnDispose ()
 end

 LoginSystemData.url = "http://localhost:8080/api/account"

return LoginSystemData