using UnityEngine;

namespace Toast.Kit.Manager.Ui
{
    internal abstract class UiView
    {
        protected readonly ToastKitManagerWindow window;

        protected UiView(ToastKitManagerWindow window)
        {
            this.window = window;
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnDisable()
        {
        }

        public virtual void OnDestroy()
        {
        }

        public abstract void OnGUI(Rect rect);

        public virtual void Update()
        {
        }

        public virtual void Clear()
        {
        }
    }
}