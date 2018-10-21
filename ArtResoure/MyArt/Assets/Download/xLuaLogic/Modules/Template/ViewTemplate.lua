ViewTemplate = {};

local this = ViewTemplate;

local transform;
local gameObject;

function ViewTemplate.awake(obj)
	gameObject = obj;
	transform = obj.transform;
	
	this.InitView();
end

function ViewTemplate.InitView()
	--找到UI组件
end

function ViewTemplate.start()

end

function ViewTemplate.update()

end

function ViewTemplate.ondestroy()

end