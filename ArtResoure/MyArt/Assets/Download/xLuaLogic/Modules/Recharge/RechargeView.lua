RechargeView = {};

local this = RechargeView;

local transform;
local gameObject;

function RechargeView.awake(obj)
	gameObject = obj;
	transform = obj.transform;
	
	this.InitView();
end

function RechargeView.InitView()
	--找到UI组件
	this.txtTotalRecharge = transform:Find("imgTotalRecharge/txtTotalRecharge"):GetComponent("UnityEngine.UI.Text");
	this.content = transform:Find("Scroll View/Viewport/Content");
end

function RechargeView.start()
	RechargeCtrl.OnStart();
end

function RechargeView.update()

end

function RechargeView.ondestroy()
	RechargeCtrl.OnDestroy();
end