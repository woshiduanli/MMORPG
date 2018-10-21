--服务器返回角色信息
RoleOperation_SelectRoleInfoReturnProto = { ProtoCode = 10010, IsSuccess = false, MsgCode = 0, RoldId = 0, RoleNickName = "", JobId = 0, Level = 0, TotalRechargeMoney = 0, Money = 0, Gold = 0, Exp = 0, MaxHP = 0, MaxMP = 0, CurrHP = 0, CurrMP = 0, Attack = 0, Defense = 0, Hit = 0, Dodge = 0, Cri = 0, Res = 0, Fighting = 0, LastInWorldMapId = 0, LastInWorldMapPos = "" }

--这句是重定义元表的索引，就是说有了这句，这个才是一个类
RoleOperation_SelectRoleInfoReturnProto.__index = RoleOperation_SelectRoleInfoReturnProto;

function RoleOperation_SelectRoleInfoReturnProto.New()
    local self = { }; --初始化self
    setmetatable(self, RoleOperation_SelectRoleInfoReturnProto); --将self的元表设定为Class
    return self;
end


--发送协议
function RoleOperation_SelectRoleInfoReturnProto.SendProto(proto)

    local ms = CS.LuaHelper.Instance:CreateMemoryStream();
    ms:WriteUShort(proto.ProtoCode);

    ms:WriteBool(proto.IsSuccess);
    if(proto.IsSuccess) then
        ms:WriteInt(RoldId);
        ms:WriteUTF8String(RoleNickName);
        ms:WriteByte(JobId);
        ms:WriteInt(Level);
        ms:WriteInt(TotalRechargeMoney);
        ms:WriteInt(Money);
        ms:WriteInt(Gold);
        ms:WriteInt(Exp);
        ms:WriteInt(MaxHP);
        ms:WriteInt(MaxMP);
        ms:WriteInt(CurrHP);
        ms:WriteInt(CurrMP);
        ms:WriteInt(Attack);
        ms:WriteInt(Defense);
        ms:WriteInt(Hit);
        ms:WriteInt(Dodge);
        ms:WriteInt(Cri);
        ms:WriteInt(Res);
        ms:WriteInt(Fighting);
        ms:WriteInt(LastInWorldMapId);
        ms:WriteUTF8String(LastInWorldMapPos);
        else
        ms:WriteInt(MsgCode);
    end

    CS.LuaHelper.Instance:SendProto(ms:ToArray());
    ms:Dispose();
end


--解析协议
function RoleOperation_SelectRoleInfoReturnProto.GetProto(buffer)

    local proto = RoleOperation_SelectRoleInfoReturnProto.New(); --实例化一个协议对象
    local ms = CS.LuaHelper.Instance:CreateMemoryStream(buffer);

    proto.IsSuccess = ms:ReadBool();
    if(proto.IsSuccess) then
        proto.RoldId = ms:ReadInt();
        proto.RoleNickName = ms:ReadUTF8String();
        proto.JobId = ms:ReadByte();
        proto.Level = ms:ReadInt();
        proto.TotalRechargeMoney = ms:ReadInt();
        proto.Money = ms:ReadInt();
        proto.Gold = ms:ReadInt();
        proto.Exp = ms:ReadInt();
        proto.MaxHP = ms:ReadInt();
        proto.MaxMP = ms:ReadInt();
        proto.CurrHP = ms:ReadInt();
        proto.CurrMP = ms:ReadInt();
        proto.Attack = ms:ReadInt();
        proto.Defense = ms:ReadInt();
        proto.Hit = ms:ReadInt();
        proto.Dodge = ms:ReadInt();
        proto.Cri = ms:ReadInt();
        proto.Res = ms:ReadInt();
        proto.Fighting = ms:ReadInt();
        proto.LastInWorldMapId = ms:ReadInt();
        proto.LastInWorldMapPos = ms:ReadUTF8String();
        else
        proto.MsgCode = ms:ReadInt();
    end

    ms:Dispose();
    return proto;
end