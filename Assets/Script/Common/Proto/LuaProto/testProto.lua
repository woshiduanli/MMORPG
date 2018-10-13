--新建协议
testProto = { ProtoCode = 17001, id = 0 }

--这句是重定义元表的索引，就是说有了这句，这个才是一个类
testProto.__index = testProto;

function testProto.New()
    local self = { }; --初始化self
    setmetatable(self, testProto); --将self的元表设定为Class
    return self;
end


--发送协议
function testProto.SendProto(proto)

    local ms =CSGlobal.CreateMemoryStream();
    ms:WriteUShort(proto.ProtoCode);

    ms:WriteInt(proto.id);

   CSGlobal.SendProto(ms:ToArray());
    ms:Dispose();
end


--解析协议
function testProto.GetProto(buffer)

    local proto = testProto.New(); --实例化一个协议对象
    local ms =CSGlobal.CreateMemoryStream(buffer);

    proto.id = ms:ReadInt();

    ms:Dispose();
    return proto;
end