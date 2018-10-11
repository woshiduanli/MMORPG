local base = GameUI
local CLoginUI = class(base)
local ObjectManager = ObjectManager

function CLoginUI:ctor(ui)
    base.ctor(self, ui)
    self.isFullScreen = true
    self.Layer = UIManager.Layer.FullWindow
end

function CLoginUI:Initialize()
    self.CurPage = "Login"
    self:ChangePage()
    self:SetEvent(
        self.HashIDTable.btn_denglu,
        UIManager.TriggerEventID.PointerClick,
        self:GetSelfFunc(CLoginUI.OnClickLogin)
    )
    self:SetEvent(
        self.HashIDTable.btn_zhuce,
        UIManager.TriggerEventID.PointerClick,
        self:GetSelfFunc(CLoginUI.OnClickRegister)
    )
end

function CLoginUI:ChangePage()
    if self.CurPage == "Login" then
        -- 设置登录页面
        self:SetCText(self.HashIDTable.titaltext, Language.YONGHUDEGNLU)
        self:SetCText(self.HashIDTable.zhuceText, Language.YONGHUDEGNLU3)
        self:SetCText(self.HashIDTable.dengluText, Language.YONGHUDEGNLU4)

        self:SetGoActive(self.HashIDTable.YaoQingMa, false)
    else
        -- 设置注册页面
        self:SetCText(self.HashIDTable.titaltext, Language.YONGHUDZHUCE)
        self:SetCText(self.HashIDTable.zhuceText, Language.YONGHUDEGNLU2)
        self:SetCText(self.HashIDTable.dengluText, Language.YONGHUDEGNLU1)

        self:SetGoActive(self.HashIDTable.YaoQingMa, true)
    end
end

function CLoginUI:OnClickLogin()
    local data = ObjectManager.GetSingleObj("LoginSystemData")
    data.ClickPanel = "Login"

    if self.CurPage ~= "Login" then
        self.CurPage = "Login"
        self:ChangePage()
    else
        local data = {}
        data.Type = 1
        data.UserName = self:GetCText(self.HashIDTable.nichegnText)
        data.ChannelId = "22"
        data.Pwd = self:GetCText(self.HashIDTable.mimaText)

        local login = ObjectManager.GetSingleObj("LoginSystemData")
        CSGlobal.SendData(login.url, login:GetSelfFunc( login.CallBack ), true, Json.encode(data))
    end
end

function CLoginUI:OnClickRegister()
    local data = ObjectManager.GetSingleObj("LoginSystemData")
    data.ClickPanel = "Register"

    if self.CurPage ~= "Register" then
        self.CurPage = "Register"
        self:ChangePage()
    else
        local data = {}
        data.Type = 0
        data.UserName = self:GetCText(self.HashIDTable.nichegnText)
        data.ChannelId = "22"
        data.Pwd = self:GetCText(self.HashIDTable.mimaText)
        local login = ObjectManager.GetSingleObj("LoginSystemData")
        CSGlobal.SendData(login.url, login:GetSelfFunc( login.CallBack ), true, Json.encode(data))
    end
end

function CLoginUI:OnUIEnable()
    print("onuienable")
end

function CLoginUI.LoadUICallback(self)
end

function CLoginUI.OnDestroy(self)
end

return CLoginUI
