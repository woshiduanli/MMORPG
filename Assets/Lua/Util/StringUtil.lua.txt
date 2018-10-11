--[[
    $desc: 字符串相关的工具$
    $author: figo$
    $date: 2018-01-30$

    Copyright (C) 2018 zwwx Ltd. All rights reserved.
--]]
local StringUtil = {}

function StringUtil.Split(str, token, func)
    if str == nil then
        return {}
    end

    local init_pos = 1
    local result = {}
    local tokenlen = string.len(token)
    local str_len = string.len(str)

    while true do
        if init_pos > str_len then
            break
        end
        local start_pos, last_pos = string.find(str, token, init_pos)
        if not start_pos then
            table.insert(result, string.sub(str, init_pos, string.len(str)))
            break
        end

        table.insert(result, string.sub(str, init_pos, start_pos - 1))
        init_pos = last_pos + 1
    end

    if func ~= nil then
        for i, v in ipairs(result) do
            result[i] = func(v)
        end
    end

    return result
end

function StringUtil.IndexOf(str, char, startIndex)
    if (startIndex == nil) then
        startIndex = 1
    end

    for i = startIndex, string.len(str) do
        local s = string.sub(str, i, i)
        if (s == char) then
            return i
        end
    end

    return -1
end

function StringUtil.IndexOfAny(str, splitChars, startIndex)
    if (startIndex == nil) then
        startIndex = 1
    end

    for i = startIndex, string.len(str) do
        local char = string.sub(str, i, i)
        for _, v in ipairs(splitChars) do
            if (char == v) then
                return i
            end
        end
    end

    return -1
end

function StringUtil.LastIndexOf(str, char, startIndex)
    if (startIndex == nil) then
        startIndex = 1
    end

    for i = string.len(str), startIndex, -1 do
        local s = string.sub(str, i, i)
        if (s == char) then
            return i
        end
    end

    return -1
end

function StringUtil.Trim(str)
    return string.gsub(str, " ", "")
end

function StringUtil.IsDigit(char)
    if (string.len(char) > 1) then
        return false
    end
    local num = tonumber(char)
    if(num == nil) then
        return false
    end
    if (num >= 0 and num <= 9) then
        return true
    end
    return false
end

function StringUtil.Substring(str, index, len)
    if (len == nil) then
        len = string.len(str)
    else
        len = index + len - 1
    end
    return string.sub(str, index, len)
end

function StringUtil.Contains(str, char)
    local index = StringUtil.IndexOf(str, char)
    if (index ~= -1) then
        return true
    end
    return false
end

function StringUtil.IsNilOrEmpty(str)
    if str == nil or str == "" then
        return true
    else
        return false
    end
end

function StringUtil.ParseString2Mem(str)
    if str == nil or str == "" then
        return str
    end
    str = (string.gsub(str, "#\\r", "\r"))
    str = (string.gsub(str, "#\\n", "\n"))
    str = (string.gsub(str, "#\\t", "\t"))
    return str
end

return StringUtil
