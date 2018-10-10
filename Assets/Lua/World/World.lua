--[[
	desc: 场景管理器，负责管理场景里面所有的实体对象
	author: CJ
	create: 2018-06-22
 ]]

local DunGeonType = GameDef.DunGeonType
local MapType = GameDef.MapType

local Eudemons = require "ClientRole.EudemonsData"
local MapObject = require "ClientRole.MapObjectData"
local Monster = require "ClientRole.MonsterData"
local NPC = require "ClientRole.NPCData"
local Player = require "ClientRole.PlayerData"
local Summon = require "ClientRole.SummonData"
local MainPlayer = require "ClientRole.MainPlayerData"

local Base = require "core.Object"
local World = class(Base)
 
function World:InitData(mr)
	World.instance = self
	self.Ref = mr
	self.MapType =  mr.Type
	self.MapID = mr.Id
	self.roles = {}
	self.MapObjects = {}
end

function World:RegEvents()
	self:RegEvent(EventCode.Scene.LevelWasLoaded ,CommonUtil.GetSelfFunc(self,self.OnLevelWasLoaded))
	NetManager.RegEvent(GameDef.NetEvent.Event, EventTag.World.EnterPlayer, CommonUtil.GetSelfFunc(self,self.OnEnterPlayer))
	NetManager.RegEvent(GameDef.NetEvent.Event, EventTag.World.EnterMonster, CommonUtil.GetSelfFunc(self,self.OnEnterMonsetr))
	NetManager.RegEvent(GameDef.NetEvent.Event, EventTag.World.DeleteRole, CommonUtil.GetSelfFunc(self,self.LeaveRole))
	NetManager.RegEvent(GameDef.NetEvent.Event, EventTag.World.RefreshMonster, CommonUtil.GetSelfFunc(self,self.OnMonsetrPropChange))
end

function World:Update()
	for _,role in pairs(self.roles) do
		role:Update()
	end
end

function World:OnLevelWasLoaded(scene)
    if (scene == "Login" or scene == "SelecteRole" or scene == "AsyncLevelLoader") then
        return 
	end
	self:CreateMainPlayer()
	self:CreateMainFace()
end

function World:CreateMainPlayer()
end

function World:CreateMainFace()
end

function World:OnMainFaceCreateDone()
end

function World:OnMainFaceCreated()
end

--玩家列表相关
function World:OnEnterPlayer(sn,data)
	if self:GetRole(sn) then
		Log.error(sn)
		self:LeaveRole(sn)
	end
	self.roles[sn] = Player.new(data,sn)
end

function World:OnEnterMonsetr(sn,data )
	local roletype = data.type
	if roletype == GameDef.RoleType.MapObject then
		if self:GetMapObject(sn) then
			Log.error(sn)
			self:LeaveRole(sn)
		end
		self:AddMapObject(sn,data)
	else
		if self:GetRole(sn) then
			Log.error(SN)
			self:LeaveRole(sn)
		end
		self:AddRole(sn,data)
	end
end

function World:AddRole(sn,data)
	local roletype = data.type
	if roletype == GameDef.RoleType.Player or roletype == GameDef.RoleType.VirTual then
		self.roles[sn] = Player.new(data,sn)
	elseif roletype == GameDef.RoleType.Monster then
		self.roles[sn] = Monster.new(data,sn)
	elseif roletype == GameDef.RoleType.Eudemons then
		self.roles[sn] = Eudemons.new(data,sn)
	elseif roletype == GameDef.RoleType.NPC then
		self.roles[sn] = NPC.new(data,sn)
	elseif roletype == GameDef.RoleType.Summon then --召唤物
		self.roles[sn] = Summon.new(data,sn)
	end
end

function World:GetRole(sn)
	if sn == MainPlayer.instance.SN then
		return MainPlayer.instance
	end
	return self.roles[sn]
end

function World:LeaveRole(sn)
	local role = self.roles[sn]
	if role then
		role:Dispose()
		self.roles[sn] = nil
	else
		self:LeaveMapObject(sn)
	end
end

--地图物件
function World:AddMapObject(sn,data)
	self.MapObjects[sn] = MapObject.new(data,sn)
end

function World:GetMapObject(sn)
	return self.MapObjects[sn]
end

function World:LeaveMapObject(sn)
	local mapobject = self.MapObjects[sn]
	if mapobject then
		mapobject:Dispose()	
		self.MapObjects[sn] = nil
	end
end

function World:OnMonsetrPropChange(sn,data)
	local mapobject = self.MapObjects[sn]
	if mapobject then
		mapobject:OnPropChange(data)	
	end
end

function World.IsCity(mapref) return mapref.Type == MapType.CITY end
function World.IsDungeon(mapref)  return mapref.Type == MapType.DUNGEON end
function World.IsFIELD(mapref)  return mapref.Type == MapType.FIELD end
function World.IsWildBoss(mapref)  return mapref.Type == MapType.WILDBOSS end
function World.IsPracTice(mapref)  return mapref.Type == MapType.PRACTICE end
function World.IsBossHome(mapref)  return mapref.Type == MapType.BOSSHOME end
function World.IsPVP(mapref)  return mapref.InstanceType == DunGeonType.PVP end
function World.IsWorldBoss(mapref)  return mapref.InstanceType == DunGeonType.WorldBoss end
function World.IsFieldHanGup(mapref)  return mapref.InstanceType == DunGeonType.FieldHanGup end
function World.IsWorldBossBattle(mapref)  return mapref.InstanceType == DunGeonType.WorldBossBattle end
function World.IsFameHall(mapref)  return mapref.InstanceType == DunGeonType.FameHall end
function World.IsBraveTower(mapref)  return mapref.InstanceType == DunGeonType.BraveTower end
function World.IsPersonalBoss(mapref)  return mapref.InstanceType == DunGeonType.PersonalBoss end
function World.IsPrivilegeBoss(mapref)  return mapref.InstanceType == DunGeonType.PrivilegeBoos end
function World.IsFrozenCellar(mapref)  return mapref.InstanceType == DunGeonType.FrozenCellar end
function World.IsDragonTreasure(mapref)  return mapref.InstanceType == DunGeonType.DragonTreasure end
function World.IsChapterDungeon(mapref)  return mapref.InstanceType == DunGeonType.ChapterDungeon end
function World.IsDaily(mapref)  return mapref.InstanceType == DunGeonType.Daily end

function World:OnDispose()
	World.instance = nil
	for _,role in pairs(self.roles) do
		role:Dispose()
	end

	for _,mapobj in pairs(self.MapObjects) do
		mapobj:Dispose()
	end

	self.roles = nil
	self.MapObjects = nil
end

return World