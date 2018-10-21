RoleEquipView = {};

local this = RoleEquipView;

local transform;
local gameObject;

local tableEquipment={}; --装备图片
local tableEquipmentBG={}; --装备背景
local tableEquipmentButton={};--装备格子按钮

local tableEquipId={}; --本地数据表编号
local tableEquipServerId={}; --装备服务器编号

function RoleEquipView.awake(obj)
	gameObject = obj;
	transform = obj.transform;
	this.InitView();
end

function RoleEquipView.GetTableEquipId()
	return tableEquipId;
end

function RoleEquipView.GetTableEquipServerId()
	return tableEquipServerId;
end

function RoleEquipView.InitView()
	--找到UI组件
	this.RoleRoot = transform:Find("3DRoleRoot");
	this.imgDragView = transform:Find("imgDragView"):GetComponent("UISelectRoleDragView");
	
	this.lblNickName = transform:Find("RoleNickName/lblNickName"):GetComponent("UnityEngine.UI.Text");
	this.lblLevel = transform:Find("RoleNickName/lblLevel"):GetComponent("UnityEngine.UI.Text");
	this.lblFighting = transform:Find("bg_Fighting/lblFighting"):GetComponent("UnityEngine.UI.Text");
	
	tableEquipment.Weapon = transform:Find("Equipment/Weapon/imgWeapon"):GetComponent("UnityEngine.UI.Image");
	tableEquipment.Pants = transform:Find("Equipment/Pants/imgPants"):GetComponent("UnityEngine.UI.Image");
	tableEquipment.Clothes = transform:Find("Equipment/Clothes/imgClothes"):GetComponent("UnityEngine.UI.Image");
	tableEquipment.Belt = transform:Find("Equipment/Belt/imgBelt"):GetComponent("UnityEngine.UI.Image");
	tableEquipment.Cuff = transform:Find("Equipment/Cuff/imgCuff"):GetComponent("UnityEngine.UI.Image");
	tableEquipment.Necklace = transform:Find("Equipment/Necklace/imgNecklace"):GetComponent("UnityEngine.UI.Image");
	tableEquipment.Shoe = transform:Find("Equipment/Shoe/imgShoe"):GetComponent("UnityEngine.UI.Image");
	tableEquipment.Ring = transform:Find("Equipment/Ring/imgRing"):GetComponent("UnityEngine.UI.Image");
	
	tableEquipmentButton.Weapon = transform:Find("Equipment/Weapon/btn_Weapon"):GetComponent("UnityEngine.UI.Button");
	tableEquipmentButton.Pants = transform:Find("Equipment/Pants/btn_Pants"):GetComponent("UnityEngine.UI.Button");
	tableEquipmentButton.Clothes = transform:Find("Equipment/Clothes/btn_Clothes"):GetComponent("UnityEngine.UI.Button");
	tableEquipmentButton.Belt = transform:Find("Equipment/Belt/btn_Belt"):GetComponent("UnityEngine.UI.Button");
	tableEquipmentButton.Cuff = transform:Find("Equipment/Cuff/btn_Cuff"):GetComponent("UnityEngine.UI.Button");
	tableEquipmentButton.Necklace = transform:Find("Equipment/Necklace/btn_Necklace"):GetComponent("UnityEngine.UI.Button");
	tableEquipmentButton.Shoe = transform:Find("Equipment/Shoe/btn_Shoe"):GetComponent("UnityEngine.UI.Button");
	tableEquipmentButton.Ring = transform:Find("Equipment/Ring/btn_Ring"):GetComponent("UnityEngine.UI.Button");
	
	tableEquipmentBG.Weapon = transform:Find("Equipment/Weapon/bg_Weapon");
	tableEquipmentBG.Pants = transform:Find("Equipment/Pants/bg_Pants");
	tableEquipmentBG.Clothes = transform:Find("Equipment/Clothes/bg_Clothes");
	tableEquipmentBG.Belt = transform:Find("Equipment/Belt/bg_Belt");
	tableEquipmentBG.Cuff = transform:Find("Equipment/Cuff/bg_Cuff");
	tableEquipmentBG.Necklace = transform:Find("Equipment/Necklace/bg_Necklace");
	tableEquipmentBG.Shoe = transform:Find("Equipment/Shoe/bg_Shoe");
	tableEquipmentBG.Ring = transform:Find("Equipment/Ring/bg_Ring");
end

--获取装备图片格子按钮
function RoleEquipView.GetEquipmentButton(equipType)
	if (equipType=="Weapon") then
		return tableEquipmentButton.Weapon;
	elseif (equipType=="Pants") then
		return tableEquipmentButton.Pants;
	elseif (equipType=="Clothes") then
		return tableEquipmentButton.Clothes;
	elseif (equipType=="Belt") then
		return tableEquipmentButton.Belt;
	elseif (equipType=="Cuff") then
		return tableEquipmentButton.Cuff;
	elseif (equipType=="Necklace") then
		return tableEquipmentButton.Necklace;
	elseif (equipType=="Shoe") then
		return tableEquipmentButton.Shoe;
	elseif (equipType=="Ring") then
		return tableEquipmentButton.Ring;
	end
end

--设置装备信息
function RoleEquipView.SetEquipment(equipType, equipID, equipServerId)
	if (equipType=="Weapon") then
		if (equipID<1) then
			tableEquipment.Weapon.gameObject:SetActive(false);
			tableEquipmentBG.Weapon.gameObject:SetActive(true);
		else
			tableEquipId.Weapon = equipID;
			tableEquipServerId.Weapon = equipServerId;
			tableEquipment.Weapon.gameObject:SetActive(true);
			tableEquipmentBG.Weapon.gameObject:SetActive(false);
			CS.LuaHelper.Instance:AutoLoadTexture(tableEquipment.Weapon.gameObject, CS.GameUtil.GetAssetPath("Download/Source/UISource/EquipIco", equipID), equipID, false);
		end
	elseif (equipType=="Pants") then
		if (equipID<1) then
			tableEquipment.Pants.gameObject:SetActive(false);
			tableEquipmentBG.Pants.gameObject:SetActive(true);
		else
			tableEquipId.Pants = equipID;
			tableEquipServerId.Pants = equipServerId;
			tableEquipment.Pants.gameObject:SetActive(true);
			tableEquipmentBG.Pants.gameObject:SetActive(false);
			CS.LuaHelper.Instance:AutoLoadTexture(tableEquipment.Pants.gameObject, CS.GameUtil.GetAssetPath("Download/Source/UISource/EquipIco", equipID), equipID, false);
		end
	elseif (equipType=="Clothes") then
		if (equipID<1) then
			tableEquipment.Clothes.gameObject:SetActive(false);
			tableEquipmentBG.Clothes.gameObject:SetActive(true);
		else
			tableEquipId.Clothes = equipID;
			tableEquipServerId.Clothes = equipServerId;
			tableEquipment.Clothes.gameObject:SetActive(true);
			tableEquipmentBG.Clothes.gameObject:SetActive(false);
			CS.LuaHelper.Instance:AutoLoadTexture(tableEquipment.Clothes.gameObject, CS.GameUtil.GetAssetPath("Download/Source/UISource/EquipIco", equipID), equipID, false);
		end
	elseif (equipType=="Belt") then
		if (equipID<1) then
			tableEquipment.Belt.gameObject:SetActive(false);
			tableEquipmentBG.Belt.gameObject:SetActive(true);
		else
			tableEquipId.Belt = equipID;
			tableEquipServerId.Belt = equipServerId;
			tableEquipment.Belt.gameObject:SetActive(true);
			tableEquipmentBG.Belt.gameObject:SetActive(false);
			CS.LuaHelper.Instance:AutoLoadTexture(tableEquipment.Belt.gameObject, CS.GameUtil.GetAssetPath("Download/Source/UISource/EquipIco", equipID), equipID, false);
		end
	elseif (equipType=="Cuff") then
		if (equipID<1) then
			tableEquipment.Cuff.gameObject:SetActive(false);
			tableEquipmentBG.Cuff.gameObject:SetActive(true);
		else
			tableEquipId.Cuff = equipID;
			tableEquipServerId.Cuff = equipServerId;
			tableEquipment.Cuff.gameObject:SetActive(true);
			tableEquipmentBG.Cuff.gameObject:SetActive(false);
			CS.LuaHelper.Instance:AutoLoadTexture(tableEquipment.Cuff.gameObject, CS.GameUtil.GetAssetPath("Download/Source/UISource/EquipIco", equipID), equipID, false);
		end
	elseif (equipType=="Necklace") then
		if (equipID<1) then
			tableEquipment.Necklace.gameObject:SetActive(false);
			tableEquipmentBG.Necklace.gameObject:SetActive(true);
		else
			tableEquipId.Necklace = equipID;
			tableEquipServerId.Necklace = equipServerId;
			tableEquipment.Necklace.gameObject:SetActive(true);
			tableEquipmentBG.Necklace.gameObject:SetActive(false);
			CS.LuaHelper.Instance:AutoLoadTexture(tableEquipment.Necklace.gameObject, CS.GameUtil.GetAssetPath("Download/Source/UISource/EquipIco", equipID), equipID, false);
		end
	elseif (equipType=="Shoe") then
		if (equipID<1) then
			tableEquipment.Shoe.gameObject:SetActive(false);
			tableEquipmentBG.Shoe.gameObject:SetActive(true);
		else
			tableEquipId.Shoe = equipID;
			tableEquipServerId.Shoe = equipServerId;
			tableEquipment.Shoe.gameObject:SetActive(true);
			tableEquipmentBG.Shoe.gameObject:SetActive(false);
			CS.LuaHelper.Instance:AutoLoadTexture(tableEquipment.Shoe.gameObject, CS.GameUtil.GetAssetPath("Download/Source/UISource/EquipIco", equipID), equipID, false);
		end
	elseif (equipType=="Ring") then
		if (equipID<1) then
			tableEquipment.Ring.gameObject:SetActive(false);
			tableEquipmentBG.Ring.gameObject:SetActive(true);
		else
			tableEquipId.Ring = equipID;
			tableEquipServerId.Ring = equipServerId;
			tableEquipment.Ring.gameObject:SetActive(true);
			tableEquipmentBG.Ring.gameObject:SetActive(false);
			CS.LuaHelper.Instance:AutoLoadTexture(tableEquipment.Ring.gameObject, CS.GameUtil.GetAssetPath("Download/Source/UISource/EquipIco", equipID), equipID, false);
		end
	end
end

function RoleEquipView.start()

end

function RoleEquipView.update()

end

function RoleEquipView.ondestroy()

end