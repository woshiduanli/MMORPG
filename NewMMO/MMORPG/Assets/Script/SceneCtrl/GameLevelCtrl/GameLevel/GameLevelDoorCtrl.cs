using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelDoorCtrl : MonoBehaviour
{

    public GameLevelDoorCtrl connectToDoor;

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
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position, 1);

        if (connectToDoor!=null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(this.transform.position, connectToDoor.transform.position);
        }
    }
#endif 

}
