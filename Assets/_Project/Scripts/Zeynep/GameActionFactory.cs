using System;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Llm
{
    // NpcAction değerini oyun durumuna uygulanabilir bir IGameAction nesnesine çevirir.
    public sealed class GameActionFactory
    {
        private readonly IOpenable exitDoor;
        private readonly DenialCounter denialCounter;

        public GameActionFactory(IOpenable exitDoor, DenialCounter denialCounter)
        {
            if (exitDoor == null)
            {
                throw new ArgumentNullException(nameof(exitDoor));
            }

            if (denialCounter == null)
            {
                throw new ArgumentNullException(nameof(denialCounter));
            }

            this.exitDoor = exitDoor;
            this.denialCounter = denialCounter;
        }

        // Tanınmayan bir değer gelirse null döner; çağıran bunu geçersiz cevap sayar.
        public IGameAction Create(NpcAction action)
        {
            switch (action)
            {
                case NpcAction.OpenDoor:
                    return new OpenDoorAction(exitDoor);
                case NpcAction.Deny:
                    return new DenyAction(denialCounter);
                case NpcAction.Alarm:
                    return new AlarmAction();
                default:
                    return null;
            }
        }
    }
}
