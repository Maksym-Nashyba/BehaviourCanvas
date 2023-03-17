using Code.Editor;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Code.Tests.BehaviourCanvasRuntimeTests
{
    public class StateMachineInitialization
    {
        private GameObject _stateMachinePrefab;
        
        [OneTimeSetUp]
        public void SetUp()
        {
            _stateMachinePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(BehaviourCanvasPaths.Tests+"/TestAssets/StateMachinePrefab.asset");
        }
        
        [Test]
        public void TestAssets_Load()
        {
            
        }
    }
}