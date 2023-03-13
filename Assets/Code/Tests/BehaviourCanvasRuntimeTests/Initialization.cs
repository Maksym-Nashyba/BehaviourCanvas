using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Code.Tests.BehaviourCanvasRuntimeTests
{
    public class Initialization : IPrebuildSetup, IPostBuildCleanup
    {
        [OneTimeSetUp]
        public void Setup()
        {
            TestScenes.IncludeInBuild(TestScenes.Initialization);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            TestScenes.RemoveFromBuild(TestScenes.Initialization);
        }
        
        [UnityTest]
        public IEnumerator InitializationScene_Loads()
        {
            AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("Initialization");
            while (!sceneLoading.isDone)
            {
                yield return null;
            }
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Initialization"));
        }
        
        [UnityTest]
        public IEnumerator InitializationWithEnumeratorPasses()
        {
            yield return null;
        }
    }
}