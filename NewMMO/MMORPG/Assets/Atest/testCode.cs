using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCode : MonoBehaviour {
    Animator a; 
// Use this for initialization
void Start () {
         a = GetComponent<Animator>();
        a.SetBool("ToRun", true);
       



        return; 
        FingerEvent.Instance.OnFingerDrag += OnFingerDrag;
    }

    private void OnFingerDrag(FingerEvent.FingerDir obj)
    {
        MyDebug.debug(obj.ToString ()); 
        switch (obj)
        {
            case FingerEvent.FingerDir.Left:
                //MyDebug.debug(); 
                //CameraCtrl.Instance.SetCameraRotate(0);
                break;
            case FingerEvent.FingerDir.Right:
                //CameraCtrl.Instance.SetCameraRotate(1);
                break;
            case FingerEvent.FingerDir.Up:
                //CameraCtrl.Instance.SetCameraUpAndDown(1);
                break;
            case FingerEvent.FingerDir.Down:
                //CameraCtrl.Instance.SetCameraUpAndDown(0);
                break;
        }


    }

    // Update is called once per frame
    void Update () {

        //a = GetComponent<Animator>();
        //a.SetBool("ToRun", true);


        AnimatorStateInfo info = a.GetCurrentAnimatorStateInfo(0);

        if (info.IsName(RoleAnimatorName.Run.ToString()))
        {
            a.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleState.Run);
        }
        else
        {
            a.SetInteger(ToAnimatorCondition.CurrState.ToString(), 0);
        }
    }
}
