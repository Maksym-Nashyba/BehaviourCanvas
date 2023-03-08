using Code.Editor.Serializers;
using Code.Runtime;
using Code.Runtime.BehaviourGraphSerialization;
using Code.Runtime.Initialization;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Editor.EditorWindows.BehaviourTreeEditor
{
    public class BehaviourCanvasEditor : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset = default;
        private ToolbarButton _toolbarButton;
        private CanvasController _canvasController;
        private BehaviourCanvasView _canvasView;
        
        [MenuItem("Window/CanvasController/BehaviourCanvasEditor")]
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
            
            _canvasView = root.Q<BehaviourCanvasView>();
            BehaviourElementModelsPool behaviourElementModelsPool = root.Q<BehaviourElementModelsPool>();
            
            string behaviourTreeAssetPath = BehaviourCanvasPaths.BehaviourTreeAssets +"/BehaviourTreeAsset.asset";
            BehaviourTreeAsset treeAsset = AssetDatabase.LoadAssetAtPath<BehaviourTreeAsset>(behaviourTreeAssetPath);
            EditorModelSerializer editorModelSerializer = new EditorModelSerializer(treeAsset);
            ViewSerializer viewSerializer = new ViewSerializer(treeAsset);
            
            CanvasModel canvasModel = new CanvasModel();
            _canvasController = new CanvasController(canvasModel, editorModelSerializer);
            
            _canvasView.Initialize(canvasModel, _canvasController, viewSerializer);
            _canvasController.Initialize();
            behaviourElementModelsPool.Initialize(_canvasController);
            
            _toolbarButton = root.Q<ToolbarButton>();
            _toolbarButton.clicked += () =>
            {
                _canvasController.SaveModel();
                _canvasView.Serialize();
            };
        }
        
        private void OnDisable()
        {
            _canvasController.Dispose();
            _canvasView.Dispose();
        }
        
        private void AddStylesheets()
        {
            StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Code/Editor/EditorWindows/" +
                                                                              "BehaviourTreeEditor/BehaviourCanvasEditor.uss");
            rootVisualElement.styleSheets.Add(stylesheet);
        }
    }
}
