using System;
using Unity.Properties;
using UnityEditor;
using UnityEngine;

namespace BehaviourCanvas.Code.Editor
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "PATHS", order = 0)]
    public class BehaviourCanvasPaths : ScriptableObject
    {
        [SerializeField] private string _behaviourCanvasRoot;
        [SerializeField] private string _stateScritps;
        [SerializeField] private string _triggerScripts;
        [SerializeField] private string _behaviourTreeAssets;

        private static BehaviourCanvasPaths _instance;
        private static BehaviourCanvasPaths Instance
        {
            get
            {
                if (_instance == null)
                {
                    string[] assets = AssetDatabase.FindAssets("t:BehaviourCanvasPaths");
                    _instance = AssetDatabase.LoadAssetAtPath<BehaviourCanvasPaths>(AssetDatabase.GUIDToAssetPath(assets[0]));
                }

                return _instance;
            }
        }

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
            set => UpdatePath("behaviour_canvas_root", value);
        }

        public static string Templates => GetSavedPath("behaviour_canvas_root") + "/Code/Templates";
        
        public static string Tests => GetSavedPath("behaviour_canvas_root") + "/Code/Tests";
        
        public static string StateScripts
        {
            get => GetSavedPath("state_scripts");
            set => UpdatePath("state_scripts", value);
        }
        
        public static string TriggerScripts
        {
            get => GetSavedPath("trigger_scripts");
            set => UpdatePath("trigger_scripts", value);
        }

        public static string BehaviourTreeAssets
        {
            get => GetSavedPath("behaviour_tree_assets");
            set => UpdatePath("behaviour_tree_assets", value);
        }

        public static string GetSavedPath(string id)
        {
            if (!EditorPrefs.HasKey(id)) throw new InvalidPathException($"Path not set for {id}. Go to 'Window/CanvasController/PathsEditor'");
            string result = String.Empty;
            switch (id)
            {
                case "behaviour_canvas_root":
                    result = Instance._behaviourCanvasRoot;
                    break;
                case "state_scripts":
                    result = Instance._stateScritps;
                    break;
                case "trigger_scripts":
                    result = Instance._triggerScripts;
                    break;
                case "behaviour_tree_assets":
                    result = Instance._behaviourTreeAssets;
                    break;
            }
            return result;
        }

        public static void UpdatePath(string id, string newPath)
        {
            if (!AssetDatabase.IsValidFolder(newPath)) throw new InvalidPathException($"'{newPath}' is not a valid directory for '{id}'");
            switch (id)
            {
                case "behaviour_canvas_root":
                    Instance._behaviourCanvasRoot = newPath;
                    break;
                case "state_scripts":
                    Instance._stateScritps = newPath;
                    break;
                case "trigger_scripts":
                    Instance._triggerScripts = newPath;
                    break;
                case "behaviour_tree_assets":
                    Instance._behaviourTreeAssets = newPath;
                    break;
            }
        }
    }
}