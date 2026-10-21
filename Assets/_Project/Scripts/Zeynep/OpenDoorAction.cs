using System;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Llm
{
    // Güvenlik ikna oldu: çıkış kapısı açılır ve oyun kazanılır.
    public sealed class OpenDoorAction : IGameAction
    {
        private readonly IOpenable door;

        public OpenDoorAction(IOpenable door)
        {
            if (door == null)
            {
                throw new ArgumentNullException(nameof(door));
            }

            this.door = door;
        }

        public string Id => "open_door";

        public void Execute(IGameState state)
        {
            door.Open();
            state.Win();
        }
    }
}
