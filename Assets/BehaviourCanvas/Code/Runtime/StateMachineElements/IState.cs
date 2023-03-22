using Code.Runtime.BehaviourGraphSerialization;

namespace Code.Runtime.StateMachineElements
{
    public interface IState
    {
        public void Start();

        public void Update();

        public void End();

        public void Reset(params object[] arguments);

        public ParameterSet GetParameters();
    }
}