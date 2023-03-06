using System;
using System.Collections.Generic;
using Code.Editor.EditorWindows.Builders.StateBuilder;
using Code.Editor.EditorWindows.Builders.TriggerBuilder;
using Code.Runtime;
using Code.Runtime.BehaviourElementModels;
using UnityEngine.UIElements;

namespace Code.Editor
{
    public class BehaviourElementModelsPool : VisualElement
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

        private CanvasController _canvasController;

        public new class UxmlFactory : UxmlFactory<BehaviourElementModelsPool> { }

        public BehaviourElementModelsPool()
        {
            
        }

        public void Initialize(CanvasController canvasController)
        {
            _canvasController = canvasController;
            QueryAllVisualElements();
            UpdateStatesList();
            UpdateTriggersList();
            SubscribeButtons();
        }

        private void UpdateStatesList()
        {
            _statesScrollView.Clear();
            IReadOnlyList<Model> models = Reflection.FindAllStates();
            List<Button> buttons = CreateBehaviourElementModelsButtons(models, 
                model => _canvasController.CreateState(model));
            foreach (Button button in buttons)
            {
                _statesScrollView.Add(button);
            }
        }
        
        private void UpdateTriggersList()
        {
            _triggersScrollView.Clear();
            IReadOnlyList<Model> models = Reflection.FindAllTriggers();
            List<Button> buttons = CreateBehaviourElementModelsButtons(models, 
                model => _canvasController.CreateTrigger(model));
            foreach (Button button in buttons)
            {
                _triggersScrollView.Add(button);
            }
        }

        private List<Button> CreateBehaviourElementModelsButtons(IReadOnlyList<Model> models, Action<Model> buttonClickEvent)
        {
            List<Button> buttons = new List<Button>(models.Count);
            foreach (Model model in models)
            {
                Button button = new Button(() => buttonClickEvent(model));
                button.text = model.Name;
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