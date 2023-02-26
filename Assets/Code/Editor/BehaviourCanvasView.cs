using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Editor
{
    public class BehaviourCanvasView : GraphView
    {
        public NodeView RootNode => _rootNode;
        public IReadOnlyList<NodeView> Nodes => _nodes;
        
        private NodeView _rootNode;
        private List<NodeView> _nodes;
        private BehaviourCanvas _behaviourCanvas;
        
        public new class UxmlFactory : UxmlFactory<BehaviourCanvasView, UxmlTraits> { }
        
        public BehaviourCanvasView()
        {
            Insert(0, new GridBackground());
            AddManipulators();
            AddStylesheets();
        }

        private void AddManipulators()
        {
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());   
        }
        
        private void AddStylesheets()
        {
            StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Code/Editor/EditorWindows/" + "BehaviourTreeEditor/BehaviourCanvasEditor.uss");
            styleSheets.Add(stylesheet);
        }

        public void Initialize(BehaviourCanvas canvas)
        {
            _behaviourCanvas = canvas;
            _nodes = new List<NodeView>(canvas.States.Count + canvas.Triggers.Count + 1);
        }

        public void CreateRootNode(int id, (string, string)[] parameters, Rect position)
        {
            _rootNode = new NodeView(id, parameters);
            _rootNode.SetPosition(position);
            _nodes.Add(_rootNode);
        }
        
        public void CreateNode(int id, (string, string)[] parameters, Rect position)
        {
            NodeView node = new NodeView(id, parameters);
            node.SetPosition(position);
            _nodes.Add(node);
        }
    
        public void CreateTriggerNode(int id, (string, string)[] parameters, bool resetTarget, Rect position)
        {
            NodeView node = new TriggerView(id, parameters, resetTarget);
            node.SetPosition(position);
            _nodes.Add(node);
        }
    }
}