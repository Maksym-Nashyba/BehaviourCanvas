using Code.Runtime.BehaviourGraphSerialization;

namespace Code.Runtime.StateMachineElements
{
    public interface ITrigger
    {
        public bool IsHit();

        public IState PrepareTarget();

        public void Reset();
        
        public ParameterSet GetParameters();
    }
}