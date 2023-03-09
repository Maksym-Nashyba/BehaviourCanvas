using System;
using System.IO;
using Code.Editor.EditorWindows.PopUpWindow;
using Code.Editor.Serializers;
using Code.Runtime.BehaviourGraphSerialization;
using Code.Runtime.Initialization;
using Code.Runtime.StateMachineElements;

namespace Code.Editor
{
    public class CanvasController : IDisposable
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
            LoadModel();
            _idStore = new IdStore(_canvasModel.GetCurrentBiggestId() + 1);
            _canvasModel.Changed += SaveModel;
        }

        public void SaveModel()
        {
            try
            {
                ModelGraphValidator.Validate(_canvasModel.Graph);
            }
            catch (InvalidDataException e)
            {
                PopUp.Show(e.Message);
                return;
            }
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
            _canvasModel.AddState(state);
        }
        
        public void CreateTrigger(Model model)
        {
            //TODO fix trigger id
            TriggerModel trigger = new TriggerModel(_idStore.ID, model, false);
            _canvasModel.AddTrigger(trigger);
        }
        
        public void DeleteBehaviourElementModel(int modelId)
        {
            _canvasModel.DeleteBehaviourElementModel(modelId);
            _idStore = new IdStore(_canvasModel.GetCurrentBiggestId() + 1);
        }
        
        public void Dispose()
        {
            _canvasModel.Changed -= SaveModel;
        }

        private void LoadModel()
        {
            _canvasModel.Initialize(_modelSerializer.DeserializeModelGraph());
        }
    }
}