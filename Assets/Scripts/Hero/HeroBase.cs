using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

namespace Assets.Scripts.Hero
{
    public abstract class HeroBase : Components.Generic.ControllerBase
    {
        // Player Number for imput and score purposes
        public int PortNum;

        public static readonly string[] HeroResourcePath = new string[4]
        {
            "Heroes/Warrior",
            "Heroes/Wizard",
            "Heroes/Elf",
            "Heroes/Valkyrie"
        };

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
        protected bool isLowHealth = false;

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
            _hitPoints.SetOnDamageEvent(OnDamageEvent);
            _hitPoints.SetOnHealEvent(OnHealEvent);

            _movement = gameObject.AddComponent<Components.Generic.VectorMovement2D>();
            rBody = GetComponent<Rigidbody>();
            if (rBody == null) { rBody = gameObject.AddComponent<Rigidbody>(); }

            _weapon = gameObject.AddComponent<Components.Generic.Weapon>();

            _input = gameObject.AddComponent<Input.InputManager>();
            _input.SetPortNum(PortNum);

            State = HeroState.State.Idle;
        }

        void Update()
        {
        }

        void FixedUpdate()
        {
            if (CurHitPoints <= 0f) { return; }

            moveDir = Vector3.zero;
            fireCounter += Time.deltaTime;

            DetermineCharacterState();

            switch (State)
            {
                case HeroState.State.Idle:
                    ProcessMotion();
                    break;
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
            
            if (_input.IsPausing())
            {
                SceneManager.Instance.TogglePause(PortNum);
            }
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
            if (fireCounter >= fireRate)
            {
                _weapon.Fire();
                fireCounter = 0f;
            }
        }

        void OnDamageEvent()
        {
            // Don't do anything if player is already dead
            if (State == HeroState.State.Dead) { return; }

            HUDManager.Instance.UpdatePlayerHealth(PortNum);

            if(CurHitPoints <= 0f)
            {
                Death();
                return;
            }

            if(!isLowHealth && CurHitPoints <= 300f)
            {
                SceneManager.Instance.PlayerIsLowHealth(PortNum);
            }

            if (CurHitPoints <= 300f)
            {
                isLowHealth = true;
            }
        }

        void OnHealEvent()
        {
            if(CurHitPoints > 300f)
            {
                isLowHealth = false;
            }

            HUDManager.Instance.UpdatePlayerHealth(PortNum);
        }

        protected void Death()
        {
            SceneManager.Instance.PlayerDied(PortNum);
            State = HeroState.State.Dead;
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
