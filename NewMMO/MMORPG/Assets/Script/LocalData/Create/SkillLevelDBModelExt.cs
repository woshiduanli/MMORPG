using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public partial class SkillLevelDBModel
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="skillId"></param>
    /// <param name="skillLevel"></param>
    /// <returns></returns>
    public SkillLevelEntity GetEntityBySkillIdAndLevel(int skillId, int skillLevel)
    {
        for (int i = 0; i < m_List.Count; i++)
        {
            if (m_List[i].SkillId == skillId && m_List[i].Level == skillLevel)
            {
                return m_List[i];
            }
        }
        return null;
    }
}
