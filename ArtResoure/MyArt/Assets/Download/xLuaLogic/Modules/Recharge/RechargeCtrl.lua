require "download/xLuaLogic/Common/Define"
require "download/xLuaLogic/Data/DBModelMgr"

require "download/xLuaLogic/Proto/RoleData_RechargeProductReturnProto"
require "download/xLuaLogic/Proto/RoleData_RechargeReturnProto"

RechargeCtrl = {};

local this = RechargeCtrl;

--local transform;
--local gameObject;

local rechargeItemObj;

local productRechargeTable; --服务器返回的充值产品表

function RechargeCtrl.New()
	
	--监听服务器返回充值产品信息
	CS.LuaHelper.Instance.SocketDispatcher:AddEventListener(ProtoCode.RoleData_RechargeProductReturn, this.OnRechargeProductCallBack);
	return this;
end

--服务器返回查询任务协议
function RechargeCtrl.OnRechargeProductCallBack(buffer)
	local proto = RoleData_RechargeProductReturnProto.GetProto(buffer);
	productRechargeTable = proto.CurrItemTable;
	
	--print('lua中 从服务器返回了充值产品表');
end

function RechargeCtrl.Awake()
	
	CS.LuaHelper.Instance.UIViewUtil:LoadWindowForLua("RechargeView", this.OnCreate, "Download/Prefab/xLuaUIPrefab/Recharge/pan_RechargeView.assetbundle");
end

function RechargeCtrl.OnCreate(obj)
	--添加协议的监听
	CS.LuaHelper.Instance.SocketDispatcher:AddEventListener(ProtoCode.RoleData_RechargeReturn, this.OnRechargeReturn);
end

function RechargeCtrl.OnDestroy()
	--窗口关闭 移除监听
	CS.LuaHelper.Instance.SocketDispatcher:RemoveEventListener(ProtoCode.RoleData_RechargeReturn, this.OnRechargeReturn);
	
	if(CS.GlobalInit.ChannelId == 600) then
		--苹果充值
		CS.AppleStoreInterface.CancelHUD();
	end
	rechargeItemObj = nil;
end

function RechargeCtrl.OnRechargeReturn(buffer)

	local proto = RoleData_RechargeReturnProto.GetProto(buffer); --充值后回调

	if(proto.IsSuccess == true) then
	
		--更新服务器端返回的充值产品表
		for i=1, #productRechargeTable, 1 do
			
			--月卡
			if(productRechargeTable[i].RechargeProductId == proto.RechargeProductId and proto.RechargeProductType == 1) then
				productRechargeTable[i].CanBuy = 0;
				productRechargeTable[i].RemainDay = proto.RemainDay;
			elseif(productRechargeTable[i].RechargeProductId == proto.RechargeProductId and proto.RechargeProductType == 2) then
			--促销礼包
				productRechargeTable[i].CanBuy = 0;
			elseif(productRechargeTable[i].RechargeProductId == proto.RechargeProductId and proto.RechargeProductType == 3) then
			--普通计费点
				productRechargeTable[i].CanBuy = 1;
				productRechargeTable[i].DoubleFlag = 0; --取消双倍
			end
		end
		
		
		CS.LuaHelper.Instance.MessageCtrl:ShowMessageView(CS.LuaHelper.Instance:GetLanguageText(102001),CommonMessageBoxTitle, CS.MessageViewType.Ok, 
		nil,nil, 
		function ()
			CS.LuaHelper.Instance.UIViewUtil:CloseWindow("RechargeView"); --关闭当前充值窗口
			CS.LuaHelper.Instance.UIDispatcher:Dispatch(DispatchMsg.RechargeOK, proto.Money); --派发充值更新
		end, nil);
	else
		--print("MsgCode="..proto.MsgCode);
		if(proto.MsgCode == 102004) then
			--月卡购买失败
			local msg = CS.LuaHelper.Instance:GetLanguageText(proto.MsgCode);
			CS.LuaHelper.Instance.MessageCtrl:ShowMessageView(string.format(msg,proto.RemainDay));
		else
			CS.LuaHelper.Instance.MessageCtrl:ShowMessageView(CS.LuaHelper.Instance:GetLanguageText(proto.MsgCode));
		end
		
	end
end

function RechargeCtrl.OnStart()
	--拿出镜像
	
	--累计充值
	RechargeView.txtTotalRecharge.text= string.format(CS.LuaHelper.Instance:GetLanguageText(102003), CS.GlobalInit.Instance.MainPlayerInfo.TotalRechargeMoney); --服务器返回
	
	CS.LuaHelper.Instance.AssetBundleMgr:LoadOrDownloadForLua("Download/Prefab/xLuaUIPrefab/Recharge/RechargeItem.assetbundle", "RechargeItem", this.OnLoadItem);
end

function RechargeCtrl.OnLoadItem(obj)
	
	rechargeItemObj = obj;

	this.LoadList();
end

--根据充值产品编号 获取服务器端的充值产品信息
function RechargeCtrl.GetServerRechargeProduct(rechargeProductId)

	for i=1, #productRechargeTable, 1 do
		if(productRechargeTable[i].RechargeProductId == rechargeProductId) then
			return productRechargeTable[i];
		end
	end

end

function RechargeCtrl.LoadList()

	local rechargeShopTable = DBModelMgr.GetDBModel(DBModelNames.RechargeShopDBModel).GetList();--本地数据表
	
		for i=1, #rechargeShopTable, 1 do

			while true do
				
				--获取服务器端的充值产品信息
				local serverRechargeProduct = this.GetServerRechargeProduct(rechargeShopTable[i].Id);
				--print("CanBuy"..serverRechargeProduct.CanBuy);
				--print("Type"..rechargeShopTable[i].Type);
				--print("Type"..rechargeShopTable[i].Id);
				
				--如果充值产品不能购买 并且 是 促销礼包 则不进行克隆

					if(serverRechargeProduct.CanBuy == 0 and rechargeShopTable[i].Type == 2 ) then
						--print("说明这个产品不能购买 不克隆");
						break;
					end
					
				
				--克隆预设
				local item = CS.UnityEngine.Object.Instantiate(rechargeItemObj); 
				item.transform.parent = RechargeView.content;
				item.transform.localPosition = Vector3.zero;
				item.transform.localScale = Vector3.one;
				
				--价格
				local txtPrice = item.transform:Find("Bottom/txtPrice"):GetComponent("UnityEngine.UI.Text");
				txtPrice.text = "￥"..tostring(rechargeShopTable[i].Price);
				
				--名字
				local txtTitle = item.transform:Find("Top/txtTitle"):GetComponent("UnityEngine.UI.Text");
				
				if(rechargeShopTable[i].Type == 1 or rechargeShopTable[i].Type ==2) then
					txtTitle.text = rechargeShopTable[i].SalesDesc;
					
					--判断 如果是月卡 并且月卡不能购买了 则显示剩余天数 并且按钮禁用
					if(rechargeShopTable[i].Type == 1 and serverRechargeProduct.CanBuy == 0) then
						txtTitle.text = string.format("剩余%s天", serverRechargeProduct.RemainDay);
						--item.transform:GetComponent("UnityEngine.UI.Button").enabled = false;
					end
				else
					txtTitle.text = rechargeShopTable[i].Name;
					
					--首充双倍
					local imgDoubleFlag = item.transform:Find("imgDoubleFlag");

					--如果服务器返回的是双倍标记 则显示双倍
					if(serverRechargeProduct.DoubleFlag ==1) then
						imgDoubleFlag.gameObject:SetActive(true);
					else
						imgDoubleFlag.gameObject:SetActive(false);
					end
					
					imgDoubleFlag = nil;
					
				end
				
				--图片
				local imgIcon = item.transform:Find("imgIcon");
				CS.LuaHelper.Instance:AutoLoadTexture(imgIcon.gameObject, CS.GameUtil.GetAssetPath("Download/Source/UISource/Recharge",rechargeShopTable[i].Icon), rechargeShopTable[i].Icon, true);
				
				--元宝
				local txtYuanBao = item.transform:Find("imgYuanBao/txtYuanBao"):GetComponent("UnityEngine.UI.Text");
				txtYuanBao.text = rechargeShopTable[i].Virtual;
				
				--道具组合
				local imgZH = item.transform:Find("imgZH");
				imgZH.gameObject:SetActive(false);
				if(rechargeShopTable[i].Type == 2) then
					imgZH.gameObject:SetActive(true);
				end
				
				
				--产品描述
				local txtProductDesc = item.transform:Find("txtProductDesc");
				txtProductDesc.gameObject:SetActive(false);
				
				if(rechargeShopTable[i].Type ~= 2) then
					txtProductDesc.gameObject:SetActive(true);
					
					--以服务器端为准
					txtProductDesc:GetComponent("UnityEngine.UI.Text").text = serverRechargeProduct.ProductDesc;
				end
				
				local btnItem = item.transform:GetComponent("UnityEngine.UI.Button");
				btnItem.onClick:AddListener(
					function ()
						print("youyou 点击了计费点"..tostring(rechargeShopTable[i].Id));
						print("youyou 点击了计费点 ChannelId"..tostring(CS.GlobalInit.CurrChannelConfig.ChannelId));
						print("youyou 点击了计费点 ChannelType"..tostring(CS.GlobalInit.CurrChannelConfig.ChannelType));

						if(CS.GlobalInit.CurrChannelConfig.ChannelId == 600) then
							--苹果充值
							CS.AppleStoreInterface.Buy(rechargeShopTable[i].Id);
						elseif(CS.GlobalInit.CurrChannelConfig.ChannelId == 601) then
							--苹果微信充值
							CS.AppleStoreInterface.BuyWeixin(rechargeShopTable[i].Id);
							
						elseif(CS.GlobalInit.CurrChannelConfig.ChannelType == 3) then
							--安卓渠道
							CS.AndroidInterface.Instance:GetCurrAndroidSDK():DoAction("pay", rechargeShopTable[i].Id, nil);
						else
							--付费服务器识别码_玩家账号_要充值到哪个GameServerId_角色ID_充值的产品Id_时间
							local orderId = string.format("%s_%s_%s_%s_%s_%s"
								,CS.GlobalInit.Instance.CurrChannelInitConfig.PayServerNo
								,CS.GlobalInit.Instance.CurrAccount.Id
								,CS.GlobalInit.Instance.CurrSelectGameServer.Id
								,CS.GlobalInit.Instance.MainPlayerInfo.RoleId
								,rechargeShopTable[i].Id
								,CS.GlobalInit.Instance:GetCurrTime()
							);
							
							--print("orderId="..orderId);
							local param = {{"orderId", orderId}};
		
							CS.LuaHelper.Instance:SendHttpData(CS.GlobalInit.Instance.CurrChannelInitConfig.RechargeUrl, this.OnSendHttpDataCallBack, true, param);
						end
						
					end
				);
			

			serverRechargeProduct=nil;
			item=nil;
			txtTitle=nil;
			imgIcon=nil;
			txtYuanBao=nil;
			imgZH=nil;
			txtProductDesc=nil;
			btnItem=nil;
			
			break;
			end
		end
end

function RechargeCtrl.OnSendHttpDataCallBack(args)
	local retValue = CS.LuaHelper.Instance:GetNetWorkRetValue(args);
end