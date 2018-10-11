
local Def = require "LuaHelper.GameDef"
local ReferenceDef = Def.ReferenceDef
local EventMgr = require "Core.Event"
local Timer = require "util.Timer"

_G.Json = require "LuaHelper.JsonHelper"
_G.CSGlobal = CS.Global
_G.EventTag = require "LuaHelper.EventTag"
_G.EventCode = require "LuaHelper.EventCode"
_G.GameDef = Def
_G.CommonUtil = require "Util.CommonUtil"
_G.Language = require "Util.Language"
_G.table = require "Util.TableUtil"
_G.ReferenceDef = ReferenceDef
_G.CObject = require "Core.Object"

_G.NetManager = NetManager
_G.UIManager = require "Manager.UIManager"
_G.ActivityManager = require "Manager.ActivityManager"
_G.ReferenceManager = require "Manager.ReferenceManager"
_G.ObjectManager = require "Manager.ObjectManager"

_G.GameUI = require "UI.GameUI"
_G.UIElement = require "UI.UIElement"
_G.World = require "World.World"
_G.Activity = require "Activity.Activity"


local Global = {}
_G.Global = Global

function Global.MyEvent(proObj)
    if proObj == nil then
        print("空了")
    else
        print(proObj.Name)
        print(proObj.price)
        print(proObj.Type)
        print("bu kong ")
    end
end

--初始化网络事件
function Global.InitData()
    Global.Event = EventMgr.new()
end

function Global.FireEvent(eid, ...)
    Global.Event:FireEvent(eid, ...)
end

function Global.RegEvent(eid, handle)
    Global.Event:RegEvent(eid, handle)
end

function Global.UnregEvent(eid, handle)
    Global.Event:UnregEvent(eid, handle)
end

function Global.Update(deltaTime, realtimeSinceStartup, unscaledDeltaTime, severtime)
    Global.deltaTime = deltaTime
    Global.realtimeSinceStartup = realtimeSinceStartup
    Global.severtime = severtime
    Global.unscaledDeltaTime = unscaledDeltaTime
    Timer.Process()

    if Global.deltaTime < 0.07 then
        if gc_flag == false then
            if gc_tick > MAX_GC_TICK then
                --GC标记，在循环中分帧分步进行收集
                gc_flag = true
                gc_tick = 1
            end
            gc_tick = gc_tick + 1
        end

        -- 分步进行垃圾收集
        if gc_flag then
            gc_step = gc_step + 1
            if collectgarbage("step", 20) then
                cprint.debug(">>>>>>> collectgarbage done total, step %d", gc_step)
                gc_step = 0
                gc_flag = false
            end
        end
    end
end

--------------C#调用Lua--------------------------------------

function Global.Within(time, amount)
    return Global.realtimeSinceStartup - time < amount
end

function Global.OnLevelWasLoaded(scene)
    Global.FireEvent(EventCode.Scene.LevelWasLoaded, scene)
end

function Global.OpenUI(ui, ...)
    Global.FireEvent(EventCode.UI.OpenUI, ui, ...)
end

function Global.DisposeEvent(scene)
    Global.FireEvent(EventCode.UI.DisposeEvent, scene)
end

function Global.NetConnect()
    Global.FireEvent(EventCode.NetWork.NetConnect)
end

function Global.NetDisConnect()
    Global.FireEvent(EventCode.NetWork.NetDisConnect)
end

--------------C#调用Lua--------------------------------------

--------------Lua调用C#--------------------------------------
function Global.CreateEffect(file, x, y, scale)
    CSGlobal.CreateEffect(file, x, y, scale)
end

function Global.CreateParentEffect(file, sn, x, y, scale)
    CSGlobal.CreateParentEffect(file, sn, x, y, scale)
end

function Global.CreatePathEffect(file, bx, by, ex, ey, scale)
    CSGlobal.CreatePathEffect(file, bx, by, ex, ey, scale)
end

function Global.Get(file, bx, by, ex, ey, scale)
    CSGlobal.CreatePathEffect(file, bx, by, ex, ey, scale)
end
--------------Lua调用C#--------------------------------------
return Global
