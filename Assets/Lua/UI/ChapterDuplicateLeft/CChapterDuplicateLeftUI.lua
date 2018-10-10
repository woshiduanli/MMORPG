
local DuplicateData = require("World.Duplicate")
local base = GameUI
local CChapterDuplicateLeftUI = class(base)

function CChapterDuplicateLeftUI:ctor(ui)
    base.ctor(self, ui)
    print("chapterduplicateleftui ctor")
    self.Loop=true
    self.LoopDuration=0.1
    self.Layer = UIManager.Layer.MainFace
end

function CChapterDuplicateLeftUI:RegEvents()
    base.RegEvents(self)
    self:RegEvent(EventCode.Duplicate.InstanceChange,CommonUtil.GetSelfFunc(self,CChapterDuplicateLeftUI.SetUI))
end


function CChapterDuplicateLeftUI:Initialize()
    --[[self.NameTxt=self.:Get("Name",typeof(CS.UnityEngine.UI.Text))
    self.TargetDesc=self.root:Get("Target",typeof(CS.UnityEngine.UI.Text))
    self.Declare=self.root:Get("Declare",typeof(CS.UnityEngine.UI.Text))
    self.taskPanel=self.root:Get("TaskPanel",typeof(CS.UnityEngine.Transform))
    self.clickArea=self.root:Get("ClickArea",typeof(CS.UnityEngine.UI.Button))]]

    --[[ self.root:SetEvent("ClickArea",CS.UnityEngine.EventSystems.EventTriggerType.PointerClick,function ( ... )
        self:OnTaskClick()
    end) ]]

    --[[ self.root:RegEventHandler(function (o,e)
        self.doMove = true
        self.refer = e.Refer
    end,typeof(CS.CEvent.MainFace.LeftTaskAndTeamPanelDoMove))
 ]]
   --[[ self.root:RegEventHandler(function (o,e)
        self.doMove = false;
        self.taskPanel.transform.position = self.refer.position --防止最后没同步上
        self.taskPanel.gameObject:SetActive(e.IsShow)
    end,typeof(CS.CEvent.MainFace.LeftTaskAndTeamPanelStopMove)) ]]
    
    --[[ self.root:RegEventHandler(function( o,e )
        self.taskPanel.gameObject:SetActive(e.isTask)
    end,typeof(CS.CEvent.MainFace.LeftToggleChanged)) ]]

    --[[ self.root:RegEventHandler(function (o,e)
        self.taskPanel.gameObject:SetActive(e.onenable)
    end,typeof(CS.CEvent.MainFace.LeftTaskAndTeamPanelOnEnable)) ]]
    --[[ Log.debug("initialize") ]]
end

function CChapterDuplicateLeftUI:OnTaskClick()
    -- body
    --[[ local event=CS.CEvent.Auto.ActiveAttack()
    self.root:FireEvent(event) ]]
end

function CChapterDuplicateLeftUI.LoadUICallback(self)
    --[[ self.taskPanel.gameObject:SetActive(true);
    self:SetUI() ]]
    -- body
end

function CChapterDuplicateLeftUI:SetUI()
    self.TargetId=DuplicateData.curTargetId
    self.TargetRef = ReferenceManager.GetReference(GameDef.ReferenceDef.MapTarget.reference,self.TargetId)
    if(self.TargetRef) then
        self:SetGoActive(self.HashIDTable.Name,true)
        self:SetCText(self.HashIDTable.Name,self.TargetRef.Name)
        if(DuplicateData.curTargetProcess == self.TargetRef.Total) then
            self:SetCText(self.HashIDTable.Target,CSGlobal.GetLanguage("QUEST_COMPLETED_WHITE"))
        else
            local targetStr=string.format( "%s(%s/%s)",self.TargetRef.Description,DuplicateData.curTargetProcess,self.TargetRef.Total )
            self:SetCText(self.HashIDTable.Target,targetStr)
        end
        self:SetCText(self.HashIDTable.Declare,self.TargetRef.Declare)
    else
        self:SetGoActive(self.HashIDTable.Name,false)
        self:SetCText(self.HashIDTable.Target,"")
        self:SetCText(self.HashIDTable.Declare,"")
    end
end

 function  CChapterDuplicateLeftUI.OnUpdate(self)
    -- body
    base.OnUpdate(self)
    print("onupdate")
    --[[ if(self.doMove) then
        self.taskPanel.position=self.refer.position
    end]]
end

return CChapterDuplicateLeftUI