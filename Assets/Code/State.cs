namespace Code
{
    public abstract class State
    {
        public Transition[] Transitions { get; protected set; }

        public abstract void Start();
        
        public abstract void Update();
    }
}