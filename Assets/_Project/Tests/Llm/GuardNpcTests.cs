using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Llm.Tests
{
    public class GuardNpcTests
    {
        private const string DenyJson = "{\"say\": \"Hayır.\", \"action\": \"deny\"}";

        // Test için ILlmClient'ın yapay uygulaması: sıradaki sonucu döner ve gelen mesaj sayısını kaydeder.
        private sealed class FakeClient : ILlmClient
        {
            public readonly Queue<LlmResult> Results = new Queue<LlmResult>();
            public int Calls;
            public int LastMessageCount;

            public Task<LlmResult> SendAsync(string systemPrompt, IReadOnlyList<ChatTurn> messages, CancellationToken cancellationToken)
            {
                Calls++;
                LastMessageCount = messages.Count;
                return Task.FromResult(Results.Dequeue());
            }
        }

        private sealed class FakeGameState : IGameState
        {
            public GameStage Stage = GameStage.GuardDialogue;
            public int WinCalls;
            public int LoseCalls;
            public LoseReason LastReason = LoseReason.None;

            public GameStage CurrentStage => Stage;
            public LoseReason LastLoseReason => LastReason;
            public bool LasersDisabled => true;
            public bool DiamondTaken => true;

            public event Action<GameStage> StageChanged
            {
                add { }
                remove { }
            }

            public bool AdvanceTo(GameStage next) { return true; }
            public void MarkLasersDisabled() { }
            public void MarkDiamondTaken() { }
            public void Win() { WinCalls++; }

            public void Lose(LoseReason reason)
            {
                LoseCalls++;
                LastReason = reason;
            }
        }

        private sealed class FakeDoor : IOpenable
        {
            public bool IsOpen { get; private set; }
            public void Open() { IsOpen = true; }
            public void Close() { IsOpen = false; }
        }

        private FakeClient client;
        private FakeGameState state;
        private DialogueSession session;

        private GuardNpc Build(int maxPlayerTurns)
        {
            client = new FakeClient();
            state = new FakeGameState();
            session = new DialogueSession(6, maxPlayerTurns);

            return new GuardNpc(
                client,
                new GuardPromptBuilder(),
                new JsonNpcReplyParser(),
                new GameActionFactory(new FakeDoor(), new DenialCounter(3)),
                state,
                new TurkishLlmErrorMessages(),
                session);
        }

        private static GuardTurnResult Talk(GuardNpc guard, string message)
        {
            return guard.TalkAsync(message, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Test]
        public void BasariliTur_GecmiseYazilir()
        {
            GuardNpc guard = Build(10);
            client.Results.Enqueue(LlmResult.Success(DenyJson));

            GuardTurnResult result = Talk(guard, "merhaba");

            Assert.IsFalse(result.IsSystemMessage);
            Assert.AreEqual(2, session.Messages.Count);
            Assert.AreEqual("merhaba", session.Messages[0].Content);
            Assert.AreEqual(1, session.PlayerTurns);
        }

        [Test]
        public void BasarisizIstek_GecmiseYazilmaz()
        {
            GuardNpc guard = Build(10);
            client.Results.Enqueue(LlmResult.Failure(LlmError.Timeout, "test"));

            GuardTurnResult result = Talk(guard, "merhaba");

            Assert.IsTrue(result.IsSystemMessage);
            Assert.AreEqual(0, session.Messages.Count);
            Assert.AreEqual(0, session.PlayerTurns);
        }

        [Test]
        public void IkinciIstek_OncekiMesajlariDaGonderir()
        {
            GuardNpc guard = Build(10);
            client.Results.Enqueue(LlmResult.Success(DenyJson));
            client.Results.Enqueue(LlmResult.Success(DenyJson));

            Talk(guard, "bir");
            Talk(guard, "iki");

            Assert.AreEqual(3, client.LastMessageCount);
        }

        [Test]
        public void TurSiniriAsilinca_KaybettirirVeIstekAtmaz()
        {
            GuardNpc guard = Build(2);
            client.Results.Enqueue(LlmResult.Success(DenyJson));
            client.Results.Enqueue(LlmResult.Success(DenyJson));

            Talk(guard, "bir");
            Talk(guard, "iki");
            GuardTurnResult result = Talk(guard, "uc");

            Assert.IsTrue(result.IsSystemMessage);
            Assert.AreEqual(GuardNpc.TurnLimitMessage, result.Text);
            Assert.AreEqual(1, state.LoseCalls);
            Assert.AreEqual(LoseReason.GuardRefused, state.LastReason);
            Assert.AreEqual(2, client.Calls);
        }

        [Test]
        public void DiyalogBitmisse_IstekAtmaz()
        {
            GuardNpc guard = Build(10);
            state.Stage = GameStage.Won;

            GuardTurnResult result = Talk(guard, "merhaba");

            Assert.IsTrue(result.IsSystemMessage);
            Assert.AreEqual(GuardNpc.DialogueEndedMessage, result.Text);
            Assert.AreEqual(0, client.Calls);
        }
    }

    public class DialogueSessionTests
    {
        [Test]
        public void Record_GecmiseVeSayaciYazar()
        {
            var session = new DialogueSession(6, 3);

            session.Record("a", "{}");

            Assert.AreEqual(1, session.PlayerTurns);
            Assert.AreEqual(2, session.Messages.Count);
        }

        [Test]
        public void TurSiniri_AsilincaBayrakTrueOlur()
        {
            var session = new DialogueSession(6, 2);

            session.Record("a", "{}");
            Assert.IsFalse(session.IsTurnLimitReached);

            session.Record("b", "{}");
            Assert.IsTrue(session.IsTurnLimitReached);
        }

        [Test]
        public void GecersizTurSiniri_IstisnaFirlatir()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DialogueSession(6, 0));
        }
    }
}
