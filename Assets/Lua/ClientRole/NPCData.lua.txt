--[[
	desc: 场景实体对象-NPC
	author: CJ
	create: 2018-06-21
 ]]
 local Base = require "ClientRole.BaseData"
 local NPCData = class(Base)
 
 function NPCData:InitData(data)
	Base.InitData(self,data)
	self.RefID = data.templet_id
	local NpcRoleRef = ReferenceManager.GetReference(GameDef.ReferenceDef.NPC,self.RefID)
	if NpcRoleRef == nil then
		Log.error(string.format( "NPC %s 找不到,将使用默认NPC 101000",self.RefID ))
		NpcRoleRef = ReferenceManager.GetReference(GameDef.ReferenceDef.NPC,101000)
	end
	self.NpcRoleRef = NpcRoleRef
	self.Scale = NpcRoleRef.Scale / 100
	self.ApprId = NpcRoleRef.ResId
	self.Name = NpcRoleRef.Name
	self.IsNpc = true
	self.IsNeverMove = true
 end

 function NPCData:OnDispose()
	Base.OnDispose(self)
	self.NpcRoleRef = nil
 end

return NPCData