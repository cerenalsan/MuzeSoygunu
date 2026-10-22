using NUnit.Framework;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Llm.Tests
{
    public class StageSceneMapTests
    {
        [TestCase(GameStage.MainMenu, "00_Menu")]
        [TestCase(GameStage.Entrance, "01_Giris")]
        [TestCase(GameStage.Statue, "01_Giris")]
        [TestCase(GameStage.LaserRoom, "02_Lazer")]
        [TestCase(GameStage.Diamond, "02_Lazer")]
        [TestCase(GameStage.Corridor, "03_Cikis")]
        [TestCase(GameStage.GuardDialogue, "03_Cikis")]
        [TestCase(GameStage.Won, "04_Sonuc")]
        [TestCase(GameStage.Lost, "04_Sonuc")]
        public void Asama_DogruSahneyeEslenir(GameStage stage, string expectedScene)
        {
            Assert.AreEqual(expectedScene, StageSceneMap.SceneFor(stage));
        }

        [Test]
        public void TanimsizAsama_NullDoner()
        {
            Assert.IsNull(StageSceneMap.SceneFor((GameStage)99));
        }
    }
}
