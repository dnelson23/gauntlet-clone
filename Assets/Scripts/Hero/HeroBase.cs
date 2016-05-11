using UnityEngine;
using System.Collections;
using Assets.Scripts;

namespace Assets.Scripts.Hero
{
    public abstract class HeroBase : Components.Generic.CustomComponentBase
    {
        Components.HeroType _type;
        public Components.HeroType Type
        {
            get { return _type; }
            private set { _type = value; }
        }

        HeroState _state;
        public HeroState.State State
        {
            get { return _state.currentState; }
            private set { _state.ChangeState(value); }
        }

        Components.Generic.HitPoints _hitPoints;
        public float CurHitPoints
        {
            get { return _hitPoints.curHitPoints; }
        }

        Components.Generic.VectorMovement2D _movement;
        float speed;

        Components.Generic.Weapon _weapon;
        float fireRate;

        float damage;

        void GetInput(ref float x, ref float y)
        {

        }
    }
}
