
local ReferenceDef = GameDef.ReferenceDef

local ReferenceManager = {}
ReferenceManager.ReferencesMap = {}
ReferenceManager.ReferencesList = {}

function ReferenceManager.InitData()
    --- 战场
    CSGlobal.CreateLuaReference(
        ReferenceDef.Battleground.reference,
        ReferenceDef.Battleground.config,
        ReferenceDef.Battleground.view
    )
    --- 矮人宝库
    CSGlobal.CreateLuaReference(
        ReferenceDef.ShortTreasure.reference,
        ReferenceDef.ShortTreasure.config,
        ReferenceDef.ShortTreasure.view
    )
    ---副本目标
    CSGlobal.CreateLuaReference(
        ReferenceDef.MindQuizQuestion.reference,
        ReferenceDef.MindQuizQuestion.config,
        ReferenceDef.MindQuizQuestion.view
    )

    CSGlobal.CreateLuaReference(
        ReferenceDef.MindQuizReward.reference,
        ReferenceDef.MindQuizReward.config,
        ReferenceDef.MindQuizReward.view
    )
end

function ReferenceManager.CacheReference(reference, data)
    --[[local obj, a, error = Json.decode(data)

    if (error ~= nil) then
        return
    end

    local newObj = {}
    obj = obj.rows

    -- 存value值
    local obj2 = {}
    for k, v in pairs(obj) do
        table.insert(obj2, v.value)
    end

    -- 存map
    for k, v in pairs(obj) do
        local key = v.key
        newObj[key] = v.value
    end

    ReferenceManager.ReferencesMap[reference] = newObj
    ReferenceManager.ReferencesList[reference] = obj2]]--
end

function ReferenceManager.GetReferences(reference)
    if ReferenceManager.ReferencesList[reference] ~= nil then
        return ReferenceManager.ReferencesList[reference]
    end
    Log.error(reference.. " not find it  ")
end

function ReferenceManager.GetReference(reference, key)
    if key == nil then
       return
    end
    local References = ReferenceManager.ReferencesMap[reference]

    if References ~= nil and References[key] ~= nil then
        return References[key]
    end
    Log.error(reference.. "not find this key :".. key)
    return nil
end

function ReferenceManager.GetEquipResource(id)
    if id <= 0 then
        return nil
    end
    if id then
        local Ref = ReferenceManager.GetReference(ReferenceDef.EquipRef,id)
        if Ref then
            local res = Ref.Model
            if res and res ~="" then
                return res
            end
        end
    end
    return nil
 end

 function ReferenceManager.GetDessResource(id)
    if id <= 0 then
        return nil
    end
    if id then
        local Ref = ReferenceManager.GetReference(ReferenceDef.DressRef,id)
        if Ref then
            local res = Ref.ModelRes
            if res and res ~="" then
                return res
            end
        end
    end
    return nil
 end

return ReferenceManager
