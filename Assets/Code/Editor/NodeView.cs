using System;
using UnityEditor.Experimental.GraphView;

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
    }
}