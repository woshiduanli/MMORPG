
require "download/xLuaLogic/Common/Define"

require "download/xLuaLogic/Data/Create/TaskDBModel"
require "download/xLuaLogic/Data/TaskDBModelExt"

require "download/xLuaLogic/Data/Create/EquipDBModel" --装备
require "download/xLuaLogic/Data/Create/ItemDBModel" --道具
require "download/xLuaLogic/Data/Create/MaterialDBModel" --材料

require "download/xLuaLogic/Data/Create/RechargeShopDBModel" --充值商店
require "download/xLuaLogic/Data/Create/ShopCategoryDBModel" --商城分类
require "download/xLuaLogic/Data/Create/ShopDBModel" --商城

require "download/xLuaLogic/Data/GoodsDBModel" --物品

require "download/xLuaLogic/Data/Create/JobLevelDBModel" --职业等级


DBModelMgr = {};

local this = DBModelMgr;

--数据管理器列表
local dbModelList = {};

--初始化数据管理器
function DBModelMgr.Init()
	dbModelList[DBModelNames.TaskDBModel] = TaskDBModel.New();
	dbModelList[DBModelNames.TaskDBModel].Init();
	
	dbModelList[DBModelNames.TaskDBModelExt] = TaskDBModelExt.New();
	
	
	--充值商店
	dbModelList[DBModelNames.RechargeShopDBModel] = RechargeShopDBModel.New();
	dbModelList[DBModelNames.RechargeShopDBModel].Init();
	
	
	--商城分类
	dbModelList[DBModelNames.ShopCategoryDBModel] = ShopCategoryDBModel.New();
	dbModelList[DBModelNames.ShopCategoryDBModel].Init();
	
	--商城列表
	dbModelList[DBModelNames.ShopDBModel] = ShopDBModel.New();
	dbModelList[DBModelNames.ShopDBModel].Init();
	
	--物品
	dbModelList[DBModelNames.GoodsDBModel] = GoodsDBModel.New();
	
	--装备
	dbModelList[DBModelNames.EquipDBModel] = EquipDBModel.New();
	dbModelList[DBModelNames.EquipDBModel].Init();
	
	--道具
	dbModelList[DBModelNames.ItemDBModel] = ItemDBModel.New();
	dbModelList[DBModelNames.ItemDBModel].Init();
	
	--材料
	dbModelList[DBModelNames.MaterialDBModel] = MaterialDBModel.New();
	dbModelList[DBModelNames.MaterialDBModel].Init();
	
	--职业等级
	dbModelList[DBModelNames.JobLevelDBModel] = JobLevelDBModel.New();
	dbModelList[DBModelNames.JobLevelDBModel].Init();
	
	return this;
end


function DBModelMgr.GetDBModel(dbModelName)
	return dbModelList[dbModelName];
end