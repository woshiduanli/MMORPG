require "download/xLuaLogic/Common/Define"
require "download/xLuaLogic/Data/DBModelMgr"
require "download/xLuaLogic/Proto/Shop_BuyProductProto"
require "download/xLuaLogic/Proto/Shop_BuyProductReturnProto"

ShopCtrl = {};

local this = ShopCtrl;

local transform;
local gameObject;

--这些是缓存列表
local txtGoodsNameTable;
local imgIconTable;
local txtOldPriceTable;
local txtPriceTable;
local imgStatusTable;
local btnBuyTable;

function ShopCtrl.New()
	return this;
end

function ShopCtrl.Awake()
	
	txtGoodsNameTable={};
	imgIconTable={};
	txtOldPriceTable={};
	txtPriceTable={};
	imgStatusTable={};
	btnBuyTable={};

	CS.LuaHelper.Instance.UIViewUtil:LoadWindowForLua("ShopView", this.OnCreate, "Download/Prefab/xLuaUIPrefab/Shop/pan_ShopView.assetbundle");
end

function ShopCtrl.OnDestroy()
	
	--print("商城销毁");
	
	--内存释放
	for key,v in ipairs(txtGoodsNameTable) do
		txtGoodsNameTable[key] = nil;
	end
	txtGoodsNameTable = nil;
	
	for key,v in ipairs(imgIconTable) do
		imgIconTable[key] = nil;
	end
	imgIconTable = nil;
	
	for key,v in ipairs(txtOldPriceTable) do
		txtOldPriceTable[key] = nil;
	end
	txtOldPriceTable = nil;
	
	for key,v in ipairs(txtPriceTable) do
		txtPriceTable[key] = nil;
	end
	txtPriceTable = nil;
	
	for key,v in ipairs(imgStatusTable) do
		imgStatusTable[key] = nil;
	end
	imgStatusTable = nil;
	
	for key,v in ipairs(btnBuyTable) do
		btnBuyTable[key] = nil;
	end
	btnBuyTable = nil;
	
	
	--窗口关闭 移除监听
	CS.LuaHelper.Instance.SocketDispatcher:RemoveEventListener(ProtoCode.Shop_BuyProductReturn, this.OnShopBuyProductReturn);
	
end

function ShopCtrl.OnCreate(obj)
	
	--添加协议的监听
	CS.LuaHelper.Instance.SocketDispatcher:AddEventListener(ProtoCode.Shop_BuyProductReturn, this.OnShopBuyProductReturn);
end

--服务器返回购买商城物品消息
function ShopCtrl.OnShopBuyProductReturn(buffer)
	local proto = Shop_BuyProductReturnProto.GetProto(buffer);
	
	local msg = CS.LuaHelper.Instance:GetLanguageText(proto.MsgCode);
	CS.LuaHelper.Instance.MessageCtrl:ShowMessageView(msg, CommonMessageBoxTitle, CS.MessageViewType.Ok, nil, nil, nil, nil);
	msg = nil;
	proto = nil;
end

local shopExtTable = nil;--分类数据

function ShopCtrl.OnStart()
	CS.LuaHelper.Instance.AssetBundleMgr:LoadOrDownloadForLua("Download/Prefab/xLuaUIPrefab/Shop/ShopTabBtn.assetbundle", "ShopTabBtn", this.OnLoadShopTabBtn);
end

function ShopCtrl.OnLoadShopTabBtn(obj)
	
	--obj 就是Tab按钮的镜像
	--克隆镜像 并添加到TapContent下
	
		--委托回调
	ShopView.MultiScrollViewVertical.OnItemCreate = this.OnItemCreate;
	
	--获取商城分类
	local shopCategoryTable = DBModelMgr.GetDBModel(DBModelNames.ShopCategoryDBModel).GetList();--本地数据表
	
	for i=1, #shopCategoryTable, 1 do
		--克隆预设
		local item = CS.UnityEngine.Object.Instantiate(obj); 
		item.transform.parent = ShopView.ScrollViewTabContent;
		item.transform.localPosition = Vector3.zero;
		item.transform.localScale = Vector3.one;
		
		--找到Text 并设置按钮文字
		local btnText = item.transform:Find("Text"):GetComponent("UnityEngine.UI.Text");
		btnText.text = shopCategoryTable[i].Name;
		
		local btnItem = item.transform:GetComponent("UnityEngine.UI.Button");
		btnItem.onClick:AddListener(
			function ()
				--print("点击了分类"..shopCategoryTable[i].Id);
				this.LoadList(shopCategoryTable[i].Id);
			end
		);
		
		item = nil;
		btnText = nil;
		btnItem = nil;
	end
	
	--加载默认分类编号是1
	this.LoadList(1);
end

function ShopCtrl.LoadList(categoryId)

	--加载限时抢购的列表数据
	shopExtTable = DBModelMgr.GetDBModel(DBModelNames.GoodsDBModel).GetList(categoryId);
	
	--数量
	ShopView.MultiScrollViewVertical.DataCount = #shopExtTable; --一定要先设置数量
	ShopView.MultiScrollViewVertical:ResetScroller(); --然后重置

end

function ShopCtrl.OnItemCreate(index, obj)

	local instanceId = obj:GetInstanceID();

	local txtGoodsName;
	local imgIcon;
	local txtOldPrice;
	local txtPrice;
	local imgStatus;
	local btnBuy;

	--物品名称
	if(txtGoodsNameTable[instanceId] ~= nil) then
		txtGoodsName = txtGoodsNameTable[instanceId]; --如果存在 直接从缓存表中获取
	else
		txtGoodsName = obj.transform:Find("txtGoodsName"):GetComponent("UnityEngine.UI.Text");
			txtGoodsNameTable[instanceId] = txtGoodsName; --加入缓存表
	end
	
	--图标
	if(imgIconTable[instanceId] ~= nil) then
		imgIcon = imgIconTable[instanceId];
	else
		imgIcon = obj.transform:Find("imgIcon"):GetComponent("UnityEngine.UI.Image");
			imgIconTable[instanceId] = imgIcon;
	end
	
	
	--原价
	if(txtOldPriceTable[instanceId] ~= nil) then
		txtOldPrice = txtOldPriceTable[instanceId];
	else
		txtOldPrice = obj.transform:Find("txtOldPrice"):GetComponent("UnityEngine.UI.Text");
			txtOldPriceTable[instanceId] = txtOldPrice;
	end
	
	
	--现价
	if(txtPriceTable[instanceId] ~= nil) then
		txtPrice = txtPriceTable[instanceId];
	else
		txtPrice = obj.transform:Find("txtPrice"):GetComponent("UnityEngine.UI.Text");
			txtPriceTable[instanceId] = txtPrice;
	end
	
	
	--状态
	if(imgStatusTable[instanceId] ~= nil) then
		imgStatus = imgStatusTable[instanceId];
	else
		imgStatus = obj.transform:Find("imgStatus"):GetComponent("UnityEngine.UI.Image");
			imgStatusTable[instanceId] = imgStatus;
	end
	
	
	--购买按钮
	if(btnBuyTable[instanceId] ~= nil) then
		btnBuy = btnBuyTable[instanceId];
	else
		btnBuy = obj.transform:Find("btnBuy"):GetComponent("UnityEngine.UI.Button");
			btnBuyTable[instanceId] = btnBuy;
	end
	
	--print("i=="..index); --拿到了索引
	
	local newIndex=index+1;
	
	--赋值开始
	txtGoodsName.text = shopExtTable[newIndex].Name;
	txtOldPrice.text = "原价：<color=#df0a0a>"..shopExtTable[newIndex].OldPrice.."</color>元宝";
	
	if(shopExtTable[newIndex].SellStatus ~= 1) then
		txtOldPrice.gameObject:SetActive(false);
	else
		txtOldPrice.gameObject:SetActive(true);
	end
	
	txtPrice.text = "售价：<color=#47e700>"..shopExtTable[newIndex].Price.."</color>元宝";
	
	--给状态图标赋值
	if(shopExtTable[newIndex].SellStatus == 0) then
		imgStatus.gameObject:SetActive(false);
	else
		
		local imgStatusName="";
		imgStatus.gameObject:SetActive(true);
		
		if(shopExtTable[newIndex].SellStatus == 1) then
			imgStatusName = "shop_discount"; --打折
		elseif(shopExtTable[newIndex].SellStatus == 2) then
			imgStatusName = "shop_new"; --新品
		elseif(shopExtTable[newIndex].SellStatus == 3) then
			imgStatusName = "shop_hot"; --热卖
		elseif(shopExtTable[newIndex].SellStatus == 4) then
			imgStatusName = "shop_time"; --限时
		end
		
		CS.LuaHelper.Instance:AutoLoadTexture(imgStatus.gameObject, string.format("Download/Source/UISource/UICommon/%s.assetbundle", imgStatusName), imgStatusName);
		imgStatusName = nil;
	end

	--给物品图标赋值
	if(shopExtTable[newIndex].GoodsType == 0) then --装备
		CS.LuaHelper.Instance:AutoLoadTexture(imgIcon.gameObject, CS.GameUtil.GetAssetPath("Download/Source/UISource/EquipIco", shopExtTable[newIndex].GoodsId), shopExtTable[newIndex].GoodsId, false);
	elseif(shopExtTable[newIndex].GoodsType == 1) then --道具
		CS.LuaHelper.Instance:AutoLoadTexture(imgIcon.gameObject, CS.GameUtil.GetAssetPath("Download/Source/UISource/ItemIco", shopExtTable[newIndex].GoodsId), shopExtTable[newIndex].GoodsId, false);
	elseif(shopExtTable[newIndex].GoodsType == 2) then --材料
		CS.LuaHelper.Instance:AutoLoadTexture(imgIcon.gameObject, CS.GameUtil.GetAssetPath("Download/Source/UISource/MaterialIco", shopExtTable[newIndex].GoodsId), shopExtTable[newIndex].GoodsId, false);
	end
	--赋值结束
	
	
	
	btnBuy.onClick:RemoveAllListeners(); --先把旧的监听全部移除
	btnBuy.onClick:AddListener(
					function ()
						--print("点击了"..shopExtTable[newIndex].Id);
						local msg = CS.LuaHelper.Instance:GetLanguageText(102006);
						CS.LuaHelper.Instance.MessageCtrl:ShowMessageView(string.format(msg, shopExtTable[newIndex].Price, shopExtTable[newIndex].Name), CommonMessageBoxTitle, CS.MessageViewType.OkAndCancel, 
							nil,nil,
							function ()
								--Ok 回调
								print("您要购买"..shopExtTable[newIndex].Name);
								--像服务器发送购买商城物品消息
								local proto = Shop_BuyProductProto.New();
								proto.ProductId = shopExtTable[newIndex].Id;
								Shop_BuyProductProto.SendProto(proto);
								proto = nil;
							end,
							nil
						);
					end
				);
		
	--释放内存
	txtGoodsName = nil;
	imgIcon = nil;
	txtOldPrice = nil;
	txtPrice = nil;
	imgStatus = nil;
	btnBuy = nil;
end