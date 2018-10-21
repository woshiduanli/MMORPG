RoleBackpackView = {};

local this = RoleBackpackView;

local transform;
local gameObject;

function RoleBackpackView.awake(obj)
	gameObject = obj;
	transform = obj.transform;
	
	this.InitView();
end

function RoleBackpackView.InitView()
	--找到UI组件
	
	--先找到按钮
	this.btnRole = transform:Find("btnContainer/btnRole"):GetComponent("UnityEngine.UI.Button");
	this.btnBackpack = transform:Find("btnContainer/btnBackpack"):GetComponent("UnityEngine.UI.Button");
	
	--找到容器
	this.roleEquipContainer = transform:Find("panContainer/roleEquipContainer").transform;
	this.roleInfoContainer = transform:Find("panContainer/roleInfoContainer").transform;
end

function RoleBackpackView.start()
	RoleBackpackCtrl.OnStart();
end

function RoleBackpackView.update()

end

function RoleBackpackView.ondestroy()
	RoleBackpackCtrl.OnDestroy();
end