
using UnityEngine;
using System.Collections;

public partial class GameLevelGradeDBModel
{
    /// <summary>
    /// 根据关卡编号和难度等级 获取实体
    /// </summary>
    /// <param name="gameLevelId"></param>
    /// <param name="grade"></param>
    /// <returns></returns>
    public GameLevelGradeEntity GetEntityByGameLevelIdAndGrade(int gameLevelId, GameLevelGrade grade)
    {
        for (int i = 0; i < m_List.Count; i++)
        {
            if (m_List[i].GameLevelId == gameLevelId && m_List[i].Grade == (int)grade)
            {
                return m_List[i];
            }
        }
        return null;
    }
}