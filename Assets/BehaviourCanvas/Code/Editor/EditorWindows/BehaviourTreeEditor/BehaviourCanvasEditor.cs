using System;
using Code.Editor.Serializers;
using Code.Runtime.BehaviourGraphSerialization;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Editor.EditorWindows.BehaviourTreeEditor
{
    public class BehaviourCanvasEditor : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset = default;
        private ToolbarButton _saveButton;
        private CanvasController _canvasController;
        private BehaviourCanvasView _canvasView;
        private BehaviourElementModelsPool _behaviourElementModelsPool;
        private Label _selectedAssetName;
        
        public static void OpenWithAsset(BehaviourTreeAsset asset)
        {
            BehaviourCanvasEditor wnd = GetWindow<BehaviourCanvasEditor>();
            wnd.titleContent = new GUIContent("BehaviourCanvasEditor");
            if (asset == null) throw new NullReferenceException("Behaviour Canvas Editor was opened with NULL as target asset.");
            wnd.Initialize(asset);
        }
        
        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            _visualTreeAsset.CloneTree(root);
            AddStylesheets();
            
            _canvasView = root.Q<BehaviourCanvasView>();
            _behaviourElementModelsPool = root.Q<BehaviourElementModelsPool>();
            _saveButton = root.Q<ToolbarButton>();
            _selectedAssetName = root.Q<Label>("SelectedAssetName");
            
            _saveButton.clicked += OnSaveButtonClicked;
        }

        private void Initialize(BehaviourTreeAsset treeAsset)
        {
            EditorModelSerializer editorModelSerializer = new EditorModelSerializer(treeAsset);
            ViewSerializer viewSerializer = new ViewSerializer(treeAsset);
            
            CanvasModel canvasModel = new CanvasModel();
            _canvasController = new CanvasController(canvasModel, editorModelSerializer);
            
            _canvasView.Initialize(canvasModel, _canvasController, viewSerializer);
            _canvasController.Initialize();
            _behaviourElementModelsPool.Initialize(_canvasController);

            _selectedAssetName.text = treeAsset.name;
        }

        private void OnDisable()
        {
            _behaviourElementModelsPool?.Dispose();
            _canvasController?.Dispose();
            _canvasView?.Dispose();
        }
        
        private void OnSaveButtonClicked()
        {
            _canvasController.SaveModel();
            _canvasView.Serialize();
        }
        
        private void AddStylesheets()
        {
            StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(BehaviourCanvasPaths.BehaviourCanvasRoot+"/Code/Editor/EditorWindows/" +
                                                                              "BehaviourTreeEditor/BehaviourCanvasEditor.uss");
            rootVisualElement.styleSheets.Add(stylesheet);
        }
    }
}
