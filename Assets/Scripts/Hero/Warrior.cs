using System;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Hero
{
    class Warrior : HeroBase
    {
        public float WarriorSpeed = 0.008f;
        public float WarriorFireRate = 0.4f;
        public float WarriorDamage = 100f;

        void Start()
        {
            _type = Components.HeroType.Warrior;
            speed = WarriorSpeed;
            fireRate = WarriorFireRate;
            damage = WarriorDamage;
        }
    }
}
