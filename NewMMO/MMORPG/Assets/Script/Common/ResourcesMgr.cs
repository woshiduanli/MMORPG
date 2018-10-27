
using UnityEngine;
using System.Collections;
using System.Text;

public class ResourcesMgr : Singleton<ResourcesMgr>
{
    #region ResourceType 资源类型
    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ResourceType
    {
        /// <summary>
        /// 场景UI
        /// </summary>
        UIScene,
        /// <summary>
        /// 窗口
        /// </summary>
        UIWindow,
        /// <summary>
        /// 角色
        /// </summary>
        Role,
        /// <summary>
        /// 特效
        /// </summary>
        Effect,
        /// <summary>
        /// 其他
        /// </summary>
        UIOther
    }
    #endregion

    /// <summary>
    /// 预设的列表
    /// </summary>
    private Hashtable m_PrefabTable;

    public ResourcesMgr()
    {
        m_PrefabTable = new Hashtable();
    }

    #region Load 加载资源
    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="type">资源类型</param>
    /// <param name="path">短路径</param>
    /// <param name="cache">是否放入缓存</param>
    /// <returns>预设克隆体</returns>
    public GameObject Load(ResourceType type, string path, bool cache = false)
    {
        GameObject obj = null;
        if (m_PrefabTable.Contains(path))
        {
            obj = m_PrefabTable[path] as GameObject;
        }
        else
        {
            StringBuilder sbr = new StringBuilder();
            switch (type)
            {
                case ResourceType.UIScene:
                    sbr.Append("UIPrefab/UIScene/");
                    break;
                case ResourceType.UIWindow:
                    sbr.Append("UIPrefab/UIWindows/");
                    break;
                case ResourceType.Role:
                    sbr.Append("RolePrefab/");
                    break;
                case ResourceType.Effect:
                    sbr.Append("EffectPrefab/");
                    break;
                case ResourceType.UIOther:
                    sbr.Append("UIPrefab/UIOther/");
                    break;
            }

            sbr.Append(path);
            string str33 = sbr.ToString();
            MyDebug.debug(str33);
            obj = Resources.Load(str33) as GameObject;
            if (cache)
            {
                m_PrefabTable.Add(path, obj);
            }
        }
        if (obj == null)
        {
            MyDebug.debug("resourece not find:" + path);
            return new UnityEngine.GameObject();
        }
        return Object.Instantiate(obj);
    }
    #endregion

    #region Dispose 释放资源
    /// <summary>
    /// 释放资源
    /// </summary>
    public override void Dispose()
    {
        base.Dispose();

        m_PrefabTable.Clear();

        //把未使用的资源进行释放
        Resources.UnloadUnusedAssets();
    }
    #endregion
}