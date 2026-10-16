using System;

namespace MuzeSoygunu.Shared
{
    // Oyunun durumunu okuyan ve değiştiren herkesin göreceği sözleşme.
    public interface IGameState
    {
        GameStage CurrentStage { get; }
        LoseReason LastLoseReason { get; }
        bool LasersDisabled { get; }
        bool DiamondTaken { get; }

        // Aşama değişince haber verir.
        event Action<GameStage> StageChanged;

        // İzin verilen bir geçişse true döner, değilse false (oyun çökmez).
        bool AdvanceTo(GameStage next);

        void MarkLasersDisabled();
        void MarkDiamondTaken();
        void Win();
        void Lose(LoseReason reason);
    }
}