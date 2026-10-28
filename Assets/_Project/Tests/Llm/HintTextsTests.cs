using NUnit.Framework;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Llm.Tests
{
    public class HintTextsTests
    {
        [Test]
        public void Giris_ZiplamayiAnlatir()
        {
            Assert.IsTrue(HintTexts.For(GameStage.Entrance).Contains("Space"));
        }

        [Test]
        public void LazerOdasi_AjanEmriniAnlatir()
        {
            Assert.IsTrue(HintTexts.For(GameStage.LaserRoom).Contains("Q"));
        }

        [Test]
        public void Elmas_EtkilesimiAnlatir()
        {
            Assert.IsTrue(HintTexts.For(GameStage.Diamond).Contains("E"));
        }

        [TestCase(GameStage.MainMenu)]
        [TestCase(GameStage.GuardDialogue)]
        [TestCase(GameStage.Won)]
        [TestCase(GameStage.Lost)]
        public void DigerAsamalar_IpucuYok(GameStage stage)
        {
            Assert.AreEqual(string.Empty, HintTexts.For(stage));
        }
    }
}