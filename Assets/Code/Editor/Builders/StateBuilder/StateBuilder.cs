using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Editor.Builders.StateBuilder
{
    public class StateBuilder : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset = default;

        [MenuItem("Window/StateBuilder")]
        public static void ShowExample()
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
