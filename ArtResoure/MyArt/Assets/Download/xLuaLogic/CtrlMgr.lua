--Lua控制器的管理器 作用就是注册所有的控制器

print('启动CtrlManager.lua')

require "download/xLuaLogic/Common/Define"

require "download/xLuaLogic/Modules/RoleData/RoleDataCtrl"
require "download/xLuaLogic/Modules/UIRoot/UIRootCtrl"
require "download/xLuaLogic/Modules/Task/TaskCtrl"
require "download/xLuaLogic/Modules/Recharge/RechargeCtrl"
require "download/xLuaLogic/Modules/Shop/ShopCtrl"
require "download/xLuaLogic/Modules/RoleBackpack/RoleBackpackCtrl"
require "download/xLuaLogic/Modules/Goods/GoodsEquipCtrl"
require "download/xLuaLogic/Modules/Goods/GoodsItemCtrl"
require "download/xLuaLogic/Modules/Goods/GoodsMaterialCtrl"

CtrlMgr = {};

local this = CtrlMgr;

--控制器列表
local ctrlList = {};

--初始化 往列表中添加所有的控制器
function CtrlMgr.Init()
	ctrlList[CtrlNames.RoleDataCtrl] = RoleDataCtrl.New();
	ctrlList[CtrlNames.UIRootCtrl] = UIRootCtrl.New();
	ctrlList[CtrlNames.TaskCtrl] = TaskCtrl.New();
	ctrlList[CtrlNames.RechargeCtrl] = RechargeCtrl.New();
	ctrlList[CtrlNames.ShopCtrl] = ShopCtrl.New();
	ctrlList[CtrlNames.RoleBackpackCtrl] = RoleBackpackCtrl.New();
	ctrlList[CtrlNames.GoodsEquipCtrl] = GoodsEquipCtrl.New();
	ctrlList[CtrlNames.GoodsItemCtrl] = GoodsItemCtrl.New();
	ctrlList[CtrlNames.GoodsMaterialCtrl] = GoodsMaterialCtrl.New();
	return this;
end

--根据控制器的名称 获取控制器
function CtrlMgr.GetCtrl(ctrlName)
	return ctrlList[ctrlName];
end