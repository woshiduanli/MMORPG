
 local ReferenceDef = GameDef.ReferenceDef
 local PropertyType = GameDef.PropertyType
 local BuffSystem = require "GameSystem.BuffSystem"

 local Base= require "ClientRole.BaseData"
 local MainPlayerData = class(Base)
 
 function MainPlayerData:ctor(data)
	MainPlayerData.instance = self
	self.Systems = {}
	self:CreateSystems(data)
	Base.ctor(self,data,data.SN)
 end

 function MainPlayerData:CreateSystems(data)
	--self:CreateSystem(GameDef.MPSystem.Equip.Name,GameDef.MPSystem.Equip.Path)
 end

 function MainPlayerData:CreateSystem(name,path)
	local def = require(path)
	self.Systems[name] = def.new()
 end

 function MainPlayerData:InitData(data)
	self.x = data.pos.x
    self.z = data.pos.y
    self.Dirx = data.forward.x
    self.Dirz = data.forward.y
	local gene = data.gene
	self.worldSN = data.worldSN
    self.RoleOcc = gene//10
    self.Gender = gene % 10
	self.RefID = gene
	local PlayerRef = ReferenceManager.GetReference(ReferenceDef.Player,self.RefID)
    self.PlayerRoleRef = PlayerRef
    self.ApprId = PlayerRef.AppearanceId
	self.XPApprId = PlayerRef.ChangedId
	local pkinfo = data.pk_info
    if pkinfo then
        self.PkValue = pkinfo.value
        self.PkValueDuration = pkinfo.duration
	end
	self:SetData(PropertyType.LEVEL, data.level)
	self:SetData(PropertyType.FIGHTVALUE, data.battlePower)
	self:SetData(PropertyType.TITLE, data.title)
	self:SetData(PropertyType.PKMODE, data.pk_mode)
	self:SetData(PropertyType.GROUPID, data.group_id)
	self:SetData(PropertyType.EudemonInBodyHp, data[PropertyType.EudemonInBodyHp])
	self:SetData(PropertyType.EudemonInBodyMaxHp, data[PropertyType.EudemonInBodyMaxHp])
	self:SetData(PropertyType.EudemonHelth, data[PropertyType.EudemonHelth])
	self:SetData(PropertyType.EUDEMONLIMIT, data[PropertyType.EUDEMONLIMIT])
	self:SetData(PropertyType.OFFLINEHOSTING, math.floor(data.offline_hosting * 60))
	self:SetData(PropertyType.DEVILLAYER, data.devil_tower_layer)
	self:SetProperty(data.stats)
	self:SetProperty(data.attributes)
	self:SetProperty(data.points)
	self:SetProperty(data.currencies)
	--self:SetProperty(data.tokens)
	
	self.RoleBuff = BuffSystem.new(data)
	self.CanShowPropoKeys = {}
	local Ptypes = ReferenceManager.GetReferences(ReferenceDef.PropertyType)
	for _,value in ipairs(Ptypes) do
		table.insert( self.CanShowPropoKeys, value )
	end

	for k,system in pairs(self.Systems) do
		system.ReadJsonData(data)
	end

	self:FireEvent(EventCode.MainPlayer.ReadData, data)
 end

 function MainPlayerData:RegEvents()
	 
 end

 function MainPlayerData:OnDispose()
	Base.OnDispose(self)
	self.Systems = nil
	self.PlayerRoleRef = nil
	MainPlayerData.instance = nil
 end

return MainPlayerData