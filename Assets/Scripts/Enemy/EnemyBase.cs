using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Enemy
{
    public abstract class EnemyBase : Components.ControllerBase
    {
        Components.HitPoints _health;
        protected float maxHP = 100f;
        public float CurrentHP
        {
            get { return _health.curHitPoints; }
            private set { _health.SetMaxHitPoints(value); }
        }

        Components.VectorMovement2D _movement;

        protected float damage = 100f;

        protected override void Awake()
        {
            base.Awake();

            _movement = _parent.gameObject.AddComponent<Components.VectorMovement2D>();

            _health = _parent.gameObject.AddComponent<Components.HitPoints>();
            CurrentHP = maxHP;
        }
        
        void Update() { }

        public override void Destroy() { }
    }
}
