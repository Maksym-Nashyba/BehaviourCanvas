using Code.Editor;
using Code.Runtime.StateMachineElements;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Code.Tests.BehaviourCanvasRuntimeTests
{
    [TestFixture]
    public class StateMachineInitialization : ZenjectUnitTestFixture
    {
        private GameObject _stateMachinePrefab;
        
        [SetUp]
        public void SetUp()
        {
            Container.Bind<BehaviourTreeBuilder>().FromNew().AsSingle();
            _stateMachinePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(BehaviourCanvasPaths.Tests+"/TestAssets/StateMachinePrefab.prefab");
        }

        [Test]
        public void Instantiation_DoesntThrow()
        {
            Container.InstantiatePrefab(_stateMachinePrefab);
            Assert.Pass();
        }
        
        [Test]
        public void TestAssets_Load()
        {
            Assert.That(_stateMachinePrefab, Is.Not.EqualTo(null));
            Assert.That(Container.Resolve<BehaviourTreeBuilder>(), Is.Not.Null);
        }
    }
}