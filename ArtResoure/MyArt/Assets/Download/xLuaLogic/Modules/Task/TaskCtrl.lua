require "download/xLuaLogic/Common/Define"
require "download/xLuaLogic/Data/DBModelMgr"

require "download/xLuaLogic/Proto/Task_SearchTaskProto"
require "download/xLuaLogic/Proto/Task_SearchTaskReturnProto"


TaskCtrl = {};

local this = TaskCtrl;

local transform;
local gameObject;
local taskTable;

local taskItemObj;
local taskItemObjTable = {};

local currStatus = -1; --当前状态

function TaskCtrl.New()
	return this;
end

function TaskCtrl.Awake()
	print("TaskCtrl.Awake");
	CS.LuaHelper.Instance.UIViewUtil:LoadWindowForLua("TaskView", this.OnCreate, "Download/Prefab/xLuaUIPrefab/pan_TaskView.assetbundle");
end

function TaskCtrl.OnCreate(obj)
	--添加协议的监听
	CS.LuaHelper.Instance:AddEventListener(ProtoCode.Task_SearchTaskReturn, this.OnSearchTaskCallBack);
	
	TaskView.btnStatus0.onClick:AddListener(this.OnbtnStatus0Click);
	TaskView.btnStatus1.onClick:AddListener(this.OnbtnStatus1Click);
	TaskView.btnStatus2.onClick:AddListener(this.OnbtnStatus2Click);
	
end

function TaskCtrl.OnDestroy()
	--窗口关闭 移除监听
	CS.LuaHelper.Instance:RemoveEventListener(ProtoCode.Task_SearchTaskReturn, this.OnSearchTaskCallBack);
end

function TaskCtrl.OnbtnStatus0Click()
	this.LoadList(0);
end

function TaskCtrl.OnbtnStatus1Click()
	this.LoadList(1);
end

function TaskCtrl.OnbtnStatus2Click()
	this.LoadList(2);
end

--服务器返回查询任务协议
function TaskCtrl.OnSearchTaskCallBack(buffer)
	print("在Lua中 收到服务器回调");
	
	local proto = Task_SearchTaskReturnProto.GetProto(buffer);
	taskTable = proto.TaskTable;
	
	this.LoadList(0);
end

function TaskCtrl.OnStart()
	print("任务列表创建完毕");
	
	--拿出镜像
	CS.LuaHelper.Instance.AssetBundleMgr:LoadOrDownloadForLua("Download/Prefab/xLuaUIPrefab/TaskItemView.assetbundle", "TaskItemView", this.OnLoadItem);

end

function TaskCtrl.OnLoadItem(obj)
	
	taskItemObj = obj;
	--给服务器发消息
	local proto = Task_SearchTaskProto.New();
	Task_SearchTaskProto.SendProto(proto);
	print("lua给服务器发协议");
end

function TaskCtrl.LoadList(status)
		
	if(currStatus == status) then
		return;
	end
	
	currStatus = status;
	
	--先销毁之前的item
	--要倒着销毁 否则移除表元素会报错
	for i=#taskItemObjTable, 1, -1 do
		CS.UnityEngine.Object.Destroy(taskItemObjTable[i]);
		table.remove(taskItemObjTable, i);
	end
		
	--新的过滤状态的表
	local taskStatusTable = {};
	
	for i=1, #taskTable, 1 do
		if(taskTable[i].Status == status) then
			taskStatusTable[#taskStatusTable+1] = taskTable[i];
		end
	end
	
	
	for i=1, #taskStatusTable, 1 do
		
			--克隆预设
			local item = CS.UnityEngine.Object.Instantiate(taskItemObj); -- .用于类的方法 :用于类成员变量的方法
			item.transform.parent = TaskView.content;
			item.transform.localPosition = Vector3.zero;
			item.transform.localScale = Vector3.one;
			
			--把克隆出的物体 加入taskItemObjTable 目的为了下次销毁
			taskItemObjTable[#taskItemObjTable+1] = item;
			--
			
			local task = taskStatusTable[i];
			
			local txtTitle = item.transform:Find("txtTitle"):GetComponent("UnityEngine.UI.Text");
			local txtStatus = item.transform:Find("txtStatus"):GetComponent("UnityEngine.UI.Text");
			
			txtTitle.text = task.Name;
			txtStatus.text = this.GetTaskStatusName(task.Status);
			
			local btnItem = item.transform:GetComponent("UnityEngine.UI.Button");
			btnItem.onClick:AddListener(
				function ()
					TaskView.detailContainer.gameObject:SetActive(true);
					TaskView.txtTaskId.text = tostring(task.Id);
					TaskView.txtTaskName.text = task.Name;
					TaskView.txtTaskStatus.text = this.GetTaskStatusName(task.Status);
					TaskView.txtTaskContent:DOText(task.Content, 0.5);
					
				end
			);
	end
end


function TaskCtrl.GetTaskStatusName(status)
	if(status == 0) then
		return "<color=#06C300FF>未接</color>"
	elseif(status == 1) then
		return "<color=#00CAFFFF>已接</color>"
	elseif(status == 2) then
		return "<color=#FFAE00FF>已完成</color>"
	end
end