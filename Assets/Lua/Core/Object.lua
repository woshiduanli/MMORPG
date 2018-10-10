--[[
	desc: lua CObject的实现
	author: CJ
	create: 2018-06-15
]]
local CommonUtil = require "Util.CommonUtil"
require "core.Class"
local CObject = class()
local Timer = require("util.Timer")

function CObject:InitData(...)
end

function CObject:RegEvents()
end

function CObject:ctor(...)
    self.disposed = false
    -- 存放事件及回调信息
    self.callbacks = {}
    -- timers
    self.timers = {}
    self:InitData(...)
    self:RegEvents()
end

function CObject:OnDispose()
end

function CObject:Dispose()
    self.disposed = true
    for _, v in ipairs(self.callbacks) do
        Global.UnregEvent(v.eid, v.func)
    end
    
    for _, v in ipairs(self.timers) do
        if (v ~= nil) then
            v:Stop()
        end
    end
    
    self.timers = {}
    self.callbacks = nil
    self.class = nil

    self:OnDispose()
end

-- 注册事件
function CObject:RegEvent(evt, cb)
    if disposed then
        return
    end
    if evt == nil or cb == nil then
        return
    end

    Global.RegEvent(evt, cb)
    table.insert(self.callbacks, {eid = evt, func = cb})
end

-- 分发事件
function CObject:FireEvent(evt, ...)
    if disposed then
        return
    end
    Global.FireEvent(evt, ...)
end

function CObject:GetSelfFunc(func)
    return CommonUtil.GetSelfFunc(self, func)
end

--[[
	@duration: 间隔时间
	@loopcount: 循环次数，-1为无限循环
	@return [util.Timer#Timer]
--]]
function CObject:AddTimer(func, duration, loopcount)
    --@RefType [util.Timer#Timer]
    local timer = Timer.new(func, duration, loopcount, true)
    timer:Start()
    table.insert(self.timers, timer)
    return timer
end

return CObject
