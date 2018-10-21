UIRootView = {};

local this = UIRootView;

local transform;
local gameObject;

function UIRootView.awake(obj)
	gameObject = obj;
	transform = obj.transform;
	
	this.InitView();
end

function UIRootView.InitView()
	--ÕÒµ½UI×é¼þ
	print("UIRootView.InitView");
	
	this.btnOpenTask = transform:Find("Canvas/ContainerBottomRight/btnOpenTask"):GetComponent("UnityEngine.UI.Button");
end

function UIRootView.start()

end

function UIRootView.update()

end

function UIRootView.ondestroy()

end