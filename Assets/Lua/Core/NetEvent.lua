--[[
	desc: LUA层网络事件
	author: CJ
	create: 2018-06-23
]]

local Log = require "LuaHelper.Log"

local NetEvent = class()

function NetEvent:ctor()
	self.name = "NetEvent"
	self.events = {}
end

-- LUA层注册事件
function NetEvent:RegEvent(tag, handle) 
	if tag == nil or handle == nil then
		return
	end
	self.events[tag] = handle
end

-- LUA层分发事件
function NetEvent:FireEvent(tag, ...)
	local handle = self.events[tag]
	if handle == nil then
		return 0
	end

	Log.xpcall(handle, ...)

	return 1
end

return NetEvent
