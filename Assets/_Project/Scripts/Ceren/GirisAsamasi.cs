using MuzeSoygunu.Shared;
using UnityEngine;

namespace MuzeSoygunu.Ceren
{
    public class GirisAsamasi : MonoBehaviour
    {
        private void Start()
        {
            IGameState durum = GameStateManager.Instance;
            if (durum == null)
            {
                return;
            }

            if (durum.CurrentStage == GameStage.MainMenu)
            {
                durum.AdvanceTo(GameStage.Entrance);
            }
        }
    }
}