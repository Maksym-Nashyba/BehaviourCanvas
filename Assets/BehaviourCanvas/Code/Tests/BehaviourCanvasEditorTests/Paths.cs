using System.Collections;
using Code.Editor;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine.TestTools;

namespace Code.Tests
{
    public class Paths
    {
        [Test]
        public void AllPathsFilled()
        {
            foreach (string id in BehaviourCanvasPaths.Ids)
            {
                if(!EditorPrefs.HasKey(id)) Assert.Fail($"No path set for {id}. Go to Window/CanvasController/PathsEditor");
            }
            Assert.Pass();
        }
        
        [Test]
        public void BehaviourTreeAssetsInCorrectFolder()
        {
            string[] behaviourTreeAssetPaths = AssetDatabase.FindAssets("t:BehaviourTreeAsset");
            foreach (string assetPath in behaviourTreeAssetPaths)
            {
                if (assetPath.StartsWith(BehaviourCanvasPaths.BehaviourTreeAssets))
                {
                    Assert.Fail($"Asset '{assetPath}' is in wrong directory.\nCorrect directory: '{BehaviourCanvasPaths.BehaviourTreeAssets}'");
                }
            }
            Assert.Pass();
        }
    }
}