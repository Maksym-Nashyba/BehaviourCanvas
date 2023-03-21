using System;
using Code.Runtime.BehaviourGraphSerialization;
using UnityEditor;
using UnityEngine.UIElements;

namespace Code.Editor.EditorWindows.BehaviourTreeEditor
{
    public class ModelPoolElement : VisualElement, IDisposable
    {
        #region VisualElements
        private Label _modelName;
        private Label _parameterOne;
        private Label _parameterTwo;
        private Label _parameterThree;
        private Button _addButton;
        #endregion
        private Action _onAddButton;
        
        public ModelPoolElement()
        {
            VisualElement root = LoadTemplate();
            Add(root);
            QueryElements(root);
        }

        public void Initialize(IReadOnlyBehaviourElementModel model, Action onAddButtonCallback)
        {
            _onAddButton = onAddButtonCallback;
            _modelName.text = model.GetModel().Name;
            DisplayParameter(_parameterOne, model.GetModel().Parameters[0]);
            DisplayParameter(_parameterTwo, model.GetModel().Parameters[1]);
            DisplayParameter(_parameterThree, model.GetModel().Parameters[2]);
            _addButton.clicked += onAddButtonCallback;
        }
        
        private VisualElement LoadTemplate()
        {
            VisualTreeAsset asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(BehaviourCanvasPaths.BehaviourCanvasRoot +
                                                           "/Code/Editor/EditorWindows/BehaviourTreeEditor/ModelPoolElement.uxml");
            return asset.Instantiate();
        }
        
        private void QueryElements(VisualElement root)
        {
            _modelName = root.Q<Label>("ModelName");
            _parameterOne = root.Q<Label>("ParameterOne");
            _parameterThree = root.Q<Label>("ParameterTwo");
            _parameterTwo = root.Q<Label>("ParameterThree");
            _addButton = root.Q<Button>("AddButton");
        }

        private void DisplayParameter(Label label, Parameter parameter)
        {
            label.text = $"{parameter.Type.Name}: {parameter.Name}";
        }
        
        public new class UxmlFactory : UxmlFactory<ModelPoolElement> 
        {
        }

        public void Dispose()
        {
            if (_onAddButton is not null) _addButton.clicked -= _onAddButton;
        }
    }
}
