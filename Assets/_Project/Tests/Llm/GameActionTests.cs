using System;
using NUnit.Framework;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Llm.Tests
{
    public class GameActionTests
    {
        // Test için IGameState'in yapay uygulaması: hangi çağrıların yapıldığını sayar.
        private sealed class FakeGameState : IGameState
        {
            public int WinCalls;
            public int LoseCalls;
            public LoseReason LastReason = LoseReason.None;

            public GameStage CurrentStage => GameStage.GuardDialogue;
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

        // Test için IOpenable'ın yapay uygulaması.
        private sealed class FakeDoor : IOpenable
        {
            public bool IsOpen { get; private set; }
            public void Open() { IsOpen = true; }
            public void Close() { IsOpen = false; }
        }

        private FakeGameState state;
        private FakeDoor door;
        private DenialCounter counter;
        private GameActionFactory factory;

        [SetUp]
        public void SetUp()
        {
            state = new FakeGameState();
            door = new FakeDoor();
            counter = new DenialCounter(3);
            factory = new GameActionFactory(door, counter);
        }

        [Test]
        public void OpenDoor_KapiyiAcarVeKazandirir()
        {
            new OpenDoorAction(door).Execute(state);

            Assert.IsTrue(door.IsOpen);
            Assert.AreEqual(1, state.WinCalls);
            Assert.AreEqual(0, state.LoseCalls);
        }

        [Test]
        public void Alarm_GuardAlarmIleKaybettirir()
        {
            new AlarmAction().Execute(state);

            Assert.AreEqual(1, state.LoseCalls);
            Assert.AreEqual(LoseReason.GuardAlarm, state.LastReason);
            Assert.AreEqual(0, state.WinCalls);
        }

        [Test]
        public void Deny_SinirinAltinda_KaybettirmezAmaSayar()
        {
            var deny = new DenyAction(counter);

            deny.Execute(state);
            deny.Execute(state);
            deny.Execute(state);

            Assert.AreEqual(3, counter.Count);
            Assert.AreEqual(0, state.LoseCalls);
        }

        [Test]
        public void Deny_SinirAsilinca_GuardRefusedIleKaybettirir()
        {
            var deny = new DenyAction(counter);

            deny.Execute(state);
            deny.Execute(state);
            deny.Execute(state);
            deny.Execute(state);

            Assert.AreEqual(1, state.LoseCalls);
            Assert.AreEqual(LoseReason.GuardRefused, state.LastReason);
        }

        [Test]
        public void Fabrika_OpenDoorIcinDogruEylemUretir()
        {
            IGameAction action = factory.Create(NpcAction.OpenDoor);

            Assert.IsNotNull(action);
            Assert.AreEqual("open_door", action.Id);
        }

        [Test]
        public void Fabrika_DenyIcinDogruEylemUretir()
        {
            IGameAction action = factory.Create(NpcAction.Deny);

            Assert.IsNotNull(action);
            Assert.AreEqual("deny", action.Id);
        }

        [Test]
        public void Fabrika_AlarmIcinDogruEylemUretir()
        {
            IGameAction action = factory.Create(NpcAction.Alarm);

            Assert.IsNotNull(action);
            Assert.AreEqual("alarm", action.Id);
        }

        [Test]
        public void Fabrika_TanimsizDegerdeNullDoner()
        {
            IGameAction action = factory.Create((NpcAction)99);

            Assert.IsNull(action);
        }
    }
}
