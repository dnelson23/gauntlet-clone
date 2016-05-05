using UnityEngine;

namespace Assets.Scripts.Components
{
    /*
     * Base Controller class for creating object controllers
     */

    public abstract class ControllerBase : CustomComponentBase
    {
        protected override void Awake()
        {
            base.Awake();
            InitializeCustomComponents();
        }

        public virtual void Destroy()
        {
            GameObject.Destroy(gameObject);
        }

        void InitializeCustomComponents()
        {
            CustomComponentBase[] components = GetComponents<CustomComponentBase>();
            foreach (CustomComponentBase component in components)
            {
                component.Load(_parent.gameObject);
            }
        }
    }
}
