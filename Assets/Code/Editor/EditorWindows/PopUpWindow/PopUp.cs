using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Editor.EditorWindows.PopUpWindow
{
    public class PopUp : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset _visualTreeAsset = default;
    
        public static void Show(string message)
        {
            PopUp wnd = GetWindow<PopUp>();
            wnd.titleContent = new GUIContent("PopUp");
            wnd.rootVisualElement.Q<Label>().text = message;
            wnd.rootVisualElement.Q<Button>().clicked += () => wnd.Close();
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            _visualTreeAsset.CloneTree(root);
        }
    }
}
