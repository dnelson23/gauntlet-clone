﻿using UnityEngine;

namespace Assets.Scripts.Pickups
{
    public enum Types { Key, Potion }

    public abstract class Item : MonoBehaviour
    {
        protected Types _type;
        public Types Type
        {
            get { return _type; }
        }
    }
}
