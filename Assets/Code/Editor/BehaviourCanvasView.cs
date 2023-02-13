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
            
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Code/Editor/BehaviourCanvasEditor.uss");
            styleSheets.Add(stylesheet);
        }
    }
}