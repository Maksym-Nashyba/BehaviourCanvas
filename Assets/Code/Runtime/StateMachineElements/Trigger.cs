using Code.Runtime.BehaviourGraphSerialization;

namespace Code.Runtime.StateMachineElements 
{
    public abstract class Trigger : ITrigger
    {
        public abstract bool IsHit();

        public abstract IState PrepareTarget();
        
        public abstract void Reset();
        
        public abstract ParameterSet GetParameters();
    }
    
    public abstract class Trigger<T0> : ITrigger
    {
        public abstract bool IsHit();

        public abstract IState PrepareTarget();
        
        public abstract void Reset();
        
        public abstract ParameterSet GetParameters();
    }
    
    public abstract class Trigger<T0, T1> : ITrigger
    {
        public abstract bool IsHit();

        public abstract IState PrepareTarget();

        public abstract void Reset();
        
        public abstract ParameterSet GetParameters();
    }
    
    public abstract class Trigger<T0, T1, T2> : ITrigger
    {
        public abstract bool IsHit();

        public abstract IState PrepareTarget();

        public abstract void Reset();
        
        public abstract ParameterSet GetParameters();
    }
}