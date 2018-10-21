require "download/xLuaLogic/Common/Define"


function TestNumber (x)

	return x + 10 - (x / 3);

end

function TestString(str)

	--return string.len(str); --数值 字母 占 1个字符   一个汉字占3个字符
	--return #str; --返回字符的长度 另一种方式
	
	--return string.lower(str); --返回字母的小写形式
	--return string.upper(str); --返回字母的大写形式
	--return str .. "你好Lua"; --使用 .. 连接两个字符
	
	--return string.find(str, "c"); --返回指定字符在字符串中的位置
	--return string.format("你好%s，欢迎来到Lua, 谢谢%s",str, str);
	
	return string.rep(str, 5); --让字符串重复5次
end

function TestIf(x)

	local retValue=0; --定义局部变量 最好加上 local
	
	--在Lua中 没有Switch
	
	if(x == 1) then
		retValue = x * 10;
	elseif (x == 2) then
		retValue = x * 20;
	else
		retValue = x * -1;
	end
	
	return retValue;

end

function TestDoLoop ()

	--i=1 i<=10 i++
	for i=1, 10, 1 do
		print("i="..i);
	end

end

function TestArr ()

	local arr = {"你好", "悠游", "欢迎来到Lua"};
	
	--[[for i=1, #arr, 1 do
		print(arr[i])
	end--]]
	
	--foreach key相当于索引  v就是值
	for key,v in ipairs(arr) do
		print(key)
	end

end

function TestTable()
	
	local myTable={};

	myTable[#myTable+1] = "你好";
	myTable[#myTable+1] = "悠游";
	myTable[#myTable+1] = "欢迎来到Lua";
	
	--myTable[1] = nil; --只是把数据设置为nil
	
	table.remove(myTable, 2);
	table.insert(myTable, 1, "我又来了")
	
	print(table.concat(myTable, "|"));
	
	myTable.x = "我是定义的X";
	myTable.y = "我是定义的Y";
	
	
	
	--print(#myTable);
	
	
end


TestTable();
--print('得到的值='.. TestIf(5));



--常用语句 这个是弹出提示窗口的
--Ok
--OkAndCancel

CS.LuaHelper.Instance.MessageCtrl:ShowMessageView("提示的内容",CommonMessageBoxTitle, CS.MessageViewType.Ok, 
function ()
	--OnShow回调
end, 
function ()
	--OnHide回调
end, 
function ()
	--Ok回调
end, 
function ()
	--Cancel回调
end
);