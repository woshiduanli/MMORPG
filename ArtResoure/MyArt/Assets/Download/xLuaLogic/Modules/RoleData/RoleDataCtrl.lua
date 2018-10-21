require "download/xLuaLogic/Common/Define"

require "download/xLuaLogic/Proto/Backpack_SearchProto"
require "download/xLuaLogic/Proto/Backpack_SearchReturnProto"
require "download/xLuaLogic/Proto/Backpack_GoodsChangeReturnProto"

RoleDataCtrl = {};

local this = RoleDataCtrl;

local backpackItemTable;

function RoleDataCtrl.New()
	
	--添加协议的监听
	CS.LuaHelper.Instance.SocketDispatcher:AddEventListener(ProtoCode.Backpack_SearchReturn, this.OnBackpackSearchReturn);
	
	
	--print("====>>>监听服务器返回背包项更新消息");
	--监听服务器返回背包项更新消息
	CS.LuaHelper.Instance.SocketDispatcher:AddEventListener(ProtoCode.Backpack_GoodsChangeReturn, this.OnBackpackGoodsChangeReturn);
	return this;
end

function RoleDataCtrl.Awake()
	--向服务器发送查询角色背包项消息
	local proto = Backpack_SearchProto.New();
	Backpack_SearchProto.SendProto(proto);
end

function RoleDataCtrl.OnBackpackSearchReturn(buffer)
	local proto = Backpack_SearchReturnProto.GetProto(buffer);
	backpackItemTable = proto.ItemTable;
	--print("服务器返回消息 背包项数量="..proto.BackpackItemCount);
end

--服务器返回背包项更新消息
function RoleDataCtrl.OnBackpackGoodsChangeReturn(buffer)
	
	
	local proto = Backpack_GoodsChangeReturnProto.GetProto(buffer);
	local itemTable = proto.ItemTable;
	--print("====>>>OnBackpackGoodsChangeReturn 服务器返回背包项更新消息"..tostring(proto.BackpackItemChangeCount));

	for i=1, #itemTable, 1 do
		--print("更新的背包项目=="..tostring(itemTable[i].ChangeType));
		
		--循环需要更新的项目
		if (itemTable[i].ChangeType == 0) then
			this.BackpackGoodsAdd(itemTable[i]);
		elseif(itemTable[i].ChangeType == 1) then
			this.BackpackGoodsUpdate(itemTable[i]);
		else
			this.BackpackGoodsDelete(itemTable[i]);
		end
	end
	
	CS.LuaHelper.Instance.UIDispatcher:Dispatch(DispatchMsg.BackpackChange, {}); --派发背包更新消息
end

--背包项增加
function RoleDataCtrl.BackpackGoodsAdd(item)
	
	local _Item = Item.New();
	_Item.BackpackItemId = item.BackpackId;
	_Item.GoodsType = item.GoodsType;
	_Item.GoodsId = item.GoodsId;
	_Item.GoodsServerId = item.GoodsServerId;
	_Item.GoodsOverlayCount = item.GoodsCount;
	
	
	
	backpackItemTable[#backpackItemTable+1] = _Item;
end

--背包项修改
function RoleDataCtrl.BackpackGoodsUpdate(item)
		for i = #backpackItemTable, 1, -1 do
		if(backpackItemTable[i].BackpackItemId == item.BackpackId) then
			backpackItemTable[i].GoodsOverlayCount = item.GoodsCount;
		end
	end
end

--背包项删除
function RoleDataCtrl.BackpackGoodsDelete(item)

	for i = #backpackItemTable, 1, -1 do
		if(backpackItemTable[i].BackpackItemId == item.BackpackId) then
			table.remove(backpackItemTable, i);
		end
	end
end



-- -1=全部 0=装备 1=道具 2=材料
function RoleDataCtrl.GetBackpackItemTable(goodsType)
	
	if(goodsType == -1) then
		
		table.sort(backpackItemTable, this.Comp);
		return backpackItemTable;
	end
	
	local tempTable = {};
	for i = 1, #backpackItemTable, 1 do
		if(backpackItemTable[i].GoodsType == goodsType) then
			tempTable[#tempTable+1] = backpackItemTable[i];
		end
	end
	
	table.sort(tempTable, this.Comp);
	return tempTable;
end

--按照物品类型进行排序
function RoleDataCtrl.Comp(first, second)
	if(first.GoodsType ~= second.GoodsType) then
		return first.GoodsType < second.GoodsType
	end
	
	return first.BackpackItemId>second.BackpackItemId;
end


--根据物品编号获取物品的数量
function RoleDataCtrl.GetGoodsCount(goodsId)
	local count = 0;
	
	for i = 1, #backpackItemTable, 1 do
		if(backpackItemTable[i].GoodsId == goodsId) then
			count = count+backpackItemTable[i].GoodsOverlayCount;
		end
	end
	
	return count;

end