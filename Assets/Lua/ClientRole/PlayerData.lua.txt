
 local tblinsert = table.insert
 local ReferenceDef = GameDef.ReferenceDef
 local PropertyType = GameDef.PropertyType
 local EudSoulDatas = require "ClientRole.EudSoulDatas"
 local BuffSystem = require "GameData.BuffSystem"

 local Base= require "ClientRole.BaseData"
 local PlayerData = class(Base)

 function PlayerData:InitData(data)
    Base.InitData(self,data)
    self.IsPlayer = true
    if data.wild_leader_blongs > 0 then
        self.wildLeaderBelong = true
    end
    self:SetData(data.body_state)
    self:SetData(PropertyType.TITLE,data[PropertyType.TITLE])
    self:SetData(PropertyType.APPELLATION,data[PropertyType.APPELLATION])
    self:SetData(PropertyType.LEVEL,data[PropertyType.LEVEL])
    self:SetData(PropertyType.GROUPID,data[PropertyType.GROUPID])
    self:SetData(PropertyType.EudemonInBodyHp,data[PropertyType.EudemonInBodyHp])
    self:SetData(PropertyType.EudemonInBodyMaxHp,data[PropertyType.EudemonInBodyMaxHp])
    self:SetData(PropertyType.EudemonHelth,data[PropertyType.EudemonHelth])
    self:SetData(PropertyType.EUDEMONLIMIT,data[PropertyType.EUDEMONLIMIT])
    self:SetData(PropertyType.Nobility,data[PropertyType.Nobility])
    self:SetData(PropertyType.Prestige,data[PropertyType.Prestige])
    self:SetData(PropertyType.TEAMID,data[PropertyType.TEAMID])

    self.GroupMemberType = data[PropertyType.GROUPMEMBERTYPE]
    self.GroupSecondMemberType = data[PropertyType.GROUPSECONDMEMBERTYPE]
    self.GroupName = data[PropertyType.GROUPNAME]

    self.Name = data.name
    local gene = data.gene
    self.RoleOcc = gene//10
    self.Gender = gene % 10
    self.RefID = gene
    local PlayerRef = ReferenceManager.GetReference(ReferenceDef.Player,self.RefID)
    self.PlayerRoleRef = PlayerRef
    self.ApprId = PlayerRef.AppearanceId
    self.XPApprId = PlayerRef.ChangedId
    self.ExpRef = ReferenceManager.GetReference(ReferenceDef.RoleExp,self:GetData(PropertyType.LEVEL))
    self:ReadEquip(data.exterior)
    local mref = ReferenceManager.GetReference(ReferenceDef.MountRef,self:GetData(PropertyType.MOUNTING))
    if mref and mref.ResId > 0 then
        self.MountAppride = mref.ResId
        self.isRideState = true
    end
    local pkinfo = data.pk_info
    if pkinfo then
        self.PkValue = pkinfo.value
        self.PkValueDuration = pkinfo.duration
    end
    self.EudSouls = EudSoulDatas.new(data)
    self.RoleBuff = BuffSystem.new(data)
 end

 function PlayerData:ReadEquip(equipdata)
     self.Equip = {}

     local weaponres = ReferenceManager.GetEquipResource(equipdata.weapon)
     if weaponres  then
        tblinsert(self.Equip, weaponres)
     end

     local clothres = ReferenceManager.GetEquipResource(equipdata.cloth)
     if clothres  then
        tblinsert(self.Equip, clothres)
     end

     local dressres = ReferenceManager.GetDessResource(equipdata.dress)
     if dressres  then
        tblinsert(self.Equip, dressres)
     end
 end
 
 function PlayerData:LevelChange() 
 end
 
 function PlayerData:HpChange() 
 end
 
 function PlayerData:XpStateChange() 
 end
 
 function PlayerData:MountStateChange() 
 end
 
 function PlayerData:RankChanged() 
 end
 
 function PlayerData:NameChange(newName)  
 end

 function PlayerData:OnPropertyChange(key,data)
     if key ==  PropertyType.LEVEL then
        self:LevelChange()
        return
      end
      if key ==  PropertyType.HP then
        self:HpChange()
        return
      end
      if key ==  PropertyType.BODYSTATE then
        self:XpStateChange()
        return
      end
      if key ==  PropertyType.MOUNTING then
        self:MountStateChange()
        return
      end
     
     if key ==  PropertyType.RANK then
        self:RankChanged()
        return
     end
     if key ==  PropertyType.GROUPID then
        self:FireEvent(EventCode.Group.OnPropertyGroupChange , self.SN)
        self:FireEvent(EventCode.Group.OnPropertyGroupIdChange , self.SN)
     end
     if key ==  PropertyType.GROUPNAME then
        self:FireEvent(EventCode.Group.OnPropertyGroupChange , self.SN)
        return
     end
     if key ==  PropertyType.TITLE then
        self:FireEvent(EventCode.Title.TitleChange , PropertyType.TITLE , self.SN)
        return
     end
     if key ==  PropertyType.APPELLATION then
        self:FireEvent(EventCode.Appellation.AppellationChange , self.SN)
        return
     end
     if key ==  PropertyType.NAME then
        self:NameChange(data.name)
        self:FireEvent(EventCode.PlayerInfo.PlayerNameChanged , self.SN)
        return
     end
     if key ==  PropertyType.Prestige or key ==  PropertyType.Nobility then
        self:FireEvent(EventCode.Nobility.NobilityOrPrestigeChanged , self.SN)
        return
     end
     if key ==  PropertyType.GROUPMEMBERTYPE then
        self:FireEvent(EventCode.PlayerInfo.PlayerGroupInfoChanged , self.SN)
        return
     end
     if key ==  PropertyType.GROUPSECONDMEMBERTYPE then
        self:FireEvent(EventCode.PlayerInfo.PlayerGroupInfoChanged , self.SN)
        return
     end
     if key ==  PropertyType.TEAMID then
        self:FireEvent(EventCode.PlayerInfo.OnPropertyTeamIdChange , self.SN)
        return
     end
 end

 --死亡幻兽灵魂
 function PlayerData:UpdateDeadPetNum(num)
     self.EudSouls:UpdateDeadPetNum(num)
 end

 function PlayerData:OnDispose()
    Base.OnDispose(self)
	self.PlayerRoleRef = nil
 end

return PlayerData