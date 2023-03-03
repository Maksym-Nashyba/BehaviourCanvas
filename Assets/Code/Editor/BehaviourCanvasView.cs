using System.Collections.Generic;
using Code.Runtime;
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
        private IdStore _idStore;

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
            StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Code/Editor/EditorWindows/" +
                                                                              "BehaviourTreeEditor/BehaviourCanvasEditor.uss");
            styleSheets.Add(stylesheet);
        }

        public void Initialize(BehaviourCanvas canvas, BehaviourCanvasSerializer canvasSerializer)
        {
            _nodes = new List<NodeView>(canvas.States.Count + canvas.Triggers.Count);
            _idStore = new IdStore();
            CreateBehaviourCanvasGraph(canvas, canvasSerializer);
        }

        private void CreateBehaviourCanvasGraph(BehaviourCanvas canvas, BehaviourCanvasSerializer canvasSerializer)
        {
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;
            foreach (StateModel state in canvas.States)
            {
                CreateNode(state, canvasSerializer.GetNodePosition(state.ID.ToString()));
            }
            foreach (TriggerModel trigger in canvas.Triggers)
            {
                CreateTriggerNode(trigger, canvasSerializer.GetTriggerNodePosition(trigger.ID.ToString()));
            }
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove == null) return graphViewChange;
            graphViewChange.elementsToRemove.Clear();
            return graphViewChange;
        }

        public void CreateRootNode(string nodeName, int id, (string, string)[] parameters, Rect position)
        {
            _rootNode = new NodeView(nodeName, id, parameters);
            _rootNode.SetPosition(position);
            AddElement(_rootNode);
            _nodes.Add(_rootNode);
        }

        public void CreateNode(StateModel state, Rect position)
        {
            CreateNode(state.Model.Name, state.ID, state.Model.Parameters, position);
        }
        
        public void CreateTriggerNode(TriggerModel trigger, Rect position)
        {
            CreateTriggerNode(trigger.Model.Name, trigger.ID, trigger.Model.Parameters, trigger.ResetTarget, position);
        }
        
        private void CreateNode(string nodeName, int id, (string, string)[] parameters, Rect position)
        {
            NodeView node = new NodeView(nodeName, id, parameters);
            node.SetPosition(position);
            AddElement(node);
            _nodes.Add(node);
        }

        private void CreateTriggerNode(string nodeName, int id, (string, string)[] parameters, bool resetTarget, Rect position)
        {
            NodeView node = new TriggerView(nodeName, id, parameters, resetTarget);
            node.SetPosition(position);
            AddElement(node);
            _nodes.Add(node);
        }
    }
}