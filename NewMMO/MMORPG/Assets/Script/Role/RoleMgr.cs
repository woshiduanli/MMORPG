
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

            // 设置角色物理攻击
            GlobalInit.Instance.MainPlayerInfo.SetPhySkilId (JobDBModel.Instance.Get(GlobalInit.Instance.MainPlayerInfo.JobId).UsedPhyAttackIds);
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
        GameObject obj = AssetBundleMgr.Instance.Load("Role/" + prefabName.ToLower() + ".assetbundle", prefabName);
        return GameObject.Instantiate(obj);
    }

   public Sprite  LoadSkillPic (string name){
       return null; 
   }

    public GameObject LoadSprite(int spriteId)
    {

        SpriteEntity s = SpriteDBModel.Instance.Get(spriteId);
        if (s == null) return null;
        //if (s.IsBoss == 1) return null; 
        return AssetBundleMgr.Instance.Load("role/" + s.PrefabName.ToLower() + ".assetbundle", s.PrefabName);
    }

    public GameObject LoadPlayer(int JobId)
    {
        GameObject obj = GlobalInit.Instance.JobObjectDic[JobId];
        return Object.Instantiate(obj);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}