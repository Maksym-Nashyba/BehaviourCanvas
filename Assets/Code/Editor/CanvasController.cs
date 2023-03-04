using System;
using System.Linq;
using Code.Editor.Serializers;
using Code.Runtime.BehaviourElementModels;

namespace Code.Editor
{
    public class CanvasController
    {
        private IdStore _idStore;
        private readonly CanvasModel _canvasModel;
        private readonly EditorModelSerializer _modelSerializer;

        public CanvasController(CanvasModel canvasModel, EditorModelSerializer modelSerializer)
        {
            _canvasModel = canvasModel;
            _modelSerializer = modelSerializer;
        }

        public void Initialize()
        {
            DeserializeModel();
            _idStore = new IdStore(GetIncrementedBiggestId());
        }

        public void SerializeModel()
        {
            _modelSerializer.Serialize(_canvasModel.States, _canvasModel.Triggers);
        }
        
        public void SetRootState(int stateId)
        {
            if(stateId == StateModel.RootId) return;
            _canvasModel.RootState.Id = _idStore.ID;
            _canvasModel.States.First(state => state.Id == stateId).Id = StateModel.RootId;
        }

        public void CreateState(Model model)
        {
            StateModel state = new StateModel(_idStore.ID, model);
            AddStateToModel(state);
        }
        
        public void CreateTrigger(Model model)
        {
            TriggerModel trigger = new TriggerModel(_idStore.ID, model, false);
            AddTriggerToModel(trigger);
        }
        
        private void DeserializeModel()
        {
            _canvasModel.Deserialize(_modelSerializer.DeserializeStateModels(),
                _modelSerializer.DeserializeTriggerModels());
        }

        private void AddStateToModel(StateModel state)
        {
            _canvasModel.AddState(state);
        }

        private void AddTriggerToModel(TriggerModel trigger)
        {
            _canvasModel.AddTrigger(trigger);
        }
        
        public void DeleteTreeModel(int modelID)
        {
            foreach (StateModel state in _canvasModel.States)
            {
                if (state.Id != modelID) continue;
                _canvasModel.RemoveState(state);
                _idStore = new IdStore(GetIncrementedBiggestId());
                return;
            }
            
            foreach (TriggerModel trigger in _canvasModel.Triggers)
            {
                if (trigger.Id != modelID) continue;
                _canvasModel.RemoveTrigger(trigger);
                _idStore = new IdStore(GetIncrementedBiggestId());
                return;
            }
        }
        
        private int GetIncrementedBiggestId()
        {
            int firstId = _canvasModel.States.Count != 0 ? _canvasModel.States[^1].Id : StateModel.RootId - 1;
            int secondId = _canvasModel.Triggers.Count != 0 ? _canvasModel.Triggers[^1].Id : StateModel.RootId - 1;
            return Math.Max(firstId, secondId) + 1;
        }
    }
}