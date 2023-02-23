namespace Code
{
    public interface IState
    {
        public void Start();

        public void Update();

        public void End();
    }
}