namespace Code.Runtime.StateMachineElements
{
    public interface IState
    {
        public void Start();

        public void Update();

        public void End();

        public void Reset(params object[] arguments);
    }
}