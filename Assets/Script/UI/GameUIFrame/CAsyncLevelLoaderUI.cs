using UniRx;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CAsyncLevelLoaderUI : MonoBehaviour
{
    private CLoaderUI Loaderbar;
    public static void Create()
    {
        GameObject Laugo = Object.Instantiate(Resources.Load("UI/Login/UIPrefab/AsyncLevelLoader")) as GameObject;
        if (Laugo.transform.parent != null)
        {
            MyDebug.debug("ni le ");
            MyDebug.debug(Laugo.transform.parent.gameObject.name);
        }
        else
        {
            MyDebug.debug("kong le ");

        }

        if (Laugo)
        {
            //string bg = string.Empty;
            //if (world)
            //      bg = world.Ref.LoadingImage;
            CAsyncLevelLoaderUI cAsyncLevel = Laugo.AddComponent<CAsyncLevelLoaderUI>();
            //cAsyncLevel.LoadImage(bg);
            //if (cReference && !cReference.load_complete)
            //Awake(); 
            cAsyncLevel.SetProgressSpeed(10, 30);
        }

    }
    
    void Awake()
    {
        Time.timeScale = 1;
        NGUILink link = this.gameObject.GetComponent(typeof(NGUILink)) as NGUILink;
        if (link == null)
            return;
        Loaderbar = this.gameObject.AddComponent(typeof(CLoaderUI)) as CLoaderUI;
    }

    private void LoadImage(string bg)
    {
        if (!string.IsNullOrEmpty(bg))
            Loaderbar.LoadImage(bg);
    }

    private void SetProgressSpeed(int speed, int custom)
    {
        if (Loaderbar == null)
        Loaderbar = this.gameObject.AddComponent(typeof(CLoaderUI)) as CLoaderUI;
        Loaderbar.SetProgressSpeed(speed, custom);
    }
}
