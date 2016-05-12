namespace Assets.Scripts.Hero
{
    public class HeroState : Components.Generic.StatesBase<HeroState.State>
    {
        public enum State { Idle, Move, Wait, Attack, Dead, Revive };

        State _state;
        public override State currentState { get; protected set; }

        public override void ChangeState(State newState)
        {
            if(_state == State.Dead && newState != State.Revive)
            {
                return;
            }

            _state = newState;
        }
    }
}
