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

        protected HeroState _state = new HeroState();
        public HeroState.State State
        {
            get { return _state.currentState; }
            protected set { _state.ChangeState(value); }
        }

        protected Components.Generic.HitPoints _hitPoints;
        public float CurHitPoints
        {
            get { return _hitPoints.curHitPoints; }
        }
        protected float maxHealth = 1000f;

        protected Components.Generic.VectorMovement2D _movement;
        protected float speed = 1f;
        protected Vector3 moveDir = Vector3.zero;
        protected Rigidbody rBody;

        protected Components.Generic.Weapon _weapon;
        protected float fireRate = 1f;
        protected float fireCounter = 0f;
        protected float damage = 100f;

        protected int keys = 0;
        protected int potions = 0;

        protected Input.InputManager _input;

        protected override void Awake()
        {
            base.Awake();
            _hitPoints = gameObject.AddComponent<Components.Generic.HitPoints>();
            _hitPoints.SetMaxHitPoints(maxHealth);

            _movement = gameObject.AddComponent<Components.Generic.VectorMovement2D>();
            rBody = GetComponent<Rigidbody>();
            if (rBody == null) { rBody = gameObject.AddComponent<Rigidbody>(); }

            _weapon = gameObject.AddComponent<Components.Generic.Weapon>();

            _input = gameObject.AddComponent<Input.InputManager>();
            _input.SetPortNum(PortNum);
        }

        void Update()
        {
            moveDir = Vector3.zero;
            fireCounter += Time.deltaTime;

            DetermineCharacterState();

            switch (State)
            {
                case HeroState.State.Move:
                    ProcessMotion();
                    break;
                case HeroState.State.Wait:
                    break;
                case HeroState.State.Attack:
                    Attack();
                    break;
                default:
                    break;
            }

            _movement.SetMoveVector(moveDir * speed);
        }

        protected void DetermineCharacterState()
        {
            if (_input.IsAttacking())
            {
                State = HeroState.State.Attack;
                return;
            }

            if (_input.GetVerticalAxis() != 0f || _input.GetHorizontalAxis() != 0f)
            {
                State = HeroState.State.Move;
                return;
            }

            State = HeroState.State.Idle;
        }

        protected void ProcessMotion()
        {
            float x = 0f;
            float z = 0f;
            GetInput(ref x, ref z);

            moveDir = new Vector3(x, 0f, z);
            _movement.Rotate(moveDir);
        }

        protected void Attack()
        {
            Debug.Log("attacking");
            return;

            if (fireCounter >= fireRate)
            {
                _weapon.Fire();
                fireCounter = 0f;
            }
        }

        protected void GetInput(ref float x, ref float y)
        {
            x = _input.GetHorizontalAxis();
            y = _input.GetVerticalAxis();
        }

        void OnCollisionEnter(Collision col)
        {
            Pickups.Item item = col.gameObject.GetComponent<Pickups.Item>();
            if(item == null)
            {
                return;
            }

            Pickups.Types type = item.Type;
            Destroy(item.gameObject);

            switch(type)
            {
                case Pickups.Types.Key:
                    if (keys < 4)
                    {
                        keys++;
                        HUDManager.Instance.ShowPlayerKey(PortNum, keys);
                    }
                    break;
                case Pickups.Types.Potion:
                    if(potions < 4)
                    {
                        potions++;
                        HUDManager.Instance.ShowPlayerPot(PortNum, potions);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
