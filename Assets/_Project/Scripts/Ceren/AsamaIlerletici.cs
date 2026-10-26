using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Ceren
{
    public static class AsamaIlerletici
    {
        public static void Ilerlet(IGameState durum, GameStage hedef)
        {
            while (durum.CurrentStage < hedef)
            {
                GameStage onceki = durum.CurrentStage;
                durum.AdvanceTo((GameStage)((int)onceki + 1));
                if (durum.CurrentStage == onceki) break;
            }
        }

        public static bool OyunBitti(IGameState durum)
        {
            return durum.CurrentStage == GameStage.Lost || durum.CurrentStage == GameStage.Won;
        }
    }
}