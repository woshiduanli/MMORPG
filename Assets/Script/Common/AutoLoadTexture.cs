using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XLua;

/// <summary>
/// �Զ�����ͼƬ
/// </summary>
public class AutoLoadTexture : MonoBehaviour
{
    /// <summary>
    /// ͼƬ����
    /// </summary>
    public string ImgName;

    /// <summary>
    /// ͼƬ·��
    /// </summary>
    public string ImgPath;

    /// <summary>
    /// �Ƿ�����ͼƬԭ����С
    /// </summary>
    public bool IsSetNativeSize;

    void Start()
    {
    }

    public void SetImg()
    {
        Image img = GetComponent<Image>();

        if (img != null && !string.IsNullOrEmpty(ImgPath))
        {
            //AssetBundleMgr.Instance.LoadOrDownload<Texture2D>(ImgPath, ImgName, (Texture2D obj) =>
            //{
            //    if (obj == null) return;

            //    var iconRect = new Rect(0, 0, obj.width, obj.height);
            //    var iconSprite = Sprite.Create(obj, iconRect, new Vector2(.5f, .5f));

            //    img.overrideSprite = iconSprite;
            //    if (IsSetNativeSize)
            //    {
            //        img.SetNativeSize();
            //    }
            //}, type: 1);
        }
    }
}