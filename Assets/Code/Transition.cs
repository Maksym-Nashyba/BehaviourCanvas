namespace Code
{
    public abstract class Transition
    {
        public abstract bool ShouldFire();

        public abstract State Fire();
        
        public abstract void Reset();
    }
}