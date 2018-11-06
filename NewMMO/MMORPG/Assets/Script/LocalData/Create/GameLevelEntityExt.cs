
using UnityEngine;
using System.Collections;

public partial class GameLevelEntity
{
    private Vector2 m_Postion = Vector2.zero;

    /// <summary>
    /// 在地图上的本地坐标
    /// </summary>
    public Vector2 Postion
    {
        get
        {
            if (m_Postion == Vector2.zero)
            {
                if (!string.IsNullOrEmpty(PosInMap))
                {
                    string[] arr = PosInMap.Split('_');
                    if (arr.Length >= 2)
                    {
                        float x = 0, y = 0;
                        float.TryParse(arr[0], out x);
                        float.TryParse(arr[1], out y);
                        m_Postion = new Vector2(x, y);
                    }
                }
            }
            return m_Postion;
        }
    }
}