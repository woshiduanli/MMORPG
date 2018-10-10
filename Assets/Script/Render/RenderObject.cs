using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ZRender
{
    /// <summary>
    /// 资源实例基类,由IRenderResource实例化出来
    /// </summary>
    public abstract class IRenderObject : PListNode
    {
        private int HashID_ = Def.INVALID_ID;
        public int HashID
        {
            get
            {
                if (HashID_ == Def.INVALID_ID)
                    HashID_ = this.GetHashCode();
                return HashID_;
            }
        }
        public int FixedStep = 1;
        public bool complete { get; private set; }
        private bool visible = true;
        //private float opacity = 1.0f;
        private Vector3 init_position = Vector3.zero;
        private Vector3 init_scale = Vector3.one;
        private Quaternion init_rotation = Quaternion.identity;
        private Transform parent_transform = null;
        private bool set_parent_transform = false;
        private int layer;
        protected int SortingOrder = -1;
        public IRenderResource owner { get; private set; }
        private IRenderObject parent;
        private PList children;
        private Map<Type, IController> ctrls;
        private TimerEvent timer;
        private CResourceFactory Factory;
        private List<Renderer> Renderers = new List<Renderer>();
        //private bool unsafedestroy;
        public bool destroy { get; private set; }

        public GameObject gameObject { get; protected set; }

        public void AddChild(IRenderObject child)
        {
            if (children == null)
            {
                children = new PList();
                children.Init();
            }
            children.AddTail(child);
            if (this.gameObject && child.gameObject)
                child.gameObject.transform.parent = this.gameObject.transform;
        }

        public void RemoveChild(IRenderObject child)
        {
            if (this.children == null)
                return;
            children.Remove(child);
            if (this.gameObject &&
                child.gameObject &&
                child.gameObject.transform.parent == this.gameObject.transform)
            {
                child.gameObject.transform.parent = null;
            }
        }

        public T AddController<T>() where T : IController, new()
        {
            if (ctrls == null)
                ctrls = new Map<Type, IController>();
            T c = new T();
            c.Create(this);
            ctrls.Add(typeof(T), c);
            return c;
        }

        public void RemoveController<T>() where T : IController
        {
            if (ctrls == null)
                return;
            if (ctrls.ContainsKey(typeof(T)))
            {
                IController c = ctrls[typeof(T)];
                if (c != null)
                    c.Destroy();
                ctrls.Remove(typeof(T));
            }
        }

        public void RemoveController(IController ctrl)
        {
            if (ctrls == null)
                return;
            if (ctrls.ContainsKey(ctrl.GetType()))
            {
                IController c = ctrls[ctrl.GetType()];
                if (c != null)
                    c.Destroy();
                ctrls.Remove(ctrl.GetType());
            }
        }

        public T GetController<T>() where T : IController
        {
            if (ctrls == null)
                return default(T);
            if (ctrls.ContainsKey(typeof(T)))
            {
                return ctrls[typeof(T)] as T;
            }
            else
            {
                return default(T);
            }
        }

        public int GetInstanceId()
        {
            if (this.gameObject)
                return this.gameObject.GetInstanceID();
            return Def.INVALID_ID;
        }

        public IRenderObject GetParent() { return this.parent; }

        public void SetParent(IRenderObject parent)
        {
            if (this.parent != null)
            {
                this.parent.RemoveChild(this);
            }

            this.parent = parent;
            if (this.parent != null)
            {
                this.parent.AddChild(this);
            }
        }

        public void SetParentTransform(Transform ts)
        {
            // ensure that the parent transform is
            if (ts == null)
            {
                // fix when parent tranform has been destroyed after create
                if (this.set_parent_transform)
                    Destroy();
                return;
            }

            if (this.gameObject)
            {
                this.gameObject.transform.parent = ts;
                CClientCommon.ZeroTransform(this.gameObject.transform);
            }


            //这里会重置init_position等数据导致位置不对
            //SetPosition(Vector3.zero);
            //SetRotation(Vector3.zero);
            //SetScale(Vector3.one);
            this.parent_transform = ts;
            this.set_parent_transform = true;
        }

        public IRenderResource GetOwner() { return this.owner; }

        public void SetOwner(IRenderResource owner, CResourceFactory factory)
        {
            this.owner = owner;
            this.Factory = factory;
        }

        public int GetGameQuality()
        {
            //if (this.Factory == null)
            //    return (int)GameSetSystem.Quality.HD;
            //GameSetSystem gameSet = this.Factory.GetSingleT<GameSetSystem>();
            //if (gameSet != null)
            //    return gameSet.quality;
            //return (int)GameSetSystem.Quality.HD;
            return 9; 
        }

        public Vector3 GetPosition()
        {
            if (this.gameObject)
                return this.gameObject.transform.position;
            else
                return init_position;
        }

        public Quaternion GetRotation()
        {
            if (this.gameObject)
                return this.gameObject.transform.rotation;
            else
                return init_rotation;
        }

        public void SetPosition(Vector3 position)
        {
            if (this.gameObject)
            {
                this.gameObject.transform.localPosition = position;
            }
            this.init_position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            if (this.gameObject)
            {
                this.gameObject.transform.localRotation = rotation;
            }
            this.init_rotation = rotation;
        }

        public void SetRotation(Vector3 euler)
        {
            Quaternion rotation = Quaternion.Euler(euler);
            SetRotation(rotation);
        }

#if false
        public void LookAt (Vector3 position)
        {
            Vector3 vec = position - this.init_position;
            vec.y = 0f;
            vec.Normalize ();
            Quaternion quaternion = Quaternion.LookRotation (vec);
            SetRotation (quaternion);
        }
#endif

        public Vector3 GetForward()
        {
            if (this.gameObject)
                return this.gameObject.transform.forward;
            else
            {
                return Vector3.zero;
            }
        }

        public void SetForward(Vector3 forward)
        {
            Quaternion quaternion = Quaternion.LookRotation(forward);
            SetRotation(quaternion);
        }

        public virtual void SetScale(Vector3 scale)
        {
            if (this.gameObject)
                this.gameObject.transform.localScale = scale;
            this.init_scale = scale;
        }

        public Vector3 GetScale()
        {
            return this.init_scale;
        }

        public bool IsVisible() { return visible; }

        /// <summary>
        /// 隐藏或显示render
        /// </summary>
        /// <param name="visible"></param>
        /// <param name="hideInBodyEff">true为隐藏角色身上的特效，false为不隐藏</param>
        public void SetVisible(bool visible, bool hideInBodyEff = true)
        {
            this.visible = visible;

            if (!complete)
                return;

            this.CacheRenders();
            for (int i = 0; i < Renderers.Count; i++)
            {
                if (!Renderers[i])
                    continue;
                if (!this.visible)
                {
                    bool isEffect = Renderers[i].gameObject.layer == CDefines.Layer.Effect;
                    if (isEffect)
                    {
                        Renderers[i].enabled = !hideInBodyEff;
                        continue;
                    }
                }
                Renderers[i].enabled = this.visible;
            }
            OnVisible();
        }

        public int GetLayer() { return this.layer; }

        public virtual void SetLayer(int layer)
        {
            if (layer == 0)
                return;
            this.layer = layer;

            if (children != null)
            {
                for (PListNode n = children.next; n != children; n = n.next)
                {
                    IRenderObject child = (IRenderObject)n;
                    if (child.layer == 0)
                        child.layer = layer;
                }
            }

            if (this.gameObject)
                SetLayerRecursively(this.gameObject);

            this.OnSetLayer();
        }

        public int GetSortingOrder() { return this.SortingOrder; }

        public virtual void SetSortingOrder(int order)
        {
            if (order < 0)
                return;
            this.SortingOrder = order;

            if (children != null)
            {
                for (PListNode n = children.next; n != children; n = n.next)
                {
                    IRenderObject child = (IRenderObject)n;
                    child.SetSortingOrder(order);
                }
            }

            this.CacheRenders();

            foreach (Renderer r in Renderers)
            {
                if (!r)
                    continue;
                r.sortingOrder = order;
            }
        }

        /*
        public void SetLayer(int layer, int ignore_layer, bool ignore_children) {
            if (this.gameObject ) {
                if ((gameObject.layer & ignore_layer) == 0)
                    gameObject.layer = layer;
                if (!ignore_children) {

                }
            }
        }
        */

        private void SetLayerRecursively(GameObject go)
        {
            go.layer = this.layer;
            this.CacheRenders();
            foreach (var r in Renderers)
            {
                if (!r)
                    continue;
                r.gameObject.layer = this.layer;
            }
        }

        protected void ApplyParticleScale(float scale)
        {
            if (this.gameObject == null)
                return;
            //CClientCommon.ApplyParticleScale(this.gameObject.transform, scale);
        }

        public void Create()
        {
            if (destroy)
                return;
            complete = true;
            this.OnCreate();
            this.CacheRenders(true);

            if (this.parent != null)
            {
                if (this.gameObject && this.parent.gameObject)
                {
                    this.gameObject.transform.parent = this.parent.gameObject.transform;
                }

                // Inherit the parent's layer, when child doesn't assign a layer.
                if (this.layer == 0 && this.parent.layer != 0)
                    this.layer = this.parent.layer;
            }

            ApplyInitPosition();

            if (children != null)
            {
                for (PListNode n = children.next; n != children; n = n.next)
                {
                    IRenderObject child = (IRenderObject)n;
                    if (this.gameObject && child.gameObject != null)
                    {
                        child.gameObject.transform.parent = this.gameObject.transform;
                        child.ApplyInitPosition();
                    }
                }
            }

            SetLayer(this.layer);
            SetSortingOrder(this.SortingOrder);
            if (!this.visible)
                SetVisible(this.visible);
        }

        protected void CacheRenders(bool whatever = false)
        {
            if (!this.gameObject)
                return;

            if (whatever && Renderers.Count > 0)
                Renderers.Clear();

            if (Renderers.Count > 0)
                return;

            Renderer renderer = this.gameObject.GetComponent<Renderer>();
            if (renderer)
                Renderers.Add(renderer);

            Renderer[] renderers = this.gameObject.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer r in renderers)
                Renderers.Add(r);
        }

        public void Destroy()
        {
            if (destroy)
                return;
            destroy = true;
            try
            {
                if (children != null)
                {
                    for (PListNode n = children.next, next; n != children; n = next)
                    {
                        next = n.next;
                        IRenderObject child = (IRenderObject)n;
                        if (child != null)
                            child.SetParent(null);
                    }
                    children = null;
                }

                if (ctrls != null)
                {
                    for (ctrls.Begin(); ctrls.Next();)
                    {
                        if (ctrls.Value != null)
                            ctrls.Value.Destroy();
                    }
                    ctrls.Clear();
                    ctrls = null;
                }

                if (timer != null)
                {
                    timer.Clear();
                    timer = null;
                }

                this.SetParent(null);
                this.OnDestroy();
                if (this.owner != null)
                {
                    this.owner.RemoveInstance(this);
                    this.owner = null;
                }

            }
            catch (Exception e)
            {
                LOG.LogError(e.ToString(), this.gameObject);
            }
        }

        public void Update()
        {
            if (destroy)
                return;


            if (ctrls != null)
            {
                if (Time.frameCount % FixedStep == 0)
                {
                    for (ctrls.Begin(); ctrls.Next();)
                    {
                        if (ctrls.Value != null && ctrls.Value.enabled)
                            ctrls.Value.Update();
                    }
                }
            }

            OnUpdate();

            if (children != null)
            {
                for (PListNode n = children.next, next; n != children; n = next)
                {
                    next = n.next;
                    IRenderObject child = (IRenderObject)n;
                    if (child != null)
                        child.Update();
                }
            }

            if (timer != null)
                timer.Process();
        }

        public void LateUpdate()
        {
            if (destroy)
                return;

            if (ctrls != null)
            {
                for (ctrls.Begin(); ctrls.Next();)
                {
                    if (ctrls.Value != null && ctrls.Value.enabled)
                        ctrls.Value.LateUpdate();
                }
            }
            OnLateUpdate();
        }

        protected virtual void OnVisible() { }
        protected virtual void OnSkinVisible() { }
        protected virtual void OnCreate() { }
        public virtual void SetData(object data) { }
        protected virtual void OnSetLayer() { }
        protected virtual void OnDestroy() { }
        protected virtual void OnUpdate() { }

        protected virtual void OnLateUpdate() { }

        protected void ApplyInitPosition()
        {
            if (this.gameObject == null)
                return;
            SetParentTransform(this.parent_transform);
            SetPosition(this.init_position);
            SetRotation(this.init_rotation);
            SetScale(this.init_scale);
        }

        //--------------------------------------------------------------------
        public TimerEventObject AddTimer(TimerEventObject.TimerProc proc, object obj, int p1, int p2)
        {
            return AddTimer(1, 1, proc, obj, p1, p2);
        }

        public TimerEventObject AddTimer(float time, TimerEventObject.TimerProc proc, object obj, int p1, int p2)
        {
            int frame = Convert.ToInt32(Application.targetFrameRate * time);
            return AddTimer(frame, frame, proc, obj, p1, p2);
        }

        private TimerEventObject AddTimer(int start, int interval, TimerEventObject.TimerProc proc, object obj, int p1, int p2)
        {
            if (timer == null)
                timer = new TimerEvent(1);
            return timer.Add(start, interval, proc, obj, p1, p2);
        }

        public void RemoveTimer(TimerEventObject proc)
        {
            if (timer == null)
                return;
            timer.Remove(proc);
        }

    }
}