namespace Assets.Scripts.Hero
{
    class Elf : HeroBase
    {
        public float ElfSpeed = 0.01f;
        public float ElfFireRate = 0.5f;
        public float ElfDamage = 75f;

        void Start()
        {
            PortNum = 3;
            _type = Components.HeroType.Elf;
            speed = ElfSpeed;
            fireRate = ElfFireRate;
            damage = ElfDamage;
        }
    }
}
