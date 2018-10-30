
using UnityEngine;
using System.Collections;

public class AssetBundleLoaderAsync : MonoBehaviour
{
    private string m_FullPath;
    private string m_Name;

    private AssetBundleCreateRequest request;
    private AssetBundle bundle;

    public System.Action<Object> OnLoadComplete;

    public void Init(string path, string name)
    {
        m_FullPath = LocalFileMgr.Instance.LocalFilePath + path;
        m_Name = name;
    }

	void Start () 
	{
        StartCoroutine(Load());
	}

    private IEnumerator Load()
    {
        request = AssetBundle.LoadFromMemoryAsync(LocalFileMgr.Instance.GetBuffer(m_FullPath));
        yield return request;
        bundle = request.assetBundle;
        if (OnLoadComplete != null)
        {
            MyDebug.debug("加载资源完成");
            UnityEngine.Object dddd = bundle.LoadAsset(m_Name); 
            if (bundle.LoadAsset(m_Name))
            {

            }

            OnLoadComplete(bundle.LoadAsset(m_Name));
            DestroyImmediate(gameObject);
        }
    }

    void OnDestroy()
    {
        if (bundle != null) bundle.Unload(false);
        MyDebug.debug("卸载资源");
        m_FullPath = null;
        m_Name = null;
    }
}