using System.Collections.Generic;

namespace Code.Runtime.BehaviourGraphSerialization
{
    public interface IReadOnlyModelGraph
    {
        public IReadOnlyBehaviourElementModel GetRootState();
        public IReadOnlyDictionary<int, IReadOnlyBehaviourElementModel> GetStates();
        public IReadOnlyDictionary<int, IReadOnlyTriggerModel> GetTriggers();
    }
}