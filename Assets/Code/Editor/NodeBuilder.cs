using System.Collections.Generic;
using Code.Editor.EditorWindows.Builders.StateBuilder;
using Code.Editor.EditorWindows.Builders.TriggerBuilder;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Editor
{
    public class NodeBuilder : VisualElement
    {
        private Button _statesSectionButton;
        private Button _triggersSectionButton;

        private VisualElement _statesVisual;
        private ScrollView _statesScrollView;
        private Button _createStateButton;
        
        private VisualElement _triggersVisual;
        private ScrollView _triggersScrollView;
        private Button _createTriggerButton;

        private BehaviourCanvasView _canvasView;
        private NodeBuilderSerializer _serializer;

        public new class UxmlFactory : UxmlFactory<NodeBuilder> { }

        public NodeBuilder()
        {
            
        }

        public void Initialize(BehaviourCanvasView canvasView, NodeBuilderSerializer serializer)
        {
            _canvasView = canvasView;
            _serializer = serializer;
            
            _statesSectionButton = this.Q<Button>("states-section-button");
            _triggersSectionButton = this.Q<Button>("triggers-section-button");
            
            _statesVisual = this.Q<VisualElement>("states-visual");
            _statesScrollView = _statesVisual.Q<ScrollView>();
            _createStateButton = _statesVisual.Q<Button>();
            
            _triggersVisual = this.Q<VisualElement>("triggers-visual");
            _triggersScrollView = _triggersVisual.Q<ScrollView>();
            _createTriggerButton = _triggersVisual.Q<Button>();

            Model[] states = Reflection.FindAllStates();
            Model[] triggers = Reflection.FindAllTriggers();
            //UpdateStatesList();
            //UpdateTriggersList();
            SubscribeButtons();
        }

        private void UpdateStatesList()
        {
            _statesScrollView.Clear();
            IReadOnlyList<StateModel> states = _serializer.DeserializeStateModels();
            List<Button> buttons = CreateStatesListContentButtons(states);
            foreach (Button button in buttons)
            {
                _statesScrollView.Add(button);
            }
        }
        
        private void UpdateTriggersList()
        {
            _triggersScrollView.Clear();
            IReadOnlyList<TriggerModel> triggers = _serializer.DeserializeTriggerModels();
            List<Button> buttons = CreateTriggersListContentButtons(triggers);
            foreach (Button button in buttons)
            {
                _triggersScrollView.Add(button);
            }
        }

        private List<Button> CreateStatesListContentButtons(IReadOnlyList<StateModel> states)
        {
            List<Button> buttons = new List<Button>(states.Count);
            foreach (StateModel state in states)
            {
                Button button = new Button(() =>
                {
                    _canvasView.CreateNode(state, new Rect(100, 100, 200, 100));
                });
                button.text = state.Model.Name;
                buttons.Add(button);
            }

            return buttons;
        }
        
        private List<Button> CreateTriggersListContentButtons(IReadOnlyList<TriggerModel> triggers)
        {
            List<Button> buttons = new List<Button>(triggers.Count);
            foreach (TriggerModel trigger in triggers)
            {
                Button button = new Button(() =>
                {
                    _canvasView.CreateTriggerNode(trigger, new Rect(100, 100, 200, 100));
                });
                button.text = trigger.Model.Name;
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