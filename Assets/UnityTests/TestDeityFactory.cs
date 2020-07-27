using System.IO;
using Model.Deity;
using NUnit.Framework;
using Player.Data;

namespace UnityTests
{
    [TestFixture]
    public class TestDeityFactory
    {
        private const string Path =
            "/home/jonas/Documents/projects/unity/DawnOfWorlds/DawnOfWorlds/testdata/dietyfactory/";
        
        [Test]
        public void TestInitializationWithoutFile()
        { 
            DeityFactory.GetInstance(Path + "test1");
            Assert.That(File.Exists(Path + "test1/deities/deities.json"), "File at " + Path + "test1/deities/deities.json should exist.");
        }

        [Test]
        public void TestCreateFirstDeity()
        {
            var factory = DeityFactory.GetInstance(Path + "test2");
            factory.CreateDeity("Thor");
            Assert.AreEqual(new Deity(1, "Thor", 0), factory.CurrentDeity);
        }


        [OneTimeTearDown]
        public void CleanUpDirectories()
        {
            File.Delete(Path + "test1/deities/deities.json");
            Directory.Delete(Path + "test1/deities");
            File.Delete(Path + "test2/deities/deities.json");
            Directory.Delete(Path + "test1/deities");
        }
    }
}
