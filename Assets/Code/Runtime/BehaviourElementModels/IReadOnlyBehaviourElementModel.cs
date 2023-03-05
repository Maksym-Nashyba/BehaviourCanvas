namespace Code.Runtime.BehaviourElementModels
{
    public interface IReadOnlyBehaviourElementModel
    {
        public int GetId();
        public Model GetModel();
        public IReadOnlyBehaviourElementModel GetTargetModel();
    }
}