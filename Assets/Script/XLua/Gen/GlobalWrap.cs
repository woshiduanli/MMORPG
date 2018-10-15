#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class GlobalWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Global);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 22, 4, 4);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "SetEvent", _m_SetEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetCText", _m_SetCText_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetCText", _m_GetCText_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateSprite", _m_CreateSprite_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetGoActive", _m_SetGoActive_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UIClose", _m_UIClose_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "InitData", _m_InitData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CloseUIByTouch", _m_CloseUIByTouch_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetLan", _m_GetLan_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SendData", _m_SendData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Connect", _m_Connect_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RegProto", _m_RegProto_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RegProtoLua", _m_RegProtoLua_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateMemoryStream", _m_CreateMemoryStream_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SendProto", _m_SendProto_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadLevel", _m_LoadLevel_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RegLevelWasLoaded", _m_RegLevelWasLoaded_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetLocalPos", _m_SetLocalPos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateUIEvent", _m_CreateUIEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CloseUIEvent", _m_CloseUIEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RegUpdate", _m_RegUpdate_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "MaskSp", _g_get_MaskSp);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "MaskSprite", _g_get_MaskSprite);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "UIMgr", _g_get_UIMgr);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Link", _g_get_Link);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "MaskSp", _s_set_MaskSp);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "MaskSprite", _s_set_MaskSprite);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "UIMgr", _s_set_UIMgr);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "Link", _s_set_Link);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Global does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string ui = LuaAPI.lua_tostring(L, 1);
                    int linkid = LuaAPI.xlua_tointeger(L, 2);
                    int eid = LuaAPI.xlua_tointeger(L, 3);
                    int eventID = LuaAPI.xlua_tointeger(L, 4);
                    System.Action callback = translator.GetDelegate<System.Action>(L, 5);
                    
                    Global.SetEvent( ui, linkid, eid, eventID, callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetCText_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string ui = LuaAPI.lua_tostring(L, 1);
                    int linkid = LuaAPI.xlua_tointeger(L, 2);
                    int tid = LuaAPI.xlua_tointeger(L, 3);
                    string text = LuaAPI.lua_tostring(L, 4);
                    
                    Global.SetCText( ui, linkid, tid, text );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCText_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string ui = LuaAPI.lua_tostring(L, 1);
                    int linkid = LuaAPI.xlua_tointeger(L, 2);
                    int tid = LuaAPI.xlua_tointeger(L, 3);
                    
                        string __cl_gen_ret = Global.GetCText( ui, linkid, tid );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateSprite_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string ui = LuaAPI.lua_tostring(L, 1);
                    int linkid = LuaAPI.xlua_tointeger(L, 2);
                    int sid = LuaAPI.xlua_tointeger(L, 3);
                    string spname = LuaAPI.lua_tostring(L, 4);
                    string spritename = LuaAPI.lua_tostring(L, 5);
                    
                    Global.CreateSprite( ui, linkid, sid, spname, spritename );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetGoActive_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string ui = LuaAPI.lua_tostring(L, 1);
                    int linkid = LuaAPI.xlua_tointeger(L, 2);
                    int gid = LuaAPI.xlua_tointeger(L, 3);
                    int active = LuaAPI.xlua_tointeger(L, 4);
                    
                    Global.SetGoActive( ui, linkid, gid, active );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UIClose_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string ui = LuaAPI.lua_tostring(L, 1);
                    
                    Global.UIClose( ui );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitData_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    CUIManager uimgr = (CUIManager)translator.GetObject(L, 1, typeof(CUIManager));
                    
                    Global.InitData( uimgr );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseUIByTouch_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action action = translator.GetDelegate<System.Action>(L, 1);
                    System.Func<bool> checkFunc = translator.GetDelegate<System.Func<bool>>(L, 2);
                    
                    Global.CloseUIByTouch( action, checkFunc );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLan_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string str = LuaAPI.lua_tostring(L, 1);
                    
                        string __cl_gen_ret = Global.GetLan( str );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendData_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 4&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<string>>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)) 
                {
                    string url = LuaAPI.lua_tostring(L, 1);
                    System.Action<string> callBack = translator.GetDelegate<System.Action<string>>(L, 2);
                    bool isPost = LuaAPI.lua_toboolean(L, 3);
                    string json = LuaAPI.lua_tostring(L, 4);
                    
                    Global.SendData( url, callBack, isPost, json );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<string>>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    string url = LuaAPI.lua_tostring(L, 1);
                    System.Action<string> callBack = translator.GetDelegate<System.Action<string>>(L, 2);
                    bool isPost = LuaAPI.lua_toboolean(L, 3);
                    
                    Global.SendData( url, callBack, isPost );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<string>>(L, 2)) 
                {
                    string url = LuaAPI.lua_tostring(L, 1);
                    System.Action<string> callBack = translator.GetDelegate<System.Action<string>>(L, 2);
                    
                    Global.SendData( url, callBack );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string url = LuaAPI.lua_tostring(L, 1);
                    
                    Global.SendData( url );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Global.SendData!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Connect_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Global.Connect(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegProto_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    ushort protoCode = (ushort)LuaAPI.xlua_tointeger(L, 1);
                    EventDispatcher.OnLuaActionHandler action = translator.GetDelegate<EventDispatcher.OnLuaActionHandler>(L, 2);
                    
                    Global.RegProto( protoCode, action );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegProtoLua_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action<ushort, byte[]> RecServer = translator.GetDelegate<System.Action<ushort, byte[]>>(L, 1);
                    
                    Global.RegProtoLua( RecServer );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateMemoryStream_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 0) 
                {
                    
                        MMO_MemoryStream __cl_gen_ret = Global.CreateMemoryStream(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    byte[] buffer = LuaAPI.lua_tobytes(L, 1);
                    
                        MMO_MemoryStream __cl_gen_ret = Global.CreateMemoryStream( buffer );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Global.CreateMemoryStream!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendProto_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    byte[] dataArr = LuaAPI.lua_tobytes(L, 1);
                    
                    Global.SendProto( dataArr );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadLevel_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string SceneName = LuaAPI.lua_tostring(L, 1);
                    
                    Global.LoadLevel( SceneName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegLevelWasLoaded_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action<string> SceneChange = translator.GetDelegate<System.Action<string>>(L, 1);
                    
                    Global.RegLevelWasLoaded( SceneChange );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLocalPos_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string ui = LuaAPI.lua_tostring(L, 1);
                    int linkid = LuaAPI.xlua_tointeger(L, 2);
                    int tid = LuaAPI.xlua_tointeger(L, 3);
                    int x = LuaAPI.xlua_tointeger(L, 4);
                    int y = LuaAPI.xlua_tointeger(L, 5);
                    int z = LuaAPI.xlua_tointeger(L, 6);
                    
                    Global.SetLocalPos( ui, linkid, tid, x, y, z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateUIEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string ui = LuaAPI.lua_tostring(L, 1);
                    object[] args = translator.GetParams<object>(L, 2);
                    
                    Global.CreateUIEvent( ui, args );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseUIEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string ui = LuaAPI.lua_tostring(L, 1);
                    
                    Global.CloseUIEvent( ui );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegUpdate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action<float, float, float, uint> LuaUpdate = translator.GetDelegate<System.Action<float, float, float, uint>>(L, 1);
                    
                    Global.RegUpdate( LuaUpdate );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MaskSp(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, Global.MaskSp);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MaskSprite(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, Global.MaskSprite);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UIMgr(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Global.UIMgr);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Link(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Global.Link);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MaskSp(RealStatePtr L)
        {
		    try {
                
			    Global.MaskSp = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MaskSprite(RealStatePtr L)
        {
		    try {
                
			    Global.MaskSprite = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UIMgr(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Global.UIMgr = (CUIManager)translator.GetObject(L, 1, typeof(CUIManager));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Link(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Global.Link = (NGUILink)translator.GetObject(L, 1, typeof(NGUILink));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
