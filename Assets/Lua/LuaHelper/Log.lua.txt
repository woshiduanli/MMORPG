local Config = require "LuaHelper.ConfigHelper"
local CSLOG = CS.LOG
local string_format = string.format
local debug_traceback = debug.traceback
local tostring = tostring
local xpcall = xpcall

local Log = {}

function Log.error(...)
	if(not Config.EnableLog) then
		return
	end

	local str = string_format(...)
    if str == nil then 
        return
    end
	CSLOG.Erro(debug_traceback(tostring(str)))
end

--默认常用输出函数
function Log.debug(...)
    if(not Config.EnableLog) then
		return
	end
	print(...)
end

--支持输出堆栈信息，不要随意使用
function Log.traceback(...)
	if(not Config.EnableLog) then
		return
	end

	local str = string_format(...)
    if str == nil then 
        return
    end
	print(debug_traceback(tostring(str)))
end

local function TracebackError(errmsg)
    Log.error(debug_traceback(tostring(errmsg)))
end

function Log.xpcall(func, ...)
	return xpcall(func, TracebackError, ...)
end

return Log