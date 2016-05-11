using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Hero
{
    class Elf : HeroBase
    {
        void Start()
        {
            speed = 1.3f;
            damage = 75f;
        }
    }
}
