using UnityEngine;

namespace Assets.Scripts.Components.Generic
{
    public class HitPoints : CustomComponentBase
    {
        public delegate void HitPointsDelegate();
        protected HitPointsDelegate OnDamage;
        protected HitPointsDelegate OnHeal;

        public float curHitPoints { get; private set; }
        float maxHitPoints;

        public HitPoints()
        { }

        public HitPoints(float maxHP)
        {
            maxHitPoints = maxHP;
        }

        public void SetMaxHitPoints(float maxHP)
        {
            curHitPoints = maxHitPoints = maxHP;
        }

        public void SetOnDamageEvent(HitPointsDelegate dEvent)
        {
            OnDamage = dEvent;
        }

        public void SetOnHealEvent(HitPointsDelegate hEvent)
        {
            OnHeal = hEvent;
        }

        public void TakeDamage(float amount)
        {
            curHitPoints -= amount;
            if(curHitPoints < 0f)
            {
                curHitPoints = 0f;
            }

            OnDamage();
        }

        public void Heal(float amount)
        {
            curHitPoints += amount;
            if(curHitPoints < maxHitPoints)
            {
                curHitPoints = maxHitPoints;
            }

            OnHeal();
        }
    }
}
