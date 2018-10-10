--[[
	desc: lua 游戏管理器
	author: CJ
	create: 2018-06-16
]]

local Input = CS.UnityEngine.Input
local KeyCode = CS.UnityEngine.KeyCode
require "Core.Class"
local Config = require "LuaHelper.ConfigHelper"
local LuaDebugHelper = require "LuaHelper.LuaDebugHelper"

_G.Log = require "LuaHelper.Log"
_G.NetManager = require "Manager.NetManager"
_G.Global = require "Global"
local GameState = require "GameState.GameState"

local GameManager = {}

function GameManager.InitData()

    LuaDebugHelper.InitData()
    Config.LoadConfig()



    -- if Config.isEditor then
    --     LuaDebugHelper.InitData()
    -- end
    NetManager.InitData()
    Global.InitData()
    GameManager.NetMgr = NetManager
    GameManager.Global = Global
    GameManager.RefMgr = ReferenceManager
    GameManager.RegEvents()
    GameManager.InitManagers()
end 

--注册事件
function GameManager.RegEvents()
    GameState.RegEvents()
end

--所有Manager的初始化
function GameManager.InitManagers()
    ObjectManager.InitData()
    UIManager.InitData()
    ActivityManager.InitData()

    ObjectManager.CreateSingleObj("GameSystem.LoginSystemData","LoginSystemData")
end

function GameManager.Update(deltaTime,realtimeSinceStartup,frameCount,severtime)
    if Config.isEditor then
        LuaDebugHelper.Update()
    end
    Global.Update(deltaTime,realtimeSinceStartup,frameCount,severtime)
end

function GameManager.RateUpdate()
    if Config.isEditor then
        LuaDebugHelper.RateUpdate()
    end
end

return GameManager
