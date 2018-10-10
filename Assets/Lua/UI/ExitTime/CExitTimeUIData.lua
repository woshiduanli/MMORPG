local CExitTimeUIData = {}

-- 开始时间戳
CExitTimeUIData.startTime = 0

-- 结束时间戳
CExitTimeUIData.endTime = 0

-- 是否需要星星显示功能
CExitTimeUIData.StarFunction = false

-- 获取剩余时间
CExitTimeUIData.GetRemainTime = function()
    return CExitTimeUIData.endTime - Global.severtime
end

-- 副本是否开始
CExitTimeUIData.IsStart = function(...)
    return Global.severtime - CExitTimeUIData.startTime > 0 
end

-- 暂停时间显示
CExitTimeUIData.PauseTime = false

-- 是否需要54321
CExitTimeUIData.needDownTime = false

-- 退出按钮回调
CExitTimeUIData.exitBtnClick = nil

-- 是否需要退出确定
CExitTimeUIData.needExitConfirm = true

-- exitTime 界面销毁的时候，调用此清除数据
CExitTimeUIData.ClearData = function(...)

    CExitTimeUIData.needExitConfirm = true
    CExitTimeUIData.exitBtnClick = nil
    CExitTimeUIData.needDownTime = false
    CExitTimeUIData.PauseTime = false
    CExitTimeUIData.StarFunction = false
    CExitTimeUIData.startTime = 0
    CExitTimeUIData.endTime = 0

end

return CExitTimeUIData
