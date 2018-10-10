
local EventCode = { }

EventCode.UI = 
{ 
	OpenUI = 1,    -- 加载显示UI
    CloseUI = 2,-- 隐藏UI
    DisposeEvent = 3, --销毁所有UI
}

EventCode.Scene = 
{
    LevelWasLoaded = 10,
}

EventCode.FreshGuide = 
{
    Begin = 20,
}

EventCode.Activity = 
{
    DataChange = 30,
    RewardedDataChange = 31,
    LikeDataUpdate = 32,
}

EventCode.MainPlayer = 
{
    ReadData = 40,
}

EventCode.RedDot = 
{
    Refresh = 50,
}

EventCode.SignIn = 
{
    RetrieveActivityUpdateRoleData = 60,
}

EventCode.Property = 
{
    LEVEL = 70,
    HP = 71,
    MaxHP = 72,
    BODYSTATE = 73,
    MOUNTING = 74,
    RANK = 75,
    CommonChange = 76,
}

EventCode.Group = 
{
    OnPropertyGroupChange = 80,
    OnPropertyGroupIdChange = 81,
}

EventCode.Title = 
{
    TitleChange = 90,
}

EventCode.Appellation = 
{
    AppellationChange = 100,
}

EventCode.PlayerInfo = 
{
    PlayerNameChanged = 110,
    PlayerGroupInfoChanged = 111,
    OnPropertyTeamIdChange = 112,
}

EventCode.Nobility = 
{
    NobilityOrPrestigeChanged = 120,
}

EventCode.NetWork = 
{
    NetConnect = 130,
    NetDisConnect = 131,
}

EventCode.Battleground = {
	UpdateLeftData = 140,
	ToFinish = 141
}

EventCode.ShortTreasure = {
	UpdateCurScore = 151,
	UpdateRewardLevel = 152,
}

EventCode.Duplicate={
    InstanceChange=160,
    BattleResult=161,
}

EventCode.MindQuiz = {
    AddAnswerBarrage = 170,
    OpenSettlementPanel = 171,
    AnnouncingAnswer = 172,
    ChooseAnswer = 173,
    UpdateTopThree = 174,
    RefreshUI = 175,
    RefreshEndTopThree = 176,
    BeginAnswer = 177,
}

return EventCode