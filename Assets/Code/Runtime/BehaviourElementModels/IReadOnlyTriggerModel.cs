namespace Code.Runtime.BehaviourElementModels
{
    public interface IReadOnlyTriggerModel : IReadOnlyBehaviourElementModel
    {
        public bool GetResetTarget();
    }
}