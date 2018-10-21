ShopView = {};

local this = ShopView;

local transform;
local gameObject;

function ShopView.awake(obj)
	gameObject = obj;
	transform = obj.transform;
	
	this.InitView();
end

function ShopView.InitView()
	--找到UI组件
	this.ScrollViewTabContent = transform:Find("ScrollViewTab/Viewport/Content");
	this.MultiScrollViewVertical = transform:Find("imgContent/MultiScrollViewVertical"):GetComponent("UIMultiScroller");
end

function ShopView.start()
	ShopCtrl.OnStart();
end

function ShopView.update()

end

function ShopView.ondestroy()
	ShopCtrl.OnDestroy();
end