--[[
    $desc: 实现C#中的TimePeriodList$
    $author: figo$
    $date: 2018-02-28$

    Copyright (C) 2018 zwwx Ltd. All rights reserved.
--]]
local CommonUtil = require "util.CommonUtil"

local TimePeriodList = class()

function TimePeriodList:ctor()
    self.IsValid = false
    self.IsOver = false
    self.IsDaily = false
    self.IsWeekly = false
    self.IsPeriod = false
    self.IsAllTime = false

    self.begin_time = 0
    self.end_time = 0

    self.current = nil
    self.next = nil

    self.list = {}
end

local function CmpFunc(a, b)
    if a.type ~= b.type then
        return a.type < b.type
    end
    if a.IsAllTime then
        return true
    elseif a.IsPeriod then
        if a.begin_timedata[3] ~= b.begin_timedata[3] then return a.begin_timedata[3] < b.begin_timedata[3] end
        if a.begin_timedata[1] ~= b.begin_timedata[1] then return a.begin_timedata[1] < b.begin_timedata[1] end
        if a.begin_timedata[2] ~= b.begin_timedata[2] then return a.begin_timedata[2] < b.begin_timedata[2] end
        if a.begin_timedata[4] ~= b.begin_timedata[4] then return a.begin_timedata[4] < b.begin_timedata[4] end
        if a.begin_timedata[5] ~= b.begin_timedata[5] then return a.begin_timedata[5] < b.begin_timedata[5] end
        return true
    elseif a.IsWeekly then
        if a.begin_timedata[3] ~= b.begin_timedata[3] then return a.begin_timedata[3] < b.begin_timedata[3] end
        if a.begin_timedata[4] ~= b.begin_timedata[4] then return a.begin_timedata[4] < b.begin_timedata[4] end
        if a.begin_timedata[5] ~= b.begin_timedata[5] then return a.begin_timedata[5] < b.begin_timedata[5] end
        return true
    elseif a.IsDaily then
        if a.begin_timedata[4] ~= b.begin_timedata[4] then return a.begin_timedata[4] < b.begin_timedata[4] end
        if a.begin_timedata[5] ~= b.begin_timedata[5] then return a.begin_timedata[5] < b.begin_timedata[5] end
        return true
    else
        return true
    end
end

local function GetCurrent(list, time)
    local current, lateest, closest
    local lateest_time, closest_time
    for i, tp in ipairs(list) do
        if tp.IsAllTime then
            current = tp
            break
        end

        if (tp.begin_time < time) and (tp.end_time >= time) then
            current = tp
            break
        end

        if tp.end_time <= time then
            if (lateest_time == nil) or ((time - tp.end_time) < lateest_time) then
                lateest_time = time - tp.end_time
                lateest = tp
            end
        end
        
        if tp.begin_time >= time then
            if closest_time == nil or ((tp.begin_time - time) < closest_time) then
                closest_time = tp.begin_time - time
                closest = tp
            end
        end
    end

    if current == nil then
        local now = os.date("*t", time)
        if lateest ~= nil then
            local dt = os.date("*t", lateest.end_time)
            if dt.year  == now.year and dt.month == now.month and dt.day == now.day then
                current = lateest
            end
        end
        if closest ~= nil then
            local dt = os.date("*t", closest.begin_time)
            if dt.year == now.year and dt.month == now.month and dt.day == now.day then
                current = closest
            end
        end
        if current == nil then
            if closest ~= nil then 
                current = closest 
            elseif lateest ~= nil then
                current = lateest
            end
        end
    end
    return current
end

function TimePeriodList:Add(tp)
    if not tp.IsValid then
        return
    end

    table.insert(self.list, tp)
    table.sort(self.list, CmpFunc)
    for i, v in ipairs(self.list) do
        --local j = CommonUtil.MOD(i+1, #self.list) + 1
        local j = i + 1
        if j > #self.list then  j = 1 end
        self.list[i].next = self.list[j]
    end
    self.IsValid = true
end

function TimePeriodList:AddAction(begin, ahead, proc, o)
    for _, tp in ipairs(self.list) do
        tp:AddAction(begin, ahead, proc, o)
    end
end

function TimePeriodList:Flush(year, month, day, hour, minute, time)
    local i = 1
    local n = #self.list
    while i <= #self.list do
        self.list[i]:Flush(year, month, day, hour, minute, time)
        if not self.list[i].IsValid then
            table.remove(self.list, i)
        else
            i = i + 1
        end
    end
    if n ~= #self.list then
        for i, v in ipairs(self.list) do
            --local j = CommonUtil.MOD(i+1, #self.list)
            local j = i + 1
            if j > #self.list then j = 1 end
            self.list[i].next = self.list[j]
        end
    end

    self.IsValid = #self.list > 0
    self.current = GetCurrent(self.list, time)
    self.version = year * 10000 + month *100 + day
end

function TimePeriodList:Update(year, month, day, hour, minute, time)
    if self.current == nil then
        self.current = GetCurrent(self.list, time)
        if self.current == nil then
            return
        end
    end

    local new_version = year * 10000 + month * 100 + day
    self.current:Update(year, month, day, hour, minute, time)
    if not self.current.IsValid then
        local nnext = self.current.next
        -- remove current from list
        for i, tp in ipairs(self.list) do
            if tp == self.current then
                table.remove(self.list, i)
                break
            end
        end
        self.current = nnext
        for i, v in ipairs(self.list) do
            --local j = CommonUtil.MOD(i+1, #self.list)
            local j = i + 1
            if j > #self.list then j = 1 end
            self.list[i].next = self.list[j]
        end
    else
        if self.current.IsOver then
            if self.current.IsPeriod and #self.list == 1 then
                -- remove current from list
                for i, tp in ipairs(self.list) do
                    if tp == self.current then
                        table.remove(self.list, i)
                        break
                    end
                end
            else
                if self.version ~= new_version then
                    self.current = self.current.next
                else
                    local nnext = self.current.next
                    if nnext ~= self.current and nnext.begin_time ~= 0 then
                        local dt = os.date("*t", nnext.begin_time)
                        if dt.year == year and dt.month == month and dt.day == day then
                            self.current = nnext
                        end
                    end
                end
            end
        end
    end

    self.version = new_version
    if #self.list == 0 then
        self.IsValid = false
    end
end

return TimePeriodList

