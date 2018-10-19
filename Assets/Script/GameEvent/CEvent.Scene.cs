using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IEvent { }
public static partial class CEvent
{
    public struct Scene
    {
        public struct LoadLevelBegin : IEvent
        {
            public string sceneName;
            public LoadLevelBegin(string name)
            {
                this.sceneName = name;
            }
        }

        public struct LuaLoadLevel : IEvent
        {
            public string json;

            public LuaLoadLevel(string json)
            {
                this.json = json;
            }
        }


        //场景加载完毕事件
        public struct LevelWasLoaded : IEvent
        {
            public string sceneName;
            public string targetscene;
            public LevelWasLoaded(string name, string target)
            {
                sceneName = name;
                targetscene = target;
            }
        }

        public struct LoadLevel : IEvent
        {
            public string sceneName;
            public List<string> Assets;

            /// <summary>
            /// 是否等待配置表加载完成
            /// </summary>
            public bool WaitRef;
            public LoadLevel(string name)
            {
                this.sceneName = name;
                this.Assets = null;
                this.WaitRef = false;
            }

            public LoadLevel(string name, List<string> assets, bool wait = false)
            {
                this.sceneName = name;
                this.Assets = assets;
                this.WaitRef = wait;
            }
        }

        public struct LoadSceneBegin : IEvent
        {
            public string sceneName;
            public LoadSceneBegin(string name)
            {
                this.sceneName = name;
            }
        }
    }
}
