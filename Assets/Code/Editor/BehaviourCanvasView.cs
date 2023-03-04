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
        public IReadOnlyList<NodeView> Nodes => _nodes;

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
                    if (menuEvent.target is NodeView node && node.ModelType != typeof(TriggerModel))
                    {
                        menuEvent.menu.AppendAction("Set root state", 
                            _ =>
                            {
                                _canvasController.SetRootState(node.Id);
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
            _canvasModel.ModelRemoved -= DeleteNode;
            graphViewChanged -= OnGraphViewChanged;
        }
        
        private void SubscribeOnEvents()
        {
            _canvasModel.Changed += BuildGraph;
            _canvasModel.ModelAdded += CreateNodeView;
            _canvasModel.ModelRemoved += DeleteNode;
            graphViewChanged += OnGraphViewChanged;
        }
        #endregion

        public void Serialize()
        {
            _serializer.Serialize(_nodes);
        }

        private void BuildGraph()
        {
            foreach (StateModel state in _canvasModel.States)
            {
                CreateNodeView(state, FindNodePosition(state.Id));
            }
            foreach (TriggerModel trigger in _canvasModel.Triggers)
            {
                CreateNodeView(trigger, FindNodePosition(trigger.Id));
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

        private void DeleteNode(int modelID)
        {
            NodeView nodeView = _nodes.Find(node => node.Id == modelID);
            _nodes.Remove(nodeView);
            contentViewContainer[0].Remove(nodeView);
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove == null)
            {
                Serialize();
                return graphViewChange;
            }
            graphViewChange.elementsToRemove.ForEach(element =>
            {
                if (element is NodeView node) _canvasController.DeleteTreeModel(node.Id);
            });
            graphViewChange.elementsToRemove.Clear();
            Serialize();
            return graphViewChange;
        }
    }
}