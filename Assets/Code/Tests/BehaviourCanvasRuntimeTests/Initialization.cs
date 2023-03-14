using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Code.Tests.BehaviourCanvasRuntimeTests
{
    public class Initialization : IPrebuildSetup, IPostBuildCleanup
    {
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
        }

        public void Cleanup()
        {
            TestScenes.RemoveFromBuild(TestScenes.Initialization);
        }
        
        [Test]
        public void InitializationScene_Loads()
        {
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Initialization"));
        }
    }
}