local CCommonConfirmUIData = {}

CCommonConfirmUIData.DefaultTipStr = "是否退出，退出不会获得奖励"

-- 确认的回调
CCommonConfirmUIData.SureBtnCallBack = nil

CCommonConfirmUIData.SetSureCallBack = function(CallBackflag)
    if (CallBackflag == "ShortTreasure") then
        CCommonConfirmUIData.SureBtnCallBack = function()
            CSGlobal.Request(GameDef.EventTag.ExitDuplicate)
        end
    end
end

-- 关闭界面恢复初始数据
CCommonConfirmUIData.ReSetData = function()
    CCommonConfirmUIData.SureBtnCallBack = function()
        CSGlobal.Request(GameDef.EventTag.ExitDuplicate)
    end
    CCommonConfirmUIData.DefaultTipStr = "是否退出，退出不会获得奖励"
end

return CCommonConfirmUIData
