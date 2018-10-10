--[[
	desc: lua 游戏状态
	author: CJ
	create: 2018-07-03
]]
local MainPlayer = require "ClientRole.MainPlayerData"
local World = require "World.World"

local GameState = {}

--注册网络事件，游戏内部事件
function GameState.RegEvents()
	NetManager.RegEvent(GameDef.NetEvent.Response, EventTag.Login.PlayerDBList , GameState.OnPlayerDBList)
    NetManager.RegEvent(GameDef.NetEvent.Response, EventTag.Login.Login , GameState.OnLogin)
    NetManager.RegEvent(GameDef.NetEvent.Info, EventTag.Login.LoginInfo , GameState.OnLoginInfo)
    NetManager.RegEvent(GameDef.NetEvent.Response, EventTag.Login.EnterGame , GameState.OnEnterWorld)
    NetManager.RegEvent(GameDef.NetEvent.Event, EventTag.Login.GotoScene, GameState.OnGotoScene)
    Global.RegEvent(EventCode.Scene.LevelWasLoaded , GameState.LevelWasLoaded)
end

function GameState.OnPlayerDBList(data)
    GameState.Players = data
end

function GameState.OnLogin(sn,worldsn)
    GameState.MPData = {}
    GameState.MPData.SN = sn
    GameState.MPData.worldSN = worldsn
end

function GameState.OnLoginInfo(sn,data)
    for key, value in pairs(data) do
        GameState.MPData[key] = value
    end
    GameState.MP = MainPlayer.new(GameState.MPData)
end

function GameState.OnEnterWorld(mapid,x,z)
    GameState.MP.x = x
	GameState.MP.z = z
	GameState.MP.MapId = mapid
    if GameState.World and not GameState.World.disposed then
        GameState.World:Dispose()
    end
    GameState.World = GameState.CreateWorld(mapid)
end

function GameState.CreateWorld(mapid)
    local mr = ReferenceManager.GetReference(ReferenceDef.Map,mapid)
    if (World.IsDungeon (mr) ) then
        local f = GameState.switch[mr.InstanceType]
        if (f) then
            return f().new (mr)
        else
            return require("World.Duplicate").new(mr)
        end
    end
  
    return World.new(mr)
end

GameState.switch = {
    [ GameDef.DunGeonType.ShortTreasure.Id] = function()
        return  require ("World."..GameDef.DunGeonType.ShortTreasure.WorldName)
    end,

    [GameDef.DunGeonType.WorldBoss] = function(self)
        return self.root:CreateSingleT(typeof(CS.GodTrialDuplicate))
    end,
}


function GameState.LevelWasLoaded(scene)
    if (scene == "Login" or scene == "SelecteRole") then
        if GameState.World and not GameState.World.disposed then
			GameState.World:Dispose()
			GameState.World = nil
        end
        
        if GameState.MP and not GameState.MP.disposed then
			GameState.MP:Dispose()
			GameState.MP = nil
        end
	end
end

function GameState.OnGotoScene(worldSn,type,mapid)
    GameState.MP.worldSN = worldSn
end

return GameState
