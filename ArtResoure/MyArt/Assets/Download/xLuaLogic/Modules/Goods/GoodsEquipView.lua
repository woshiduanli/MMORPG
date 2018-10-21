GoodsEquipView = {};

local this = GoodsEquipView;

local transform;
local gameObject;

function GoodsEquipView.awake(obj)
	gameObject = obj;
	transform = obj.transform;
	
	this.InitView();
end

function GoodsEquipView.InitView()
	--找到UI组件
	
	this.lblGoodsName = transform:Find("lblGoodsName"):GetComponent("UnityEngine.UI.Text"); --物品名称
	this.lblLevel = transform:Find("lblLevel"):GetComponent("UnityEngine.UI.Text"); --物品使用等级
	this.lblGoodsType = transform:Find("lblGoodsType"):GetComponent("UnityEngine.UI.Text"); --物品子类类型
	this.imgIcon = transform:Find("GoodsIcon/imgIcon"):GetComponent("UnityEngine.UI.Image"); --物品图标
	
	this.imgJewel1 = transform:Find("imgJewelContainer/imgJewelHole1/imgJewel"):GetComponent("UnityEngine.UI.Image"); --宝石1
	this.imgJewel2 = transform:Find("imgJewelContainer/imgJewelHole2/imgJewel"):GetComponent("UnityEngine.UI.Image"); --宝石2
	this.imgJewel3 = transform:Find("imgJewelContainer/imgJewelHole3/imgJewel"):GetComponent("UnityEngine.UI.Image"); --宝石3
	this.imgJewel4 = transform:Find("imgJewelContainer/imgJewelHole4/imgJewel"):GetComponent("UnityEngine.UI.Image"); --宝石4
	
	this.attrContent = transform:Find("Scroll View/Viewport/Content"); --属性容器
	
	this.btnPutOn = transform:Find("btnPutOn"):GetComponent("UnityEngine.UI.Button"); --穿上
	this.txtPutOn = transform:Find("btnPutOn/Text"):GetComponent("UnityEngine.UI.Text"); --穿上脱下按钮文本
	this.btnSell = transform:Find("btnSell"):GetComponent("UnityEngine.UI.Button"); --出售
end

function GoodsEquipView.start()

end

function GoodsEquipView.update()

end

function GoodsEquipView.ondestroy()
	GoodsEquipCtrl.OnDestroy();
end