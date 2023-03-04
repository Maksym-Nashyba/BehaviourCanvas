namespace Code.Runtime.BehaviourElementModels
{
    public interface IReadOnlyBehaviourElementModel
    {
        public int GetId();
        public string GetName();
        public (string, string)[] GetParameters();
    }
}