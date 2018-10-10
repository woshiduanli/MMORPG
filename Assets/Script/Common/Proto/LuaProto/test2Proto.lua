--新建协议
test2Proto = { ProtoCode = 17001, ID = 0 }

--这句是重定义元表的索引，就是说有了这句，这个才是一个类
test2Proto.__index = test2Proto;

function test2Proto.New()
    local self = { }; --初始化self
    setmetatable(self, test2Proto); --将self的元表设定为Class
    return self;
end


--发送协议
function test2Proto.SendProto(proto)

    local ms = CS.LuaHelper.Instance:CreateMemoryStream();
    ms:WriteUShort(proto.ProtoCode);

    ms:WriteInt(proto.ID);

    CS.LuaHelper.Instance:SendProto(ms:ToArray());
    ms:Dispose();
end


--解析协议
function test2Proto.GetProto(buffer)

    local proto = test2Proto.New(); --实例化一个协议对象
    local ms = CS.LuaHelper.Instance:CreateMemoryStream(buffer);

    proto.ID = ms:ReadInt();

    ms:Dispose();
    return proto;
end