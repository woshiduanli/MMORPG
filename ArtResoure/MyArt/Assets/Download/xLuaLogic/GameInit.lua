print('启动GameInit.lua')

require "download/xLuaLogic/CtrlMgr"
require "download/xLuaLogic/Data/DBModelMgr"


GameInit = {};
local this = GameInit;

function GameInit.InitViews()
	require('download/xLuaLogic/Modules/UIRoot/UIRootView');
	require('download/xLuaLogic/Modules/Task/TaskView');
	
	require('download/xLuaLogic/Modules/Recharge/RechargeView');
	require('download/xLuaLogic/Modules/Shop/ShopView');
	require('download/xLuaLogic/Modules/RoleBackpack/RoleBackpackView');
	require('download/xLuaLogic/Modules/RoleBackpack/RoleInfoView');
	require('download/xLuaLogic/Modules/RoleBackpack/BackpackView');
	require('download/xLuaLogic/Modules/RoleBackpack/RoleEquipView');
	require('download/xLuaLogic/Modules/Goods/GoodsEquipView');
	require('download/xLuaLogic/Modules/Goods/GoodsItemView');
	require('download/xLuaLogic/Modules/Goods/GoodsMaterialView');
end


function GameInit.Init()
	this.InitViews();
	CtrlMgr.Init();
	
	DBModelMgr.Init();
	
	--GameInit.LoadView(CtrlNames.UIRootCtrl);
end

function GameInit.LoadView(type, parameters)
	local ctrl = CtrlMgr.GetCtrl(type);
	if ctrl ~= nil then
		ctrl.Awake(parameters);
	end
end