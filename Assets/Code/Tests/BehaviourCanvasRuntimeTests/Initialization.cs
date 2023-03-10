using System.Collections;
using UnityEngine.TestTools;

namespace Code.Tests.BehaviourCanvasRuntimeTests
{
    public class Initialization : IPrebuildSetup, IPostBuildCleanup
    {
        [UnityTest]
        public IEnumerator InitializationWithEnumeratorPasses()
        {
            yield return null;
        }

        public void Setup()
        {
            throw new System.NotImplementedException();
        }

        public void Cleanup()
        {
            
        }
    }
}