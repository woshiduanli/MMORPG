--[[
	desc: 场景实体对象-死亡幻兽
	author: CJ
	create: 2018-06-22
 ]]

 local PropertyType = GameDef.PropertyType

 local EudSoulData = class()
 
 function EudSoulData:ctor(i)
	self.index = i
 end

 function EudSoulData:OnDispose()
	 
 end


return EudSoulData