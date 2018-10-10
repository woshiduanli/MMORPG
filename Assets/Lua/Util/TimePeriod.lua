--[[
    $desc: 实现C#的TimePeriod$
    $author: figo$
    $date: 2018-02-27$

    Copyright (C) 2018 zwwx Ltd. All rights reserved.
--]]
local CommonUtil = require "util.CommonUtil"

local TimePeriod = class()

local TimePeriodType = {
    None = -1,
    AllTime = 0,
    Period = 1,
    Daily = 2,
    Weekly = 3,
}

-- 检查时间是否合法
local function Check(self)
   if (self.begin_timedata[1] == 0) ~= (self.end_timedata[1] == 0) then
       return false
   end
   if (self.begin_timedata[2] == 0) ~= (self.end_timedata[2] == 0) then
       return false
   end
   if (self.begin_timedata[3] == 0) ~= (self.end_timedata[3] ==0) then
       return false
   end
   if (self.begin_timedata[1] == 0) ~= (self.begin_timedata[2] ==0) then
       return false
   end

   -- 由于复杂度较高，这里不检查具体时间值
   if self.begin_timedata[1] ~= 0 then
       self.type = TimePeriodType.Period
       self.IsPeriod = true
   elseif self.begin_timedata[3] ~= 0 then
       self.type = TimePeriodType.Weekly
       self.IsWeekly = true
   else
       if self.begin_timedata[4] == 0 and self.begin_timedata[5] == 0 and self.end_timedata[4] == 0 and self.end_timedata[5] == 0 then
           self.type = TimePeriodType.AllTime
           self.IsAllTime = true
       else
           self.type = TimePeriodType.Daily
           self.IsDaily = true
       end
   end

   return true
end

local function ProcessAction(self, func)
    local n = 1
    while n < #self.action_list do
        local obj = self.action_list[n]

        if obj.enable and func(obj) then
            if obj.period_count < self.period_round then
                if not obj.proc(obj.obj) then
                    obj.enable = false
                end
                obj.period_count = self.period_round
            end
        end

        if not obj.enable then
            obj.proc = nil
            obj.obj = nil
            table.remove(self.action_list, n)
        else
            n = n + 1
        end
    end
end

local function GetWeekTime(timedata, year, month, day, forward_weeks)
    local date = {
        year    = year,
        month   = month,
        day     = day,
        hour    = timedata[4],
        min     = timedata[5],
        sec     = 0
    }
    local time = os.time(date)
    local wday = tonumber(os.date("%w", time))
    if wday == 0 then wday = 7 end
    local offset = timedata[3] - wday
    time = time + (offset*3600)
    if forward_weeks > 0 then
        time = time + (forward_weeks*7*3600)
    end
    return time
end

local function GetDayTime(timedata, year, month, day)
    local date = { 
        year    = ((timedata[3] ~=0) and timedata[3]) or year,
        month   = ((timedata[1] ~=0) and timedata[1]) or month, 
        day     = ((timedata[2] ~=0) and timedata[2]) or day,
        hour    = timedata[4],
        min     = timedata[5],
        sec     = 0
    }
    local time = os.time(date)
    return time
end

------------------------------------------------------------------------------
function TimePeriod:ctor(begin_month, begin_day, begin_weekday_or_year, begin_hour, begin_minute, end_month, end_day, end_weekday_or_year, end_hour, end_minute)
    self.IsValid = false
    self.IsOver = false
    self.IsDaily  = false
    self.IsWeekly = false
    self.IsPeriod = false
    self.IsAllTime = false

    self.type = TimePeriodType.None
    self.begin_time = 0
    self.end_time = 0

    self.begin_timedata = {}
    self.end_timedata = {}
    self.action_list = {}

    self.period_round = 0
    self.forward_weeks = 0
    self.version = 0

    table.insert(self.begin_timedata, begin_month)
    table.insert(self.begin_timedata, begin_day)
    table.insert(self.begin_timedata, begin_weekday_or_year)
    table.insert(self.begin_timedata, begin_hour)
    table.insert(self.begin_timedata, begin_minute)

    table.insert(self.end_timedata, end_month)
    table.insert(self.end_timedata, end_day)
    table.insert(self.end_timedata, end_weekday_or_year)
    table.insert(self.end_timedata, end_hour)
    table.insert(self.end_timedata, end_minute)

    self.current = self

    self.IsValid = Check(self)
    if self.IsValid then self.next = self end
end

-- 设置进入时间区t和离开时间区的执行动作
function TimePeriod:AddAction(begin, ahead, proc, o)
    local obj = { 
        begin = begin,
        ahead = ahead,
        proc = proc,
        obj = o,
        enable = true,
        period_count = -1
    }

    table.insert(self.action_list, obj)
end

-- 一分钟调用 一次
function TimePeriod:Update(year, month, day, hour, minute, time)
    local minute_time = math.floor(time/60)
    local begin_minute = math.floor(self.begin_time/60)
    local end_minute = math.floor(self.end_time/60)

    if self.type == TimePeriodType.AllTime then
        if begin_minute ~= 0 and minute_time < begin_minute then
            -- 被时间修正过，需要待时间轴到来
        else 
            ProcessAction(self, function(obj) return (obj.begin and (obj.ahead == 0)) end)
        end
    else 
        if (minute_time > begin_minute) and (minute_time <= end_minute) then
            self.period_round = (CommonUtil.MOD(self.period_round, 2) == 0) and (self.period_round + 1) or self.period_round
        else
            self.period_round = (CommonUtil.MOD(self.period_round, 2) == 0) and self.period_round or (self.period_round + 1)
        end
        local delta_begin = minute_time - begin_minute
        local delta_end = minute_time - end_minute
        local action = function(obj)
            if CommonUtil.MOD(self.period_round, 2) == 0 then
                if obj.begin and obj.ahead < 0 and delta_begin < 0 then
                    if delta_begin >= obj.ahead then
                        return true
                    end
                    if (not obj.begin) and obj.ahead > 0 and delta_end > 0 then
                        if delta_end >= obj.ahead then
                            return true
                        end
                    end
                end
            else
                if obj.begin and obj.ahead >= 0 and delta_begin >= 0 then
                    if delta_begin >= obj.ahead then
                        return true
                    end
                end
                if (not obj.begin) and obj.ahead <= 0 and delta_end <= 0 then
                    if delta_end >= obj.ahead then
                        return true
                    end
                end
                return false
            end
        end
        ProcessAction(self, action)
    end
    self:Flush(year, month, day, hour, minute, time)
end

-- 判定是否过期
function TimePeriod:IsExpired(time)
    if not self.valid then
        return true 
    end
    if self.IsAllTime then
        return false
    end
    return time > self.end_time
end

-- 设置向前移动的周数
function TimePeriod:SetForwardWeeks(weeks)
    if weeks <= 0 then return end
    self.forward_weeks = weeks
end

-- 刷新period时间
function TimePeriod:Flush(year, month, day, hour, minute, time)
    local check_version = year*10000 + month*100+day
    if self.version ~= check_version then
        if self.type == TimePeriodType.Daily then
            self.begin_time = GetDayTime(self.begin_timedata, year, month, day)
            self.end_time = GetDayTime(self.end_timedata, year, month, day)
        elseif self.type == TimePeriodType.Weekly then
            if self.version ~= 0 and self.forward_weeks > 0 then
                local now = {year=year, month=month, day=day, hour=hour, min=minute, sec=0}
                local time = os.time(now)
                now = os.date("*t", time)
                if now.wday == 1 then self.forward_weeks = self.forward_weeks - 1 end
            end
            self.begin_time = GetWeekTime(self.begin_timedata, year, month, day, self.forward_weeks)
            self.end_time = GetWeekTime(self.end_timedata, year, month, day, self.forward_weeks)
            if self.begin_time > self.end_time then self.begin_time = self.begin_time - (7*24*3600) end
        elseif self.type == TimePeriodType.Period then
            self.begin_time = GetDayTime(self.begin_timedata, year, month, day)
            self.end_time = GetDayTime(self.end_timedata, year, month, day)
            if (self.begin_timedata[3] == 0) and (self.begin_timedata[1] > self.end_timedata[1]) then
                self.end_time = GetDayTime(self.end_timedata, year + 1, month, day)
            end
        elseif self.type == TimePeriodType.AllTime then
        end
        self.version = check_version
    end

    if (self.end_time ~= 0) and (self.end_time <= time) then
        self.IsOver = true
    end
    if self.IsPeriod and self.IsOver then
        local dt = os.date("*t", self.end_time)
        if (dt.year ~= year) or (dt.month ~= month) or (dt.day ~= day) then
            self.IsValid = false
        end
    end
end

function TimePeriod:ToString()
    if self.IsAllTime then
        return "all the time"
    else
        return string.format("%s - %s",os.date("%c", self.begin_time),  os.date("%c", self.end_time))
    end
end

return TimePeriod
