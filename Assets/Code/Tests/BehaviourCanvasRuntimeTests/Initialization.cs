using Code.Runtime.BehaviourGraphSerialization;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using Code.Editor;
using Code.Runtime.States;
using Code.Runtime.Triggers;
using UnityEngine;

namespace Code.Tests.BehaviourCanvasRuntimeTests
{
    public class Initialization : IPrebuildSetup, IPostBuildCleanup
    {
        private TestStateMachine _stateMachine;
        
        #region Fixture

        public void Setup()
        {
            TestScenes.IncludeInBuild(TestScenes.Initialization);
        }

        [UnitySetUp]
        public IEnumerator OneTimeSetUp()
        {
            AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("Initialization");
            while (!sceneLoading.isDone)
            {
                yield return null;
            }

            _stateMachine = Object.FindObjectOfType<TestStateMachine>();
        }

        public void Cleanup()
        {
            TestScenes.RemoveFromBuild(TestScenes.Initialization);
        }

        #endregion
        
        [Test]
        public void InitializationScene_Loads()
        {
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Initialization"));
        }

        [Test]
        public void SerializedGraph_IsValid()
        {
            ModelSerializer modelSerializer = new ModelSerializer();
            Assert.DoesNotThrow(()=>
            {
                ModelGraphValidator.Validate(
                        modelSerializer.DeserializeModelGraph(_stateMachine.Asset.BehaviourTreeXML));
            });
        }

        [Test]
        public void BehaviourTree_Builds()
        {
            if(_stateMachine.Started)Assert.Inconclusive("Behaviour tree was already built on this instance of StateMachine");
            Assert.DoesNotThrow(() =>
            {
                _stateMachine.BuildTree();
            });
        }

        [Test]
        public void OnTrigger_StateTransitions()
        {
            if(!_stateMachine.Started) _stateMachine.BuildTree();
            if(_stateMachine.Tree.CurrentTriggers is null 
               || _stateMachine.Tree.CurrentTriggers.Count < 1) Assert.Inconclusive("The state machine is on the last state when test run");
            
            TestTrigger trigger = _stateMachine.Tree.CurrentTriggers[0] as TestTrigger;
            if (trigger is null) Assert.Inconclusive("Test graph has a non-test Trigger", trigger);

            TestState targetState = trigger.TargetState;
            trigger.Activate();
            _stateMachine.Step();
            Assert.That(_stateMachine.Tree.CurrentState, Is.EqualTo(targetState));
        }
    }
}