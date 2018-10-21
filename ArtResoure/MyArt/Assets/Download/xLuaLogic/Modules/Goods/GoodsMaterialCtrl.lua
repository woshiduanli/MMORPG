GoodsMaterialCtrl = {};

local this = GoodsMaterialCtrl;

local transform;
local gameObject;

local goodsType;
local goodsId;
local goodsServerId;
local roleBackpackId;
local goodsName;
local maxAmount; --最大叠加数量

function GoodsMaterialCtrl.New()
	return this;
end

function GoodsMaterialCtrl.Awake(parameters)
	goodsType = tonumber(parameters[1]);
	goodsId = tonumber(parameters[2]);
	goodsServerId = tonumber(parameters[3]);
	roleBackpackId = tonumber(parameters[4]);
	
	CS.LuaHelper.Instance.UIViewUtil:LoadWindowForLua("GoodsMaterialView", this.OnCreate, "Download/Prefab/xLuaUIPrefab/Goods/pan_GoodsMaterialView.assetbundle");
	
end

function GoodsMaterialCtrl.OnCreate(obj)
	--本地信息
	local materialEntity = DBModelMgr.GetDBModel(DBModelNames.MaterialDBModel).GetEntity(goodsId);
	
	goodsName = materialEntity.Name;
	maxAmount = materialEntity.maxAmount;
	
	GoodsMaterialView.lblGoodsName.text = materialEntity.Name;
	GoodsMaterialView.lblLevel.text = string.format("等级：%s", 1);
	GoodsMaterialView.lblGoodsType.text = this.GetMaterialTypeName(materialEntity.Type);
	GoodsMaterialView.lblDesc.text = materialEntity.Description;
	GoodsMaterialView.lblSell.text = string.format("售价：%s", materialEntity.SellMoney);
	
	--图标
	CS.LuaHelper.Instance:AutoLoadTexture(GoodsMaterialView.imgIcon.gameObject, CS.GameUtil.GetAssetPath("Download/Source/UISource/MaterialIco", goodsId), goodsId, false);
	
	GoodsMaterialView.btnSell.onClick:AddListener(
		function ()
			--print("maxAmount=="..tostring(maxAmount));

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
	
	
	materialEntity = nil;
end

function GoodsMaterialCtrl.OnDestroy()
	transform = nil;
	gameObject = nil;
	goodsType = nil;
	goodsId = nil;
	goodsServerId = nil;
	roleBackpackId = nil;
	goodsName = nil;
	maxAmount = nil;
end

function GoodsMaterialCtrl.GetMaterialTypeName(type)

	local typeName="";
	
	if(type == 1) then
		typeName = "攻击宝石";
	elseif(type == 2) then
		typeName = "防御宝石";
	elseif(type == 3) then
		typeName = "暴击宝石";
	elseif(type == 4) then
		typeName = "抗性宝石";
	elseif(type == 5) then
		typeName = "生命宝石";
	elseif(type == 6) then
		typeName = "魔法宝石";
	end
	
	return typeName;
end