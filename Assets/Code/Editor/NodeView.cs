using System;
using Code.Runtime.BehaviourElementModels;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Code.Editor
{
    public class NodeView : Node
    {
        public int Id => _behaviourElementModel.GetId();
        public Type ModelType => _behaviourElementModel.GetType();
        private readonly IReadOnlyBehaviourElementModel _behaviourElementModel;

        public NodeView(IReadOnlyBehaviourElementModel behaviourElementModel)
        {
            _behaviourElementModel = behaviourElementModel;
        }

        public void Draw()
        {
            
            TextField titleTextField = new TextField()
            {
                value = _behaviourElementModel.GetName()
            };
            titleContainer.Insert(0, titleTextField);

            /*--------------------------------------------*/
            
            //TODO if(_behaviourElementModel is IReadOnlyTriggerModel) AddCheckBoxField;
            
            VisualElement parametersContainer = new VisualElement();
            Foldout parametersFoldout = new Foldout()
            {
                text = "Parameters"
            };

            foreach ((string, string) parameter in _behaviourElementModel.GetParameters())
            {
                Foldout parameterFoldout = new Foldout()
                {
                    text = "Parameter"
                };
                parameterFoldout.Add(new Label($"Type: {parameter.Item1}\nName: {parameter.Item2}"));
                parametersFoldout.Add(parameterFoldout);
            }

            parametersContainer.Add(parametersFoldout);
            extensionContainer.Add(parametersContainer);
        }
    }
}