using System.Collections.Generic;
using Code.Runtime.StateMachineElements;

namespace Code.Runtime.BehaviourGraphSerialization
{
    public interface IReadOnlyBehaviourElementModel
    {
        public int GetId();
        public Model GetModel();
        public List<IReadOnlyBehaviourElementModel> GetTargetModels();
        public abstract bool CanTarget(IReadOnlyBehaviourElementModel targetModel);
    }
}