using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelRegionCtrl : MonoBehaviour
{

    public int RegionId; 

    [SerializeField]
    public Transform RoleBornPos;

    [SerializeField]
    Transform[] MonsterBornPos;

    [SerializeField]
    private GameLevelDoorCtrl[] AllDoor; 

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(this.transform.position, 1);

        Gizmos.color = Color.red;
        if (RoleBornPos != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(RoleBornPos.position, 1);
            Gizmos.DrawLine(transform.position, RoleBornPos.position);
        }
        if (MonsterBornPos != null && MonsterBornPos.Length > 0)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < MonsterBornPos.Length; i++)
            {
                Gizmos.DrawSphere(MonsterBornPos[i].position, 1);
                Gizmos.DrawLine(transform.position, MonsterBornPos[i].position);
            }
        }
        if (AllDoor != null)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < AllDoor.Length; i++)
            {
                Gizmos.DrawSphere(AllDoor[i].transform.position, 1);
                Gizmos.DrawLine(transform.position, AllDoor[i].transform.position);
            }
        }

    }
#endif 

}
