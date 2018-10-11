--[[
	desc: lua模拟基类
 ]]

-- new()时不会自动依次调用继承链基类的ctor,只能使用base.ctor(self, ...)
-- 访问基类的属性调用使用self.xxx,如果本类中有同名的属性会覆盖掉基类的
-- 访问基类的方法调用使用base.func(self, ...)
-- 调用self:func()可以访问基类的方法,并依继承链序往顶层找,找到后就直接返回

local function clone(object)
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

function class(super)
    local superType = type(super)
    local cls

    if superType ~= "function" and superType ~= "table" then
        superType = nil
        super = nil
    end

    if super then
        cls = clone(super)
        cls.super = super
    else
        cls = {ctor = function() end}
    end

    cls.__index = cls
	cls.__name = tostring(cls)

    function cls.new(...)
        local instance = setmetatable({}, cls)
        instance.class = cls
        instance:ctor(...)
        return instance
    end    

    return cls
end
