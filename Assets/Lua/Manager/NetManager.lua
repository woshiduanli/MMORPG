--[[
	desc: LUA层网络事件管理器
	author: CJ
	create: 2018-07-03
]]

local Json = require "LuaHelper.JsonHelper"
local Log = require "LuaHelper.Log"
local NetEvent = require "Core.NetEvent"
local GameDef = require "LuaHelper.GameDef"
local NetManager = {}

function NetManager.InitData()
	NetManager.NetEvent = NetEvent.new()
    NetManager.NetInfo = NetEvent.new()
	NetManager.NetResponse = NetEvent.new()
end

function NetManager.RegEvent(eid,tag,handle)
	if eid == GameDef.NetEvent.Event  then
		NetManager.NetEvent:RegEvent(tag,handle)
	elseif eid == GameDef.NetEvent.Info  then
		NetManager.NetInfo:RegEvent(tag,handle)
	else
		NetManager.NetResponse:RegEvent(tag,handle)
	end
end

function NetManager.HandleMsg( msg )
	--[[local obj, a, error = Json.decode(msg)
    if (error ~= nil) then
        return
	end
	
	NetManager.Dispatch(table.unpack(obj))]]--
end

function NetManager.Dispatch(tag,...)
	if tag == "message" then
		return 
	end

	if tag == "event" then
		for _,value in ipairs({...}) do
			NetManager.NetEvent:FireEvent(table.unpack(value))
		end
		
	elseif tag == "info" then
		for _,value in ipairs({...}) do
			NetManager.NetInfo:FireEvent(table.unpack(value))
		end
	else
		NetManager.NetResponse:FireEvent(tag,...)
	end
end

return NetManager
