using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;

public static partial class CEvent
{
    public struct UI
    {
        public struct DisposeEvent : IEvent
        {
            public string sceneName;
            public DisposeEvent(string scene)
            {
                this.sceneName = scene;
            }
        }

        public struct LuaOpenUI : IEvent
        {

            public string ui;
            public object[] objs;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ui">  预制物品的名字 </param>
            /// <param name="objs"></param>
            public LuaOpenUI(string ui, params object[] objs)
            {

                this.ui = ui;
                this.objs = objs;
            }
        }

        public struct UIEnableEvent : IEvent
        {
            public CGameUI ui;
            public UIEnableEvent(CGameUI ui) { this.ui = ui; }
        }

        public struct UICloseEvent : IEvent
        {
            public CGameUI ui;
            public UICloseEvent(CGameUI ui) { this.ui = ui; }
        }

        public struct OpenUI : IEvent
        {
            public string UI;
            public object[] Args;
            public OpenUI(string ui, params object[] args)
            {
                this.UI = ui;
                this.Args = args;
            }
        }

        public struct ShowUI : IEvent
        {
            public string UI;
            public ShowUI(string ui)
            {
                this.UI = ui;
            }
        }

        public struct CloseUI : IEvent
        {
            public string UI;
            public CloseUI(string ui)
            {
                this.UI = ui;
            }
        }

        public struct RemoveUI : IEvent
        {
            public string UI;
            public RemoveUI(string ui)
            {
                this.UI = ui;
            }
        }

        public struct CloseSundryUINode : IEvent
        {
            //public SundryUINodeName name;
            //public CloseSundryUINode(SundryUINodeName name)
            //{
            //    this.name = name;
            //}
        }

        public struct WaitingClose : IEvent { }


        public struct BreakLoadBar : IEvent
        {
            public bool isDamage;
            public BreakLoadBar(bool isDamge = false)
            {
                this.isDamage = isDamge;
            }
        }

        public struct SkillButtonDown : IEvent
        {
            //public CSkillButton SB;
            //public BaseEventData EventData;
            //public SkillButtonDown(CSkillButton SB, BaseEventData EventData)
            //{
            //    this.SB = SB;
            //    this.EventData = EventData;
            //}
        }

        public struct SkillButtonDrag : IEvent
        {
            public BaseEventData EventData;
            public SkillButtonDrag(BaseEventData EventData)
            {
                this.EventData = EventData;
            }
        }

        public struct SkillButtonUp : IEvent
        {
            public BaseEventData EventData;
            public SkillButtonUp(BaseEventData EventData)
            {
                this.EventData = EventData;
            }
        }

        public struct HarmPrompteUIClose : IEvent
        {

        }

        public struct EudemonsOwnerHpColorChanged : IEvent
        {
            public long sn;
            public string spriteName;
            public EudemonsOwnerHpColorChanged(long sn, string spriteName)
            {
                this.sn = sn;
                this.spriteName = spriteName;
            }
        }

        /// <summary>
        /// 当CMessageboxUI关闭时处理一些事情
        /// </summary>
        public struct MessageBoxClose : IEvent { }
    }
}