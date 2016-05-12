using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Enemy
{
    public abstract class EnemyBase : Components.Generic.ControllerBase
    {
        Components.Generic.HitPoints _health;
        protected float maxHP = 100f;
        public float CurrentHP
        {
            get { return _health.curHitPoints; }
            private set { _health.SetMaxHitPoints(value); }
        }

        Components.Generic.VectorMovement2D _movement;

        protected float damage = 100f;

        public bool isVisible = false;

        protected override void Awake()
        {
            base.Awake();

            _movement = _parent.gameObject.AddComponent<Components.Generic.VectorMovement2D>();

            _health = _parent.gameObject.AddComponent<Components.Generic.HitPoints>();
            CurrentHP = maxHP;
        }
        
        void Update() { }

        public override void Destroy() { }
    }
}
