RoleInfoView = {};

local this = RoleInfoView;

local transform;
local gameObject;

function RoleInfoView.awake(obj)
	gameObject = obj;
	transform = obj.transform;
	this.InitView();
end

function RoleInfoView.InitView()
	--找到UI组件
	this.lblMoney = transform:Find("img_G1/imgMoney/lblMoney"):GetComponent("UnityEngine.UI.Text");
	this.lblGold = transform:Find("img_G1/imgGold/lblGold"):GetComponent("UnityEngine.UI.Text");
	
	this.sliderHP = transform:Find("img_G2/lblHPDis/sliderHP"):GetComponent("UnityEngine.UI.Slider");
	this.sliderMP = transform:Find("img_G2/lblMP/sliderMP"):GetComponent("UnityEngine.UI.Slider");
	this.siliderExp = transform:Find("img_G2/lblEXP/siliderExp"):GetComponent("UnityEngine.UI.Slider");
	
	this.lblHP = transform:Find("img_G2/lblHPDis/lblHP"):GetComponent("UnityEngine.UI.Text");
	this.lblMP = transform:Find("img_G2/lblMP/lblMP"):GetComponent("UnityEngine.UI.Text");
	this.lblExp = transform:Find("img_G2/lblEXP/lblExp"):GetComponent("UnityEngine.UI.Text");
	
	this.lblAttack = transform:Find("img_G3/lblAttackDis/lblAttack"):GetComponent("UnityEngine.UI.Text");
	this.lblDefense = transform:Find("img_G3/lblDefenseDis/lblDefense"):GetComponent("UnityEngine.UI.Text");
	this.lblDodge = transform:Find("img_G3/lblDodgeDis/lblDodge"):GetComponent("UnityEngine.UI.Text");
	this.lblHit = transform:Find("img_G3/lblHitDis/lblHit"):GetComponent("UnityEngine.UI.Text");
	this.lblCri = transform:Find("img_G3/lblCriDis/lblCri"):GetComponent("UnityEngine.UI.Text");
	this.lblRes = transform:Find("img_G3/lblResDis/lblRes"):GetComponent("UnityEngine.UI.Text");
	
	
end

function RoleInfoView.start()

end

function RoleInfoView.update()

end

function RoleInfoView.ondestroy()

end