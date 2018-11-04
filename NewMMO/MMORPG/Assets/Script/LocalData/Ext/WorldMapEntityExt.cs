
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class WorldMapEntity
{
    private Vector3 m_RoleBirthPostion = Vector3.zero;

    /// <summary>
    /// 主角的出生点坐标
    /// </summary>
    public Vector3 RoleBirthPostion
    {
        get
        {
            if (m_RoleBirthPostion == Vector3.zero)
            {
                string[] arr = RoleBirthPos.Split('_');
                if (arr.Length < 3)
                {
                    return Vector3.zero;
                }

                float x = 0, y = 0, z = 0;

                float.TryParse(arr[0], out x);
                float.TryParse(arr[1], out y);
                float.TryParse(arr[2], out z);

                m_RoleBirthPostion = new Vector3(x, y, z);
            }
            return m_RoleBirthPostion;
        }
    }

    private float m_RoleBirthEulerAnglesY = -1;
    /// <summary>
    /// Y周的旋转
    /// </summary>
    public float RoleBirthEulerAnglesY
    {
        get
        {
            if (m_RoleBirthEulerAnglesY == -1)
            {
                string[] arr = RoleBirthPos.Split('_');
                if (arr.Length < 4)
                {
                    return 0;
                }
                float y = 0;
                float.TryParse(arr[0], out y);

                m_RoleBirthEulerAnglesY = y;
            }
            return m_RoleBirthEulerAnglesY;
        }
    }

    private List<NPCWorldMapData> m_NPCWorldMapList;

    /// <summary>
    /// 世界地图场景里的NPC列表
    /// </summary>
    public List<NPCWorldMapData> NPCWorldMapList
    {
        get
        {
            if (m_NPCWorldMapList == null)
            {
                m_NPCWorldMapList = new List<NPCWorldMapData>();

                string[] arr1 = NPCList.Split('|');

                for (int i = 0; i < arr1.Length; i++)
                {
                    string[] arr2 = arr1[i].Split('_');

                    if (arr2.Length < 6)
                    {
                        break;
                    }

                    int npcId = 0;
                    int.TryParse(arr2[0], out npcId);

                    float x = 0, y = 0, z = 0, anglesY = 0;
                    float.TryParse(arr2[1], out x);
                    float.TryParse(arr2[2], out y);
                    float.TryParse(arr2[3], out z);
                    float.TryParse(arr2[4], out anglesY);

                    string prologue = arr2[5];

                    NPCWorldMapData entity = new NPCWorldMapData();
                    entity.NPCId = npcId;
                    entity.NPCPostion = new Vector3(x, y, z);
                    entity.EulerAnglesY = anglesY;
                    entity.Prologue = prologue;

                    m_NPCWorldMapList.Add(entity);

                }
            }
            return m_NPCWorldMapList;
        }
    }
}