
using UnityEngine;
using System.Collections;

public class InitSceneCtrl : MonoBehaviour 
{
	void Start ()
	{
        StartCoroutine(LoadLogOn());
	}

    private IEnumerator LoadLogOn()
    {
        yield return new WaitForSeconds(2f);
        SceneMgr.Instance.LoadToLogOn();
    }
}