--[[
	desc: 场景实体对象-怪
	author: CJ
	create: 2018-06-21
 ]]
local MonsterType = GameDef.MonsterType
local PropertyType = GameDef.PropertyType
local BuffSystem = require "GameSystem.BuffSystem"

local Base = require "ClientRole.BaseData"
local MonsterData = class(Base)

function MonsterData:InitData(data)
    Base.InitData(self, data)
    self.RefID = data.templet_id
    self:SetData(PropertyType.CAMP, data.camp)
    local MonsterRef = ReferenceManager.GetReference(GameDef.ReferenceDef.Monster, self.RefID)
    self.MonsterRoleRef = MonsterRef
    if MonsterRef == nil then
        Log.error(string.format("monster %s reference is null", self.RefID))
    end
    if (MonsterRoleRef ~= nil) then
        self.Scale = MonsterRoleRef.ScalePercent / 100
    end
    self.Name = MonsterRef.Name
    self.ApprId = MonsterRef.ResId
    local monsterType = MonsterRef.Type
    self.monsterType = monsterType
    self.IsMonster = true

    if monsterType == MonsterType.Normal then
        self.IsNomalMonster = true
    end

    if monsterType == MonsterType.Elite then
        self.IsElite = true
    end

    if monsterType == MonsterType.Boss then
        self.IsBoss = true
    end

    if monsterType == MonsterType.WorldBoss then
        self.IsWorldBoss = true
    end

    --[[self.IsBorn = data.is_born
	self.masterSn = data.master_id
	local exterior = data.exterior
	if exterior then
		local showdata = exterior.show_data
		if showdata then
			local summonType,summonData,masterName = table.unpack(showdata)
			self.masterName = masterName
			self.isCarrige = summonType == "cart"
			self.carrigeLevel = 0
			if self.isCarrige then
				self.carrigeLevel = summonData
			end
		end
	end]]
    self.RoleBuff = BuffSystem.new(data)
end

function MonsterData:OnDispose()
    Base.OnDispose(self)
    self.MonsterRoleRef = nil
end

return MonsterData
