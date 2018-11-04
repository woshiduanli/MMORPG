
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoleMgr : Singleton<RoleMgr>
{
    bool m_IsMainPlayerInit;
    public void InitMainPlayer()
    {
        if (m_IsMainPlayerInit) return; m_IsMainPlayerInit = true;

        if (GlobalInit.Instance.MainPlayerInfo != null)
        {
            GameObject mainPlayerObj = Object.Instantiate(GlobalInit.Instance.JobObjectDic[GlobalInit.Instance.MainPlayerInfo.JobId]);
            Object.DontDestroyOnLoad(mainPlayerObj);
            GlobalInit.Instance.CurrPlayer = mainPlayerObj.GetComponent<RoleCtrl>();
            GlobalInit.Instance.CurrPlayer.
                Init(RoleType.MainPlayer, GlobalInit.Instance.MainPlayerInfo, new RoleMainPlayerCityAI(GlobalInit.Instance.CurrPlayer));
        }

    }

    #region LoadRole 根据角色预设名称 加载角色
    /// <summary>
    /// 根据角色预设名称 加载角色
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject LoadRole(string name, RoleType type)
    {
        string path = string.Empty;

        switch (type)
        {
            case RoleType.MainPlayer:
                path = "Player";
                break;
            case RoleType.Monster:
                path = "Monster";
                break;
        }

        return ResourcesMgr.Instance.Load(ResourcesMgr.ResourceType.Role, string.Format("{0}/{1}", path, name), cache: true);
    }
    #endregion

    public GameObject LoadNPC(string prefabName)
    {
        GameObject obj = AssetBundleMgr.Instance.Load("role/" + prefabName.ToLower() + ".assetbundle", prefabName);
        return GameObject.Instantiate(obj);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}