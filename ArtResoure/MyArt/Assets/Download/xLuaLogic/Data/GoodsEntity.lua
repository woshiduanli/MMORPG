--这个实体是扩展的 物品实体

GoodsEntity = { Id = 0, ShopCategoryId = 0, GoodsType = 0, GoodsId = 0, OldPrice = 0, Price = 0, SellStatus = 0, Name="", UsedLevel=0, UsedMethod="", Quality=0, Description="" }

--这句是重定义元表的索引，就是说有了这句，这个才是一个类
GoodsEntity.__index = GoodsEntity;

function GoodsEntity.New(Id, ShopCategoryId, GoodsType, GoodsId, OldPrice, Price, SellStatus, Name, UsedLevel, UsedMethod, Quality, Description)
    local self = { }; --初始化self
    setmetatable(self, GoodsEntity); --将self的元表设定为Class

    self.Id = Id;
    self.ShopCategoryId = ShopCategoryId;
    self.GoodsType = GoodsType;
    self.GoodsId = GoodsId;
    self.OldPrice = OldPrice;
    self.Price = Price;
    self.SellStatus = SellStatus;
	self.Name = Name;
	self.UsedLevel = UsedLevel;
	self.UsedMethod = UsedMethod;
	self.Quality = Quality;
	self.Description = Description;

    return self;
end