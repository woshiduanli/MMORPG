////===================================================
////作    者：边涯  http://www.u3dol.com  QQ群：87481002
////创建时间：2016-06-30 21:54:15
////备    注：
////===================================================
//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;

//public class UIMainCitySmallMapView : UISubViewBase
//{
//    public static UIMainCitySmallMapView Instance;

//    /// <summary>
//    /// 小地图
//    /// </summary>
//    public Image SmallMap;

//    /// <summary>
//    /// 小箭头
//    /// </summary>
//    public Image SmallMapArr;

//    protected override void OnAwake()
//    {
//        base.OnAwake();
//        Instance = this;
//    }

//    protected override void OnStart()
//    {
//        base.OnStart();
//        try
//        {
//            string strImgName = WorldMapDBModel.Instance.Get(SceneMgr.Instance.CurrWorldMapId).SmallMapImg;
//            AssetBundleMgr.Instance.LoadOrDownload<Texture2D>(string.Format("Download/Source/UISource/SmallMap/{0}.assetbundle", strImgName), strImgName, (Texture2D obj) =>
//            {
//                if (obj == null) return;

//                var iconRect = new Rect(0, 0, obj.width, obj.height);
//                var iconSprite = Sprite.Create(obj, iconRect, new Vector2(.5f, .5f));

//                SmallMap.overrideSprite = iconSprite;

//                //SmallMap.overrideSprite = obj;
//            }, type: 1);
//        }
//        catch { }    }

//    protected override void BeforeOnDestroy()
//    {
//        base.BeforeOnDestroy();
//        SmallMap = null;
//        SmallMapArr = null;
//    }
//}