using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Editor.EditorWindows.BehaviourTreeEditor
{
    public class BehaviourCanvasEditor : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset = default;
        private BehaviourTreeAsset _treeAsset;
        private BehaviourCanvasSerializer _serializer;
        private BehaviourCanvas _canvas;
        private BehaviourCanvasView _canvasView;

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

            StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Code/Editor/EditorWindows/" +
                                                                              "BehaviourTreeEditor/BehaviourCanvasEditor.uss");
            root.styleSheets.Add(stylesheet);

            _canvasView = root.Q<BehaviourCanvasView>();
        }
    }
}
