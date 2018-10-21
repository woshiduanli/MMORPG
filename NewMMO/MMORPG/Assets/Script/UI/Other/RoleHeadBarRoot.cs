
using UnityEngine;
using System.Collections;

public class RoleHeadBarRoot : MonoBehaviour 
{
    public static RoleHeadBarRoot Instance;

    void Awake ()
	{
        Instance = this;
    }
}