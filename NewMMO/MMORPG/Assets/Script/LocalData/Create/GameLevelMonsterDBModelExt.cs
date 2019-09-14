
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class GameLevelMonsterDBModel
{
    #region GetGameLevelMonsterCount 根据游戏关卡编号和难度等级获取怪的总数量
    /// <summary>
    /// 根据游戏关卡编号和难度等级获取怪的总数量
    /// </summary>
    /// <param name="gameLevelId"></param>
    /// <param name="grade">难度等级</param>
    /// <returns></returns>
    public int GetGameLevelMonsterCount(int gameLevelId, GameLevelGrade grade)
    {
        int count = 0;
        for (int i = 0; i < m_List.Count; i++)
        {
            if (m_List[i].GameLevelId == gameLevelId && m_List[i].Grade == (int)grade)
            {
                count += m_List[i].SpriteCount;
            }
        }
        return count;
    }
    #endregion

    #region GetGameLevelMonsterId 根据游戏关卡编号和难度等级获取怪的种类
    /// <summary>
    /// 根据游戏关卡编号和难度等级获取怪的种类
    /// </summary>
    /// <param name="gameLevelId"></param>
    /// <param name="grade"></param>
    /// <returns></returns>
    public int[] GetGameLevelMonsterId(int gameLevelId, GameLevelGrade grade)
    {
        List<int> lst = new List<int>();

        for (int i = 0; i < m_List.Count; i++)
        {
            if (m_List[i].GameLevelId == gameLevelId && m_List[i].Grade == (int)grade)
            {
                if (!lst.Contains(m_List[i].SpriteId))
                {
                    lst.Add(m_List[i].SpriteId);
                }
            }
        }
        return lst.ToArray();
    }
    #endregion



    /// <summary>
    /// 根据游戏关卡编号和难度等级和区域编号获取怪的总数量
    /// </summary>
    /// <param name="gameLevelId"></param>
    /// <param name="grade"></param>
    /// <param name="regionId"></param>
    /// <returns></returns>
    public int GetGameLevelMonsterCount(int gameLevelId, GameLevelGrade grade, int regionId)
    {
        int count = 0;
        for (int i = 0; i < m_List.Count; i++)
        {
            //if (m_List[i].SpriteId==1003)
            if (m_List[i].GameLevelId == gameLevelId && m_List[i].Grade == (int)grade && m_List[i].RegionId == regionId)
            {


                count += m_List[i].SpriteCount;
            }
        }
        return count;
    }

    private List<GameLevelMonsterEntity> retLst = new List<GameLevelMonsterEntity>();

    /// <summary>
    /// 根据游戏关卡编号和难度等级和区域编号获取所有怪
    /// </summary>
    /// <param name="gameLevelId"></param>
    /// <param name="grade"></param>
    /// <param name="regionId"></param>
    /// <returns></returns>
    public List<GameLevelMonsterEntity> GetGameLevelMonster(int gameLevelId, GameLevelGrade grade, int regionId)
    {
        retLst.Clear();
        for (int i = 0; i < m_List.Count; i++)
        {
            if (m_List[i].GameLevelId == gameLevelId && m_List[i].Grade == (int)grade && m_List[i].RegionId == regionId)
            {
                retLst.Add(m_List[i]);
            }
        }
        return retLst;
    }

    //Dictionary<int, int> retDic = new Dictionary<int, int>();
    //public Dictionary<int, int> GetGameLevelMonster(int gameLevelId, GameLevelGrade grade, int regionId)
    //{
    //    retDic.Clear();
    //    for (int i = 0; i < m_List.Count; i++)
    //    {
    //        if (m_List[i].GameLevelId == gameLevelId && m_List[i].Grade == (int)grade && m_List[i].RegionId == regionId)
    //        {
    //            if (!retDic.ContainsKey(m_List[i].SpriteId))
    //            {
    //                retDic.Add(m_List[i].SpriteId, m_List[i].SpriteCount);
    //            }
    //        }
    //    }
    //    return retDic;
    //}

    /// <summary>
    /// 获取游戏关卡区域怪实体
    /// </summary>
    /// <param name="gameLevelId"></param>
    /// <param name="grade"></param>
    /// <param name="regionId"></param>
    /// <param name="monsterId"></param>
    /// <returns></returns>
    public GameLevelMonsterEntity GetGameLevelMonsterEntity(int gameLevelId, GameLevelGrade grade, int regionId, int monsterId)
    {
        for (int i = 0; i < m_List.Count; i++)
        {
            if (m_List[i].GameLevelId == gameLevelId && m_List[i].Grade == (int)grade && m_List[i].RegionId == regionId && m_List[i].SpriteId == monsterId)
            {
                return m_List[i];
            }
        }
        return null;
    }
}