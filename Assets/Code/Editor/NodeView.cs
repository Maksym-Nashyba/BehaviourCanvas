using System;
using Code.Runtime.BehaviourElementModels;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Editor
{
    public class NodeView : Node
    {
        public Port InputPort => inputContainer[0] as Port;
        public Port OutputPort => outputContainer[0] as Port;
        
        public int ModelId => _behaviourElementModel.GetId();
        public Type ModelType => _behaviourElementModel.GetType();

        private readonly IReadOnlyBehaviourElementModel _behaviourElementModel;

        public NodeView(IReadOnlyBehaviourElementModel behaviourElementModel)
        {
            _behaviourElementModel = behaviourElementModel;
        }

        public void Draw()
        {
            UpdateNodeTitleDisplay();

            /*--------------------------------------------*/

            Type portType = _behaviourElementModel is TriggerModel ? typeof(StateModel) : typeof(TriggerModel);
            Port.Capacity outputPortCapacity = _behaviourElementModel is TriggerModel ? Port.Capacity.Single : Port.Capacity.Multi;
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, portType);
            Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, outputPortCapacity, portType);

            inputPort.portName = "Source";
            outputPort.portName = "Target";
            
            inputContainer.Add(inputPort);
            outputContainer.Add(outputPort);
            
            //TODO if(_behaviourElementModel is IReadOnlyTriggerModel) AddCheckBoxField;
            
            VisualElement parametersContainer = new VisualElement();
            Foldout parametersFoldout = new Foldout()
            {
                text = "Parameters"
            };

            foreach ((string, string) parameter in _behaviourElementModel.GetModel().Parameters)
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
        
        public void UpdateNodeTitleDisplay()
        {
            Label titleLabel = new Label()
            {
                text = "Name: " + _behaviourElementModel.GetModel().Name + "\nId: " + _behaviourElementModel.GetId(),
                style = { unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter)}
            };
            titleContainer.RemoveAt(0);
            titleContainer.Insert(0, titleLabel);
        }
    }
}