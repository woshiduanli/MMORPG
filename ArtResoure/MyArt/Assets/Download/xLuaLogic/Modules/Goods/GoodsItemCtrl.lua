require "download/xLuaLogic/Common/Define"
require "download/xLuaLogic/Proto/Goods_SellToSysProto"
require "download/xLuaLogic/Proto/Goods_SellToSysReturnProto"
require "download/xLuaLogic/Proto/Goods_UseItemProto"
require "download/xLuaLogic/Proto/Goods_UseItemReturnProto"

GoodsItemCtrl = {};

local this = GoodsItemCtrl;

local transform;
local gameObject;

local goodsType;
local goodsId;
local goodsServerId;
local roleBackpackId;
local goodsName;
local maxAmount; --最大叠加数量

function GoodsItemCtrl.New()
	return this;
end

function GoodsItemCtrl.Awake(parameters)
	goodsType = tonumber(parameters[1]);
	goodsId = tonumber(parameters[2]);
	goodsServerId = tonumber(parameters[3]);
	roleBackpackId = tonumber(parameters[4]);
	
	CS.LuaHelper.Instance.UIViewUtil:LoadWindowForLua("GoodsItemView", this.OnCreate, "Download/Prefab/xLuaUIPrefab/Goods/pan_GoodsItemView.assetbundle");
	
end

function GoodsItemCtrl.OnDestroy()
	goodsType = nil;
	goodsId = nil;
	goodsServerId = nil;
	roleBackpackId = nil;
	goodsName = nil;
	maxAmount = nil;
end

function GoodsItemCtrl.OnCreate(obj)
	--本地信息
	local itemEntity = DBModelMgr.GetDBModel(DBModelNames.ItemDBModel).GetEntity(goodsId);
	goodsName = itemEntity.Name;
	maxAmount = itemEntity.maxAmount;
	
	GoodsItemView.lblGoodsName.text = itemEntity.Name;
	GoodsItemView.lblLevel.text = string.format("等级：%s", itemEntity.UsedLevel);
	GoodsItemView.lblGoodsType.text = this.GetItemTypeName(itemEntity.Type);
	GoodsItemView.lblUseMethod.text = itemEntity.UsedMethod;
	GoodsItemView.lblDesc.text = itemEntity.Description;
	GoodsItemView.lblSell.text = string.format("售价：%s", itemEntity.SellMoney);
	
	--图标
	CS.LuaHelper.Instance:AutoLoadTexture(GoodsItemView.imgIcon.gameObject, CS.GameUtil.GetAssetPath("Download/Source/UISource/ItemIco", goodsId), goodsId, false);
	
	GoodsItemView.btnUse.onClick:AddListener(
		function ()
			print("使用按钮");
			local proto = Goods_UseItemProto.New();
			proto.roleBackpackId = roleBackpackId;
			proto.GoodsId = goodsId;
			Goods_UseItemProto.SendProto(proto);
			proto = nil;
		end
	);
	
	GoodsItemView.btnSell.onClick:AddListener(
		function ()
			--判断叠加数量 如果数量是1的 弹出确认框 否则 弹出选择数量的窗口
			if (maxAmount == 1) then
				CS.LuaHelper.Instance.MessageCtrl:ShowMessageView(string.format("您确定要出售<color=#005c9d>%s</color>吗", goodsName),CommonMessageBoxTitle, CS.MessageViewType.Ok, 
				nil,nil, 
				function ()
					--给服务器发送出售物品消息
					local proto = Goods_SellToSysProto.New();
					proto.roleBackpackId = roleBackpackId;
					proto.GoodsType = goodsType;
					proto.GoodsId = goodsId;
					proto.GoodsServerId = goodsServerId;
					proto.SellCount = 1;
					Goods_SellToSysProto.SendProto(proto);
					proto = nil;
				end, nil);
			else
				CS.LuaHelper.Instance.MessageCtrl:ShowChooseCountView("您要出售多少", RoleDataCtrl.GetGoodsCount(goodsId), 10, nil, nil, 
					function (chooseValue)
						print("您选择的数量"..tostring(chooseValue));
						
						local proto = Goods_SellToSysProto.New();
						proto.roleBackpackId = roleBackpackId;
						proto.GoodsType = goodsType;
						proto.GoodsId = goodsId;
						proto.GoodsServerId = goodsServerId;
						proto.SellCount = chooseValue;
						Goods_SellToSysProto.SendProto(proto);
						proto = nil;
					end
				, nil);
			end
		end
	);
	
	itemEntity = nil;
end

function GoodsItemCtrl.GetItemTypeName(type)

	local typeName="";
	
	if(type == 1) then
		typeName = "元宝";
	elseif(type == 2) then
		typeName = "金币";
	elseif(type == 3) then
		typeName = "经验";
	elseif(type == 4) then
		typeName = "体力";
	elseif(type == 5) then
		typeName = "宝箱";
	elseif(type == 6) then
		typeName = "复活药";
	elseif(type == 7) then
		typeName = "回血药";
	end
	
	return typeName;
end