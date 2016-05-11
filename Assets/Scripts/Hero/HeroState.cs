namespace Assets.Scripts.Hero
{
    public class HeroState : Components.Generic.StatesBase<HeroState.State>
    {
        public enum State { Idle, Wait, Attack };

        State _state;
        public override State currentState { get; protected set; }
    }
}
