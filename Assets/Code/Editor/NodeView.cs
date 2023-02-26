using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Code.Editor
{
    public class NodeView : Node
    {
        public readonly int ID;
        public readonly (string, string)[] Parameters;
        
        public NodeView(int id, (string, string)[] parameters)
        {
            ID = id;
            Parameters = parameters;
        }
    
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
        }
    }
}