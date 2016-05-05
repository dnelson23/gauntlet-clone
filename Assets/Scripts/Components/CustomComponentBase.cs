using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Components
{
    /*
     * Base class for custom components
     * Inherit this class on specialized custom components
     * Allows for very basic GameObject usage in components
     * 
     * Feel free to add to this script in implementation to fit your needs
     */

    public abstract class CustomComponentBase : MonoBehaviour
    {
        protected virtual void Awake()
        {
            Load(gameObject);
        }

        protected Transform _parent { get; set; }

        // When creating custom components this function may be overriden to adhere to any particular needs
        // See Movement custom component for an example
        public virtual void Load(GameObject parent)
        {
            _parent = parent.transform;
        }
    }
}
