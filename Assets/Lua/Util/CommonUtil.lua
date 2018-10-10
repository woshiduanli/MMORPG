--[[
	desc: 通用帮助工具
]]
local CommonUtil = {}

-- 返回新的函数，调用func时将self作为第一个参数传入
function CommonUtil.GetSelfFunc(self, func)
    if nil == func then
        return
    end

    local ret = function(...)
        func(self, ...)
    end

    return ret
end

function CommonUtil.clone(object)
    local lookup_table = {}
    local function _copy(object)
        if type(object) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end
        local new_table = {}
        lookup_table[object] = new_table
        for key, value in pairs(object) do
            new_table[_copy(key)] = _copy(value)
        end
        return setmetatable(new_table, getmetatable(object))
    end
    return _copy(object)
end

function CommonUtil.MOD(a, b)
    return a - math.floor(a / b) * b
end


function CommonUtil.IntegerToChinese(value)
    local sb = {}
    -- 如果是负数则令其为正，返回串前缀“负”
    if (value < 0) then
        value = -value
        table.insert(sb, CErrorCode.Common.MathSign[0])
    end

    -- 如果不大于9，则直接返回单个字符
    if (value <= 9) then
        table.insert(sb, CErrorCode.Common.IntegerToChineseLower[value])
        local ret = ""
        for i, v in ipairs(sb) do
            ret = ret .. v
        end
        return ret
    end

    -- 处理大于9的数值
    -- 将数值按“个十百千……”位分开
    local str = tostring(value)
    local chars = {}
    local tempvalue = value
    while tempvalue ~= 0 do
        tempvalue = math.floor(tempvalue / 10)
        local remainder = math.floor(value % 10)
        value = tempvalue
        table.insert(chars, remainder)
    end

    -- “单位”数组的下标
    local unit = #chars - 2
    -- 上一位数值是零吗？
    local lastIsZero = false
    for i = #chars, 1, -1 do
        local num = tonumber(chars[i])
        -- 如果数值为0，则不需要转换为“零”字符，且：
        --		若当前位是“亿”或“万”时，加上当前位的“单位”
        --		（注，须避免“亿”和“万”两个单位相连）
        if (num == 0) then
            if (unit == 7 or (unit == 3 and sb[#sb] ~= CErrorCode.Common.IntegerToChineseUnitLower[7])) then
                table.insert(sb, CErrorCode.Common.IntegerToChineseUnitLower[unit])
            end
            lastIsZero = true
        else
            -- // 如果当前位数值不为0，则判断上一位是否为0
            -- //		若是，则需把上一位的0转换为“零”字符
            if (lastIsZero) then
                table.insert(sb, CErrorCode.Common.IntegerToChineseLower[0])

                lastIsZero = false
            end
            table.insert(sb, CErrorCode.Common.IntegerToChineseLower[num])

            if (unit >= 0) then
                table.insert(sb, CErrorCode.Common.IntegerToChineseUnitLower[unit])
            end
        end
        unit = unit - 1
    end
    if
        (#sb >= 2 and sb[1] == CErrorCode.Common.IntegerToChineseLower[1] and
            sb[2] == CErrorCode.Common.IntegerToChineseUnitLower[0])
     then
        table.remove(sb, 1)
    end

    local ret = ""
    for i, v in ipairs(sb) do
        ret = ret .. v
    end

    return ret
end

--@desc: 保留两位小数
function CommonUtil.ToString2f(x)
    local s = tostring(x)
    local index = string.find(s, "%.")
    if index >= 0 then
        if x % 1 == 0 then --小数部分为0,例如5.0
            s = string.sub(s, 1, index - 1)
        else
            s = string.sub(s, 1, index + 3)
        end
    end
    return s
end


--@desc: 求字符串长度
function CommonUtil.Strlength(str)
    if StringUtil.IsNilOrEmpty(str) then
        return 0
    end
    local len = 0
    local b
    local j = 1
    for i = 1, #str do
        b = string.byte(str, j)
        if b == nil then
            break
        end
        --中文占3字节,第1个字节取值范围228-233,第2、3字节取值范围128-191
        if b > 127 then --非英文字符
            j = j + 3
        else
            j = j + 1
        end
        len = len + 1
    end
    return len
end

function CommonUtil.GetColorStrByQualityNum(quality)
    if(quality == 1) then return "#FFFFFF" end
    if(quality == 2) then return "#77FF25" end
    if(quality == 3) then return "#0799FF" end
    if(quality == 4) then return "#EE0DED" end
    if(quality == 5) then return "#FF8213" end
    if(quality == 6) then return "#FF1313" end
    return "#FFFFFF"
end

function CommonUtil.GenerateItems(item, list, count)
    local length = #list
    if count > length then
        for index = length + 1, count do
            local newItem = item:Clone()
            table.insert(list, newItem)
        end
    elseif count < length then
        for index = count + 1, length do
            list[index]:SetActive(0);
        end
    end
end

-- time_tick:毫秒
function CommonUtil.GetDateTimeMS(time_tick)
    return os.date("%Y%m%d%H", time_tick / 1000)
end

-- time_tick:秒
function CommonUtil.GetDateTimeS(time_tick)
    return os.date("%Y%m%d%H", time_tick)
end

return CommonUtil
