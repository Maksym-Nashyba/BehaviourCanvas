using System.Collections.Generic;
using System.Linq;

namespace Code.Runtime.BehaviourGraphSerialization
{
    public readonly struct ModelGraph : IReadOnlyModelGraph
    {
        private IReadOnlyBehaviourElementModel RootState => _statesDictionary[StateModel.RootId];
        private readonly Dictionary<int, IReadOnlyBehaviourElementModel> _statesDictionary;
        private readonly Dictionary<int, IReadOnlyTriggerModel> _triggersDictionary;
        
        public ModelGraph(IEnumerable<IReadOnlyBehaviourElementModel> states,
            IEnumerable<IReadOnlyTriggerModel> triggers)
        {
            _statesDictionary = states.ToDictionary(state => state.GetId());
            _triggersDictionary = triggers.ToDictionary(trigger => trigger.GetId());
        }
        
        public void AddTargetModel(int startModelId, int targetModelId)
        {
            if (GetModel(startModelId) is not BehaviourElementModel startModel) return;
            if(startModel.GetTargetModels() == null) 
                startModel.SetTargetModels = new List<IReadOnlyBehaviourElementModel>();
            startModel.GetTargetModels().Add(GetModel(targetModelId));
        }

        public void ClearTargetModel(int modelId)
        {
            if (GetModel(modelId) is BehaviourElementModel model) model.SetTargetModels = null;
        }

        private IReadOnlyBehaviourElementModel GetModel(int modelId)
        {
            IReadOnlyBehaviourElementModel model = null;
            if (_statesDictionary.TryGetValue(modelId, out IReadOnlyBehaviourElementModel stateModel))
            {
                model = stateModel;
            }
            else if (_triggersDictionary.TryGetValue(modelId, out IReadOnlyTriggerModel triggerModel))
            {
                model = triggerModel;
            }
            return model;
        }

        #region IReadOnlyGraphModel
        public IReadOnlyBehaviourElementModel GetRootState()
        {
            return RootState;
        }

        public IReadOnlyDictionary<int, IReadOnlyBehaviourElementModel> GetStates()
        {
            return _statesDictionary;
        }

        public IReadOnlyDictionary<int, IReadOnlyTriggerModel> GetTriggers()
        {
            return _triggersDictionary;
        }
        #endregion
        
        #region States
        public void SetRootState(int stateId)
        {
            BehaviourElementModel newRootState = _statesDictionary[stateId] as BehaviourElementModel;
            if (RootState is BehaviourElementModel oldRootState) oldRootState.Id = stateId;
            if (newRootState != null) newRootState.Id = StateModel.RootId;
        }
        
        public bool IsState(int modelId) => _statesDictionary.ContainsKey(modelId);

        public void AddState(StateModel state)
        {
            _statesDictionary.Add(state.Id, state);
        }

        public void RemoveState(int stateId)
        {
            _statesDictionary.Remove(stateId);
        }
        #endregion
        
        #region Triggers
        public bool IsTrigger(int modelId) => _triggersDictionary.ContainsKey(modelId);

        public void AddTrigger(TriggerModel trigger)
        {
            _triggersDictionary.Add(trigger.Id, trigger);
        }

        public void RemoveTrigger(int triggerId)
        {
            _triggersDictionary.Remove(triggerId);
        }
        #endregion
    }
}