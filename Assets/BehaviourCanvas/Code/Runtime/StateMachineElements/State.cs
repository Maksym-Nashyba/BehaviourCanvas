using Code.Runtime.BehaviourGraphSerialization;

namespace Code.Runtime.StateMachineElements
{
    public abstract class State : IState
    {
        protected abstract void ResetStateParameters();
        public abstract void Start();
        public abstract void Update();
        public abstract void End();
        public abstract ParameterSet GetParameters();
        public void Reset(params object[] arguments)
        {
            ResetStateParameters();
        }
    }

    public abstract class State<T0> : IState
    {
        protected abstract void ResetStateParameters(T0 param0);
        public abstract void Start();
        public abstract void Update();
        public abstract void End();
        public abstract ParameterSet GetParameters();
        public void Reset(params object[] arguments)
        {
            ResetStateParameters((T0)arguments[0]);
        }
    }
    
    public abstract class State<T0,T1> : IState
    {
        protected abstract void ResetStateParameters(T0 param0, T1 param1);
        public abstract void Start();
        public abstract void Update();
        public abstract void End();
        public abstract ParameterSet GetParameters();
        public void Reset(params object[] arguments)
        {
            ResetStateParameters((T0)arguments[0], (T1)arguments[1]);
        }
    }
    
    public abstract class State<T0,T1,T2> : IState
    {
        protected abstract void ResetStateParameters(T0 param0, T1 param1, T2 param2);
        public abstract void Start();
        public abstract void Update();
        public abstract void End();
        public abstract ParameterSet GetParameters();
        public void Reset(params object[] arguments)
        {
            ResetStateParameters((T0)arguments[0], (T1)arguments[1], (T2)arguments[2]);
        }
    }
}