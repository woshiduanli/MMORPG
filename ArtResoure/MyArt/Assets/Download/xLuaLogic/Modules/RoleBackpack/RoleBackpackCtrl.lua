require "download/xLuaLogic/Common/Define"

RoleBackpackCtrl = {};

local this = RoleBackpackCtrl;

local transform;
local gameObject;

local viewType; --视图类型
local roleEquipViewPrefab; --角色装备视图预设
local roleInfoViewPrefab; --角色信息视图预设
local backpackViewPrefab; --背包视图预设

local roleEquipView; --角色装备视图
local roleInfoView; --角色信息视图
local backpackView; --背包视图

local backpackItemTable; --背包数据表
local CurrGoodsType = -2; --当前的物品类型

function RoleBackpackCtrl.New()
	return this;
end

function RoleBackpackCtrl.Awake(parameters)
	
	viewType = tonumber(parameters[0]);

	CS.LuaHelper.Instance.UIViewUtil:LoadWindowForLua("RoleBackpackView", this.OnCreate, "Download/Prefab/xLuaUIPrefab/RoleBackpack/pan_RoleBackpackView.assetbundle");

	
	CS.LuaHelper.Instance.UIDispatcher:AddEventListener(DispatchMsg.BackpackChange, this.OnBackpackChange);
	CS.LuaHelper.Instance.UIDispatcher:AddEventListener(DispatchMsg.MoneyChange, this.OnMoneyChange);
	CS.LuaHelper.Instance.UIDispatcher:AddEventListener(DispatchMsg.GoldChange, this.OnGoldChange);
	CS.LuaHelper.Instance.UIDispatcher:AddEventListener(DispatchMsg.ChangeRoleEquipInfo, this.OnChangeRoleEquipInfo);
end

function RoleBackpackCtrl.OnMoneyChange(parameters)
	CS.GameUtil.AutoNumberAnimation(BackpackView.lblMoney.gameObject, tonumber(parameters[0]));
	
	if (RoleInfoView.lblMoney ~= nil) then
		RoleInfoView.lblMoney.text = tostring(CS.LuaHelper.Instance.RoleInfoMainPlayer.Money);
	end
end

function RoleBackpackCtrl.OnGoldChange(parameters)
	CS.GameUtil.AutoNumberAnimation(BackpackView.lblGold.gameObject, tonumber(parameters[0]));
	
	if (RoleInfoView.lblGold ~= nil) then
		RoleInfoView.lblGold.text = tostring(CS.LuaHelper.Instance.RoleInfoMainPlayer.Gold);
	end
end

function RoleBackpackCtrl.OnChangeRoleEquipInfo(parameters)
	
	if(roleEquipView ~= nil) then
		RoleEquipView.lblFighting.text = string.format("综合战斗力：<color='#ff0000'>%s</color>", CS.LuaHelper.Instance.RoleInfoMainPlayer.Fighting);
		
		RoleEquipView.SetEquipment("Weapon", CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_WeaponTableId, CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_Weapon);
		RoleEquipView.SetEquipment("Pants", CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_PantsTableId, CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_Pants);
		RoleEquipView.SetEquipment("Clothes", CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_ClothesTableId, CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_Clothes);
		RoleEquipView.SetEquipment("Belt", CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_BeltTableId, CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_Belt);
		RoleEquipView.SetEquipment("Cuff", CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_CuffTableId, CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_Cuff);
		RoleEquipView.SetEquipment("Necklace", CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_NecklaceTableId, CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_Necklace);
		RoleEquipView.SetEquipment("Shoe", CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_ShoeTableId, CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_Shoe);
		RoleEquipView.SetEquipment("Ring", CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_RingTableId, CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_Ring);
	end
	
	if(roleInfoView ~= nil) then
		RoleInfoView.sliderHP.value = CS.LuaHelper.Instance.RoleInfoMainPlayer.CurrHP / CS.LuaHelper.Instance.RoleInfoMainPlayer.MaxHP;
		RoleInfoView.sliderMP.value = CS.LuaHelper.Instance.RoleInfoMainPlayer.CurrMP / CS.LuaHelper.Instance.RoleInfoMainPlayer.MaxMP;
		
		RoleInfoView.lblHP.text = string.format("%s/%s", CS.LuaHelper.Instance.RoleInfoMainPlayer.CurrHP, CS.LuaHelper.Instance.RoleInfoMainPlayer.MaxHP);
		RoleInfoView.lblMP.text = string.format("%s/%s", CS.LuaHelper.Instance.RoleInfoMainPlayer.CurrMP, CS.LuaHelper.Instance.RoleInfoMainPlayer.MaxMP);

		CS.GameUtil.AutoNumberAnimation(RoleInfoView.lblAttack.gameObject, CS.LuaHelper.Instance.RoleInfoMainPlayer.Attack);
		CS.GameUtil.AutoNumberAnimation(RoleInfoView.lblDefense.gameObject, CS.LuaHelper.Instance.RoleInfoMainPlayer.Defense);
		CS.GameUtil.AutoNumberAnimation(RoleInfoView.lblHit.gameObject, CS.LuaHelper.Instance.RoleInfoMainPlayer.Hit);
		CS.GameUtil.AutoNumberAnimation(RoleInfoView.lblDodge.gameObject, CS.LuaHelper.Instance.RoleInfoMainPlayer.Dodge);
		CS.GameUtil.AutoNumberAnimation(RoleInfoView.lblCri.gameObject, CS.LuaHelper.Instance.RoleInfoMainPlayer.Cri);
		CS.GameUtil.AutoNumberAnimation(RoleInfoView.lblRes.gameObject, CS.LuaHelper.Instance.RoleInfoMainPlayer.Res);
	end

end

function RoleBackpackCtrl.OnBackpackChange(parameters)
	
	CS.LuaHelper.Instance.UIViewUtil:CloseWindow("GoodsEquipView");
	CS.LuaHelper.Instance.UIViewUtil:CloseWindow("GoodsItemView");
	CS.LuaHelper.Instance.UIViewUtil:CloseWindow("GoodsMaterialView");
	this.LoadBackpackdata(CurrGoodsType, true);
end

function RoleBackpackCtrl.OnCreate(obj)
	

end

function RoleBackpackCtrl.OnStart()
	CS.LuaHelper.Instance.AssetBundleMgr:LoadOrDownloadForLua("Download/Prefab/xLuaUIPrefab/RoleBackpack/roleEquipView.assetbundle", "roleEquipView", this.OnLoadRoleEquipView);
end

function RoleBackpackCtrl.OnDestroy()
	
	CS.LuaHelper.Instance.UIDispatcher:RemoveEventListener(DispatchMsg.BackpackChange, this.OnBackpackChange);
	CS.LuaHelper.Instance.UIDispatcher:RemoveEventListener(DispatchMsg.MoneyChange, this.OnMoneyChange);
	CS.LuaHelper.Instance.UIDispatcher:RemoveEventListener(DispatchMsg.GoldChange, this.OnGoldChange);
	
	transform = nil;
	gameObject = nil;
	viewType = nil;
	roleEquipViewPrefab = nil;
	roleInfoViewPrefab = nil;
	backpackViewPrefab = nil;
	roleEquipView = nil;
	roleInfoView = nil;
	backpackView = nil;
	backpackItemTable = nil;
	CurrGoodsType = nil;
	
	role3D = nil;
end

--1.加载出角色装备视图预设
function RoleBackpackCtrl.OnLoadRoleEquipView(obj, userData)
	roleEquipViewPrefab = obj; --等到全部镜像加载完毕后 一起克隆
	CS.LuaHelper.Instance.AssetBundleMgr:LoadOrDownloadForLua("Download/Prefab/xLuaUIPrefab/RoleBackpack/RoleInfoView.assetbundle", "RoleInfoView", this.OnLoadRoleInfoView);
end

--2.加载出角色信息视图预设
function RoleBackpackCtrl.OnLoadRoleInfoView(obj, userData)
	roleInfoViewPrefab = obj;
	
	CS.LuaHelper.Instance.AssetBundleMgr:LoadOrDownloadForLua("Download/Prefab/xLuaUIPrefab/RoleBackpack/BackpackView.assetbundle", "BackpackView", this.OnLoadBackpackView);
end

--3.加载出背包视图预设
function RoleBackpackCtrl.OnLoadBackpackView(obj, userData)
	backpackViewPrefab = obj;


	
	this.LoadPrefabOver();
end

--4.预设加载完毕 开始克隆预设
function RoleBackpackCtrl.LoadPrefabOver()
	
	RoleBackpackView.btnRole.onClick:AddListener(
		function ()
			viewType = 0;
			this.LoadView();
		end
	);
	RoleBackpackView.btnBackpack.onClick:AddListener(
		function ()
			viewType = 1;
			this.LoadView();
		end
	);
	
	
	--克隆装备模块
	roleEquipView = CS.UnityEngine.Object.Instantiate(roleEquipViewPrefab); 
	roleEquipView.transform.parent = RoleBackpackView.roleEquipContainer;
	roleEquipView.transform.localPosition = Vector3.zero;
	roleEquipView.transform.localScale = Vector3.one;
	
	this.LoadView();
	this.LoadRoleEquipView();
end

local role3D;
local y=1;

--6.加载角色装备视图
function RoleBackpackCtrl.LoadRoleEquipView()
	--克隆3D角色
	local obj = CS.LuaHelper.Instance.RoleMgr:LoadPlayer(CS.LuaHelper.Instance.RoleInfoMainPlayer.JobId);
	CS.GameObjectUtil.SetParent(obj, RoleEquipView.RoleRoot.transform);
	CS.GameObjectUtil.SetLayer(obj, "UI");
	RoleEquipView.imgDragView.OnDraging = this.OnDraging;
	role3D = obj;
	
	RoleEquipView.lblNickName.text = CS.LuaHelper.Instance.RoleInfoMainPlayer.RoleNickName;
	RoleEquipView.lblLevel.text = string.format("Lv.%s", CS.LuaHelper.Instance.RoleInfoMainPlayer.Level);
	RoleEquipView.lblFighting.text = string.format("综合战斗力：<color='#ff0000'>%s</color>", CS.LuaHelper.Instance.RoleInfoMainPlayer.Fighting);
	
	RoleEquipView.SetEquipment("Weapon", CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_WeaponTableId, CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_Weapon);
	RoleEquipView.SetEquipment("Pants", CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_PantsTableId, CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_Pants);
	RoleEquipView.SetEquipment("Clothes", CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_ClothesTableId, CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_Clothes);
	RoleEquipView.SetEquipment("Belt", CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_BeltTableId, CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_Belt);
	RoleEquipView.SetEquipment("Cuff", CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_CuffTableId, CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_Cuff);
	RoleEquipView.SetEquipment("Necklace", CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_NecklaceTableId, CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_Necklace);
	RoleEquipView.SetEquipment("Shoe", CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_ShoeTableId, CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_Shoe);
	RoleEquipView.SetEquipment("Ring", CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_RingTableId, CS.LuaHelper.Instance.RoleInfoMainPlayer.Equip_Ring);
		
	RoleEquipView.GetEquipmentButton("Weapon").onClick:AddListener(
		function ()
			this.EquipButtonClick("Weapon");
		end
	);
	
	RoleEquipView.GetEquipmentButton("Pants").onClick:AddListener(
		function ()
			this.EquipButtonClick("Pants");
		end
	);
	
	RoleEquipView.GetEquipmentButton("Clothes").onClick:AddListener(
		function ()
			this.EquipButtonClick("Clothes");
		end
	);
	
	RoleEquipView.GetEquipmentButton("Belt").onClick:AddListener(
		function ()
			this.EquipButtonClick("Belt");
		end
	);
	
	RoleEquipView.GetEquipmentButton("Cuff").onClick:AddListener(
		function ()
			this.EquipButtonClick("Cuff");
		end
	);
	
	RoleEquipView.GetEquipmentButton("Necklace").onClick:AddListener(
		function ()
			this.EquipButtonClick("Necklace");
		end
	);
	
	RoleEquipView.GetEquipmentButton("Shoe").onClick:AddListener(
		function ()
			this.EquipButtonClick("Shoe");
		end
	);
	
	RoleEquipView.GetEquipmentButton("Ring").onClick:AddListener(
		function ()
			this.EquipButtonClick("Ring");
		end
	);
end

function RoleBackpackCtrl.EquipButtonClick(type)
	local goodsId;
	local goodsServerId;
	
	if (type=="Weapon") then
		goodsId = RoleEquipView.GetTableEquipId().Weapon;
		goodsServerId = RoleEquipView.GetTableEquipServerId().Weapon;
	elseif (type=="Pants") then
		goodsId = RoleEquipView.GetTableEquipId().Pants;
		goodsServerId = RoleEquipView.GetTableEquipServerId().Pants;
	elseif (type=="Clothes") then
		goodsId = RoleEquipView.GetTableEquipId().Clothes;
		goodsServerId = RoleEquipView.GetTableEquipServerId().Clothes;
	elseif (type=="Belt") then
		goodsId = RoleEquipView.GetTableEquipId().Belt;
		goodsServerId = RoleEquipView.GetTableEquipServerId().Belt;
	elseif (type=="Cuff") then
		goodsId = RoleEquipView.GetTableEquipId().Cuff;
		goodsServerId = RoleEquipView.GetTableEquipServerId().Cuff;
	elseif (type=="Necklace") then
		goodsId = RoleEquipView.GetTableEquipId().Necklace;
		goodsServerId = RoleEquipView.GetTableEquipServerId().Necklace;
	elseif (type=="Shoe") then
		goodsId = RoleEquipView.GetTableEquipId().Shoe;
		goodsServerId = RoleEquipView.GetTableEquipServerId().Shoe;
	elseif (type=="Ring") then
		goodsId = RoleEquipView.GetTableEquipId().Ring;
		goodsServerId = RoleEquipView.GetTableEquipServerId().Ring;
	end
	
	if(goodsId ~= nil) then
		GameInit.LoadView(CtrlNames.GoodsEquipCtrl, {0, goodsId, goodsServerId, 0});
	end
end

function RoleBackpackCtrl.OnDraging(dir)
	--print("--->"..tostring(dir)); 0=左 1=右
	y=1;
	if (dir == 1) then
		y=-1;
	end
	
	role3D.transform:Rotate(0, y*6, 0); --这里的6就是旋转的速度
end


--5.加载视图
function RoleBackpackCtrl.LoadView()
	
	--print("viewType="..viewType);
	
	if(viewType == 0) then --克隆角色信息
		RoleBackpackView.btnBackpack.image.color = Color.white;
		RoleBackpackView.btnRole.image.color = Color(255 / 255, 228 / 255, 183 / 255, 255 / 255);
	
		if(roleInfoView ~= nil) then
			if(backpackView ~= nil) then
				backpackView.transform.gameObject:SetActive(false);
			end
			roleInfoView.transform.gameObject:SetActive(true);
			return;
		end
	
		roleInfoView = CS.UnityEngine.Object.Instantiate(roleInfoViewPrefab);
		roleInfoView.transform.parent = RoleBackpackView.roleInfoContainer;
		roleInfoView.transform.localPosition = Vector3.zero;
		roleInfoView.transform.localScale = Vector3.one;
		
		--赋值数据 把角色身上的数据复制到UI
		RoleInfoView.lblMoney.text = tostring(CS.LuaHelper.Instance.RoleInfoMainPlayer.Money);
		RoleInfoView.lblGold.text = tostring(CS.LuaHelper.Instance.RoleInfoMainPlayer.Gold);
		
		RoleInfoView.sliderHP.value = CS.LuaHelper.Instance.RoleInfoMainPlayer.CurrHP / CS.LuaHelper.Instance.RoleInfoMainPlayer.MaxHP;
		RoleInfoView.sliderMP.value = CS.LuaHelper.Instance.RoleInfoMainPlayer.CurrMP / CS.LuaHelper.Instance.RoleInfoMainPlayer.MaxMP;
		
		--计算当前角色等级的最大经验
		local maxExp = 0;
		local jobLevelEntity = DBModelMgr.GetDBModel(DBModelNames.JobLevelDBModel).GetEntity(CS.LuaHelper.Instance.RoleInfoMainPlayer.Level);
		if(jobLevelEntity ~= nil) then
			maxExp = jobLevelEntity.NeedExp;
		end
		jobLevelEntity = nil;
		
		
		RoleInfoView.siliderExp.value = CS.LuaHelper.Instance.RoleInfoMainPlayer.Exp / maxExp;
	
		RoleInfoView.lblHP.text = string.format("%s/%s", CS.LuaHelper.Instance.RoleInfoMainPlayer.CurrHP, CS.LuaHelper.Instance.RoleInfoMainPlayer.MaxHP);
		RoleInfoView.lblMP.text = string.format("%s/%s", CS.LuaHelper.Instance.RoleInfoMainPlayer.CurrMP, CS.LuaHelper.Instance.RoleInfoMainPlayer.MaxMP);
		RoleInfoView.lblExp.text = string.format("%s/%s", CS.LuaHelper.Instance.RoleInfoMainPlayer.Exp, maxExp);
		
		RoleInfoView.lblAttack.text = tostring(CS.LuaHelper.Instance.RoleInfoMainPlayer.Attack);
		RoleInfoView.lblDefense.text = tostring(CS.LuaHelper.Instance.RoleInfoMainPlayer.Defense);
		RoleInfoView.lblHit.text = tostring(CS.LuaHelper.Instance.RoleInfoMainPlayer.Hit);
		RoleInfoView.lblDodge.text = tostring(CS.LuaHelper.Instance.RoleInfoMainPlayer.Dodge);
		RoleInfoView.lblCri.text = tostring(CS.LuaHelper.Instance.RoleInfoMainPlayer.Cri);
		RoleInfoView.lblRes.text = tostring(CS.LuaHelper.Instance.RoleInfoMainPlayer.Res);
		
		maxExp = nil;
		jobLevelEntity = nil;
		
	elseif(viewType == 1) then --克隆背包模块
	
		RoleBackpackView.btnRole.image.color = Color.white;
		RoleBackpackView.btnBackpack.image.color = Color(255 / 255, 228 / 255, 183 / 255, 255 / 255);
		
		if(backpackView ~= nil) then
			if(roleInfoView ~= nil) then
				roleInfoView.transform.gameObject:SetActive(false);
			end
			backpackView.transform.gameObject:SetActive(true);
			return;
		end

	
		backpackView = CS.UnityEngine.Object.Instantiate(backpackViewPrefab);
		backpackView.transform.parent = RoleBackpackView.roleInfoContainer;
		backpackView.transform.localPosition = Vector3.zero;
		backpackView.transform.localScale = Vector3.one;

		BackpackView.lblMoney.text = tostring(CS.LuaHelper.Instance.RoleInfoMainPlayer.Money);
		BackpackView.lblGold.text = tostring(CS.LuaHelper.Instance.RoleInfoMainPlayer.Gold);
		
		this.LoadBackpackdata(-1, false);
		
		BackpackView.btnAll.onClick:AddListener(
			function ()
				this.LoadBackpackdata(-1, false);
			end
		);
		BackpackView.btnEquip.onClick:AddListener(
			function ()
				this.LoadBackpackdata(0, false);
			end
		);
		BackpackView.btnItem.onClick:AddListener(
			function ()
				this.LoadBackpackdata(1, false);
			end
		);
		BackpackView.btnMaterial.onClick:AddListener(
			function ()
				this.LoadBackpackdata(2, false);
			end
		);
		
	end
	
end


function RoleBackpackCtrl.LoadBackpackdata(goodsType, reload)
	
	if (backpackView == nil) then
		return;
	end
	
	if(reload == false) then
		if(goodsType == CurrGoodsType) then
			return;
		end
	end
	
	BackpackView.btnAll.image.color = Color.white;
	BackpackView.btnEquip.image.color = Color.white;
	BackpackView.btnItem.image.color = Color.white;
	BackpackView.btnMaterial.image.color = Color.white;
		
	if(goodsType == -1) then
		BackpackView.btnAll.image.color = Color(255 / 255, 228 / 255, 183 / 255, 255 / 255);
	elseif(goodsType == 0) then
		BackpackView.btnEquip.image.color = Color(255 / 255, 228 / 255, 183 / 255, 255 / 255);
	elseif(goodsType == 1) then
		BackpackView.btnItem.image.color = Color(255 / 255, 228 / 255, 183 / 255, 255 / 255);
	elseif(goodsType == 2) then
		BackpackView.btnMaterial.image.color = Color(255 / 255, 228 / 255, 183 / 255, 255 / 255);
	end
	
	
	CurrGoodsType = goodsType;
	
	backpackItemTable = CtrlMgr.GetCtrl(CtrlNames.RoleDataCtrl).GetBackpackItemTable(goodsType);
	BackpackView.PageView.OnItemCreate = this.OnItemCreate;
	BackpackView.PageView:InitData(#backpackItemTable);
end

function RoleBackpackCtrl.OnItemCreate(index, obj)
	--print(index);
	local newIndex = index + 1;

	local imgIcon = obj.transform:Find("imgIcon"):GetComponent("UnityEngine.UI.Image"); --图标
	local lblCount = obj.transform:Find("imgCountBG/lblCount"):GetComponent("UnityEngine.UI.Text"); --数量
	local btnItem = obj.transform:Find("btnItem"):GetComponent("UnityEngine.UI.Button"); --按钮
	
	local goodsType;
	local goodsId;
	local goodsServerId;
	local backpackItemId;
	
	if (newIndex > #backpackItemTable) then
		--print("空项目");
		imgIcon.gameObject:SetActive(false);
		lblCount.transform.parent.gameObject:SetActive(false);
	else
		--print(backpackItemTable[newIndex].GoodsId);
		imgIcon.gameObject:SetActive(true);
		lblCount.transform.parent.gameObject:SetActive(true);
		
		goodsType = backpackItemTable[newIndex].GoodsType;
		goodsId = backpackItemTable[newIndex].GoodsId;
		goodsServerId = backpackItemTable[newIndex].GoodsServerId;
		backpackItemId = backpackItemTable[newIndex].BackpackItemId;
		
		
			--给物品图标赋值
		if(goodsType == 0) then --装备
			CS.LuaHelper.Instance:AutoLoadTexture(imgIcon.gameObject, CS.GameUtil.GetAssetPath("Download/Source/UISource/EquipIco", goodsId), goodsId, false);
		elseif(goodsType == 1) then --道具
			CS.LuaHelper.Instance:AutoLoadTexture(imgIcon.gameObject, CS.GameUtil.GetAssetPath("Download/Source/UISource/ItemIco", goodsId), goodsId, false);
		elseif(goodsType == 2) then --材料
			CS.LuaHelper.Instance:AutoLoadTexture(imgIcon.gameObject, CS.GameUtil.GetAssetPath("Download/Source/UISource/MaterialIco", goodsId), goodsId, false);
		end
	
		lblCount.text = backpackItemTable[newIndex].GoodsOverlayCount; --叠加数量
		
		btnItem.onClick:RemoveAllListeners(); --先把旧的监听全部移除
		btnItem.onClick:AddListener(
			function ()
				--print("点击的物品类型"..tostring(goodsType));
				--print("点击的服务器端编号"..tostring(goodsId));
				--print("点击的服务器端编号"..tostring(goodsServerId));
				
				if(goodsType == 0) then --装备
					--弹出装备详情窗口
					GameInit.LoadView(CtrlNames.GoodsEquipCtrl, {goodsType, goodsId, goodsServerId, backpackItemId});
				elseif(goodsType == 1) then
					--弹出道具详情窗口
					GameInit.LoadView(CtrlNames.GoodsItemCtrl, {goodsType, goodsId, goodsServerId, backpackItemId});
				elseif(goodsType == 2) then
					--弹出材料详情窗口
					GameInit.LoadView(CtrlNames.GoodsMaterialCtrl, {goodsType, goodsId, goodsServerId, backpackItemId});
				end
			end
		);
	end
	
	newIndex = nil;
	imgIcon = nil;
	lblCount = nil;
	btnItem = nil;
end