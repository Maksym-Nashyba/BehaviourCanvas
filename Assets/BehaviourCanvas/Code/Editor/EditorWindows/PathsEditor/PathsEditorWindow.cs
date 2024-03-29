using System.Collections.Generic;
using BehaviourCanvas.Code.Editor.EditorWindows.PopUpWindow;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviourCanvas.Code.Editor.EditorWindows.PathsEditor
{
    public class PathsEditorWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset = default;
        private Dictionary<string, PathVisualElement> _idToPathField;

        [MenuItem("Window/CanvasController/PathsEditor")]
        public static void ShowWindow()
        {
            PathsEditorWindow wnd = GetWindow<PathsEditorWindow>();
            wnd.titleContent = new GUIContent("PathsEditor");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            _visualTreeAsset.CloneTree(root);
            _idToPathField = new Dictionary<string, PathVisualElement>();

            foreach (string id in BehaviourCanvasPaths.Ids)
            {
                PathVisualElement pathVisualElement = new PathVisualElement();
                pathVisualElement.Setup(id, () => OnClicked(id));
                _idToPathField.Add(id, pathVisualElement);
                root.Add(pathVisualElement);
            }
        }

        private void OnClicked(string id)
        {
            string input = _idToPathField[id].PathText;
            try { ValidatePath(input); }
            catch (InvalidPathException e)
            {
                PopUp.Show($"'{input}' is not a valid Path\n{e}");
                return;
            }
            input = input.TrimEnd('/');
            BehaviourCanvasPaths.UpdatePath(id, input);
            PopUp.Show("Path changed");
        }

        private void ValidatePath(string path)
        {
            if (!path.StartsWith("Assets")) throw new InvalidPathException("Should start with 'Assets'");
            if (!AssetDatabase.IsValidFolder(path)) throw new InvalidPathException("Directory doesn't exist");
        }
    }
}
