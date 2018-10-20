
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
            Debug.Log("加载资源完成");
            OnLoadComplete(bundle.LoadAsset(m_Name));
            DestroyImmediate(gameObject);
        }
    }

    void OnDestroy()
    {
        if (bundle != null) bundle.Unload(false);
        Debug.Log("卸载资源");
        m_FullPath = null;
        m_Name = null;
    }
}