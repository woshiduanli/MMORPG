
 local Log = require "LuaHelper.Log"
 local ReferenceMgr = require "Manager.ReferenceManager"
 local Def = require "LuaHelper.GameDef"

 local Base = require "core.Object"
 local BuffSystem = class(Base)
 
 function BuffSystem:ctor(data)
	self.Buffs = {}
	local buffs = data.buff_list
	if buffs == nil then
		return
	end
 end

 function BuffSystem:OnDispose()
	self.Buffs = nil
 end

return BuffSystem