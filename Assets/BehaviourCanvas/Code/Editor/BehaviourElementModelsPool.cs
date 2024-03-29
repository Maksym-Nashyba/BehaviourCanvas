﻿using System;
using System.Collections.Generic;
using BehaviourCanvas.Code.Editor.EditorWindows.BehaviourTreeEditor;
using BehaviourCanvas.Code.Editor.EditorWindows.Builders.StateBuilder;
using BehaviourCanvas.Code.Editor.EditorWindows.Builders.TriggerBuilder;
using BehaviourCanvas.Code.Runtime;
using BehaviourCanvas.Code.Runtime.BehaviourGraphSerialization;
using UnityEngine.UIElements;

namespace BehaviourCanvas.Code.Editor
{
    public class BehaviourElementModelsPool : VisualElement, IDisposable
    {
        #region VisualElements
            private Toggle _statesSectionToggle;
            private Toggle _triggersSectionToggle;
        
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
            foreach (VisualElement element in _statesScrollView.Children())
            {
                if(element is IDisposable disposable)disposable.Dispose();
            }
            _statesScrollView.Clear();
            IReadOnlyList<Model> models = Reflection.FindAllStates();
            List<ModelPoolElement> buttons = CreateBehaviourElementModelsButtons(models, 
                model => _canvasController.CreateState(model));
            foreach (ModelPoolElement button in buttons)
            {
                _statesScrollView.Add(button);
            }
        }
        
        private void UpdateTriggersList()
        {
            foreach (VisualElement element in _triggersScrollView.Children())
            {
                if(element is IDisposable disposable)disposable.Dispose();
            }
            _triggersScrollView.Clear();
            IReadOnlyList<Model> models = Reflection.FindAllTriggers();
            List<ModelPoolElement> buttons = CreateBehaviourElementModelsButtons(models, 
                model => _canvasController.CreateTrigger(model));
            foreach (ModelPoolElement button in buttons)
            {
                _triggersScrollView.Add(button);
            }
        }

        private List<ModelPoolElement> CreateBehaviourElementModelsButtons(IReadOnlyList<Model> models, Action<Model> buttonClickEvent)
        {
            List<ModelPoolElement> buttons = new List<ModelPoolElement>(models.Count);
            foreach (Model model in models)
            {
                ModelPoolElement poolElement = new ModelPoolElement();
                poolElement.Initialize(model,() => buttonClickEvent(model));
                buttons.Add(poolElement);
            }
            return buttons;
        }
        
        private void QueryAllVisualElements()
        {
            _statesSectionToggle = this.Q<Toggle>("StatesToggle");
            _triggersSectionToggle = this.Q<Toggle>("TriggersToggle");
            
            _statesVisual = this.Q<VisualElement>("states-visual");
            _statesScrollView = _statesVisual.Q<ScrollView>();
            _createStateButton = _statesVisual.Q<Button>();
            
            _triggersVisual = this.Q<VisualElement>("triggers-visual");
            _triggersScrollView = _triggersVisual.Q<ScrollView>();
            _createTriggerButton = _triggersVisual.Q<Button>();
        }

        private void SubscribeButtons()
        {
            _statesSectionToggle.RegisterValueChangedCallback(OnStatesButton);
            _triggersSectionToggle.RegisterValueChangedCallback(OnTriggersButton);
            _createStateButton.clicked += OnCreateStateButton;
            _createTriggerButton.clicked += OnCreateTriggerButton;
        }

        private void OnStatesButton(ChangeEvent<bool> evt)
        {
            if(evt.newValue == true) DisplayStates();
            else DisplayTriggers();
        }
        
        private void OnTriggersButton(ChangeEvent<bool> evt)
        {
            if(evt.newValue == true) DisplayTriggers();
            else DisplayStates();
        }

        private void DisplayStates()
        {
            _triggersSectionToggle.SetValueWithoutNotify(false);
            _triggersVisual.style.display = DisplayStyle.None;
            _statesVisual.style.display = DisplayStyle.Flex;
        }

        private void DisplayTriggers()
        {
            _statesSectionToggle.SetValueWithoutNotify(false);
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

        public void Dispose()
        {
            _statesSectionToggle.UnregisterValueChangedCallback(OnStatesButton);
            _triggersSectionToggle.UnregisterValueChangedCallback(OnTriggersButton);
            _createStateButton.clicked -= OnCreateStateButton;
            _createTriggerButton.clicked -= OnCreateTriggerButton;
        }
    }
}