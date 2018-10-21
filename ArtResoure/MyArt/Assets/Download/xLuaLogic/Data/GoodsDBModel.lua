require "download/xLuaLogic/Data/Create/ShopEntity"
require "download/xLuaLogic/Data/GoodsEntity" --物品实体

--装备
require "download/xLuaLogic/Data/Create/EquipEntity"
require "download/xLuaLogic/Data/Create/EquipDBModel"

--道具
require "download/xLuaLogic/Data/Create/ItemEntity"
require "download/xLuaLogic/Data/Create/ItemDBModel"

--材料
require "download/xLuaLogic/Data/Create/MaterialEntity"
require "download/xLuaLogic/Data/Create/MaterialDBModel"

--数据访问
GoodsDBModel = { }

local this = GoodsDBModel;

function GoodsDBModel.New()
    return this;
end

--根据商城分类 加载商城列表数据
function GoodsDBModel.GetList(shopCategoryId)
    local shopTable = DBModelMgr.GetDBModel(DBModelNames.ShopDBModel).GetList();
	
	local retTable = {};
	
	local name="";
	local usedLevel=0;
	local usedMethod="";
	local quality=0;
	local description="";
	
	for i=1, #shopTable,1 do
		if(shopTable[i].ShopCategoryId == shopCategoryId) then
			
			--在这里 封装扩展的商城实体
			if(shopTable[i].GoodsType == 0) then
				--装备
				local entity = EquipDBModel.GetEntity(shopTable[i].GoodsId);
				if(entity ~= nil) then
					name = entity.Name;
					usedLevel = entity.UsedLevel;
					usedMethod="";
					quality = entity.Quality;
					description = entity.Description;
				end
				entity = nil;
			elseif(shopTable[i].GoodsType == 1) then
				--道具
				local entity = ItemDBModel.GetEntity(shopTable[i].GoodsId);
				if(entity ~= nil) then
					name = entity.Name;
					usedLevel = entity.UsedLevel;
					usedMethod = UsedMethod;
					quality = entity.Quality;
					description = entity.Description;
				end
				entity = nil;
			elseif(shopTable[i].GoodsType == 2) then
				--材料
				local entity = MaterialDBModel.GetEntity(shopTable[i].GoodsId);
				if(entity ~= nil) then
					name = entity.Name;
					usedLevel = 1;
					usedMethod = "";
					quality = entity.Quality;
					description = entity.Description;
				end
				entity = nil;
			end
			
			
			retTable[#retTable+1] = GoodsEntity.New(
				shopTable[i].Id, 
				shopTable[i].ShopCategoryId, 
				shopTable[i].GoodsType, 
				shopTable[i].GoodsId, 
				shopTable[i].OldPrice, 
				shopTable[i].Price, 
				shopTable[i].SellStatus,
				name,
				usedLevel,
				usedMethod,
				quality,
				description
			);
		end
	end
	
	name = nil;
	usedMethod = nil;
	description = nil;
	
	return retTable;
end