require "download/xLuaLogic/Common/Define"

require "download/xLuaLogic/Proto/Goods_SearchEquipDetailProto"
require "download/xLuaLogic/Proto/Goods_SearchEquipDetailReturnProto"
require "download/xLuaLogic/Proto/Goods_SellToSysProto"
require "download/xLuaLogic/Proto/Goods_SellToSysReturnProto"
require "download/xLuaLogic/Proto/Goods_EquipPutProto"

GoodsEquipCtrl = {};

local this = GoodsEquipCtrl;

local transform;
local gameObject;

local goodsType;
local goodsId;
local goodsServerId;
local roleBackpackId; --背包项编号

local lblAttrPrefab;
local equipEntity;
local goodsName;

function GoodsEquipCtrl.New()
	return this;
end

function GoodsEquipCtrl.Awake(parameters)
	goodsType = tonumber(parameters[1]);
	goodsId = tonumber(parameters[2]);
	goodsServerId = tonumber(parameters[3]);
	roleBackpackId = tonumber(parameters[4]);
	
	equipEntity = DBModelMgr.GetDBModel(DBModelNames.EquipDBModel).GetEntity(goodsId);
	goodsName =equipEntity.Name;
	
--[[	print("点击的物品类型"..tostring(goodsType));
	print("点击的物品编号"..tostring(goodsId));
	print("点击的服务器端编号"..tostring(goodsServerId));--]]
				
	--添加协议的监听
	CS.LuaHelper.Instance.SocketDispatcher:AddEventListener(ProtoCode.Goods_SearchEquipDetailReturn, this.OnGoodsSearchEquipDetailReturn);
	
	CS.LuaHelper.Instance.UIViewUtil:LoadWindowForLua("GoodsEquipView", this.OnCreate, "Download/Prefab/xLuaUIPrefab/Goods/pan_GoodsEquipView.assetbundle");
end

function GoodsEquipCtrl.OnCreate(obj)
	--先拿到lblAttr预设
	CS.LuaHelper.Instance.AssetBundleMgr:LoadOrDownloadForLua("Download/Prefab/xLuaUIPrefab/Goods/lblAttr.assetbundle", "lblAttr", this.LoadlblAttrComplete);
	
	if (roleBackpackId == 0) then
		GoodsEquipView.txtPutOn.text="脱下";
		GoodsEquipView.btnSell.enabled = false;
	else
		GoodsEquipView.txtPutOn.text="穿上";
		GoodsEquipView.btnSell.enabled = true;
	end
end

function GoodsEquipCtrl.OnDestroy()
	CS.LuaHelper.Instance.SocketDispatcher:RemoveEventListener(ProtoCode.Goods_SearchEquipDetailReturn, this.OnGoodsSearchEquipDetailReturn);
	goodsType = nil;
	goodsId = nil;
	goodsServerId = nil;
	roleBackpackId = nil;
	lblAttrPrefab = nil;
	equipEntity = nil;
	goodsName = nil;
end

function GoodsEquipCtrl.LoadlblAttrComplete(lblAttr)
	
	lblAttrPrefab = lblAttr;
	
	--查询装备详情
	local proto = Goods_SearchEquipDetailProto.New();
	proto.GoodsServerId = goodsServerId;
	Goods_SearchEquipDetailProto.SendProto(proto);
	proto = nil;
end

function GoodsEquipCtrl.OnGoodsSearchEquipDetailReturn(buffer)
	local proto = Goods_SearchEquipDetailReturnProto.GetProto(buffer);
	
	--本地信息
	GoodsEquipView.lblGoodsName.text = equipEntity.Name;
	GoodsEquipView.lblLevel.text = string.format("等级：%s", equipEntity.UsedLevel);
	GoodsEquipView.lblGoodsType.text = this.GetEquipTypeName(equipEntity.Type);
	
	--GoodsEquipView.imgJewel1.gameObject:SetActive(false);
	GoodsEquipView.imgJewel2.gameObject:SetActive(false);
	GoodsEquipView.imgJewel3.gameObject:SetActive(false);
	GoodsEquipView.imgJewel4.gameObject:SetActive(false);
	
	--图标
	CS.LuaHelper.Instance:AutoLoadTexture(GoodsEquipView.imgIcon.gameObject, CS.GameUtil.GetAssetPath("Download/Source/UISource/EquipIco", goodsId), goodsId, false);
	
	--按钮点击
	GoodsEquipView.btnPutOn.onClick:AddListener(
		function ()
			
			if (roleBackpackId == 0) then
				--给服务器发送脱下装备消息
				local proto = Goods_EquipPutProto.New();
				proto.Type = 1;
				proto.GoodsId = goodsId;
				proto.GoodsServerId = goodsServerId;
				Goods_EquipPutProto.SendProto(proto);
				proto = nil;
			else
				--给服务器发送穿上装备消息
				local proto = Goods_EquipPutProto.New();
				proto.Type = 0;
				proto.GoodsId = goodsId;
				proto.GoodsServerId = goodsServerId;
				Goods_EquipPutProto.SendProto(proto);
				proto = nil;
			end

		end
	);
	
	GoodsEquipView.btnSell.onClick:AddListener(
		function ()
			--print("出售按钮点击");
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
		end
	);
	
	--服务器信息
	--GoodsEquipView.attrContent
	local posY = 0;
	
	
	--基础属性标题
	local baseAttrName = CS.GameUtil.AddChild(GoodsEquipView.attrContent.transform, lblAttrPrefab);
	baseAttrName.transform.localPosition = Vector3(10, posY, 0);
	
	local baseAttrNameText = baseAttrName:GetComponent("UnityEngine.UI.Text");
	baseAttrNameText.text = "基础属性";
	baseAttrName = nil;
	baseAttrNameText = nil;
	
	--基础属性1类型名称
	posY = posY - 40;
	local baseAttr1 = CS.GameUtil.AddChild(GoodsEquipView.attrContent.transform, lblAttrPrefab);
	baseAttr1.transform.localPosition = Vector3(20, posY, 0);
	
	local baseAttr1Text = baseAttr1:GetComponent("UnityEngine.UI.Text");
	baseAttr1.transform:GetComponent("RectTransform").sizeDelta = Vector2(260, 24);
	baseAttr1Text.fontSize = 20;
	baseAttr1Text.text = string.format("<color=#005c9d>%s：</color><color=#1e9d00>%s</color>", this.GetEquipAttrTypeName(proto.BaseAttr1Type), proto.BaseAttr1Value);
	baseAttr1 = nil;
	baseAttr1Text = nil;
	
	--基础属性2类型名称
	posY = posY - 24;
	local baseAttr2 = CS.GameUtil.AddChild(GoodsEquipView.attrContent.transform, lblAttrPrefab);
	baseAttr2.transform.localPosition = Vector3(20, posY, 0);
	
	local baseAttr2Text = baseAttr2:GetComponent("UnityEngine.UI.Text");
	baseAttr2Text:GetComponent("RectTransform").sizeDelta = Vector2(260, 24);
	baseAttr2Text.fontSize = 20;
	baseAttr2Text.text = string.format("<color=#005c9d>%s：</color><color=#1e9d00>%s</color>", this.GetEquipAttrTypeName(proto.BaseAttr2Type), proto.BaseAttr2Value);
	baseAttr2 = nil;
	baseAttr2Text = nil;
	
	--随机属性
	posY = posY - 44;
	local randomAttrName = CS.GameUtil.AddChild(GoodsEquipView.attrContent.transform, lblAttrPrefab);
	randomAttrName.transform.localPosition = Vector3(10, posY, 0);
	
	local randomAttrNameText = randomAttrName:GetComponent("UnityEngine.UI.Text");
	randomAttrName.transform:GetComponent("RectTransform").sizeDelta = Vector2(260, 40);
	randomAttrNameText.fontSize = 22;
	randomAttrNameText.text = "随机属性";
	randomAttrName = nil;
	randomAttrNameText = nil;
	
	posY = posY - 40;
	
	local randomAttrTable = {};
	
	if(proto.HP > 0) then
		randomAttrTable[#randomAttrTable + 1] = {0, proto.HP}
	end
	if(proto.MP > 0) then
		randomAttrTable[#randomAttrTable + 1] = {1, proto.MP}
	end
	if(proto.Attack > 0) then
		randomAttrTable[#randomAttrTable + 1] = {2, proto.Attack}
	end
	if(proto.Defense > 0) then
		randomAttrTable[#randomAttrTable + 1] = {3, proto.Defense}
	end
	if(proto.Hit > 0) then
		randomAttrTable[#randomAttrTable + 1] = {4, proto.Hit}
	end
	if(proto.Dodge > 0) then
		randomAttrTable[#randomAttrTable + 1] = {5, proto.Dodge}
	end
	if(proto.Cri > 0) then
		randomAttrTable[#randomAttrTable + 1] = {6, proto.Cri}
	end
	if(proto.Res > 0) then
		randomAttrTable[#randomAttrTable + 1] = {7, proto.Res}
	end
	
	for i=1, #randomAttrTable, 1 do
		if(i > 1) then
			posY = posY - 24;
		end
		
		local attr = CS.GameUtil.AddChild(GoodsEquipView.attrContent.transform, lblAttrPrefab);
		attr.transform.localPosition = Vector3(20, posY, 0);
		
		local attrText = attr:GetComponent("UnityEngine.UI.Text");
		attr:GetComponent("RectTransform").sizeDelta = Vector2(230, 24);
		attrText.fontSize = 20;
		attrText.text = string.format("<color=#005c9d>%s：</color><color=#1e9d00>%s</color>",this.GetEquipAttrTypeName(randomAttrTable[i][1]), randomAttrTable[i][2]);
		
		attr = nil;
		attrText = nil;
	end
	
	--描述
	posY = posY - 44;
	local descName = CS.GameUtil.AddChild(GoodsEquipView.attrContent.transform, lblAttrPrefab);
	descName.transform.localPosition = Vector3(10, posY, 0);
	
	local descNameText = descName:GetComponent("UnityEngine.UI.Text");
	descNameText.text = "描述";
	descName = nil;
	descNameText = nil;
	
	--描述内容
	posY = posY - 40;
	local descContent = CS.GameUtil.AddChild(GoodsEquipView.attrContent.transform, lblAttrPrefab);
	descContent.transform.localPosition = Vector3(20, posY, 0);
	
	local descContentText = descContent:GetComponent("UnityEngine.UI.Text");
	descContent:GetComponent("RectTransform").sizeDelta = Vector2(230, 48);
	descContentText.fontSize = 20;
	descContentText.text = string.format("<color=#ff5a00>%s</color>", equipEntity.Description);
	descContent = nil;
	descContentText = nil;
	
	--售价
	posY = posY - 48;
	local sellPrice = CS.GameUtil.AddChild(GoodsEquipView.attrContent.transform, lblAttrPrefab);
	sellPrice.transform.localPosition = Vector3(10, posY, 0);
	
	local sellPriceText = sellPrice:GetComponent("UnityEngine.UI.Text");
	sellPrice:GetComponent("RectTransform").sizeDelta = Vector2(230, 40);
	sellPriceText.fontSize = 20;
	sellPriceText.text = string.format("<color=#005c9d>售价：</color><color=#1e9d00>%s 金币</color>", equipEntity.SellMoney);
	sellPrice = nil;
	sellPriceText = nil;
	
	posY = posY - 40;
	GoodsEquipView.attrContent.transform:GetComponent("RectTransform").sizeDelta = Vector2(260, posY*-1);
	
	proto = nil;
	equipEntity = nil;
end


function GoodsEquipCtrl.GetEquipTypeName(type)

	local typeName="";
	
	if(type == 100) then
		typeName = "武器";
	elseif(type == 200) then
		typeName = "护腕";
	elseif(type == 300) then
		typeName = "衣服";
	elseif(type == 400) then
		typeName = "护腿";
	elseif(type == 500) then
		typeName = "鞋";
	elseif(type == 600) then
		typeName = "戒指";
	elseif(type == 700) then
		typeName = "项链";
	elseif(type == 800) then
		typeName = "腰带";
	end
	
	return typeName;
end

-- 0=HP 1=MP 2=攻击 3=防御 4=命中 5=闪避 6=暴击 7=抗性
function GoodsEquipCtrl.GetEquipAttrTypeName(type)

	local typeName="";
	
	if(type == 0) then
		typeName = "生命";
	elseif(type == 1) then
		typeName = "魔法";
	elseif(type == 2) then
		typeName = "攻击";
	elseif(type == 3) then
		typeName = "防御";
	elseif(type == 4) then
		typeName = "命中";
	elseif(type == 5) then
		typeName = "闪避";
	elseif(type == 6) then
		typeName = "暴击";
	elseif(type == 7) then
		typeName = "抗性";
	end
	
	return typeName;
end