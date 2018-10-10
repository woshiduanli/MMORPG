using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UILink = NGUILink.UILink;

[System.AttributeUsage(System.AttributeTargets.Class)]
public class CGameUIViewportAttribute : System.Attribute
{
    public CGameUIViewportAttribute(string name) : this(name, string.Empty) { }

    public CGameUIViewportAttribute(string name, string content)
    {
        this.name = name;
        this.content = content;
    }

    public string Content { get { return content; } }
    public string Name { get { return name; } }
    private string name;
    private string content;
}

/// <summary>
///  游戏UI基类，控制UI相关逻辑处理
/// </summary>
public abstract class CGameUI : MonoBehaviour
{
    #region 字体
    //public Font uifont { get { return ui_mgr.uifont; } }
    //public Font uifont_title { get { return ui_mgr.uifont_title; } }
    #endregion

    //protected List<CEventManager.Handler> event_handlers = new List<CEventManager.Handler>();
    public CUIManager ui_mgr;
    //public TimelineSystem timelineSystem { get { return ui_mgr.timelineSystem; } }
    //public CActivityManager acts { get { return ui_mgr.acts; } }
    //public CReferenceManager Ref_mgr { get { return ui_mgr.Ref_mgr; } }
    public bool PlayAudio = true;

    //public CMainPlayer Mp { get { return ui_mgr.Mp; } }
    public bool isInGame { get { return ui_mgr.isInGame; } }
    //public StoreRouter Router { get { return ui_mgr.Store.Router; } }
    public object[] args;
    public Camera MainCamera
    {
        get
        {
            //if (asset != null && !asset.destroy)
            //    return asset.MainCamera;
            return null;
        }
    }

    private Map<int, CUIElement> ChildMap = new Map<int, CUIElement>();
    private List<int> Childidles = new List<int>();

    //public CWorld World { get { return ui_mgr.World; } }
    public abstract bool isFullScreen { get; }
    public abstract int Layer { get; }
    /// <summary>
    /// 当顶替该界面的界面被关闭后是否需要自动加载该界面
    /// </summary>
    public abstract bool autoLoad { get; }


    public bool autoCreate { protected set; get; }
    protected bool NeedRefresh = true;
    public bool CheckData = true;
    public Camera UICamera { get { return this.ui_mgr.UICamera; } }
    public Canvas Canvas { private set; get; }
    public int SortingOrder = 1;
    public int planeDistance = 2;
    public bool dontDestroy { protected set; get; }
    private GameObject _gameObject;
    private Transform _transfrom;
    public new GameObject gameObject
    {
        get
        {
            if (_gameObject == null)
                _gameObject = base.gameObject;
            return _gameObject;
        }
    }
    public new Transform transform
    {
        get
        {
            if (_transfrom == null)
                _transfrom = base.transform;
            return _transfrom;
        }
    }
    public string Name { get; protected set; }
    public CGameUIAsset asset { get; private set; }

    public Map<string, bool> ClosedDic = new Map<string, bool>();
    private Map<string, long> CDMap = new Map<string, long>();
    public const int OPERATE_INTERVAL = 500; //操作时间间隔(毫秒)
    public const int MAXDISTANCE = 100;
    public int LifeTime;

    public void InitData(CGameUIAsset asset)
    {
        // 此时初始化的当前对象就是 CGameLuaUI
        this.ui_mgr = asset.uiMgr;
        this.SetAsset(asset);
        this.context = asset.Args;
        this.autoCreate = this.Layer == CUILayer.FullWindow && this.isFullScreen && this.context == null;
        this.Initialize();
        this.SetName(asset.UIName);
        // asset.IsVisible() 决定是不是要显示出来当前的对象 ，此时默认就是true
        // 在add函数里面， 初始化lua ，然后显示ui 
        this.ui_mgr.Add(this, asset.IsVisible());
        this.LoadUICallback();
    }

    private void SetAsset(CGameUIAsset asset)
    {
        //if (this.Layer == CUILayer.Free || this.Layer == CUILayer.Tip)
        //    LifeTime = 10;
        //if (this.Layer == CUILayer.FullWindow)
        //{
        //    if (isFullScreen)
        //        LifeTime = 300;//5分钟
        //    else
        //        LifeTime = 60;
        //}
        //if (this.Layer == CUILayer.MainFace)
        //    LifeTime = int.MaxValue;//永不销毁

        //this.asset = asset;
        this.Canvas = this.gameObject.GetComponent<Canvas>();
        //if (this.Canvas)
        //    SortingOrder = this.Canvas.sortingOrder;

        //CanvasScaler canvasScaler = this.gameObject.GetComponent<CanvasScaler>();
        //if (canvasScaler)
        //    canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

        //if (Layer != CUILayer.FullWindow)
        //    return;

        //GameObject Mask = new GameObject("Mask");
        //Mask.transform.SetParent(this.gameObject.transform);
        //Mask.transform.SetSiblingIndex(0);

        //CImage image = CClientCommon.AddComponent<CImage>(Mask);
        //image.color = new Color32(255, 255, 255, 2);
        //this.CreateSprite(image, "newpublic", "heisezhezhao");
        //RectTransform MakRect = Mask.transform as RectTransform;
        //MakRect.pivot = new Vector2(0.5f, 0.5f);
        //MakRect.anchorMin = Vector2.zero;
        //MakRect.anchorMax = Vector2.one;
        //MakRect.anchoredPosition = Vector3.zero;
    }

    private void SetName(string name)
    {
        this.Name = name;
        this.gameObject.name = name;

        this.gameObject.transform.SetParent(this.ui_mgr.UIRoot.transform);
        this.gameObject.transform.localPosition = Vector3.zero;
        this.gameObject.transform.localEulerAngles = Vector3.zero;
        if (this.Canvas)
        {
            this.Canvas.planeDistance = this.planeDistance;
            this.Canvas.renderMode = RenderMode.ScreenSpaceCamera;
            this.Canvas.worldCamera = this.ui_mgr.UICamera;
            SortingOrder = this.Canvas.sortingOrder;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="isGrey"> true 表示变成黑白图</param>
    public void SetGreyColor(CImage image, bool isGrey = true)
    {
        if (image.material.HasProperty("_IsGrey"))
        {
            int grep = isGrey ? 1 : 0;
            image.material.SetFloat("_IsGrey", grep);
        }
    }



    // 灰度图片
    public void SetGreyColor(CRawImage RawImage, bool isGrey = true)
    {
        if (RawImage.material.HasProperty("_IsGrey"))
        {
            int grep = isGrey ? 1 : 0;
            RawImage.material.SetFloat("_IsGrey", grep);
        }
    }

    public virtual void UpdateUIData() { }

    public int index = -1;
    public object[] context; //加载时传入的数据

    public float CloseTime;
    protected CGameUI() { } // disable public call
    public void SetPosition(Vector3 pos)
    {
        this.transform.position = pos;
    }

#if false
    public virtual void ProLoadTextures () { }
#endif

    private NGUILink _nguiLink = null;
    public NGUILink nguiLink
    {
        get
        {
            if (_nguiLink == null)
                _nguiLink = gameObject.GetComponent<NGUILink>();

            return _nguiLink;
        }
    }

    public void Show()
    {
        if (UIShow())
            // 当自己显示成功的时候， 就关闭其他全屏的界面
            this.ui_mgr.CloseActiveUIs(this);
    }



    public void AddCloseUI(CGameUI ui)
    {
        ui.SetActive(false);
        ClosedDic[ui.Name] = true; //ui.autoLoad;
    }

    public void AddChild(CUIElement element)
    {
        if (ChildMap != null)
            ChildMap[element.HashID] = element;
    }

    public void RemoveChild(CUIElement element)
    {
        if (Childidles != null)
            Childidles.Add(element.HashID);
    }

    /// <summary>
    /// 单纯的显示界面，不会顶掉UI
    /// </summary>
    /// <returns></returns>
    public bool UIShow()
    {
        if (disposed)
            return false;
        CClientCommon.SetActiveOverload(this.gameObject, true);
        if (this.Layer == CUILayer.MainFace && this.ui_mgr.HasFullWindowUI())
        {
            CClientCommon.SetUIActive(this.gameObject, false);
            return false;
        }
        if (!IsShow())
        {
            CClientCommon.SetUIActive(this.gameObject, true);
            return true;
        }
        return false;
    }




    public void SetActive(bool active)
    {
        if (this.gameObject && !this.gameObject.activeSelf)
            this.gameObject.SetActive(true);
        this.enabled = active;
    }

    public CSimpleSpriteObject CreateSprite(CImage sprite, string spname, string spritename)
    {
        if (this.asset == null || sprite == null)
            return null;
        sprite.SpriteName = spritename;
        sprite.enabled = false;
        return this.asset.CreateImage(sprite, spname);
    }
    /// <summary>
    /// 会自动打开上次被顶掉的UI
    /// </summary>
    public virtual void Close()
    {
        if (IsShow())
        {
            SetActive(false);
            if (this.ui_mgr.Hold)
                return;
            for (ClosedDic.Begin(); ClosedDic.Next();)
            {
                CGameUI ui = this.ui_mgr.Get(ClosedDic.Key);
                if (ui)
                    ui.WakeUp();
                else if (ClosedDic.Value)
                    this.ui_mgr.LoadUI(string.Concat("C", ClosedDic.Key, "UI"));
            }
            ClosedDic.Clear();
        }
    }

    protected void WakeUp()
    {
        OnWakeUp();
        UIShow();
    }

    protected virtual void OnWakeUp() { }


    /// <summary>
    /// 单纯的关闭界面，不会显示被顶掉的UI ,如果当前UI是全屏，防止主界面丢失 ，会打开主界面
    /// </summary>
    /// <returns></returns>
    public void SimpleClose()
    {
        if (disposed)
            return;
        if (IsShow())
        {
            ClosedDic.Clear();
            SetActive(false);
            if (this.ui_mgr.Hold)
                return;

            //if (Layer == CUILayer.FullWindow)
            //{
            //    if (this.ui_mgr.Hold)
            //        return;
            //    this.ui_mgr.OpenMainFaceUI();
            //}
        }
    }

    public bool IsShow()
    {
        if (disposed)
            return false;
        return transform.localScale == Vector3.one;
    }

    protected void Awake() { }
    public virtual void InitLua() { }
    public virtual void Initialize() { }
    protected virtual void LateUpdate()
    {
        //if (this.asset != null)
        //    asset.Update();

        //if (CheckData && this.isInGame)
        //{
        //    if (Mp == null || World == null)
        //    {
        //        SetActive(false);
        //        return;
        //    }
        //}

        //if (timelineSystem && this.UICamera)
        //    this.UICamera.enabled = !timelineSystem.IsPlaying();

        OnLateUpdate();
    }

    public virtual void LoadUICallback() { }

    //注：为了防止派生类忘记调用基类的OnEnable与OnDisable，不让派生类重写这两个接口，而是分别重写OnUIEnable与OnUIDisable接口
    protected void OnEnable()
    {
        CloseTime = 0;
        if (Canvas)
            Canvas.planeDistance = this.planeDistance;
        if (Layer == CUILayer.FullWindow && this.isFullScreen)
            ui_mgr.curFullWindow = this;
        OnUIEnable();
        FireEvent(new CEvent.UI.UIEnableEvent(this));

        // 如果是全屏界面， 此时应该关闭，ui 摄像机的渲染
        //if (this.Layer == CUILayer.FullWindow && this.isFullScreen)
        //    FireEvent(new CEvent.CameraCtrl.CameraActive(false));

        for (int i = 0; i < Childidles.Count; i++)
            ChildMap.Remove(Childidles[i]);
        Childidles.Clear();

        for (ChildMap.Begin(); ChildMap.Next();)
        {
            if (ChildMap.Value)
                ChildMap.Value.enabled = true;
        }
    }

    protected void OnDisable()
    {
        if (Canvas)
            Canvas.planeDistance = MAXDISTANCE;
        //CloseTime = GameTimer.time;
        //OnUIDisable();
        //FireEvent(new CEvent.UI.UICloseEvent(this));

        //if (ui_mgr.curFullWindow == this && this.isFullScreen)
        //    ui_mgr.curFullWindow = null;

        //for (int i = 0; i < Childidles.Count; i++)
        //    ChildMap.Remove(Childidles[i]);
        //Childidles.Clear();

        //for (ChildMap.Begin(); ChildMap.Next();)
        //{
        //    if (ChildMap.Value)
        //        ChildMap.Value.enabled = false;
        //}

        //if (this.ui_mgr.curFullWindow)
        //    return;

        //if (this.Layer == CUILayer.FullWindow && this.isFullScreen)
        //    FireEvent(new CEvent.CameraCtrl.CameraActive(true));
    }

    public virtual void OnUIEnable() { }
    public virtual void OnUIDisable() { }
    public virtual void OnContextChange() { }
    protected virtual void OnLateUpdate() { }
    public virtual void OnDestroy() { }
    protected virtual void OnUIDispose() { }

    public bool disposed { private set; get; }
    public void Dispose()
    {
        if (disposed)
            return;
        FireEvent(new CEvent.UI.RemoveUI(this.Name));
        this.transform.SetParent(null);
        disposed = true;
        ChildMap.Clear();
        ChildMap = null;
        Childidles.Clear();
        Childidles = null;
        CDMap.Clear();
        CDMap = null;
        context = null;
        ClosedDic.Clear();
        ClosedDic = null;
        //for (int i = 0; i < this.event_handlers.Count; ++i)
        //    this.ui_mgr.obj_mgr.UnregEventHandler(this.event_handlers[i]);
        //event_handlers.Clear();
        OnUIDispose();

        //删除UI对象，必须放最后
        if (this.asset != null)
        {
            //this.asset.Destroy();
            this.asset = null;
        }
    }

    //>--------------------------------------------------------------------
    // CObject的事件接品
    public void FireEvent(IEvent e)
    {
        this.ui_mgr.FireEvent(e);
    }

    //public T GetFromAT<T>(int at) where T : CActivity
    //{
    //    return acts.GetFromAT<T>(at);
    //}

    //public CEventManager.Handler RegEventHandler<T>(CEventManager.OnEventRecv<T> recv) where T : IEvent
    //{
    //    CEventManager.Handler h = this.ui_mgr.obj_mgr.RegEventHandler<T>(recv);
    //    this.event_handlers.Add(h);
    //    return h;
    //}

    //public CEventManager.Handler RegEventHandler(CEventManager.OnEventRecv recv, Type t)
    //{
    //    CEventManager.Handler h = this.ui_mgr.obj_mgr.RegEventHandler(recv, t);
    //    this.event_handlers.Add(h);
    //    return h;
    //}

    //public void UnRegEventHandler(CEventManager.Handler h)
    //{
    //    if (h == null)
    //        return;
    //    if (this.event_handlers.Count == 0)
    //        return;
    //    if (this.event_handlers.Remove(h))
    //        this.ui_mgr.obj_mgr.UnregEventHandler(h);
    //}

    //public CUIRedDot AddRedDot(CEvent.RedDot.CallBack c, RedDotType t, GameObject parent, Vector2 anchPosOffset)
    //{
    //    if (!parent)
    //        return null;
    //    GameObject go = new GameObject("redDot");
    //    Transform tf = go.transform;
    //    tf.SetParent(parent.transform);
    //    tf.localPosition = Vector3.zero;
    //    tf.localRotation = Quaternion.identity;
    //    tf.localScale = Vector3.one;
    //    go.layer = parent.layer;

    //    CUIRedDot rd = go.AddComponent<CUIRedDot>();
    //    rd.Arg = new object[] { c, t, anchPosOffset };
    //    return rd;
    //}

    ////>-------------------------------------------------------------------------------------------

    ///// <summary>
    ///// 初始化/更新sprite
    ///// </summary>
    ///// <param name="sprite"></param>
    ///// <param name="spname"></param>
    ///// <param name="spritename"></param>
    //public CSimpleSpriteObject CreateSprite(CImage sprite, string spname, string spritename)
    //{
    //    if (this.asset == null || sprite == null)
    //        return null;
    //    sprite.SpriteName = spritename;
    //    sprite.enabled = false;
    //    return this.asset.CreateImage(sprite, spname);
    //}

    ///// <summary>
    ///// 生成并初始化texture
    ///// </summary>
    ///// <param name="image"></param>
    ///// <param name="filename"></param>
    ///// <param name="hasalpha"></param 是否有Alpha图片>
    ///// <returns></returns>
    //public CTexture CreateRawImage(RawImage image, string filename)
    //{
    //    if (this.asset == null)
    //        return null;
    //    if (!image)
    //        return null;
    //    image.enabled = false;
    //    return this.asset.CreateRawImage(image, filename);
    //}

    //public CFontObject CreateFont(CText text, string font)
    //{
    //    if (this.asset == null)
    //        return null;
    //    if (!text)
    //        return null;
    //    return this.asset.CreateFont(font, text);
    //}

    //// 替换本来的颜色为白色
    //public CFontObject CreateFontReplaceWhite(CText text, string font)
    //{
    //    if (this.asset == null)
    //        return null;
    //    if (!text)
    //        return null;
    //    text.color = Color.white;
    //    return this.asset.CreateFont(font, text);
    //}

    //// 生成特效对象
    //public CEffectObject CreateEffect(string filename, Transform parent, Vector3 position, Vector3 scale, Vector3 euler_angle)
    //{
    //    if (this.asset == null) return null;
    //    return this.asset.CreateEffect(filename, parent, position, scale, euler_angle, this.SortingOrder);
    //}

    //public CEffectObject CreateNomalEffect(string filename, Transform parent)
    //{
    //    if (this.asset == null)
    //        return null;
    //    CEffectObject cEffect = this.asset.CreateEffect(filename, parent, Vector3.zero, Vector3.one, Vector3.zero, this.SortingOrder);
    //    return cEffect;
    //}

    //public CUIObject CreateSimpleUIObject(int apprid, ClientEquip[] equips, Transform parent, ClientDress[] dress = null, bool adjustSize = true)
    //{
    //    if (this.asset == null)
    //        return null;
    //    CUIObject uIObject = this.asset.CreateSimpleUIObject(apprid, equips, parent, this.SortingOrder, dress, adjustSize);
    //    this.ui_mgr.ActiveUILIght(this);
    //    return uIObject;
    //}

    //public CUIObject CreateMirrorUIObject(int apprid, ClientEquip[] equips, ClientDress[] dress = null, float x = 0, float y = 0, float scale = 0.5f)
    //{
    //    if (this.asset == null)
    //        return null;
    //    CUIObject uIObject = this.asset.CreateMirrorUIObject(apprid, equips, this.SortingOrder, dress, x, y, scale);
    //    this.ui_mgr.CreateShahow(this, x, y);
    //    return uIObject;
    //}

    ///// <summary>
    ///// 根据时装返回RoleID
    ///// </summary>
    ///// <param name="dresss"></param>
    ///// <param name="apprid"></param>
    ///// <returns></returns>
    //public int GetRoleID(ClientDress[] dresss, int apprid)
    //{
    //    if (dresss == null)
    //        return apprid;

    //    for (int i = 0; i < dresss.Length; i++)
    //    {
    //        ClientDress dress = dresss[i];
    //        if (dress == null || dress.RoleId == 0)
    //            continue;
    //        return dress.RoleId;
    //    }
    //    return apprid;
    //}

    ///// <summary>
    ///// 加载声音
    ///// </summary>
    ///// <param name="filename"></param>
    ///// <returns></returns>
    //public CAudioSoundAsset CreateAudio(string filename)
    //{
    //    if (this.asset == null)
    //        return null;
    //    return this.asset.CreateAudio(filename, this.ui_mgr.setSystem);
    //}

    //public void PlayCloseAudio()
    //{
    //    if (this.asset == null)
    //        return;
    //    this.asset.PlayCloseAudio();
    //}

    //public T GetReference<T>(long refid) where T : PropDefine, new()
    //{
    //    return Ref_mgr.Get<T>(refid);
    //}

    //public List<T> GetReferences<T>() where T : PropDefine, new()
    //{
    //    return Ref_mgr.GetReferences<T>();
    //}

    //public T GetReference<T>(String key) where T : PropDefine, new()
    //{
    //    return Ref_mgr.Get<T>(key);
    //}

    //public PropDefine GetEspecialItemReference(long refid)
    //{
    //    return Ref_mgr.GetEspecialItemReference(refid);
    //}

    //public bool IsPointerOverUI(Vector2 point)
    //{
    //    return ui_mgr.IsPointerOverUI(point);
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="checkFunc">返回值:true-关闭，false-不关闭</param>
    //public void CloseUIByTouch(System.Func<bool> checkFunc)
    //{
    //    ui_mgr.CloseUIByTouch(this, checkFunc);
    //}

    //public void CloseElementByTouch(CUIElement element, System.Func<bool> checkFunc)
    //{
    //    ui_mgr.CloseElementByTouch(element, checkFunc);
    //}

    ///// <summary>
    ///// 是否处于CD状态
    ///// </summary>
    ///// <param name="key"></param>
    ///// <param name="time"></param>
    ///// <returns></returns>
    //public bool InCDState(string key, float time = OPERATE_INTERVAL, string tips = "")
    //{
    //    long timestamp = GameTimer.mtime;

    //    if (!CDMap.ContainsKey(key))
    //    {
    //        CDMap.Add(key, timestamp);
    //        return false;
    //    }

    //    if (timestamp - CDMap[key] >= time)
    //    {
    //        CDMap[key] = timestamp;
    //        return false;
    //    }

    //    if (tips != "")
    //        FireEvent(new CEvent.UI.OpenUI("CGameCommonTipUI", tips));

    //    return true;
    //}
}
