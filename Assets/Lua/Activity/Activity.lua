--[[
	desc: lua 活动基类
	author: CJ
	create: 2018-06-20
]]
local Base = require "core.Object"
local EventCode = require "LuaHelper.EventCode"

local Activity = class(Base)

function Activity:ctor(at,name)
	Base.ctor(self, at,name)
	self.AT = at
	self.Name = name
end

--初始化活动数据
function Activity:InitActData(actdata)
	self:ReadTimeData(actdata)
end

--更新活动数据
function Activity:UpdateData(actdata)
	self:ReadTimeData(actdata)
	self.FireEvent(EventCode.Activity.DataChange , self.AT)
end

function Activity:ReadTimeData(actdata)
	self.Time = actdata.times
	self.EnteredTime = actdata.entered
	self.BoughtTime = actdata.bought
end

function Activity:OpenUI()
end

function Activity:EnterActivity()
end


return Activity
