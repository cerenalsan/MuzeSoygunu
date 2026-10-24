using System;
using NUnit.Framework;

namespace MuzeSoygunu.Llm.Tests
{
    public class ConversationHistoryTests
    {
        [Test]
        public void Mesajlar_EklenmeSirasiylaTutulur()
        {
            var history = new ConversationHistory(6);

            history.AddPlayerMessage("a");
            history.AddNpcMessage("b");

            Assert.AreEqual(2, history.Turns.Count);
            Assert.AreEqual(ChatRole.User, history.Turns[0].Role);
            Assert.AreEqual("a", history.Turns[0].Content);
            Assert.AreEqual(ChatRole.Assistant, history.Turns[1].Role);
        }

        [Test]
        public void SinirAsilinca_EnEskiMesajlarSilinir()
        {
            var history = new ConversationHistory(3);

            history.AddPlayerMessage("1");
            history.AddNpcMessage("2");
            history.AddPlayerMessage("3");
            history.AddNpcMessage("4");

            Assert.AreEqual(3, history.Turns.Count);
            Assert.AreEqual("2", history.Turns[0].Content);
            Assert.AreEqual("4", history.Turns[2].Content);
        }

        [Test]
        public void Clear_GecmisiTemizler()
        {
            var history = new ConversationHistory(6);
            history.AddPlayerMessage("a");

            history.Clear();

            Assert.AreEqual(0, history.Turns.Count);
        }

        [Test]
        public void GecersizSinir_IstisnaFirlatir()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ConversationHistory(0));
        }
    }
}
