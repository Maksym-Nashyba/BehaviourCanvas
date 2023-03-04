using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Code.Editor
{
    public class NodeView : Node
    {
        public sealed override string title
        {
            get => base.title;
            set => base.title = value;
        }

        public int ID { get; set; }
        public (string, string)[] Parameters { get; set; }

        public NodeView()
        {
            title = string.Empty;
            ID = 0;
            Parameters = Array.Empty<(string, string)>();
        }
        
        public NodeView(string nodeName, int id, (string, string)[] parameters)
        {
            title = nodeName;
            ID = id;
            Parameters = parameters;
        }

        public override void OnSelected()
        {
            base.OnSelected();
        }

        public void Draw()
        {
            
            TextField titleTextField = new TextField()
            {
                value = title
            };
            titleContainer.Insert(0, titleTextField);

            /*--------------------------------------------*/
            
            VisualElement parametersContainer = new VisualElement();
            Foldout parametersFoldout = new Foldout()
            {
                text = "Parameters"
            };

            foreach ((string, string) parameter in Parameters)
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