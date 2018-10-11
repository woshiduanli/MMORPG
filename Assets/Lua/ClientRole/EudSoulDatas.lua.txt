
 local EudSoul = require "ClientRole.EudSoulData"
 local EudSoulDatas = class()
 
 function EudSoulDatas:ctor(data)
	self.EudSouls = {}
 end

 function EudSoulDatas:UpdateDeadPetNum(num)
	 self.DeadPetNum = num
 end

 function EudSoulDatas:UpdateBattleEudNum(num)
	self.BattleEudNum = self.BattleEudNum + num
 end

 function EudSoulDatas:CreateEudSoul(i)
 end

 function EudSoulDatas:UpdateEudSoul()
 end

 function EudSoulDatas:ClearOneEudSoul()
 end

 function EudSoulDatas:ClearAllEudsoul()
 end

 function EudSoulDatas:RestEudSoulOwner()
 end

 function EudSoulDatas:OnPlayerUpdate()
 end

 function EudSoulDatas:OnDispose()
	self.EudSouls = nil
 end

return EudSoulDatas