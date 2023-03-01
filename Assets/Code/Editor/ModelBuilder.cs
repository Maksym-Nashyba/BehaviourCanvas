using System.Collections.Generic;
using Code.Editor.EditorWindows.Builders.StateBuilder;
using Code.Editor.EditorWindows.Builders.TriggerBuilder;
using Code.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Editor
{
    public class ModelBuilder : VisualElement
    {
        private Button _statesSectionButton;
        private Button _triggersSectionButton;

        private VisualElement _statesVisual;
        private ScrollView _statesScrollView;
        private Button _createStateButton;
        
        private VisualElement _triggersVisual;
        private ScrollView _triggersScrollView;
        private Button _createTriggerButton;

        private BehaviourCanvas _canvas;
        private BehaviourCanvasView _canvasView;

        public new class UxmlFactory : UxmlFactory<ModelBuilder> { }

        public ModelBuilder()
        {
            
        }

        public void Initialize(BehaviourCanvas canvas, BehaviourCanvasView canvasView)
        {
            _canvas = canvas;
            _canvasView = canvasView;
            
            _statesSectionButton = this.Q<Button>("states-section-button");
            _triggersSectionButton = this.Q<Button>("triggers-section-button");
            
            _statesVisual = this.Q<VisualElement>("states-visual");
            _statesScrollView = _statesVisual.Q<ScrollView>();
            _createStateButton = _statesVisual.Q<Button>();
            
            _triggersVisual = this.Q<VisualElement>("triggers-visual");
            _triggersScrollView = _triggersVisual.Q<ScrollView>();
            _createTriggerButton = _triggersVisual.Q<Button>();

            UpdateStatesList();
            UpdateTriggersList();
            SubscribeButtons();
        }

        private void UpdateStatesList()
        {
            _statesScrollView.Clear();
            IReadOnlyList<Model> models = Reflection.FindAllStates();
            List<Button> buttons = CreateStatesListContentButtons(models);
            foreach (Button button in buttons)
            {
                _statesScrollView.Add(button);
            }
        }
        
        private void UpdateTriggersList()
        {
            _triggersScrollView.Clear();
            IReadOnlyList<Model> models = Reflection.FindAllTriggers();
            List<Button> buttons = CreateTriggersListContentButtons(models);
            foreach (Button button in buttons)
            {
                _triggersScrollView.Add(button);
            }
        }

        private List<Button> CreateStatesListContentButtons(IReadOnlyList<Model> models)
        {
            List<Button> buttons = new List<Button>(models.Count);
            foreach (Model model in models)
            {
                Button button = new Button(() =>
                {
                    StateModel state = new StateModel(1, model);
                    _canvas.AddState(state);
                    _canvasView.CreateNode(state, new Rect(100, 100, 200, 100)); //TODO create IdStore
                });
                button.text = model.Name;
                buttons.Add(button);
            }

            return buttons;
        }
        
        private List<Button> CreateTriggersListContentButtons(IReadOnlyList<Model> models)
        {
            List<Button> buttons = new List<Button>(models.Count);
            foreach (Model model in models)
            {
                Button button = new Button(() => //TODO method with TreeModel parameters
                {
                    TriggerModel trigger = new TriggerModel(2, model, false);
                    _canvas.AddTrigger(trigger);
                    _canvasView.CreateTriggerNode(trigger, new Rect(100, 100, 200, 100)); //TODO create IdStore
                });
                button.text = model.Name;
                buttons.Add(button);
            }

            return buttons;
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