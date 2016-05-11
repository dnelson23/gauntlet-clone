namespace Assets.Scripts.Components.Generic
{
    public abstract class StatesBase<T>
    {
        public abstract T currentState { get; protected set; }

        public void ChangeState(T newState)
        {
            currentState = newState;
        }
    }
}
