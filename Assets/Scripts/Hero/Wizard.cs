namespace Assets.Scripts.Hero
{
    class Wizard : HeroBase
    {
        public float WizSpeed = 0.01f;
        public float WizFireRate = 0.5f;
        public float WizDamage = 75f;

        void Start()
        {
            PortNum = 2;
            _type = Components.HeroType.Wizard;
            speed = WizSpeed;
            fireRate = WizFireRate;
            damage = WizDamage;
        }
    }
}
