
using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class BoxCtrl : MonoBehaviour 
{
    /// <summary>
    /// 委托原型
    /// </summary>
    /// <param name="obj"></param>
    public delegate void OnHitHandler(GameObject obj);

    /// <summary>
    /// 定义委托
    /// </summary>
    public OnHitHandler OnHit;

    ///// <summary>
    ///// 委托
    ///// </summary>
    //public System.Action<GameObject> OnHit;

    void Start()
    {

    }

    void Instance_OnChange()
    {
        transform.localScale = new Vector3(Random.Range(0.5f, 3.5f), Random.Range(0.5f, 3.5f), Random.Range(0.5f, 5.5f));
    }

    void OnDestroy()
    {

    }

    void Update()
    {

    }

    public void Hit()
    {
        if (OnHit != null)
        {
            OnHit(gameObject);
        }
    }

}