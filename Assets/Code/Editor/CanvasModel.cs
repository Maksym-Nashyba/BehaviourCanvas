using System;
using System.Collections.Generic;
using System.Linq;
using Code.Runtime.BehaviourElementModels;

namespace Code.Editor
{
    public class CanvasModel
    {
        public event Action Changed;
        public event Action<IReadOnlyBehaviourElementModel> ModelAdded;
        public event Action<int> ModelRemoved; //TODO subscribe runtimeXml serialize

        public IReadOnlyBehaviourElementModel RootState => _stateDictionary[StateModel.RootId];
        public IReadOnlyCollection<IReadOnlyBehaviourElementModel> States => _stateDictionary.Values;
        public IReadOnlyCollection<IReadOnlyTriggerModel> Triggers => _triggerDictionary.Values;

        private readonly Dictionary<int, IReadOnlyBehaviourElementModel> _stateDictionary;
        private readonly Dictionary<int, IReadOnlyTriggerModel> _triggerDictionary;

        public CanvasModel()
        {
            _stateDictionary = new Dictionary<int, IReadOnlyBehaviourElementModel>();
            _triggerDictionary = new Dictionary<int, IReadOnlyTriggerModel>();
        }

        public void Deserialize(List<StateModel> states, List<TriggerModel> triggers, List<(int, int[])> modelsAndTargets)
        {
            foreach (StateModel state in states)
            {
                _stateDictionary.Add(state.Id, state);
            }
            foreach (TriggerModel trigger in triggers)
            {
                _triggerDictionary.Add(trigger.Id, trigger);
            }
            foreach ((int, int[]) idsPair in modelsAndTargets)
            {
                foreach (int targetId in idsPair.Item2)
                {
                    AddTargetModel(idsPair.Item1, targetId);
                }
            }
            Changed?.Invoke();
        }

        public int GetCurrentBiggestId()
        {
            int firstId = _stateDictionary.Count != 0 ? _stateDictionary.Max(pair => pair.Value.GetId()) : StateModel.RootId - 1;
            int secondId = _triggerDictionary.Count != 0 ? _triggerDictionary.Max(pair => pair.Value.GetId()) : StateModel.RootId - 1;
            return Math.Max(firstId, secondId);
        }

        public void AddTargetModel(int startModelId, int targetModelId)
        {
            if (GetModelById(startModelId) is not BehaviourElementModel startModel) return;
            if(startModel.GetTargetModels() == null) 
                startModel.SetTargetModels = new List<IReadOnlyBehaviourElementModel>();
            startModel.GetTargetModels().Add(GetModelById(targetModelId));
        }

        public void ClearTargetModel(int modelId)
        {
            if (GetModelById(modelId) is BehaviourElementModel model) model.SetTargetModels = null;
        }
        
        private IReadOnlyBehaviourElementModel GetModelById(int modelId)
        {
            IReadOnlyBehaviourElementModel model = null;
            if (_stateDictionary.TryGetValue(modelId, out IReadOnlyBehaviourElementModel stateModel))
            {
                model = stateModel;
            }
            else if (_triggerDictionary.TryGetValue(modelId, out IReadOnlyTriggerModel triggerModel))
            {
                model = triggerModel;
            }
            return model;
        }

        #region States
        public bool IsState(int modelId)
        {
            return _stateDictionary.ContainsKey(modelId);
        }

        public void SetRootState(int stateId)
        {
            BehaviourElementModel newRootState = _stateDictionary[stateId] as BehaviourElementModel;
            if (RootState is BehaviourElementModel oldRootState) oldRootState.Id = stateId;
            if (newRootState != null) newRootState.Id = StateModel.RootId;
        }
        
        public void AddState(StateModel state)
        {
            _stateDictionary.Add(state.Id, state);
            ModelAdded?.Invoke(state);
        }

        public void RemoveState(int stateId)
        {
            _stateDictionary.Remove(stateId);
            ModelRemoved?.Invoke(stateId); 
        }
        #endregion
        
        #region Triggers
        public bool IsTrigger(int modelId)
        {
            return _triggerDictionary.ContainsKey(modelId);
        }
        
        public void AddTrigger(TriggerModel trigger)
        {
            _triggerDictionary.Add(trigger.Id, trigger);
            ModelAdded?.Invoke(trigger);
        }

        public void RemoveTrigger(int triggerId)
        {
            _triggerDictionary.Remove(triggerId);
            ModelRemoved?.Invoke(triggerId);
        }
        #endregion
    }
}