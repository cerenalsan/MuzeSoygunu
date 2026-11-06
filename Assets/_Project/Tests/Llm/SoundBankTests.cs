using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace MuzeSoygunu.Llm.Tests
{
    public class SoundBankTests
    {
        private static SoundBank CreateBank(params SoundBank.Entry[] entries)
        {
            SoundBank bank = ScriptableObject.CreateInstance<SoundBank>();
            FieldInfo field = typeof(SoundBank).GetField("entries", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(bank, entries);
            return bank;
        }

        [Test]
        public void AtanmisSes_Dondurulur()
        {
            AudioClip clip = AudioClip.Create("test", 1, 1, 44100, false);
            SoundBank bank = CreateBank(new SoundBank.Entry { id = SoundId.Win, clip = clip });

            Assert.AreSame(clip, bank.Get(SoundId.Win));
        }

        [Test]
        public void AtanmamisSes_NullDoner()
        {
            SoundBank bank = CreateBank();

            Assert.IsNull(bank.Get(SoundId.Alarm));
        }

        [Test]
        public void BosBank_HataVermez()
        {
            SoundBank bank = ScriptableObject.CreateInstance<SoundBank>();

            Assert.IsNull(bank.Get(SoundId.Step));
        }
    }
}
