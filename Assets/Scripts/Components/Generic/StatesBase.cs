﻿namespace Assets.Scripts.Components.Generic
{
    public abstract class StatesBase<T>
    {
        public abstract T currentState { get; protected set; }

        public virtual void ChangeState(T newState)
        {
            currentState = newState;
        }
    }
}
