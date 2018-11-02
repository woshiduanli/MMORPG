//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-06-30 21:49:51
//备    注：
//===================================================
using UnityEngine;
using System.Collections;

public class SmallMapHelper : MonoBehaviour
{
    public static SmallMapHelper Instance;
    void Awake()
    {
        Instance = this;
    }
}