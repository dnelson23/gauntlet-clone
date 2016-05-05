using UnityEngine;

namespace Assets.Scripts.Components
{
    class Bullet : CustomComponentBase
    {
        Vector3 moveVector = Vector3.zero;
        public float damage = 100f;
        public float speed = 1f;
        public bool hasTTL = false;
        public float maxTimeAlive = 5f;
        float timeCounter = 0f;

        public void SetMoveVector(Vector3 vect)
        {
            moveVector = vect * speed;
        }

        void Update()
        {
            _parent.position = _parent.position + moveVector;

            if (hasTTL)
            {
                CheckTimeToLive();
            }
        }

        void OnCollisionEnter(Collision col)
        {
            HitPoints collidedHealth = col.gameObject.GetComponent<HitPoints>();

            if (collidedHealth)
            {
                collidedHealth.TakeDamage(damage);
                Destroy(gameObject);
            }
        }

        void CheckTimeToLive()
        {
            if (timeCounter >= maxTimeAlive)
            {
                Destroy(gameObject);
            }
            timeCounter += Time.deltaTime;
        }
    }
}
