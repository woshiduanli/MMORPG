--客户端发送物品加入背包消息
Backpack_GoodsAddProto = { ProtoCode = 16004, GoodsInType = 0, GoodsKindCount = 0, ItemTable = { } }

--这句是重定义元表的索引，就是说有了这句，这个才是一个类
Backpack_GoodsAddProto.__index = Backpack_GoodsAddProto;

function Backpack_GoodsAddProto.New()
    local self = { }; --初始化self
    setmetatable(self, Backpack_GoodsAddProto); --将self的元表设定为Class
    return self;
end


--定义物品种类
Item = { GoodsType = 0, GoodsId = 0, GoodsCount = 0 }
Item.__index = Item;
function Item.New()
    local self = { };
    setmetatable(self, Item);
    return self;
end


--发送协议
function Backpack_GoodsAddProto.SendProto(proto)

    local ms = CS.LuaHelper.Instance:CreateMemoryStream();
    ms:WriteUShort(proto.ProtoCode);

    ms:WriteByte(proto.GoodsInType);
    ms:WriteInt(proto.GoodsKindCount);
    for i = 1, proto.GoodsKindCount, 1 do
        ms:WriteByte(ItemList[i].GoodsType);
        ms:WriteInt(ItemList[i].GoodsId);
        ms:WriteInt(ItemList[i].GoodsCount);
    end

    CS.LuaHelper.Instance:SendProto(ms:ToArray());
    ms:Dispose();
end


--解析协议
function Backpack_GoodsAddProto.GetProto(buffer)

    local proto = Backpack_GoodsAddProto.New(); --实例化一个协议对象
    local ms = CS.LuaHelper.Instance:CreateMemoryStream(buffer);

    proto.GoodsInType = ms:ReadByte();
    proto.GoodsKindCount = ms:ReadInt();
    for i = 1, proto.GoodsKindCount, 1 do
        local _Item = Item.New();
        _Item.GoodsType = ms:ReadByte();
        _Item.GoodsId = ms:ReadInt();
        _Item.GoodsCount = ms:ReadInt();
        proto.ItemTable[#proto.ItemTable+1] = _Item;
    end

    ms:Dispose();
    return proto;
end