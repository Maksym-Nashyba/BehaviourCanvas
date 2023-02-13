using Code.Templates;

namespace Code
{
    public interface ITrigger
    {
        public bool IsHit();

        public IState PrepareTarget();

        public void Reset();
    }
}