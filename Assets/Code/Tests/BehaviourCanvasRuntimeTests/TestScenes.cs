using System;
using System.IO;
using System.Linq;
using Code.Editor;
using Unity.Properties;
using UnityEditor;
using UnityEngine;

namespace Code.Tests.BehaviourCanvasRuntimeTests
{
    public static class TestScenes
    {
        public static EditorBuildSettingsScene Initialization => FindScene("Initialization");

        public static void IncludeInBuild(EditorBuildSettingsScene scene)
        {
            if(EditorBuildSettings.scenes.Contains(scene))return;

            EditorBuildSettings.scenes = EditorBuildSettings.scenes.Append(scene).ToArray();
        }
        
        public static void RemoveFromBuild(EditorBuildSettingsScene scene)
        {
            if (!EditorBuildSettings.scenes.Contains(scene)) throw new ArgumentException($"Scene {scene} isn't included in build.");

            EditorBuildSettings.scenes = Array.Empty<EditorBuildSettingsScene>();
            return;
            
            EditorBuildSettings.scenes =
                EditorBuildSettings.scenes.Where(includedScene => includedScene.path != scene.path).ToArray();
        }
        
        private static EditorBuildSettingsScene FindScene(string name)
        {
            string path = BehaviourCanvasPaths.Tests + "/TestScenes";
            if (!AssetDatabase.IsValidFolder(path))
                throw new InvalidPathException($"Couldn't find test scene directory. Path: {path}");
            if(!File.Exists(Application.dataPath.Substring(0, Application.dataPath.Length - 6) + path + "/" + name + ".unity"))
                throw new InvalidPathException($"Scene {name} doesn't exist.");
            
            if (!name.EndsWith(".unity")) name += ".unity";
            return new EditorBuildSettingsScene(path+"/"+name, true);
        }
    }
}