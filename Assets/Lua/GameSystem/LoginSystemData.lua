local Base = require "GameSystem.SystemData"
local LoginSystemData = class(Base)
local Json = Json

function LoginSystemData:ctor(data)
    Base:ctor(data)
end

function LoginSystemData:RegEvents()
    self:RegEvent(3, "")
end

function LoginSystemData:CallBack(json)
    print(json)
    local obj, pos, err = Json.decode(json)
    if err == nil then
		if obj.HasError == true then
			-- 打开提示ui 
			self:FireEvent(EventCode.UI.OpenUI, "CommonTip")
            print("you cuo ")
        else
            print(" not you cuo ")
        end
    end

    -- if then
end

function LoginSystemData:OnDispose()
    Base:OnDispose()
end

LoginSystemData.url = "http://localhost:8080/api/account"

return LoginSystemData
