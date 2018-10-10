/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
 */

using System;
using ZCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LitJson;
using UnityEngine;
using XLua;

//配置的详细介绍请看Doc下《XLua的配置.doc》
public static class GenConfig
{
    //lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
    [LuaCallCSharp]
    private static List<Type> luaCallCSharp = new List<Type>()
    {
        typeof(Global),
    };

    //C#静态调用Lua的配置（包括事件的原型），仅可以配delegate，interface
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>()
    {
        typeof (System.Action< string, System.Object[]>),
        typeof (System.Action<float, float, float, uint>),
        typeof (System.Action<  System.Object[]>),
        typeof (System.Action<string>),
        typeof (System.Action),
        typeof (Action<LuaTable,int>),
        typeof (Action<LuaTable>),
        typeof (Action<string, string>),
        typeof (Action<string>),
        typeof (Action<string, string, System.Object[]>),
        typeof (Action<float, float, int, uint>),
    };

    //黑名单
    [BlackList]
    public static List<List<string>> BlackList = new List<List<string>>()
    {
        new List<string>()
        {
        "UnityEngine.WWW",
        "movie"
        },
#if UNITY_WEBGL
        new List<string>()
        {
        "UnityEngine.WWW",
        "threadPriority"
        },
#endif
        new List<string>()
        {
        "UnityEngine.Texture2D",
        "alphaIsTransparency"
        },
        new List<string>()
        {
        "UnityEngine.Security",
        "GetChainOfTrustValue"
        },
        new List<string>()
        {
        "UnityEngine.CanvasRenderer",
        "onRequestRebuild"
        },
        new List<string>()
        {
        "UnityEngine.Light",
        "areaSize"
        },
        new List<string>()
        {
        "UnityEngine.AnimatorOverrideController",
        "PerformOverrideClipListCleanup"
        },
#if !UNITY_WEBPLAYER
        new List<string>()
        {
        "UnityEngine.Application",
        "ExternalEval"
        },
#endif
        new List<string>()
        {
        "UnityEngine.GameObject",
        "networkView"
        }, //4.6.2 not support
        new List<string>()
        {
        "UnityEngine.Component",
        "networkView"
        }, //4.6.2 not support
        new List<string>()
        {
        "System.IO.FileInfo",
        "GetAccessControl",
        "System.Security.AccessControl.AccessControlSections"
        },
        new List<string>()
        {
        "System.IO.FileInfo",
        "SetAccessControl",
        "System.Security.AccessControl.FileSecurity"
        },
        new List<string>()
        {
        "System.IO.DirectoryInfo",
        "GetAccessControl",
        "System.Security.AccessControl.AccessControlSections"
        },
        new List<string>()
        {
        "System.IO.DirectoryInfo",
        "SetAccessControl",
        "System.Security.AccessControl.DirectorySecurity"
        },
        new List<string>()
        {
        "System.IO.DirectoryInfo",
        "CreateSubdirectory",
        "System.String",
        "System.Security.AccessControl.DirectorySecurity"
        },
        new List<string>()
        {
        "System.IO.DirectoryInfo",
        "Create",
        "System.Security.AccessControl.DirectorySecurity"
        },
        new List<string>()
        {
        "UnityEngine.MonoBehaviour",
        "runInEditMode"
        },
    };

    [Hotfix]
    public static List<Type> Hotfix
    {
        get
        {
            List<Type> types = new List<Type>();
            Assembly assembly = Assembly.Load("Assembly-CSharp");
            types.AddRange(assembly.GetExportedTypes()
                .Where(type =>
                    (type.IsSubclassOf(typeof(CGameUI))) ||
                    //type.IsSubclassOf(typeof(CActivity)) ||
                    type.IsSubclassOf(typeof(CUIElement)) ||
                    type.IsSubclassOf(typeof(ZRender.IRenderObject)) ||
                    type.IsSubclassOf(typeof(CObject)))
                //type.IsSubclassOf(typeof(CWorld)))
                .Where(type => type.IsClass)
                .Where(type => !type.IsGenericTypeDefinition)
                .Distinct().ToList());

            List<Type> temps = new List<Type>();
            for (int i = 0; i < types.Count; i++)
            {
                //if (types[i] == typeof(XLuaManager))
                //    continue;
                Type type = types[i];
                ColletHotFixType(ref temps, type);
            }
            return temps;
        }
    }

    public static List<Type> LuaCallCSharp
    {
        get
        {
            return luaCallCSharp;
        }

        set
        {
            luaCallCSharp = value;
        }
    }

    static void ColletHotFixType(ref List<Type> collets, Type type)
    {
        if (type.IsGenericTypeDefinition || !type.IsClass || type.IsAbstract)
            return;
        if (type.BaseType != null && type.BaseType.IsAbstract && type.BaseType.IsGenericType)
            return;
        if (!collets.Contains(type))
        {
            collets.Add(type);
        }
        Type[] types = type.GetNestedTypes();
        foreach (var gt in types)
        {
            ColletHotFixType(ref collets, gt);
        }
    }
}