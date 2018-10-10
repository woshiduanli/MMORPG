local table = table

-- 此时key必须 是 1, 2, 3,4, 5 的格式
function table.removevalue(tb, value)
    if (value == nil) then
        return
    end

    local index = nil
    for i, v in ipairs(tb) do
        if (v == value) then
            index = i
            break
        end
    end

    if (index ~= nil) then
        table.remove(tb, index)
    end
end

function table.removeElementByKey(tbl, key)
    local tmp = {}
    for i in pairs(tbl) do
        table.insert(tmp, i)
    end
    
    local newTbl = {}
    local i = 1
    while i <= #tmp do
        local val = tmp[i]
        if val == key then
            table.remove(tmp, i)
        else
            newTbl[val] = tbl[val]
            i = i + 1
        end
    end
    return newTbl
end

function table.containsValue(tb, value)
    local b = false
    if tb == nil or value == nil then
        return b
    end
    for k, v in pairs(tb) do
        if v == value then
            b = true
            break
        end
    end
    return b
end

function table.containsKey(tb, key)
    local b = false
    if tb == nil or key == nil then
        return b
    end
    for k, v in pairs(tb) do
        if k == key then
            b = true
            break
        end
    end
    return b
end


function table.containsValue(tb, value)
    local b = false
    if tb == nil or value == nil then
        return b
    end
    for k, v in pairs(tb) do
        if v == value then
            b = true
            break
        end
    end
    return b
end


function table.IsArrayTable(t)
    if type(t) ~= "table" then
        return false
    end

    local n = #t
    for k, v in pairs(t) do
        if type(k) ~= "number" then
            return false
        end

        if k > n then
            return false
        end
    end

    return true
end

function table.length(tb)
    local count = 0
    for k, v in pairs(tb) do
        count = count + 1
    end

    return count
end

function table.foreach(tb, cb)
    for k, v in table.pairsByKeys(tb) do
        if (cb ~= nil) then
            cprint.xpcall(cb, k, v)
        end
    end
end

function table.pairsByKeys(t)
    local a = {}
    for n in pairs(t) do
        a[#a + 1] = n
    end
    table.sort(a)
    local i = 0
    return function()
        i = i + 1
        return a[i], t[a[i]]
    end
end

function table.addRange(tb, ...)
    local addtable = function(tb1, ptb)
        if tb1 == nil or ptb == nil then
            return
        end
        for k, v in pairs(ptb) do
            table.insert(tb1, v)
        end
    end

    local ptbl = {...}
    for i = 1, #ptbl do
        local p = ptbl[i]
        if type(p) == "table" then --如果p是个集合
            addtable(tb, p)
        else
            table.insert(tb, p)
        end
    end
end

return table
