local Input = CS.UnityEngine.Input
local KeyCode = CS.UnityEngine.KeyCode

local LuaDebugHelper = {}

function LuaDebugHelper.InitData()
     breakSocketHandle,debugXpCall = require('LuaDebug')('localhost',7003)
end

function LuaDebugHelper.RateUpdate()
    if (breakSocketHandle) then
        breakSocketHandle()
    end
end

function LuaDebugHelper.Update()
    if Input.GetKeyDown(KeyCode.F5) then
        breakSocketHandle,debugXpCall = require('LuaDebug')('localhost',7003)
    end
end

return LuaDebugHelper
