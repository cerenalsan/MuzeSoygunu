using NUnit.Framework;

namespace MuzeSoygunu.Llm.Tests
{
    public class LlmHealthCheckTests
    {
        private const string Tags = "{\"models\":[{\"name\":\"gemma2:2b\"},{\"name\":\"llama3.2:latest\"}]}";

        [Test]
        public void TamAdEslesir()
        {
            Assert.IsTrue(LlmHealthCheck.ContainsModel(Tags, "gemma2:2b"));
        }

        [Test]
        public void EtiketsizAd_LatestIleEslesir()
        {
            Assert.IsTrue(LlmHealthCheck.ContainsModel(Tags, "llama3.2"));
        }

        [Test]
        public void OlmayanModel_Eslesmez()
        {
            Assert.IsFalse(LlmHealthCheck.ContainsModel(Tags, "olmayan-model"));
        }

        [Test]
        public void BozukYaDaBosCevap_Eslesmez()
        {
            Assert.IsFalse(LlmHealthCheck.ContainsModel("", "gemma2:2b"));
            Assert.IsFalse(LlmHealthCheck.ContainsModel("bu json degil", "gemma2:2b"));
            Assert.IsFalse(LlmHealthCheck.ContainsModel("{}", "gemma2:2b"));
        }
    }
}
