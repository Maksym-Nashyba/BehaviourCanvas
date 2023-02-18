using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace Code.Editor
{
    public class BehaviourCanvasView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<BehaviourCanvasView, UxmlTraits> 
        {
            
        }
        
        public BehaviourCanvasView()
        {
            Insert(0, new GridBackground());

            AddManipulators();
            
            StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Code/Editor/BehaviourCanvasEditor.uss");
            styleSheets.Add(stylesheet);
        }

        private void AddManipulators()
        {
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());   
        }

        public void BuildBehaviourTree()
        {
            
        }

        private void CreateStateView(IState state)
        {
            StateView stateView = new StateView(state);
            AddElement(stateView);
        }

        private void CreateTriggerView(ITrigger trigger)
        {
            TriggerView triggerView = new TriggerView(trigger);
            AddElement(triggerView);
        }
    }
}