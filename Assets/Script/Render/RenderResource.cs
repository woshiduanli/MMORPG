using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

public enum PLevel
{
    Low,
    Middle,
    High,
    Max,
}

namespace ZRender
{

    public abstract class IRenderResource
    {
        public AssetBundle asset_bundle { protected set; get; }
        public string Assetname { protected set; get; }
        public bool isdestroy;
        public float linger_time = 0f;
        public int ReferenceCount { get { return this.insts.Count; } }
        protected List<IRenderObject> insts = new List<IRenderObject>();
        protected UnityEngine.Object asset;
        protected StringBuilder text;
        public UnityEngine.Object GetAsset() { return asset; }
        public StringBuilder GetText() { return text; }
        public bool isCache;
        public bool complete;
        public virtual void Destroy() { }
        public virtual void RemoveInstance(IRenderObject obj) { }

        public Object GetAsset(string filename)
        {
            if (this.asset_bundle)
                return this.asset_bundle.LoadAsset(filename);
            return null;
        }

        public Object[] GetSubAssets(string filename)
        {
            if (this.asset_bundle)
                return this.asset_bundle.LoadAssetWithSubAssets(filename);
            return null;
        }

        public bool loading;
        public abstract IEnumerator Load();
        public T CreateInstance<T>(IRenderObject parent, params object[] args)
            where T : IRenderObject
        {
            return CreateInstance(typeof(T), parent, args) as T;
        }

        public abstract IRenderObject CreateInstance(Type type, IRenderObject parent, params object[] args);
    }

    public class RenderResource : IRenderResource
    {
        public bool isEmpty { get { return Assetname == "empty"; } }
        //public bool isText { get { return StringHelper.EndsWith(this.Assetname, ".txt") || StringHelper.EndsWith(this.Assetname, ".lua"); } }
        protected CResourceFactory Factory;

        /// <summary>
        /// 值越大 加载优先级越高
        /// </summary>
        protected PLevel Priority = PLevel.Low;

        public RenderResource(string filename, CResourceFactory factory, PLevel priority = PLevel.Low, float linger_time = 0f)
        {
            this.Assetname = filename;
            this.Factory = factory;
            this.Priority = priority;
            if (linger_time < 0f) linger_time = 0f;
            this.linger_time = linger_time;
        }

        public override IRenderObject CreateInstance(Type type, IRenderObject parent, params object[] args)
        {
            IRenderObject inst = AllocInstance(type, args);
            inst.SetOwner(this, this.Factory);
            inst.SetParent(parent);

            if (this.complete)
                inst.Create();

            this.insts.Add(inst);
            //LOG.Debug("------------------ create instance {0} asset {1}", type.Name, this.Assetname);
            return inst;
        }

        protected T AllocInstance<T>(params object[] args) where T : IRenderObject
        {
            return AllocInstance(typeof(T), args) as T;
        }

        protected IRenderObject AllocInstance(Type type, params object[] args)
        {
            if (args == null)
                return Activator.CreateInstance(type) as IRenderObject;
            else
                return Activator.CreateInstance(type, args) as IRenderObject;
        }

        public override IEnumerator Load()
        {
            loading = true;
            var itr = OnLoad();
            while (itr.MoveNext()) yield return null;
            loading = false;

            Create();
        }

        protected virtual IEnumerator OnLoad()
        {
            string path = CDirectory.MakeFullMobilePath(Assetname);
            if (isEmpty)
            {
                // Do nothing.
            }
            //else if (this.isText)
            //{
            //    // TXT文件都是放在缓存中的吗？
            //    this.text = new StringBuilder(File.ReadAllText(path));
            //}
            else
            {
                this.asset_bundle = AssetBundle.LoadFromFile(CDirectory.MakeFullMobilePath(Assetname));
                if (this.asset_bundle == null)
                {
                    //LOG.Erro(CString.Format("资源找不到{0}", this.Assetname));
                    //if (DebugSetting.EnableLog) this.Factory.FireEvent(new CEvent.UI.OpenUI("CGameCommonTipUI", CString.Format("资源找不到{0}", this.Assetname)));
                    ////2018-8-31 修改资源加载报错 CJ
                    Destroy();
                    yield break;
                }

                string[] assets = asset_bundle.GetAllAssetNames();
                if (assets == null || assets.Length == 0)
                {
                    //LOG.Erro(CString.Format("资源找不到{0}", this.Assetname));
                    //if (DebugSetting.EnableLog) this.Factory.FireEvent(new CEvent.UI.OpenUI("CGameCommonTipUI", CString.Format("资源找不到{0}", this.Assetname)));
                    ////2018-8-31 修改资源加载报错 CJ
                    Destroy();
                    yield break;
                }

                //if (this.Priority == PLevel.High || this.Priority == PLevel.Middle)
                //{
                //    // Toggle to next frame
                //    yield return null;

                //    this.asset = asset_bundle.LoadAsset(assets[0]);

                //    //声音设置成15分钟
                //    if (asset is AudioClip && linger_time == 0)
                //        linger_time = 900;
                //}
                //else
                {
                    var request = asset_bundle.LoadAssetAsync(assets[0]);
                    request.priority = (int)this.Priority;
                    while (!request.isDone)
                        yield return null;
                    this.asset = request.asset;
                    //声音设置成15分钟
                    //if (asset is AudioClip && linger_time == 0 && MobileHelper.GetDeviceLevel() == DeviceLevel.LEVEL_3)
                    //    linger_time = 900;
                }
            }
        }

        void Create()
        {
            this.complete = true;

            for (int i = 0; i < insts.Count; ++i)
                insts[i].Create();

            OnCreate();
        }

        protected virtual void OnCreate() { }

        public override void Destroy()
        {
            if (this.loading)
            {
                throw new Exception(string.Format("destroy resource {0} while loading", Assetname));
            }

            //LOG.Debug("++++++++++++++++++ destroy asset {0}", this.Assetname);
            OnDestroy();

            if (this.asset_bundle)
            {
                this.asset_bundle.Unload(true);
                this.asset_bundle = null;
            }

            for (int i = 0; i < insts.Count; ++i)
            {
                //if (insts[i] != null)
                //    insts[i].Destroy();
            }

            this.insts.Clear();
            this.complete = false;
            this.loading = false;
        }

        public override void RemoveInstance(IRenderObject obj)
        {
            if (!this.insts.Remove(obj))
                return;

            //LOG.Debug("------------------ remove instance {0} asset {1}", obj.GetType().Name, this.Assetname);

            /* 资源的引用计数为0 删除 */
            if (ReferenceCount == 0)
            {
                //this.Factory.RemoveResource(this);
            }
        }

        protected virtual void OnDestroy()
        {
            if (this.asset != null)
            {
                if (this.asset is RenderTexture)
                {
                    RenderTexture rt = this.asset as RenderTexture;
                    rt.Release();
                }
                //UnityEngine.Object.Destroy(this.asset);
                this.asset = null;
            }
        }
    }

    /// <summary>
    /// 场景资源对象
    /// </summary>
    //public class SceneResource : RenderResource
    //{
    //    private AssetBundleCreateRequest Request;
    //    //public SceneResource(string filename, CResourceFactory factory) : base(filename, factory) { }

    //    protected override IEnumerator OnLoad()
    //    {
    //        this.Request = AssetBundle.LoadFromFileAsync(CDirectory.MakeFullMobilePath(this.Assetname));
    //        for (int i = 0; i < insts.Count; ++i)
    //        {
    //            //insts[i].Create();
    //            //insts[i].SetData(this.Request);
    //        }

    //        yield return this.Request;

    //        this.asset_bundle = this.Request.assetBundle;
    //    }
    //}
}
