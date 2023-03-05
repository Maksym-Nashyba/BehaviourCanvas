using System.Collections.Generic;
using Code.Editor.Serializers;
using Code.Runtime.BehaviourElementModels;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Editor
{
    public class BehaviourCanvasView : GraphView
    {
        private List<NodeView> _nodes;
        private CanvasModel _canvasModel;
        private CanvasController _canvasController;
        private ViewSerializer _serializer;

        #region UIToolkitRegion
        
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
            this.AddManipulator(CreateRootStateContextualMenuOption());
        }
        
        private IManipulator CreateRootStateContextualMenuOption()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent =>
                {
                    if (menuEvent.target is NodeView node && node.ModelType != typeof(TriggerModel)) //TODO check this with IReadOnlyTriggerModel
                    {
                        menuEvent.menu.AppendAction("Set root state", 
                            _ =>
                            {
                                _canvasController.SetRootState(node.ModelId);
                                _nodes.ForEach(nodeView => nodeView.UpdateNodeTitleDisplay());
                                Serialize();
                            });
                    }
                });
            return contextualMenuManipulator;
        }

        private void AddStylesheets()
        {
            StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Code/Editor/EditorWindows/" +
                                                                              "BehaviourTreeEditor/BehaviourCanvasEditor.uss");
            styleSheets.Add(stylesheet);
        }
        
        #endregion

        #region Initialization
        public void Initialize(CanvasModel canvasModel, CanvasController canvasController, ViewSerializer serializer)
        {
            _canvasModel = canvasModel;
            _canvasController = canvasController;
            _serializer = serializer;
            _nodes = new List<NodeView>();
            SubscribeOnEvents();
        }
        
        public void UnsubscribeFromEvents()
        {
            _canvasModel.Changed -= BuildGraph;
            _canvasModel.ModelAdded -= CreateNodeView;
            graphViewChanged -= OnGraphViewChanged;
        }
        
        private void SubscribeOnEvents()
        {
            _canvasModel.Changed += BuildGraph;
            _canvasModel.ModelAdded += CreateNodeView;
            graphViewChanged += OnGraphViewChanged;
        }
        #endregion

        public void Serialize()
        {
            _serializer.Serialize(_nodes);
        }

        private void BuildGraph()
        {
            foreach (IReadOnlyBehaviourElementModel state in _canvasModel.States)
            {
                CreateNodeView(state, FindNodePosition(state.GetId()));
            }
            foreach (IReadOnlyTriggerModel trigger in _canvasModel.Triggers)
            {
                CreateNodeView(trigger, FindNodePosition(trigger.GetId()));
            }
        }
        
        private Rect FindNodePosition(int id)
        {
            return _serializer.GetNodePosition(id.ToString());
        }
        
        private void CreateNodeView(IReadOnlyBehaviourElementModel model)
        {
            CreateNodeView(model, new Rect(0, 0, 500, 250));
        }
        
        private void CreateNodeView(IReadOnlyBehaviourElementModel model, Rect position)
        {
            NodeView node = new NodeView(model);
            node.SetPosition(position);
            node.Draw();
            _nodes.Add(node);
            AddElement(node);
        }

        private void DeleteNode(NodeView node)
        {
            _canvasController.DeleteBehaviourElementModel(node.ModelId);
            _nodes.Remove(node);
            //Node on graph will be removed automatically by graphViewChange.DeleteElements
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (((NodeView) startPort.node).ModelType == ((NodeView) port.node).ModelType) return;
                if (startPort == port) return;
                if (startPort.node == port.node) return;
                if (startPort.direction == port.direction) return;
                compatiblePorts.Add(port);
            });
            return compatiblePorts;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            graphViewChange.elementsToRemove?.ForEach(element =>
            {
                if (element is NodeView node) DeleteNode(node);
                if (element is Edge edge) _canvasController.ClearTargetModel(((NodeView) edge.output.node).ModelId);
                
            });
            graphViewChange.edgesToCreate?.ForEach(edge =>
            {
                NodeView startNode = edge.output.node as NodeView;
                NodeView targetNode = edge.input.node as NodeView;
                _canvasController.SetTargetModel(startNode.ModelId, targetNode.ModelId);
            });
            Serialize();
            return graphViewChange;
        }
    }
}