using System;
using System.Collections.Generic;
using System.IO;
using Code.BCTemplates.TriggerTemplate;
using Code.Editor.EditorWindows.PopUpWindow;
using Code.Templates.TriggerTemplate;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Code.Editor.EditorWindows.Builders.TriggerBuilder
{
    public class TriggerBuilder : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset = default;
        private TextField _triggerNameField;
        private List<ParameterVisualElement> _parameterFields;
        private DropdownField _parameterCountDropdown;
        private VisualElement _middleSegment;

        [MenuItem("Window/BehaviourCanvas/TriggerBuilder")]
        public static void ShowWindow()
        {
            TriggerBuilder wnd = GetWindow<TriggerBuilder>();
            wnd.titleContent = new GUIContent("TriggerBuilder");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            _visualTreeAsset.CloneTree(root);

            _triggerNameField = root.Q<TextField>("TriggerName");
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

            string triggerName = _triggerNameField.value;
            List<(string, Type)> parameters = new List<(string, Type)>();
            foreach (ParameterVisualElement parameterField in _parameterFields)
            {
                parameters.Add(parameterField.Value);
            }
            
            CreateTriggerFiles(triggerName, parameters.ToArray());
        }

        private void CreateTriggerFiles(string triggerName, (string, Type)[] parameters)
        {
            TriggerTemplateData templateData = new TriggerTemplateData(triggerName, parameters);
            TriggerTemplateProcessor processor = new TriggerTemplateProcessor();
            string processed = processor.Process(templateData);
            string path = Application.dataPath.Replace("/Assets", "") + "/" + BehaviourCanvasPaths.TriggerScripts;
            File.WriteAllText(path+$"/{triggerName}Trigger.cs", processed);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<TextAsset>(BehaviourCanvasPaths.TriggerScripts+$"/{triggerName}State.cs"));
        }
        
        private void ValidateInputs()
        {
            for (var i = 0; i < _parameterFields.Count; i++)
            {
                ParameterVisualElement parameterField = _parameterFields[i];
                if (!parameterField.IsFilled) throw new InvalidDataException($"Parameter with index {i} isn't filled correctly");
            }

            if (AssetDatabase.LoadAssetAtPath<Object>(BehaviourCanvasPaths.TriggerScripts +
                                                      $"{_triggerNameField.value}Trigger.cs") is not null)
            {
                throw new InvalidDataException("Trigger asset with given name already exists");
            }

            if (String.IsNullOrWhiteSpace(_triggerNameField.value)) throw new InvalidDataException("Trigger name cannot be null, empty or whitespace");
        }
    }
}