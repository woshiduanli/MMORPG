
--CS.UIRoleEquipView对应修复的类
--SetUI 对应类的方法


xlua.hotfix(CS.UIRoleEquipView, 'SetUI', function(self, data)
	--这里的self 表示 UIRoleEquipView对象
	--data:GetValue 这个 GetValue方法不能用泛型的，所以要写一个不是泛型的方法
	self.m_JobId = data:GetValue(CS.ConstDefine.JobId);
	self.lblNickName.text = data:GetValue(CS.ConstDefine.NickName);
	self.lblLevel.text = string.format("Lv.%s", data:GetValue(CS.ConstDefine.Level));
	self.lblFighting.text = string.format("全部战斗力：<color='#ff0000'>%s</color>", data:GetValue(CS.ConstDefine.Fighting));
end)