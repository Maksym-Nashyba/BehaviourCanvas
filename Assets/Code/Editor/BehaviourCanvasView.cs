using System;
using System.Collections.Generic;
using System.Linq;
using Code.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Editor
{
    public class BehaviourCanvasView : GraphView
    {
        public event Action<int> DeleteTreeModel;
        public event Action<int> SetRootState;
        
        public NodeView RootNode => _nodes.First(node => node.ID == -1);
        public IReadOnlyList<NodeView> Nodes => _nodes;

        private List<NodeView> _nodes;
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

        private void AddStylesheets()
        {
            StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Code/Editor/EditorWindows/" +
                                                                              "BehaviourTreeEditor/BehaviourCanvasEditor.uss");
            styleSheets.Add(stylesheet);
        }
        
        #endregion

        public void Initialize(ViewSerializer serializer)
        {
            graphViewChanged -= OnGraphViewChanged;

            _serializer = serializer;
            _nodes = new List<NodeView>();
            
            graphViewChanged += OnGraphViewChanged;
        }
        
        public Rect FindNodePosition(int id)
        {
            return _serializer.GetNodePosition(id.ToString());
        }

        public void SetRootNode(int newRootNodeId, int oldRootNodeNewId)
        {
            RootNode.ID = oldRootNodeNewId;
            _nodes.First(node => node.ID == newRootNodeId).ID = -1;
        }
        
        public void CreateNodeView(StateModel state, Rect position)
        {
            NodeView node = new NodeView(state.Model.Name, state.ID, state.Model.Parameters);
            node.SetPosition(position);
            node.Draw();
            _nodes.Add(node);
            AddElement(node);
        }

        public void CreateTriggerView(TriggerModel trigger, Rect position)
        { 
            NodeView node = new TriggerView(trigger.Model.Name, trigger.ID, trigger.Model.Parameters, trigger.ResetTarget);
            node.SetPosition(position);
            node.Draw();
            _nodes.Add(node);
            AddElement(node);
        }

        public void DeleteNode(int modelID)
        {
            NodeView nodeView = _nodes.Find(node => node.ID == modelID);
            _nodes.Remove(nodeView);
            contentViewContainer[0].Remove(nodeView);
        }

        private IManipulator CreateRootStateContextualMenuOption()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent =>
                {
                    if (menuEvent.target is NodeView node and not TriggerView)
                    {
                        menuEvent.menu.AppendAction("Set root state", _ => SetRootState?.Invoke(node.ID));
                    }
                });
            return contextualMenuManipulator;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove == null) return graphViewChange;
            graphViewChange.elementsToRemove.ForEach(element => 
            {
                if (element is NodeView node) DeleteTreeModel?.Invoke(node.ID); 
            });
            graphViewChange.elementsToRemove.Clear();
            return graphViewChange;
        }
    }
}