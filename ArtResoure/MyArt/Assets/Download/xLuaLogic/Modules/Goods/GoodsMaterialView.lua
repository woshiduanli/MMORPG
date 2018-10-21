GoodsMaterialView = {};

local this = GoodsMaterialView;

local transform;
local gameObject;

function GoodsMaterialView.awake(obj)
	gameObject = obj;
	transform = obj.transform;
	
	this.InitView();
end

function GoodsMaterialView.InitView()
	--找到UI组件
	this.lblGoodsName = transform:Find("lblGoodsName"):GetComponent("UnityEngine.UI.Text"); --物品名称
	this.lblLevel = transform:Find("lblLevel"):GetComponent("UnityEngine.UI.Text"); --物品使用等级
	this.lblGoodsType = transform:Find("lblGoodsType"):GetComponent("UnityEngine.UI.Text"); --物品子类类型
	this.imgIcon = transform:Find("GoodsIcon/imgIcon"):GetComponent("UnityEngine.UI.Image"); --物品图标
	
	this.lblDesc = transform:Find("imgDesc/lblDesc"):GetComponent("UnityEngine.UI.Text"); --描述
	this.lblSell = transform:Find("lblSell"):GetComponent("UnityEngine.UI.Text"); --售价
	this.btnSell = transform:Find("btnSell"):GetComponent("UnityEngine.UI.Button"); --出售按钮
end

function GoodsMaterialView.start()

end

function GoodsMaterialView.update()

end

function GoodsMaterialView.ondestroy()
	GoodsMaterialCtrl.OnDestroy();
end