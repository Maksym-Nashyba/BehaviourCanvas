using System;
using Code.BCTemplates.StateTemplate;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Editor.Builders.StateBuilder
{
    public class StateBuilder : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset = default;
        private TextField _stateName;
        private TextField _name0;
        private TextField _name1;
        private TextField _name2;
        private TextField _type0;
        private TextField _type1;
        private TextField _type2;
        
        [MenuItem("Window/StateBuilder")]
        public static void ShowWindow()
        {
            StateBuilder wnd = GetWindow<StateBuilder>();
            wnd.titleContent = new GUIContent("StateBuilder");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            _visualTreeAsset.CloneTree(root);
            Button createButton = root.Q<Button>();
            createButton.clickable.clicked += OnCreateButton;
            _stateName = root.Q<TextField>("StateName");
            _name0 = root.Q<TextField>("Name0");
            _name1 = root.Q<TextField>("Name1");
            _name2 = root.Q<TextField>("Name2");
            _type0 = root.Q<TextField>("Type0");
            _type1 = root.Q<TextField>("Type1");
            _type2 = root.Q<TextField>("Type2");
        }

        private void OnCreateButton()
        {
            StateTemplateProcessor templateProcessor = new StateTemplateProcessor();
            StateTemplateData templateData = new StateTemplateData(_stateName.value, CollectParameterInputs());
            string newStateSource = templateProcessor.Process(templateData);
            SaveState(newStateSource);
        }

        private void SaveState(string source)
        {
            
        }
        
        private (string Name, Type Type)[] CollectParameterInputs()
        {
            return null;
        }
    }
}
