using System.Collections.Generic;

namespace Code.Runtime.BehaviourElementModels
{
    public interface IReadOnlyModelGraph
    {
        public IReadOnlyBehaviourElementModel GetRootState();
        public IReadOnlyDictionary<int, IReadOnlyBehaviourElementModel> GetStates();
        public IReadOnlyDictionary<int, IReadOnlyTriggerModel> GetTriggers();
    }
}