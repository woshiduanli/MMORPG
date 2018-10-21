require "download/xLuaLogic/Data/Create/TaskEntity"

--数据访问
TaskDBModelExt = { }

local this = TaskDBModelExt;

function TaskDBModelExt.New()
    return this;
end


--根据状态获取任务列表
function TaskDBModelExt.GetListByStatus(status)
    local taskTable = DBModelMgr.GetDBModel(DBModelNames.TaskDBModel).GetList();
	
	local retTable = {};
	
	for i=1, #taskTable,1 do
		if(taskTable[i].Status == status) then
			retTable[#retTable+1] = taskTable[i];
		end
	end
	
	return retTable;
end