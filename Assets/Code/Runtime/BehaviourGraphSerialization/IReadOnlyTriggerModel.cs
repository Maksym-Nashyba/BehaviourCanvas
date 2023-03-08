namespace Code.Runtime.BehaviourGraphSerialization
{
    public interface IReadOnlyTriggerModel : IReadOnlyBehaviourElementModel
    {
        public bool GetResetTarget();
    }
}