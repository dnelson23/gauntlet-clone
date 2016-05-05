using UnityEngine;

namespace Assets.Scripts.Components
{
    class Weapon : CustomComponentBase
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
            newBullet.transform.rotation = Quaternion.Euler(90f, newBullet.transform.eulerAngles.y, newBullet.transform.eulerAngles.z);
            Bullet bul = newBullet.GetComponent<Bullet>();
            if(bul != null)
            {
                bul.SetMoveVector(transform.forward);
            }
            else
            {
                Bullet[] bullets = newBullet.GetComponentsInChildren<Bullet>();
                foreach(Bullet bullet in bullets)
                {
                    bullet.SetMoveVector(transform.forward);
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
