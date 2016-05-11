using UnityEngine;
using System.Collections;
using Assets.Scripts;

namespace Assets.Scripts.Hero
{
    public abstract class HeroBase : Components.Generic.CustomComponentBase
    {
        // Player Number for imput and score purposes
        public int PortNum;

        protected Components.HeroType _type;
        public Components.HeroType Type
        {
            get { return _type; }
            private set { _type = value; }
        }

        protected HeroState _state;
        public HeroState.State State
        {
            get { return _state.currentState; }
            private set { _state.ChangeState(value); }
        }

        protected Components.Generic.HitPoints _hitPoints;
        public float CurHitPoints
        {
            get { return _hitPoints.curHitPoints; }
        }
        protected float maxHealth;

        protected Components.Generic.VectorMovement2D _movement;
        protected float speed;

        protected Components.Generic.Weapon _weapon;
        protected float fireRate;
        protected float damage;

        protected Input.InputManager _input;

        void GetInput(ref float x, ref float y)
        {
            x = _input.GetHorizontalAxis();
            y = _input.GetVerticalAxis();
        }
    }
}
