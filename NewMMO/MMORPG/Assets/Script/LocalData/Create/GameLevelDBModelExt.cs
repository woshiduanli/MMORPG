
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class GameLevelDBModel
{
    private List<GameLevelEntity> m_RetList = new List<GameLevelEntity>();

    /// <summary>
    /// 根据章编号获取关卡集合
    /// </summary>
    /// <returns></returns>
    public List<GameLevelEntity> GetListByChapterId(int chapterId)
    {
        if (m_List == null || m_List.Count == 0) return null;
        m_RetList.Clear();

        for (int i = 0; i < m_List.Count; i++)
        {
            if (m_List[i].ChapterID == chapterId)
            {
                m_RetList.Add(m_List[i]);
            }
        }

        return m_RetList;
    }
}