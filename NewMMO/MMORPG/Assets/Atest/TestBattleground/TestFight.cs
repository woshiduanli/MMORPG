using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFight : MonoBehaviour {

    public RoleCtrl TestRole; 
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnGUI()
    {
        if (TestRole == null) {
            return;
        }

        int posY =0; 
        if (GUI.Button (new Rect( 1, posY, 60,30 ), "ptIdel")){
            TestRole.ToIdle(); 
        }

        posY += 30; 
        if (GUI.Button(new Rect(1, posY, 60, 30), "fIdel"))
        {
            TestRole.ToIdle(RoleIdleState.IdelFight); 
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "run"))
        {
            TestRole.ToRun(); 
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "Hurt"))
        {
            TestRole.ToHurt(20,0.2f); 
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "Die"))
        {
            TestRole.TestToDies();
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), ""))
        {
            TestRole.TestToDies();
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "phy1"))
        {
            TestRole.ToAttack();
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "Hurt"))
        {
            TestRole.ToHurt(20, 0.2f);
        }
    }
}
