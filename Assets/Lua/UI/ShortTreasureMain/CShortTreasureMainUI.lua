local base = GameUI
local CShortTreasureMainUI = class(base)

function CShortTreasureMainUI:ctor(ui)
    base.ctor(self, ui)
    self.IsFullScreen = true
    self.Layer = UIManager.Layer.FullWindow
end

function CShortTreasureMainUI.Initialize(self)
    print("Initialize")
    self.ItemList = {}
    self:SetEvent(self.HashIDTable.Close, UIManager.EID.PointerClick, self:GetSelfFunc(self.Close))
    self:SetEvent(self.HashIDTable.BtnJoin,UIManager.EID.PointerClick,self:GetSelfFunc(self.ReqPlayerEnter))

    -- 测试赋值item
    -- local uie =
    --     self:AddElement(
    --     self.HashIDTable.RewardItem.HashID,
    --     "UI.ShortTreasureMain.CRewardItemElement.CRewardItemElement"
    -- )

    -- uie:SetActive(0)

    -- self:SetCText(self.HashIDTable.JoinPersonNumText, "ceshi le ")
    -- for i = 1, 10 do
    --     -- print(i)
    --     local cloneObj = uie:Clone()
    --     cloneObj:SetActive(1)
    --     cloneObj:SetData(0)
    -- end

    -- for i = 1, 10 do
    --     print(i)
    -- end
end

function CShortTreasureMainUI:ReqPlayerEnter()
    CSGlobal.Request(GameDef.EventTag.ShortTreasure.ReqPlayerEnter)
end

function CShortTreasureMainUI.UIEnable(self)
    print("CShortTreasureMainUI.UIEnable")
    self.Loop = true
    base.UIEnable(self)
    CShortTreasureMainUI.ShowReward(self)
end

function CShortTreasureMainUI.OnUpdate()
end 

function CShortTreasureMainUI.ShowReward(self)
    local data = ReferenceManager.GetReference("activityexhibition", 117)

    -- if data ~= nil then
    --     local num = CClientCommon.GetTableLength(data.Rewards) - CClientCommon.GetTableLength(curSelf.ListRewardItem)
    --     if num > 0 then
    --         curSelf.ListRewardItem = CClientCommon.Clone(curSelf.RewardItem, num)
    --     end

    --     for k, v in pairs(curSelf.ListRewardItem) do
    --         CS.CClientCommon.SetActiveOverload(v.gameObject, false)
    --     end

    --     for k, v in pairs(data.Rewards) do
    --         if v ~= nil then
    --             curSelf.ListRewardItem[tonumber(k)]:SetItem(v)
    --             curSelf.ListRewardItem[tonumber(k)].tipsBtnsEnable = false
    --         end
    --     end
    -- end
end

function CShortTreasureMainUI.LoadUICallback(self)
end

function CShortTreasureMainUI.OnDestroy(self)
end

return CShortTreasureMainUI
