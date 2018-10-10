
local Base = require "World.World"
local Duplicate = class(Base)

function Duplicate:InitData(mr)
       print(" duplicate initdata")
    Base.InitData(self,mr)
    self.startTimeStamp=0
    self.finishTimeStamp=0    
    self.curTargetId=0
    self.curTargetProcess=0
    self.isStartTime=false
end


function Duplicate:RegEvents( )
    print("duplicate regevents")
    Base.RegEvents(self)
    NetManager.RegEvent(GameDef.NetEvent.Event,EventTag.Duplicate.BattleResult,CommonUtil.GetSelfFunc(self,self.OnBattleResult))
    NetManager.RegEvent(GameDef.NetEvent.Event,EventTag.Duplicate.TargetChange,Duplicate.OnTargetChange)
end


function Duplicate:OnBattleResult( data )
    print("onbattleresult")
end

function Duplicate.OnTargetChange(sn,data)
    print("ontargetchange")
    Duplicate.curTargetId=data.id
    Duplicate.curTargetProcess=data.current
    Duplicate:FireEvent(EventCode.Duplicate.InstanceChange)
end

return  Duplicate