using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Editor.EditorWindows.Builders.StateBuilder
{
    public class StateBuilder : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset = default;

        [MenuItem("Window/BehaviourCanvas/StateBuilder")]
        public static void ShowWindow()
        {
            StateBuilder wnd = GetWindow<StateBuilder>();
            wnd.titleContent = new GUIContent("StateBuilder");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            _visualTreeAsset.CloneTree(root);

        }
    }
}
