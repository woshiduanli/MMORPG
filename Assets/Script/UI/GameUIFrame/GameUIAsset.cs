using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;
//using UnityEngine.Object = Object; 


public class CBaseObject : ZRender.IRenderObject, IDisposable
{
    public void Dispose()
    {

    }
}

public class CGameUIAsset : ZRender.IRenderObject
{
    private Map<string, CSimpleSpriteObject> spriteDic = new Map<string, CSimpleSpriteObject>();
    private Map<string, CFontObject> fontDic = new Map<string, CFontObject>();
    private Map<string, CTexture> textureDic = new Map<string, CTexture>();
    private Map<int, ZRender.IRenderObject> Assets = new Map<int, ZRender.IRenderObject>();
    private Map<int, CBaseObject> Roles = new Map<int, CBaseObject>();
    private Map<int, ZRender.IRenderObject> AudioAssets = new Map<int, ZRender.IRenderObject>();
    private List<CUIDepend> Depends = new List<CUIDepend>();
    private Map<string, CAudioSoundAsset> AudioDic = new Map<string, CAudioSoundAsset>();
    private Map<string, string> audios = new Map<string, string>();
    private CUIAsset uIAsset;
    public Camera MainCamera { private set; get; }

    public CUIManager uiMgr { private set; get; }
    public string UIName { private set; get; }
    public Type UIType { private set; get; }
    public object[] Args { private set; get; }

    public bool disposed { protected set; get; }
    protected override void OnDestroy()
    {
        if (disposed)
            return;
        disposed = true;
        for (spriteDic.Begin(); spriteDic.Next();)
        {
            if (spriteDic.Value != null)
                spriteDic.Value.Destroy();
        }
        spriteDic.Clear();
        spriteDic = null;

        for (int i = 0; i < Depends.Count; i++)
        {
            if (Depends[i] != null)
                Depends[i].Destroy();
        }
        Depends.Clear();
        Depends = null;

        for (textureDic.Begin(); textureDic.Next();)
        {
            if (textureDic.Value != null)
                textureDic.Value.Destroy();
        }
        textureDic.Clear();
        textureDic = null;

        for (Assets.Begin(); Assets.Next();)
            Assets.Value.Destroy();
        Assets.Clear();
        Assets = null;

        for (fontDic.Begin(); fontDic.Next();)
        {
            if (fontDic.Value != null)
                fontDic.Value.Destroy();
        }
        fontDic.Clear();
        fontDic = null;

        for (Roles.Begin(); Roles.Next();)
            Roles.Value.Dispose();
        Roles.Clear();
        Roles = null;

        for (AudioDic.Begin(); AudioDic.Next();)
            AudioDic.Value.Destroy();
        AudioDic.Clear();
        AudioDic = null;

        audios.Clear();
        audios = null;

        if (uIAsset != null)
            uIAsset.Destroy();
        uIAsset = null;

    }

    public CGameUIAsset(CUIManager uimgr, string ui, Type uitype, params object[] args)
    {
        this.uiMgr = uimgr;
        this.UIName = ui;
        this.UIType = uitype;
        this.Args = args;
    }


    protected override void OnCreate()
    {
        AssetBundle ManifestBundle = AssetBundle.LoadFromFile(CDirectory.MakeFullMobilePath(string.Format("res/ui/manifest/{0}.cfg", this.UIName.ToLower())));
        UnityEngine.Object[] objs = ManifestBundle.LoadAllAssets();

        if (objs.Length == 0)
            return;
        TextAsset textasset = objs[0] as TextAsset;
        ByteReader reader = new ByteReader(Encoding.UTF8.GetBytes(textasset.text));
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            Depends.Add(CResourceFactory.CreateInstance<CUIDepend>(line.ToLower(), this, PLevel.High));
        }
        ManifestBundle.Unload(true);

        AddTimer(OnDependsCompleteEvent, null, 0, 0);
    }

    private bool OnDependsCompleteEvent(object obj, int p1, int p2)
    {
        for (int i = 0; i < Depends.Count; i++)
        {
            if (!Depends[i].complete)
                return true;
        }
        uIAsset = CResourceFactory.CreateInstance<CUIAsset>(string.Format("res/ui/uiprefab/{0}.ui", this.UIName.ToLower()), this, PLevel.High);
        AddTimer(OnUICompleteEvent, null, 0, 0);
        return false;
    }

    CGameLuaUI ui; 
    private bool OnUICompleteEvent(object obj, int p1, int p2)
    {
        if (!uIAsset.isDone)
            return true;
        this.gameObject = uIAsset.UI;
        if (!this.gameObject)
            return false;
        gameObject.name = gameObject.name.Replace("(Clone)", "");
        UICacheComponents uICache = this.gameObject.GetComponent<UICacheComponents>();
        // 实列化出来的对象， 给他设置ctext的值
        LoadAsset(uICache);
        // 加上控制脚本
        ui = this.gameObject.AddComponent<CGameLuaUI>();
        ui.InitData(this);
        CreateUGUIAudio(ui, uICache);
        return false;
    }

    private void LoadAsset(UICacheComponents uiCache)
    {
        if (!uiCache)
            return;
        CText[] texts = uiCache.CTexts.ToArray();
        List<CText> textlist = new List<CText>();
        for (int i = 0; i < texts.Length; ++i)
        {
            CText t = texts[i];
            t.raycastTarget = false;
            if (t.FontName == "Arial")
                t.FontName = this.uiMgr.uifont.name;

            if (t.FontName == this.uiMgr.uifont.name)
                t.font = this.uiMgr.uifont;
            else if (t.FontName == this.uiMgr.uifont_title.name)
                t.font = this.uiMgr.uifont_title;

            if (!string.IsNullOrEmpty(t.Language))
                t.text = Localization.Get(t.Language);

            if (t.IsNeedEmoji)
                textlist.Add(t);
        }
    }

    private void CreateUGUIAudio(CGameUI ui, UICacheComponents uiCache)
    {
        //CAudioSoundAsset click = this.CreateAudio(ConstFilePathDefine.Click, uiMgr.setSystem);
        //string openres = ConstFilePathDefine.OpenUI;
        //string closeres = ConstFilePathDefine.CloseUI;

        //uiMgr.GetReferences<AudioReference>().ForEach(x =>
        //{
        //    if (x.Name.Equals(gameObject.name))
        //    {
        //        if (string.IsNullOrEmpty(x.Path) && x.OperationType == (int)TrrigerType.OpenUI)
        //            openres = x.ResName;
        //        else if (string.IsNullOrEmpty(x.Path) && x.OperationType == (int)TrrigerType.CloseUI)
        //            closeres = x.ResName;
        //        else
        //            audios.Add(x.Path, x.ResName);
        //    }
        //});
        //CAudioSoundAsset open = this.CreateAudio(openres, uiMgr.setSystem);
        //CAudioSoundAsset close = this.CreateAudio(closeres, uiMgr.setSystem);

        //if (open != null)
        //    open.Play();

        //if (!uiCache)
        //    return;
        //Button[] buttons = uiCache.Buttons.ToArray();
        //for (int i = 0; i < buttons.Length; ++i)
        //{
        //    Button b = buttons[i];
        //    string res = ConstFilePathDefine.Click;
        //    for (audios.Begin(); audios.Next();)
        //    {
        //        Transform child = CClientCommon.FindChild(audios.Key, this.gameObject.transform);
        //        if (child && child.gameObject == b.gameObject)
        //        {
        //            res = audios.Value;
        //            break;
        //        }
        //    }
        //    b.gameObject.AddComponent<UGUIAudio>().SetAsset(ui, b, null, res);
        //}

        //Toggle[] toggles = uiCache.Toggles.ToArray();
        //for (int i = 0; i < toggles.Length; ++i)
        //    toggles[i].gameObject.AddComponent<UGUIAudio>().SetAsset(ui, null, toggles[i], ConstFilePathDefine.Click);
    }



    protected override void OnUpdate()
    {
        for (Assets.Begin(); Assets.Next();)
        {
            if (Assets.Value.destroy)
            {
                Assets.Remove(Assets.Key);
                break;
            }
            else
                Assets.Value.Update();
        }

        for (Roles.Begin(); Roles.Next();)
        {
            //if (!Roles.Value)
            //{
            //    Roles.Remove(Roles.Key);
            //    break;
            //}
        }
    }

    /// <summary>
    /// 生成并替换sprite中的atlas
    /// </summary>
    /// <param name="image"></param>
    /// <param name="spname"></param>
    /// <returns></returns>
    public CSimpleSpriteObject CreateImage(CImage image, string atlas)
    {
        if (string.IsNullOrEmpty(atlas))
            return null;
        atlas = atlas.ToLower();
        CSimpleSpriteObject Spriteobj = null;
        this.spriteDic.TryGetValue(atlas, out Spriteobj);
        //if (Spriteobj == null)
        //{
        //    Spriteobj = CResourceFactory.CreateInstance<CSimpleSpriteObject>(string.Format("res/ui/tex/{0}.tex", atlas), this, PLevel.High, atlas);
        //    spriteDic[atlas] = Spriteobj;
        //}
        Spriteobj.SetImage(image);
        return Spriteobj;
    }

    public CTexture CreateRawImage(RawImage image, string texname)
    {
        if (string.IsNullOrEmpty(texname))
            return null;
        texname = texname.ToLower();
        CTexture imageobj = null;
        this.textureDic.TryGetValue(texname, out imageobj);
        //if (imageobj == null)
        //{
        //    imageobj = CResourceFactory.CreateInstance<CTexture>(string.Format("res/ui/tex/{0}.tex", texname), this, PLevel.High, texname);
        //    textureDic[texname] = imageobj;
        //}
        imageobj.SetTexture(image);
        return imageobj;
    }

    public CTexture CreateRawImage(Renderer render, string texname)
    {
        if (string.IsNullOrEmpty(texname))
            return null;
        texname = texname.ToLower();
        CTexture imageobj = null;
        this.textureDic.TryGetValue(texname, out imageobj);
        //if (imageobj == null)
        //{
        //    imageobj = CResourceFactory.CreateInstance<CTexture>(string.Format("res/ui/tex/{0}.tex", texname), this, PLevel.High, texname);
        //    textureDic[texname] = imageobj;
        //}
        imageobj.SetTexture(render);
        return imageobj;
    }

    public CFontObject CreateFont(string fontname, CText text)
    {
        if (string.IsNullOrEmpty(fontname))
            return null;
        fontname = fontname.ToLower();
        CFontObject cFont = null;
        fontDic.TryGetValue(fontname, out cFont);
        if (cFont == null)
        {
            cFont = CResourceFactory.CreateInstance<CFontObject>(string.Format("res/ui/font/{0}.ft", fontname), this, PLevel.High);
            fontDic[fontname] = cFont;
        }
        cFont.SetText(text);

        return cFont;
    }

    // 生成特效对象
    //public CEffectObject CreateEffect(string filename, Transform parent, Vector3 position, Vector3 scale, Vector3 euler_angle, int layer)
    //{
    //    if (string.IsNullOrEmpty(filename))
    //        return null;
    //    filename = filename.ToLower();
    //    CEffectObject obj = CResourceFactory.CreateInstance<CEffectObject>(filename, null, PLevel.Middle);
    //    obj.SetParentTransform(parent);
    //    obj.SetSortingOrder(layer);
    //    obj.SetPosition(position);
    //    obj.SetScale(scale);
    //    obj.SetRotation(euler_angle);
    //    obj.SetLayer(CDefines.Layer.UI);
    //    if (!Assets.ContainsKey(obj.HashID))
    //        Assets.Add(obj.HashID, obj);
    //    else
    //        Assets[obj.HashID] = obj;
    //    return obj;
    //}

    /// <summary>
    /// 加载声音
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="set"></param>
    /// <returns></returns>
    //public CAudioSoundAsset CreateAudio(string filename, GameSetSystem set)
    //{
    //    if (string.IsNullOrEmpty(filename))
    //        return null;
    //    filename = filename.ToLower();
    //    CAudioSoundAsset Audioobj = null;
    //    AudioDic.TryGetValue(filename, out Audioobj);
    //    if (Audioobj == null)
    //    {
    //        Audioobj = CResourceFactory.CreateInstance<CAudioSoundAsset>(filename, null, set);
    //        AudioDic[filename] = Audioobj;
    //    }
    //    return Audioobj;
    //}

    /// <summary>
    /// 用于UI界面的英雄显示,支持SkillController和MotionController等。
    /// </summary>
    /// <param name="cr"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    //public CUIRoleObject CreateUIObject(CClientRole cr, Transform parent, int layer)
    //{
    //    CUIRoleObject ro = CUIRoleObject.Create(cr, parent);
    //    ro.SetSortingOrder(layer);
    //    Roles.Add(ro.HashID, ro);
    //    return ro;
    //}

    /// <summary>
    /// 用于UI显示的Object,支持换装。不支持SkillController和MotionController等控制器
    /// </summary>
    /// <param name="apprid"></param>
    /// <param name="equips"></param>
    /// <param name="parent"></param>
    /// <param name="dress"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    //public CUIObject CreateSimpleUIObject(int apprid, ClientEquip[] equips, Transform parent, int layer, ClientDress[] dress = null, bool adjustSize = true)
    //{
    //    CUIObject ro = CUIObject.Create(apprid, equips, parent, adjustSize, dress);
    //    ro.SetSortingOrder(layer);
    //    ro.SetLayer(CDefines.Layer.UI);
    //    ro.AdjustFlyHeight();
    //    Roles.Add(ro.HashID, ro);
    //    return ro;
    //}

    //public CUIObject CreateMirrorUIObject(int apprid, ClientEquip[] equips, int layer, ClientDress[] dress = null, float x = 0, float y = 0, float scale = 0.5f)
    //{
    //    CUIObject ro = CUIObject.Create(apprid, equips, this.uiMgr.ModelTF, false, dress);
    //    this.uiMgr.ModelTF.localScale = new Vector3(scale, scale, scale);
    //    ro.SetSortingOrder(layer);
    //    ro.SetLayer(CDefines.Layer.UIPlayer);
    //    ro.AdjustFlyHeight();
    //    Roles.Add(ro.HashID, ro);
    //    return ro;
    //}

    public void PlayCloseAudio()
    {
        //CAudioSoundAsset Audioobj;
        //AudioDic.TryGetValue(ConstFilePathDefine.CloseUI, out Audioobj);
        //if (Audioobj != null)
        //    Audioobj.Play();
    }

    public void DestroyEffect(int hash)
    {
        ZRender.IRenderObject cEffect;
        Assets.TryGetValue(hash, out cEffect);
        if (cEffect != null)
        {
            Assets.Remove(hash);
            cEffect.Destroy();
        }
    }

    public void DestroyRole(int hash)
    {
        CBaseObject ro;
        Roles.TryGetValue(hash, out ro);
        if (ro != null)
        {
            Roles.Remove(hash);
            ro.Dispose();
        }
    }
}


public class CTexture : ZRender.IRenderObject
{
    public Material Mat { get; private set; }
    public Texture texture { get; private set; }
    private List<RawImage> rawImages = new List<RawImage>();
    private List<Renderer> Renderers = new List<Renderer>();
    private string AssetName;

    public CTexture(string asset)
    {
        this.AssetName = asset;
    }

    protected override void OnCreate()
    {
        texture = this.owner.GetAsset(AssetName) as Texture;
        Mat = this.owner.GetAsset(AssetName + "mat") as Material;

        for (int i = 0; i < rawImages.Count; i++)
            SetTexture(rawImages[i]);
        rawImages.Clear();

        for (int i = 0; i < Renderers.Count; i++)
            SetTexture(Renderers[i]);
        Renderers.Clear();
    }

    protected override void OnDestroy()
    {
        this.rawImages.Clear();
        this.rawImages = null;

        this.Renderers.Clear();
        this.Renderers = null;

        this.texture = null;
        this.Mat = null;
    }

    public void SetTexture(RawImage image)
    {
        if (!image)
            return;
        if (this.texture)
        {
            image.enabled = true;
            image.texture = this.texture;
            if (Mat)
                image.material = Mat;
            return;
        }
        if (rawImages != null && !this.rawImages.Contains(image))
            this.rawImages.Add(image);
    }

    public void SetTexture(Renderer render)
    {
        if (!render || !render.sharedMaterial)
            return;
        if (this.texture)
        {
            render.enabled = true;
            render.sharedMaterial.mainTexture = this.texture;
            if (Mat)
                render.sharedMaterial.SetTexture("_AlphaTex", Mat.GetTexture("_AlphaTex"));
            return;
        }
        if (Renderers != null && !this.Renderers.Contains(render))
            this.Renderers.Add(render);
    }
}

public class CUIDepend : ZRender.IRenderObject { }

public class CSimpleSpriteObject : ZRender.IRenderObject
{
    public Material Mat { get; private set; }
    private Map<string, Sprite> SpriteDic = new Map<string, Sprite>();
    private List<CImage> imagelist = new List<CImage>();
    private string AssetName;

    public CSimpleSpriteObject(string asset)
    {
        this.AssetName = asset;
    }

    protected override void OnCreate()
    {
        UnityEngine.Object[] objects = this.owner.GetSubAssets(AssetName);
        for (int i = 0; i < objects.Length; i++)
        {
            UnityEngine.Object go = objects[i];
            if (!go)
                continue;
            SpriteDic[go.name] = go as Sprite;
        }
        Mat = this.owner.GetAsset(AssetName + "mat") as Material;

        for (int i = 0; i < imagelist.Count; i++)
            SetImage(imagelist[i]);
        imagelist.Clear();
    }

    protected override void OnDestroy()
    {
        this.SpriteDic.Clear();
        this.SpriteDic = null;
        this.imagelist.Clear();
        this.imagelist = null;
        this.Mat = null;
    }

    public void SetImage(CImage image)
    {
        if (!image)
            return;
        if (SpriteDic.Count > 0)
        {
            image.enabled = true;
            Sprite sprite;
            SpriteDic.TryGetValue(image.SpriteName, out sprite);
            image.sprite = sprite;
            if (Mat)
                image.material = Mat;
            return;
        }
        if (imagelist != null && !this.imagelist.Contains(image))
            this.imagelist.Add(image);
    }
}

public class CFontObject : ZRender.IRenderObject
{
    public Font font { get; private set; }
    private List<CText> Texts = new List<CText>();
    protected override void OnCreate()
    {
        this.font = this.GetOwner().GetAsset() as Font;
        for (int i = 0; i < this.Texts.Count; i++)
            SetText(this.Texts[i]);
        this.Texts.Clear();
    }

    protected override void OnDestroy()
    {
        this.Texts.Clear();
        this.Texts = null;
        this.font = null;
    }

    public virtual void SetText(CText t)
    {
        if (!t)
            return;
        if (font)
        {
            t.enabled = true;
            t.font = this.font;
            return;
        }
        if (!this.Texts.Contains(t))
            this.Texts.Add(t);
    }
}


public class CUIAsset : ZRender.IRenderObject
{
    public bool isDone { get; private set; }
    public GameObject UI { get; private set; }
    protected override void OnCreate()
    {
        this.UI = UnityEngine.Object.Instantiate(this.GetOwner().GetAsset()) as GameObject;
        UnityEngine.Object.DontDestroyOnLoad(this.UI);
        this.UI.SetActive(false);
        this.isDone = true;
    }

    protected override void OnDestroy()
    {
        if (this.UI)
            UnityEngine.Object.Destroy(this.UI);
    }
}



