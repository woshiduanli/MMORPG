using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelSceneCtrl : GameSceneCtrlbase
{
    [SerializeField]
    GameLevelRegionCtrl[] AllRegion;

    List<GameLevelRegionEntity> m_regionList;

    int m_curRegionIndex;

    int m_CurrGameLevelId;


    protected override void OnLoadUIMainCityViewComplete(GameObject obj)
    {
        m_CurrGameLevelId = SceneMgr.Instance.CurGameLevelId;
        base.OnLoadUIMainCityViewComplete(obj);

        //if (AllRegion != null) GlobalInit.Instance.CurrPlayer.transform.position = AllRegion[0].RoleBornPos.position; 

        // 

        m_regionList = GameLevelRegionDBModel.Instance.GetListByGameLevelId(m_CurrGameLevelId);
        m_curRegionIndex = 0;
        EnterRegion(m_curRegionIndex);


    }

    void EnterRegion(int regionIndex)
    {
        // 
        GameLevelRegionEntity entity = GetGameLevelRegionEntityByIndex(regionIndex);
        GameLevelRegionCtrl regionCtrl = null;
        if (entity != null)
            regionCtrl = GetRegionCtrlByRegionId(entity.RegionId);
        if (regionCtrl == null) return;

        if (regionIndex == 0)
            if (AllRegion != null && GlobalInit.Instance.CurrPlayer != null) GlobalInit.Instance.CurrPlayer.transform.position = regionCtrl.RoleBornPos.position;

        if (DelegateDefine.Instance.OnSceneLoadOk != null)
            DelegateDefine.Instance.OnSceneLoadOk();
    }

    GameLevelRegionCtrl GetRegionCtrlByRegionId(int regionId)
    {
        if (AllRegion != null)
            for (int i = 0; i < AllRegion.Length; i++)
            {
                if (AllRegion[i].RegionId == regionId)
                {
                    return AllRegion[i];
                }
            }
        return null;
    }

    GameLevelRegionEntity GetGameLevelRegionEntityByIndex(int index)
    {
        if (m_regionList != null && m_regionList.Count > index)
            return m_regionList[index];
        return null;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }
}
