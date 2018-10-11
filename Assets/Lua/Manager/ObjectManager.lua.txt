local ObjectManager = {}

function ObjectManager.InitData()
    ObjectManager.SingleObjMap = {}
    ObjectManager.ObjTypeMap = {}
end

--
function ObjectManager.CreateSingleObj(path, name, ...)
    local obj = ObjectManager.SingleObjMap[name]
    if obj == nil or obj == -1 then
        local def = require(path)
        local SingleObj = def.new(name, ...)
        ObjectManager.SingleObjMap[name] = SingleObj
    end
    return ObjectManager.SingleObjMap[name]
end

function ObjectManager.GetSingleObj(name, ...)
    local obj = ObjectManager.SingleObjMap[name]
    if obj == nil or obj == -1 then
        return nil
    end
    return obj
end

function ObjectManager.DestroySingleObj(name, ...)
    local obj = ObjectManager.SingleObjMap[name]
    if obj == nil or obj == -1 then
        return
    end

    ObjectManager.SingleObjMap = table.removeElementByKey(ObjectManager.SingleObjMap, name)
end

function ObjectManager.CreateObj(ObjType, path, name, ...)
    local obj = ObjectManager.ObjTypeMap[ObjType]

    if obj == nil or obj == -1 then
        ObjectManager.ObjTypeMap[ObjType] = {}
    end

    if ~table.containsKey(ObjectManager.ObjTypeMap[ObjType], name) then
        local def = require(path)
        local OneObj = def.new(name, ...)
        ObjectManager.ObjTypeMap[ObjType].name = OneObj
    end

    return ObjectManager.ObjTypeMap[ObjType].name
end

function ObjectManager.GetObj(ObjType, name)
    local obj = ObjectManager.ObjTypeMap[ObjType]
    if obj == nil or obj == -1 then
        return nil
    end
    if obj.name == nil or obj[name] == -1 then
        return nil
    end
    return obj.name
end

return ObjectManager
