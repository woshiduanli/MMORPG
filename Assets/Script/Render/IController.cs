using System;
using UnityEngine;

namespace ZRender
{

    // 控制器基类
    public abstract class IController
    {
        public IRenderObject RenderObject { get; private set; }
        public bool enabled { get; set; }

        internal void Create(IRenderObject owner)
        {
            this.RenderObject = owner;
            this.enabled = true;
            OnCreate();
        }

        internal void Destroy()
        {
            OnDestroy();
            this.RenderObject = null;
        }

        public virtual void Update() { }

        public virtual void LateUpdate() { }

        protected virtual void OnCreate() { }
        protected virtual void OnDestroy() { }
    }
}