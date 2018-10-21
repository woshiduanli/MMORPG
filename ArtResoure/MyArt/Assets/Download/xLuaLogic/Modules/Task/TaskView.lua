TaskView = {};

local this = TaskView;

local transform;
local gameObject;

function TaskView.awake(obj)
	gameObject = obj;
	transform = obj.transform;
	
	this.InitView();
end

function TaskView.InitView()
	--找到UI组件
	this.btnStatus0 = transform:Find("btnStatus0"):GetComponent("UnityEngine.UI.Button");
	this.btnStatus1 = transform:Find("btnStatus1"):GetComponent("UnityEngine.UI.Button");
	this.btnStatus2 = transform:Find("btnStatus2"):GetComponent("UnityEngine.UI.Button");
	
	--容器
	this.content = transform:Find("Scroll View/Viewport/Content");
	this.detailContainer = transform:Find("detailContainer");
	this.detailContainer.gameObject:SetActive(false);
	
	--Text
	this.txtTaskId = transform:Find("detailContainer/txtTaskId"):GetComponent("UnityEngine.UI.Text");
	this.txtTaskName = transform:Find("detailContainer/txtTaskName"):GetComponent("UnityEngine.UI.Text");
	this.txtTaskStatus = transform:Find("detailContainer/txtTaskStatus"):GetComponent("UnityEngine.UI.Text");
	this.txtTaskContent = transform:Find("detailContainer/txtTaskContent"):GetComponent("UnityEngine.UI.Text");
end

function TaskView.start()
	TaskCtrl.OnStart();
end

function TaskView.update()

end

function TaskView.ondestroy()
	TaskCtrl.OnDestroy();
end