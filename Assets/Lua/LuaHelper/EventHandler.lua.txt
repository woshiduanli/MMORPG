local EventHandler = {}
EventHandler.mEventTable = {}

-- 默认调用函数
function EventHandler.mPreInvokeFunc( EventName, Func, Object, UserData, ... )
    if Object then
        Func( Object, EventName, ... )
    else
        Func( EventName, ... )
    end

end

-- 添加
function EventHandler:RegEventHandler(EventName, Func, Object, UserData )

    assert( Func )

     self.mEventTable[ EventName ] = self.mEventTable[ EventName ] or {}

    local Event = self.mEventTable[ EventName ]

    if not Object then
        Object = "_StaticFunc"
    end

    Event[Object] = Event[Object] or {}
    local ObjectEvent = Event[Object]

    ObjectEvent[Func] = UserData or true

end


-- 派发
function EventHandler:FireEvent(EventName, ... )

    assert( EventName )

    local Event = self.mEventTable[ EventName ]

    for Object,ObjectFunc in pairs( Event ) do

        if Object == "_StaticFunc" then

            for Func, UserData in pairs( ObjectFunc ) do
                self.mPreInvokeFunc( EventName, Func, nil, UserData, ... )
            end

        else

            for Func, UserData in pairs( ObjectFunc ) do
                self.mPreInvokeFunc( EventName, Func, Object, UserData, ... )
            end

        end

    end

end

-- 清除
function EventHandler:UnRegEventHandler(EventName, Func, Object )

    assert( Func )

    local Event = self.mEventTable[ EventName ]

    if not Event then
        return
    end

    if not Object then
        Object = "_StaticFunc"
    end


    local ObjectEvent = Event[Object]

    if not ObjectEvent then
        return
    end

    ObjectEvent[Func] = nil


end

-- 清除对象的所有回调
function EventHandler:UnRegAllEventHandler( EventName, Object )

    assert( Object )

    local Event = self.mEventTable[ EventName ]

    if not Event then
        return
    end

    Event[Object] = nil

end

return EventHandler