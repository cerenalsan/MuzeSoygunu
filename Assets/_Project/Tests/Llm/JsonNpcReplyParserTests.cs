using NUnit.Framework;

namespace MuzeSoygunu.Llm.Tests
{
    public class JsonNpcReplyParserTests
    {
        private JsonNpcReplyParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new JsonNpcReplyParser();
        }

        [Test]
        public void GecerliJson_BasariylaAyristirilir()
        {
            bool ok = parser.TryParse("{\"say\": \"Merhaba.\", \"action\": \"deny\"}", out NpcReply reply, out string error);

            Assert.IsTrue(ok);
            Assert.IsNull(error);
            Assert.AreEqual("Merhaba.", reply.Say);
            Assert.AreEqual(NpcAction.Deny, reply.Action);
        }

        [Test]
        public void BosCevap_Reddedilir()
        {
            bool ok = parser.TryParse("   ", out NpcReply reply, out string error);

            Assert.IsFalse(ok);
            Assert.IsNull(reply);
            Assert.IsNotNull(error);
        }

        [Test]
        public void BosJsonNesnesi_Reddedilir()
        {
            bool ok = parser.TryParse("{}", out NpcReply reply, out string error);

            Assert.IsFalse(ok);
            Assert.IsNull(reply);
            Assert.IsNotNull(error);
        }

        [Test]
        public void BozukJson_Reddedilir()
        {
            bool ok = parser.TryParse("{\"say\": \"Merhaba.\", \"action\": ", out NpcReply reply, out string error);

            Assert.IsFalse(ok);
            Assert.IsNull(reply);
            Assert.IsNotNull(error);
        }

        [Test]
        public void JsonOlmayanMetin_Reddedilir()
        {
            bool ok = parser.TryParse("Lütfen neden çıkmak istediğinizi açıklayın.", out NpcReply reply, out string error);

            Assert.IsFalse(ok);
            Assert.IsNull(reply);
            Assert.IsNotNull(error);
        }

        [Test]
        public void CitliCevap_BasariylaAyristirilir()
        {
            string raw = "```json\n{\"say\": \"Dur.\", \"action\": \"alarm\"}\n```";

            bool ok = parser.TryParse(raw, out NpcReply reply, out string error);

            Assert.IsTrue(ok);
            Assert.AreEqual("Dur.", reply.Say);
            Assert.AreEqual(NpcAction.Alarm, reply.Action);
        }

        [Test]
        public void FazladanMetinliCevap_BasariylaAyristirilir()
        {
            string raw = "Tabii, işte cevabım: {\"say\": \"Geçebilirsin.\", \"action\": \"open_door\"} Umarım yardımcı olur.";

            bool ok = parser.TryParse(raw, out NpcReply reply, out string error);

            Assert.IsTrue(ok);
            Assert.AreEqual("Geçebilirsin.", reply.Say);
            Assert.AreEqual(NpcAction.OpenDoor, reply.Action);
        }

        [Test]
        public void BilinmeyenAction_Reddedilir()
        {
            bool ok = parser.TryParse("{\"say\": \"Tamam.\", \"action\": \"kapiyi_ac\"}", out NpcReply reply, out string error);

            Assert.IsFalse(ok);
            Assert.IsNull(reply);
            Assert.IsNotNull(error);
        }

        [Test]
        public void BuyukHarfliAction_KabulEdilir()
        {
            bool ok = parser.TryParse("{\"say\": \"Alarm!\", \"action\": \"ALARM\"}", out NpcReply reply, out string error);

            Assert.IsTrue(ok);
            Assert.AreEqual(NpcAction.Alarm, reply.Action);
        }
    }
}
