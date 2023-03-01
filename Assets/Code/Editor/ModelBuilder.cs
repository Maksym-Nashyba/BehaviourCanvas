using System;
using System.Collections.Generic;
using System.Linq;
using Code.Editor.EditorWindows.Builders.StateBuilder;
using Code.Editor.EditorWindows.Builders.TriggerBuilder;
using Code.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Editor
{
    public class ModelBuilder : VisualElement
    {
        #region VisualElements
            private Button _statesSectionButton;
            private Button _triggersSectionButton;
        
            private VisualElement _statesVisual;
            private ScrollView _statesScrollView;
            private Button _createStateButton;
                
            private VisualElement _triggersVisual;
            private ScrollView _triggersScrollView;
            private Button _createTriggerButton;
        #endregion
        
        private BehaviourCanvas _canvas;
        private BehaviourCanvasView _canvasView;
        private readonly Rect _defaultNodePosition = new Rect(100, 100, 200, 100);

        public new class UxmlFactory : UxmlFactory<ModelBuilder> { }

        public ModelBuilder()
        {
            
        }

        public void Initialize(BehaviourCanvas canvas, BehaviourCanvasView canvasView)
        {
            _canvas = canvas;
            _canvasView = canvasView;
            QueryAllVisualElements();

            UpdateStatesList();
            UpdateTriggersList();
            SubscribeButtons();
        }

        private void UpdateStatesList()
        {
            _statesScrollView.Clear();
            IReadOnlyList<Model> models = Reflection.FindAllStates();
            IReadOnlyList<StateModel> states = models.Select(model => new StateModel(1, model)).ToList(); //TODO create IdStore
            
            List<Button> buttons = CreateTreeModelsButtons(states, treeModel =>
            {
                _canvas.AddState(treeModel as StateModel);
                _canvasView.CreateNode(treeModel as StateModel, _defaultNodePosition);
            });
            
            foreach (Button button in buttons)
            {
                _statesScrollView.Add(button);
            }
        }
        
        private void UpdateTriggersList()
        {
            _statesScrollView.Clear();
            IReadOnlyList<Model> models = Reflection.FindAllTriggers();
            IReadOnlyList<TriggerModel> triggers = models.Select(model => new TriggerModel(1, model, false)).ToList(); //TODO create IdStore
            
            List<Button> buttons = CreateTreeModelsButtons(triggers, treeModel =>
            {
                _canvas.AddTrigger(treeModel as TriggerModel);
                _canvasView.CreateTriggerNode(treeModel as TriggerModel, _defaultNodePosition);
            });
            
            foreach (Button button in buttons)
            {
                _triggersScrollView.Add(button);
            }
        }

        private List<Button> CreateTreeModelsButtons(IReadOnlyList<TreeModel> treeModels, Action<TreeModel> buttonClickEvent)
        {
            List<Button> buttons = new List<Button>(treeModels.Count);
            foreach (TreeModel treeModel in treeModels)
            {
                Button button = new Button(() => buttonClickEvent(treeModel));
                button.text = treeModel.Model.Name;
                buttons.Add(button);
            }
            return buttons;
        }
        
        private void QueryAllVisualElements()
        {
            _statesSectionButton = this.Q<Button>("states-section-button");
            _triggersSectionButton = this.Q<Button>("triggers-section-button");
            
            _statesVisual = this.Q<VisualElement>("states-visual");
            _statesScrollView = _statesVisual.Q<ScrollView>();
            _createStateButton = _statesVisual.Q<Button>();
            
            _triggersVisual = this.Q<VisualElement>("triggers-visual");
            _triggersScrollView = _triggersVisual.Q<ScrollView>();
            _createTriggerButton = _triggersVisual.Q<Button>();
        }

        private void SubscribeButtons()
        {
            _statesSectionButton.clicked += OnStatesButton;
            _triggersSectionButton.clicked += OnTriggersButton;
            _createStateButton.clicked += OnCreateStateButton;
            _createTriggerButton.clicked += OnCreateTriggerButton;
        }

        private void OnStatesButton()
        {
            _triggersVisual.style.display = DisplayStyle.None;
            _statesVisual.style.display = DisplayStyle.Flex;
        }
        
        private void OnTriggersButton()
        {
            _statesVisual.style.display = DisplayStyle.None;
            _triggersVisual.style.display = DisplayStyle.Flex;
        }

        private void OnCreateStateButton()
        {
            StateBuilder.ShowWindow();
        }
        
        private void OnCreateTriggerButton()
        {
            TriggerBuilder.ShowWindow();
        }
    }
}