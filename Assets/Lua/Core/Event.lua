--[[
	desc: LUA层事件管理器
	author: figo
	create: 2018-01-22
]]

local xpcall = xpcall
local tblinsert = table.insert
local Log = require "LuaHelper.Log"

local EventMgr = class()

function EventMgr:ctor()
	self.name = "EventMgr"
	self.events = {}
end

-- LUA层注册事件
function EventMgr:RegEvent(eid, handle) 
	if eid == nil or handle == nil then
		return
	end
	self.events[eid] = self.events[eid] or {}
	local handles = self.events[eid]

	-- 重复注册检查
	for i = 1, #handles do 
		if handles[i] == handle then
			Log.erro(string.format('CEventManager:RegEvent:%d, handle is repeat', eid))			
			return
		end
	end
	tblinsert(handles, handle)
end

-- LUA层注消事件
function EventMgr:UnregEvent(eid, handle)
    local handles = self.events[eid]
    if handles == nil then
        return
    end

    for i = 1, #handles do
        if handles[i] == handle then
            table.remove(handles, i)
            break
        end
    end

    if #handles == 0 then
        self.events[eid] = nil
    end
end

-- LUA层分发事件
function EventMgr:FireEvent(eid, ...)
	local handles = self.events[eid]
	if handles == nil then
		return 0
	end

	--处理事件过程中，处理函数列表可能发生变化
	--先复制一份函数列表，每执行一次回调函数后重新检查函数是否还存在。

	--复制当前的事件处理函数列表
	local temp = {}
	for _, func in ipairs(handles) do
		tblinsert(temp, func)
	end

	--对所有事件处理函数进行回调
	while #temp > 0 do
		local firstIndex = 1
		local handle = temp[firstIndex]

		--判断处理函数是否仍然存在
		local isValid = false
		for _, func in ipairs(handles) do
			if handle == func then
				isValid = true
				break
			end
		end

		if isValid then
			--防止回调函数出错时，无法继续正常运行
			Log.xpcall(handle, ...)
		end

		--删除已回调的函数。
		table.remove(temp, firstIndex)
	end
	return 1
end

return EventMgr
