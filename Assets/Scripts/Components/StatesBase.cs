namespace Assets.Scripts.Components
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
