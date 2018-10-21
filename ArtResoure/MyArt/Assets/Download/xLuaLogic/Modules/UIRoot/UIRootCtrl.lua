UIRootCtrl = {};

local this = UIRootCtrl;

local root;
local transform;
local gameObject;


function UIRootCtrl.New()
	return this;
end

function UIRootCtrl.Awake()
	print('主界面 启动了');
	--这里就是负责 把UI克隆出来
	CS.LuaHelper.Instance.UISceneCtrl:LoadSceneUI("Download/Prefab/xLuaUIPrefab/UIRootView.assetbundle", this.OnCreate);
	
end

function UIRootCtrl.OnCreate(obj)
	print('UI克隆完毕');
	
	--这个变量类型 就是Button
	local btnOpenTask = UIRootView.btnOpenTask;
	btnOpenTask.onClick:AddListener(this.OnbtnOpenTaskClick);
	
end

function UIRootCtrl.OnbtnOpenTaskClick()
	print('按钮点击了');
	GameInit.LoadView(CtrlNames.TaskCtrl);
end