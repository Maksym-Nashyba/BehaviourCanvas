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
            _idStore = new IdStore(_canvasModel.GetCurrentBiggestId() + 1);
        }

        public void SerializeModel()
        {
            _modelSerializer.Serialize(_canvasModel.States, _canvasModel.Triggers);
        }

        public void SetRootState(int stateId)
        {
            if (stateId == StateModel.RootId) return;
            _canvasModel.SetRootState(stateId);
        }

        public void SetTargetModels(int startModelId, int targetModelId)
        {
            _canvasModel.AddTargetModel(startModelId, targetModelId);
        }

        public void ClearTargetModel(int modelId)
        {
            _canvasModel.ClearTargetModel(modelId);
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
        
        public void DeleteBehaviourElementModel(int modelId)
        {
            if(_canvasModel.IsState(modelId)) _canvasModel.RemoveState(modelId);
            else if (_canvasModel.IsTrigger(modelId)) _canvasModel.RemoveTrigger(modelId);
            _idStore = new IdStore(_canvasModel.GetCurrentBiggestId() + 1);
        }
        
        private void DeserializeModel()
        {
            _canvasModel.Deserialize(_modelSerializer.DeserializeStateModels(),
                _modelSerializer.DeserializeTriggerModels(), 
                _modelSerializer.DeserializeModelsAndTargets());
        }

        private void AddStateToModel(StateModel state)
        {
            _canvasModel.AddState(state);
        }

        private void AddTriggerToModel(TriggerModel trigger)
        {
            _canvasModel.AddTrigger(trigger);
        }
    }
}