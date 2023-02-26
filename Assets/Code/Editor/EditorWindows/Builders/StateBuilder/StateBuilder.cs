using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Editor.EditorWindows.Builders.StateBuilder
{
    public class StateBuilder : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset = default;
        private List<ParameterVisualElement> _parameterFields;
        private DropdownField _parameterCountDropdown;
        private VisualElement _middleSegment;

        [MenuItem("Window/BehaviourCanvas/StateBuilder")]
        public static void ShowWindow()
        {
            StateBuilder wnd = GetWindow<StateBuilder>();
            wnd.titleContent = new GUIContent("StateBuilder");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            _visualTreeAsset.CloneTree(root);

            _middleSegment = root.Q<VisualElement>("Middle");
            _parameterCountDropdown = root.Q<DropdownField>("ParameterCount");
            _parameterCountDropdown.choices = new List<string> {"0", "1", "2", "3"};
            root.Q<Button>("ConfirmParameterCountButton").clicked += OnParameterCountButton;
            root.Q<Button>("CreateButton").clicked += OnCreateButton;
        }

        private void OnDestroy()
        {
            rootVisualElement.Q<Button>("ConfirmParameterCountButton").clicked -= OnParameterCountButton;
            rootVisualElement.Q<Button>("CreateButton").clicked -= OnCreateButton;
        }

        private void OnParameterCountButton()
        {
            if (_parameterCountDropdown.value == null) return;

            int parameterCount = Int32.Parse(_parameterCountDropdown.value);
            _parameterFields = new List<ParameterVisualElement>();
            for (int i = 0; i < parameterCount; i++)
            {
                ParameterVisualElement parameter = new ParameterVisualElement();
                _parameterFields.Add(parameter);
                _middleSegment.Add(parameter);
            }
            
            _parameterCountDropdown.parent.style.display = DisplayStyle.None;
        }
        
        private void OnCreateButton()
        {
            throw new System.NotImplementedException();
        }
    }
}