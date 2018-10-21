BackpackView = {};

local this = BackpackView;

local transform;
local gameObject;

function BackpackView.awake(obj)
	gameObject = obj;
	transform = obj.transform;
	
	this.InitView();
end

function BackpackView.InitView()
	this.lblMoney = transform:Find("img_G1/imgMoney/lblMoney"):GetComponent("UnityEngine.UI.Text");
	this.lblGold = transform:Find("img_G1/imgGold/lblGold"):GetComponent("UnityEngine.UI.Text");
	
	this.PageView = transform:GetComponent("PageView");
	this.btnAll = transform:Find("Category/btnAll"):GetComponent("UnityEngine.UI.Button");
	this.btnEquip = transform:Find("Category/btnEquip"):GetComponent("UnityEngine.UI.Button");
	this.btnItem = transform:Find("Category/btnItem"):GetComponent("UnityEngine.UI.Button");
	this.btnMaterial = transform:Find("Category/btnMaterial"):GetComponent("UnityEngine.UI.Button");
end

function BackpackView.start()

end

function BackpackView.update()

end

function BackpackView.ondestroy()

end