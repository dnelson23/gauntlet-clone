namespace Assets.Scripts.Hero
{
    class Valkyrie : HeroBase
    {
        public float ValkyrieSpeed = 0.01f;
        public float ValkyrieFireRate = 0.5f;
        public float ValkyrieDamage = 75f;

        void Start()
        {
            PortNum = 4;
            _type = Components.HeroType.Valkyrie;
            speed = ValkyrieSpeed;
            fireRate = ValkyrieFireRate;
            damage = ValkyrieDamage;
        }
    }
}
