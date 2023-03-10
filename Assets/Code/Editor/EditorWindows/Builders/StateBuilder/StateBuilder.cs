using System;
using System.Collections.Generic;
using System.IO;
using Code.BCTemplates.StateTemplate;
using Code.Editor.EditorWindows.PopUpWindow;
using Code.Templates.StateTemplate;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Code.Editor.EditorWindows.Builders.StateBuilder
{
    public class StateBuilder : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset = default;
        private TextField _stateNameField;
        private List<ParameterVisualElement> _parameterFields;
        private DropdownField _parameterCountDropdown;
        private VisualElement _middleSegment;

        [MenuItem("Window/CanvasController/StateBuilder")]
        public static void ShowWindow()
        {
            StateBuilder wnd = GetWindow<StateBuilder>();
            wnd.titleContent = new GUIContent("StateBuilder");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            _visualTreeAsset.CloneTree(root);

            _stateNameField = root.Q<TextField>("StateName");
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
            try
            {
                ValidateInputs();
            }
            catch (InvalidDataException e)
            {
                PopUp.Show(e.Message);
                return;
            }

            string stateName = _stateNameField.value;
            List<(string, Type)> parameters = new List<(string, Type)>();
            foreach (ParameterVisualElement parameterField in _parameterFields)
            {
                parameters.Add(parameterField.Value);
            }
            
            CreateStateFiles(stateName, parameters.ToArray());
        }

        private void CreateStateFiles(string stateName, (string, Type)[] parameters)
        {
            StateTemplateData templateData = new StateTemplateData(stateName, parameters);
            StateTemplateProcessor processor = new StateTemplateProcessor();
            string processed = processor.Process(templateData);
            string path = Application.dataPath.Substring(0, Application.dataPath.Length-6) + BehaviourCanvasPaths.StateScripts;
            File.WriteAllText(path+$"/{stateName}State.cs", processed);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<TextAsset>(BehaviourCanvasPaths.StateScripts+$"/{stateName}State.cs"));
        }
        
        private void ValidateInputs()
        {
            for (var i = 0; i < _parameterFields.Count; i++)
            {
                ParameterVisualElement parameterField = _parameterFields[i];
                if (!parameterField.IsFilled) throw new InvalidDataException($"Parameter with index {i} isn't filled correctly");
            }

            if (AssetDatabase.LoadAssetAtPath<Object>(BehaviourCanvasPaths.StateScripts +
                                                      $"{_stateNameField.value}State.cs") is not null)
            {
                throw new InvalidDataException("State asset with given name already exists");
            }

            if (String.IsNullOrWhiteSpace(_stateNameField.value)) throw new InvalidDataException("State name cannot be null, empty or whitespace");
        }
    }
}