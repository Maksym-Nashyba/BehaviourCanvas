namespace Code
{
    public abstract class State<T0> : IState
    {
        public abstract void ResetStateParameters(T0 param0);
        public abstract void Start();
        public abstract void Update();
        public abstract void End();
    }
    
    public abstract class State<T0,T1> : IState
    {
        public abstract void ResetStateParameters(T0 param0, T1 param1);
        public abstract void Start();
        public abstract void Update();
        public abstract void End();
    }
    
    public abstract class State<T0,T1,T2> : IState
    {
        public abstract void ResetStateParameters(T0 param0, T1 param1, T2 param2);
        public abstract void Start();
        public abstract void Update();
        public abstract void End();
    }
}