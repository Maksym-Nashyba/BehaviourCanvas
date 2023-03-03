using System.Collections.Generic;
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
            
            string behaviourTreeAssetPath = BehaviourCanvasPaths.BehaviourTreeAssets +"/BehaviourTreeAsset.asset";
            BehaviourTreeAsset treeAsset = AssetDatabase.LoadAssetAtPath<BehaviourTreeAsset>(behaviourTreeAssetPath);
        
            BehaviourCanvasSerializer canvasSerializer = new BehaviourCanvasSerializer(treeAsset);
            BehaviourCanvas canvas = CreateBehaviourCanvas(canvasSerializer);
        
            BehaviourCanvasView canvasView = root.Q<BehaviourCanvasView>();
            ModelBuilder nodeBuilder = root.Q<ModelBuilder>();
            canvasView.Initialize(canvas, canvasSerializer);
            nodeBuilder.Initialize(canvas, canvasView);
            
            _toolbarButton = root.Q<ToolbarButton>();
            _toolbarButton.clicked += () =>
            {
                canvasSerializer.Serialize(canvas, canvasView);
            };
        }
        
        private void AddStylesheets()
        {
            StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Code/Editor/EditorWindows/" +
                                                                              "BehaviourTreeEditor/BehaviourCanvasEditor.uss");
            rootVisualElement.styleSheets.Add(stylesheet);
        }

        private BehaviourCanvas CreateBehaviourCanvas(BehaviourCanvasSerializer serializer)
        {
            List<StateModel> states = serializer.DeserializeStateModels();
            StateModel rootState = serializer.FindRootState(states);
            List<TriggerModel> triggers = serializer.DeserializeTriggerModels();
            return new BehaviourCanvas(rootState, states, triggers);
        }
    }
}
