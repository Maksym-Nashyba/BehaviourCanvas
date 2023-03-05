using System.Collections.Generic;

namespace Code.Runtime.BehaviourElementModels
{
    public interface IReadOnlyBehaviourElementModel
    {
        public int GetId();
        public Model GetModel();
        public List<IReadOnlyBehaviourElementModel> GetTargetModels();
    }
}