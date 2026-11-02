using System.IO;
using NUnit.Framework;
using UnityEngine;

namespace MuzeSoygunu.Llm.Tests
{
    public class PromptFreezeTests
    {
        private static string Normalize(string text)
        {
            return text.Replace("\r\n", "\n").TrimEnd();
        }

        [Test]
        public void SistemPromptu_OlcumDosyasiylaAyniDir()
        {
            string path = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "docs", "model-testi", "prompt-v2.txt"));
            Assert.IsTrue(File.Exists(path), "prompt-v2.txt bulunamadı: " + path);

            string fromFile = Normalize(File.ReadAllText(path));
            string fromBuilder = Normalize(new GuardPromptBuilder().BuildSystemPrompt());

            Assert.AreEqual(fromFile, fromBuilder);
        }

        [Test]
        public void Hatirlatma_Kapali()
        {
            Assert.AreEqual(string.Empty, new GuardPromptBuilder().BuildReminder());
        }

        [Test]
        public void Surum_V2()
        {
            Assert.AreEqual("v2", GuardPromptBuilder.Version);
        }
    }
}
