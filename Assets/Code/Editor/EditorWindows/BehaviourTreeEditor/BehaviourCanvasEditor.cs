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
        private BehaviourCanvas _canvas;

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
            
            BehaviourCanvasView canvasView = root.Q<BehaviourCanvasView>();
            ModelBuilder modelBuilder = root.Q<ModelBuilder>();
            
            string behaviourTreeAssetPath = BehaviourCanvasPaths.BehaviourTreeAssets +"/BehaviourTreeAsset.asset";
            BehaviourTreeAsset treeAsset = AssetDatabase.LoadAssetAtPath<BehaviourTreeAsset>(behaviourTreeAssetPath);
            BehaviourCanvasSerializer canvasSerializer = new BehaviourCanvasSerializer(treeAsset);
            ViewSerializer viewSerializer = new ViewSerializer(treeAsset);
            
            canvasView.Initialize(viewSerializer);
            modelBuilder.Initialize();

            _canvas = new BehaviourCanvas(canvasView, modelBuilder);
            _canvas.Initialize(canvasSerializer.DeserializeStateModels(), canvasSerializer.DeserializeTriggerModels());

            _toolbarButton = root.Q<ToolbarButton>();
            _toolbarButton.clicked += () =>
            {
                canvasSerializer.Serialize(_canvas.States, _canvas.Triggers);
                viewSerializer.Serialize(canvasView.Nodes);
            };
        }

        private void OnDisable()
        {
            _canvas.UnsubscribeFromEvents();
        }

        private void AddStylesheets()
        {
            StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Code/Editor/EditorWindows/" +
                                                                              "BehaviourTreeEditor/BehaviourCanvasEditor.uss");
            rootVisualElement.styleSheets.Add(stylesheet);
        }
    }
}
