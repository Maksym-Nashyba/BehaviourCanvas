using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Editor.EditorWindows.BehaviourTreeEditor
{
    public class BehaviourCanvasEditor : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset = default;
        
        private BehaviourTreeAsset _treeAsset;
        private BehaviourCanvasSerializer _behaviourCanvasSerializer;
        private NodeBuilderSerializer _nodeBuilderSerializer;
        private BehaviourCanvas _canvas;
        private BehaviourCanvasView _canvasView;
        private NodeBuilder _nodeBuilder;

        [MenuItem("Window/BehaviourCanvas/BehaviourCanvasEditor")]
        public static void OpenWindow()
        {
            BehaviourCanvasEditor wnd = GetWindow<BehaviourCanvasEditor>();
            wnd.titleContent = new GUIContent("BehaviourCanvasEditor");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            _visualTreeAsset.CloneTree(root);
            
            AddStylesheets();
            
            string behaviourTreeAssetPath = "Assets/Code/Editor/BehaviourTreeAsset.asset";
            BehaviourTreeAsset asset = AssetDatabase.LoadAssetAtPath<BehaviourTreeAsset>(behaviourTreeAssetPath);
            _behaviourCanvasSerializer = new BehaviourCanvasSerializer(asset);
            
            _canvasView = root.Q<BehaviourCanvasView>();
            _nodeBuilder = root.Q<NodeBuilder>();
        }
        
        private void AddStylesheets()
        {
            StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Code/Editor/EditorWindows/" +
                                                                              "BehaviourTreeEditor/BehaviourCanvasEditor.uss");
            rootVisualElement.styleSheets.Add(stylesheet);
        }
    }
}
