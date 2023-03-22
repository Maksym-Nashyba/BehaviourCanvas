using System.Collections.Generic;

namespace BehaviourCanvas.Code.Runtime.BehaviourGraphSerialization
{
    public interface IReadOnlyModelGraph
    {
        public IReadOnlyBehaviourElementModel GetRootState();
        public IReadOnlyDictionary<int, IReadOnlyBehaviourElementModel> GetIDStates();
        public IReadOnlyDictionary<int, IReadOnlyTriggerModel> GetIDTriggers();
    }
}