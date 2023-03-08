using System;
using System.Collections.Generic;
using System.Linq;
using Code.Runtime.BehaviourGraphSerialization;
using Code.Runtime.StateMachineElements;

namespace Code.Editor
{
    public class CanvasModel
    {
        public event Action Changed;
        public event Action Initialized;
        public event Action<IReadOnlyBehaviourElementModel> ModelAdded;
        public event Action<int> ModelRemoved; //TODO subscribe runtimeXml serialize

        public IReadOnlyModelGraph Graph => _modelGraph;
        
        public IReadOnlyCollection<IReadOnlyBehaviourElementModel> States => 
            _modelGraph.GetStates().Values as IReadOnlyCollection<IReadOnlyBehaviourElementModel>;
        public IReadOnlyCollection<IReadOnlyTriggerModel> Triggers => 
            _modelGraph.GetTriggers().Values as IReadOnlyCollection<IReadOnlyTriggerModel>;
        
        private ModelGraph _modelGraph;

        public void Initialize(ModelGraph modelGraph)
        {
            _modelGraph = modelGraph;
            Initialized?.Invoke();
        }

        #region Common
        public int GetCurrentBiggestId()
        {
            int firstId = _modelGraph.GetStates().Count != 0 ? _modelGraph.GetStates().Max(pair => pair.Value.GetId()) : StateModel.RootId - 1;
            int secondId = _modelGraph.GetTriggers().Count != 0 ? _modelGraph.GetTriggers().Max(pair => pair.Value.GetId()) : StateModel.RootId - 1;
            return Math.Max(firstId, secondId);
        }

        public void AddTargetModel(int startModelId, int targetModelId)
        {
            _modelGraph.AddTargetModel(startModelId, targetModelId);
        }

        public void ClearTargetModel(int modelId)
        {
            _modelGraph.ClearTargetModel(modelId);
        }

        public void DeleteBehaviourElementModel(int modelId)
        {
            if(_modelGraph.IsState(modelId)) _modelGraph.RemoveState(modelId);
            else if (_modelGraph.IsTrigger(modelId)) _modelGraph.RemoveTrigger(modelId);
            ModelRemoved?.Invoke(modelId); 
        }
        #endregion

        #region States
        public void SetRootState(int stateId)
        {
            _modelGraph.SetRootState(stateId);
        }
        
        public void AddState(StateModel state)
        {
            _modelGraph.AddState(state);
            ModelAdded?.Invoke(state);
        }
        #endregion
        
        #region Triggers
        
        public void AddTrigger(TriggerModel trigger)
        {
            _modelGraph.AddTrigger(trigger);
            ModelAdded?.Invoke(trigger);
        }
        #endregion
    }
}