
using UnityEngine;
using System.Collections;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{ 
    #region 单例
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                CategorySettings.Attach(obj.transform, "_loops/", false);
                DontDestroyOnLoad(obj);
                instance = obj.GetOrCreatComponent<T>();
            }
            return instance;
        }
    }
    #endregion

    void Awake()
    {
        OnAwake();
    }

    void Start()
    {
        OnStart();
    }

    void Update()
    {
        OnUpdate();
    }

    void OnDestroy()
    {
        BeforeOnDestroy();
    }

    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }
    protected virtual void BeforeOnDestroy() { }


}