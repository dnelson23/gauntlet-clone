using UnityEngine;

namespace Assets.Scripts.Components
{
    class HitPoints : CustomComponentBase
    {
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

        public void TakeDamage(float amount)
        {
            curHitPoints -= amount;
            if(curHitPoints <= 0f)
            {
                _parent.GetComponent<ControllerBase>().Destroy();
            }
        }

        public void Heal(float amount)
        {
            curHitPoints += amount;
            if(curHitPoints < maxHitPoints)
            {
                curHitPoints = maxHitPoints;
            }
        }
    }
}
