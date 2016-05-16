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

		protected float damage = 100f;

		public bool isVisible = false;

		protected override void Awake()
		{
			base.Awake();

			_health = _parent.gameObject.AddComponent<Components.Generic.HitPoints>();
			CurrentHP = maxHP;

            transform.position = new Vector3(transform.position.x, 0.33f, transform.position.z);
		}

		void Update() { }

        void FixedUpdate()
        {
            if (GetComponentInChildren<Renderer>().IsVisibleFrom(Camera.main))
            {
                isVisible = true;
            }
            else
            {
                isVisible = false;
            }
        }

		public override void Destroy() { }



        void OnCollisionStay(Collision other)
        {
            Debug.Log(other.gameObject.name);
            Hero.HeroBase player = other.gameObject.GetComponent<Hero.HeroBase>();
            Components.Generic.HitPoints playerHP = other.gameObject.GetComponent<Components.Generic.HitPoints> ();
            if (player != null && playerHP != null)
            {
                playerHP.TakeDamage (damage);
            }
        }



		//Find closest Player
		public GameObject FindClosestAlivePlayer()
		{
			float distance = Mathf.Infinity;
			Hero.HeroBase[] heroRefs = FindObjectsOfType<Hero.HeroBase> ();
			GameObject newTarget = null;
			foreach (Hero.HeroBase player in heroRefs) {
				if(Vector3.Distance (this.transform.position, player.gameObject.transform.position) < distance)
				{
					newTarget = player.gameObject;
					distance = Vector3.Distance(this.transform.position, newTarget.transform.position);
				}
			}
			if (newTarget == null)
				Debug.Log ("No Players are Alive");
			return newTarget;
		}
    }
}
