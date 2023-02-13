namespace Code
{
    public abstract class State<T> : IState
    {
        public abstract void ResetStateParameters<T>();
        public abstract void Start();
        public abstract void Update();
        public abstract void End();
    }
    
    public abstract class State<T0,T1> : IState
    {
        public abstract void ResetStateParameters<T0,T1>();
        public abstract void Start();
        public abstract void Update();
        public abstract void End();
    }
    
    public abstract class State<T0,T1,T2> : IState
    {
        public abstract void ResetStateParameters<T0,T1,T2>();
        public abstract void Start();
        public abstract void Update();
        public abstract void End();
    }
}