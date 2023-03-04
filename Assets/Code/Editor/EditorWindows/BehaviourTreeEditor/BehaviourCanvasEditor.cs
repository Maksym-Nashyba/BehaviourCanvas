using Code.Editor.Serializers;
using Code.Runtime;
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
            CanvasController canvasController = new CanvasController(canvasModel, editorModelSerializer);
            
            _canvasView.Initialize(canvasModel, canvasController, viewSerializer);
            canvasController.Initialize();
            behaviourElementModelsPool.Initialize(canvasController);
            
            _toolbarButton = root.Q<ToolbarButton>();
            _toolbarButton.clicked += () =>
            {
                canvasController.SerializeModel();
                _canvasView.Serialize();
            };
        }
        
        private void OnDisable()
        {
            _canvasView.UnsubscribeFromEvents();
        }
        
        private void AddStylesheets()
        {
            StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Code/Editor/EditorWindows/" +
                                                                              "BehaviourTreeEditor/BehaviourCanvasEditor.uss");
            rootVisualElement.styleSheets.Add(stylesheet);
        }
    }
}
