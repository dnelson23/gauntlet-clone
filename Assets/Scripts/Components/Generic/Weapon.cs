using UnityEngine;

namespace Assets.Scripts.Components.Generic
{
    public class Weapon : CustomComponentBase
    {
        GameObject BulletPrefab;
        
        public void Fire()
        {
            if(BulletPrefab == null)
            {
                Debug.LogWarning("Weapon has no bullet object");
                return;
            }

            GameObject newBullet = Instantiate(BulletPrefab, transform.position, transform.rotation) as GameObject;
            Vector3 fireVector = transform.forward * 0.3f;
            Bullet bul = newBullet.GetComponent<Bullet>();
            if(bul != null)
            {
                bul.SetMoveVector(fireVector);
            }
            else
            {
                Bullet[] bullets = newBullet.GetComponentsInChildren<Bullet>();
                foreach(Bullet bullet in bullets)
                {
                    bullet.SetMoveVector(fireVector);
                }
            }
        }

        public void SetBullet(GameObject newBullet)
        {
            if(newBullet == null)
            {
                throw new System.ArgumentNullException("Weapon Bullet", "Cannot set weapon bullet to null");
            }

            BulletPrefab = newBullet;
        }
    }
}
