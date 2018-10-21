print('启动了Define.lua')

require "download/xLuaLogic/Proto/ProtoCodeDef"


CtrlNames={
	RoleDataCtrl = "RoleDataCtrl",
	UIRootCtrl = "UIRootCtrl",
	TaskCtrl = "TaskCtrl",
	RechargeCtrl = "RechargeCtrl",
	ShopCtrl = "ShopCtrl",
	RoleBackpackCtrl = "RoleBackpackCtrl",
	GoodsEquipCtrl = "GoodsEquipCtrl",
	GoodsItemCtrl = "GoodsItemCtrl",
	GoodsMaterialCtrl = "GoodsMaterialCtrl"
}

DBModelNames={
	TaskDBModel = "TaskDBModel",
	TaskDBModelExt = "TaskDBModelExt",
	RechargeShopDBModel = "RechargeShopDBModel",
	ShopCategoryDBModel = "ShopCategoryDBModel",
	ShopDBModel = "ShopDBModel",
	GoodsDBModel = "GoodsDBModel",
	EquipDBModel = "EquipDBModel",
	ItemDBModel = "ItemDBModel",
	MaterialDBModel = "MaterialDBModel",
	JobLevelDBModel = "JobLevelDBModel"
}

DispatchMsg={
	RechargeOK = "RechargeOK",
	BackpackChange = "BackpackChange",
	MoneyChange = "MoneyChange",
	GoldChange = "GoldChange",
	ChangeRoleEquipInfo = "ChangeRoleEquipInfo" --更新角色装备信息
}

CommonMessageBoxTitle="提示";

--这里要把常用的引擎类型都加入进来
WWW = CS.UnityEngine.WWW;
GameObject = CS.UnityEngine.GameObject;
Color = CS.UnityEngine.Color;
Vector3 = CS.UnityEngine.Vector3;
Vector2 = CS.UnityEngine.Vector2;
