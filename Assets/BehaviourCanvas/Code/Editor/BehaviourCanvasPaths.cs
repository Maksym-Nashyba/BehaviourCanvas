using Unity.Properties;
using UnityEditor;

namespace Code.Editor
{
    public static class BehaviourCanvasPaths
    {
        public static readonly string[] Ids =
        {
            "behaviour_canvas_root",
            "state_scripts",
            "trigger_scripts",
            "behaviour_tree_assets"
        };
        
        public static string BehaviourCanvasRoot
        {
            get => GetSavedPath("behaviour_canvas_root");
            set => UpdatePath(BehaviourCanvasRoot, value);
        }

        public static string Templates => GetSavedPath("behaviour_canvas_root") + "/Code/Templates";
        
        public static string Tests => GetSavedPath("behaviour_canvas_root") + "/Code/Tests";
        
        public static string StateScripts
        {
            get => GetSavedPath("state_scripts");
            set => UpdatePath(StateScripts, value);
        }
        
        public static string TriggerScripts
        {
            get => GetSavedPath("trigger_scripts");
            set => UpdatePath(TriggerScripts, value);
        }

        public static string BehaviourTreeAssets
        {
            get => GetSavedPath("behaviour_tree_assets");
            set => UpdatePath(BehaviourTreeAssets, value);
        }

        public static string GetSavedPath(string id)
        {
            if (!EditorPrefs.HasKey(id)) throw new InvalidPathException($"Path not set for {id}. Go to 'Window/CanvasController/PathsEditor'");
            return EditorPrefs.GetString(id);
        }

        public static void UpdatePath(string id, string newPath)
        {
            if (!AssetDatabase.IsValidFolder(newPath)) throw new InvalidPathException($"'{newPath}' is not a valid directory for '{id}'");
            EditorPrefs.SetString(id, newPath);
        }
    }
}