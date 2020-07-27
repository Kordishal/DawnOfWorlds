using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace UnityTests
{
    public class TestTileFeatureLoader
    {
        [Test]
        public void TestTileFeatureLoaderSimplePasses()
        {
            // Use the Assert class to test conditions.
            
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator TestTileFeatureLoaderWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}