--计时器帮助类
require "core.Class"
local CommonUtil = require "util.CommonUtil"
local Time = CS.UnityEngine.Time

local runnings_ = {}
setmetatable(runnings_, {__mode = "v"})
local keeps_ = {}

local Timer = class()

--@loop: 大于0时为循环的次数，小于0时未无限循环
--@duration: 每次执行间隔时间
--@unscaled: false采用deltaTime计时；true采用unscaledDeltaTime计 时
function Timer:ctor(func, duration, loop, unscaled)
    unscaled = unscaled or false and true
    loop = loop or 1
    self.func = func
    self.duration = duration
    self.loop = loop
    self.unscaled = unscaled
    self.time = duration
    self.running = false

    self.Process = nil
    table.insert(runnings_, self)
end

function Timer:Start()
    self.running = true
    keeps_[self] = 1
end

function Timer:Stop()
    self.running = false
    keeps_[self] = nil
end

--[[
function Timer:Reset(func, duration, loop, unscaled)
	self.duration 	= duration
	self.loop		= loop or 1
	self.unscaled	= unscaled
	self.func		= func
	self.time		= duration		
end
--]]
local function Update(self, delta, unscaled_delta)
    if not self.running then
        return
    end

    if self.unscaled then
        delta = unscaled_delta
    end
	--local delta = self.unscaled and Time.unscaledDeltaTime or Time.deltaTime
    self.time = self.time - delta

    if self.time <= 0 then
        Log.xpcall(self.func)

        if self.loop > 0 then
            self.loop = self.loop - 1
            self.time = self.time + self.duration
        end

        if self.loop == 0 then
            self:Stop()
        elseif self.loop < 0 then
            self.time = self.time + self.duration
        end
    end
end

local last_n = 0
-- run timer
function Timer.Process()
	local delta = Global.deltaTime
	local unscaled_delta = Global.unscaledDeltaTime

    local n, i = #runnings_, 1
    while i <= n do
        local t = runnings_[i]
        if t ~= nil then
            Update(t, delta, unscaled_delta)
            i = i + 1
        else
            table.remove(runnings_, i)
        end
    end
    --[[
    local n = #runnings_
    for i = 1, n do
        local t = runnings_[i]
        if t ~= nil then
            Update(t, delta, unscaled_delta)
        end
    end
    --]]
    -- for test
    if last_n ~= n then
        Log.debug("timer runnings count " .. n)
        last_n = n
    end
end

return Timer
--[[
--计时器帮助类
local setmetatable = setmetatable
local UpdateBeat = require("util.UpdateBeat")
local Time = CS.UnityEngine.Time

--Timer = {}

local Timer = Timer
local mt = {__index = Timer}

--@loop: 大于0时为循环的次数，小于0时未无限循环
--@duration: 每次执行间隔时间
--@unscaled: false采用deltaTime计时；true采用unscaledDeltaTime计时
--@return [util.Timer#Timer]
function Timer.New(func, duration, loop, unscaled)
	unscaled = unscaled or false and true	
	loop = loop or 1
	return setmetatable({func = func, duration = duration, time = duration, loop = loop, unscaled = unscaled, running = false}, mt)	
end

function Timer:Start()
	self.running = true

	if not self.handle then
		self.handle = UpdateBeat:CreateListener(self, self.Update)
	end
end

function Timer:Reset(func, duration, loop, unscaled)
	self.duration 	= duration
	self.loop		= loop or 1
	self.unscaled	= unscaled
	self.func		= func
	self.time		= duration		
end

function Timer:Stop()
	self.running = false

	if self.handle then
		UpdateBeat:RemoveListener(self.handle)	
	end
end

function Timer:Update()
	if not self.running then
		return
	end

	local delta = self.unscaled and Time.unscaledDeltaTime or Time.deltaTime	
	self.time = self.time - delta
	
	if self.time <= 0 then
		self.func()
		
		if self.loop > 0 then
			self.loop = self.loop - 1
			self.time = self.time + self.duration
		end
		
		if self.loop == 0 then
			self:Stop()
		elseif self.loop < 0 then
			self.time = self.time + self.duration
		end
	end
end

--给协同使用的帧计数timer
FrameTimer = {}

local FrameTimer = FrameTimer
local mt2 = {__index = FrameTimer}

--@return [util.Timer#FrameTimer]
function FrameTimer.New(func, count, loop)	
	local c = Time.frameCount + count
	loop = loop or 1
	return setmetatable({func = func, loop = loop, duration = count, count = c, running = false}, mt2)		
end

function FrameTimer:Reset(func, count, loop)
	self.func = func
	self.duration = count
	self.loop = loop
	self.count = Time.frameCount + count	
end

function FrameTimer:Start()		
	if not self.handle then
		self.handle = UpdateBeat:CreateListener(self, self.Update)
	end
	
	self.running = true
end

function FrameTimer:Stop()	
	self.running = false

	if self.handle then
		UpdateBeat:RemoveListener(self.handle)	
	end
end

function FrameTimer:Update()	
	if not self.running then
		return
	end

	if Time.frameCount >= self.count then
		self.func()	
		
		if self.loop > 0 then
			self.loop = self.loop - 1
		end
		
		if self.loop == 0 then
			self:Stop()
		else
			self.count = Time.frameCount + self.duration
		end
	end
end

CoTimer = {}

local CoTimer = CoTimer
local mt3 = {__index = CoTimer}

--@return [util.Timer#CoTimer]
function CoTimer.New(func, duration, loop)	
	loop = loop or 1
	return setmetatable({duration = duration, loop = loop, func = func, time = duration, running = false}, mt3)			
end

function CoTimer:Start()		
	if not self.handle then	
		self.handle = UpdateBeat:CreateListener(self, self.Update)
	end
	
	self.running = true
end

function CoTimer:Reset(func, duration, loop)
	self.duration 	= duration
	self.loop		= loop or 1	
	self.func		= func
	self.time		= duration		
end

function CoTimer:Stop()
	self.running = false

	if self.handle then
		UpdateBeat:RemoveListener(self.handle)	
	end
end

function CoTimer:Update()	
	if not self.running then
		return
	end

	if self.time <= 0 then
		self.func()		
		
		if self.loop > 0 then
			self.loop = self.loop - 1
			self.time = self.time + self.duration
		end
		
		if self.loop == 0 then
			self:Stop()
		elseif self.loop < 0 then
			self.time = self.time + self.duration
		end
	end
	
	self.time = self.time - Time.deltaTime
end

return Timer
--]]
