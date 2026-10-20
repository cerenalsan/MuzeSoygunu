using UnityEngine;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Ceren
{
    [RequireComponent(typeof(Collider2D))]
    public class GecisKapisi : MonoBehaviour
    {
        [SerializeField] private GameStage hedefAsama = GameStage.LaserRoom;

        private bool kullanildi;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (kullanildi || !other.CompareTag("Player")) return;
            kullanildi = true;

            IGameState durum = GameStateManager.Instance;
            while (durum.CurrentStage < hedefAsama)
            {
                GameStage onceki = durum.CurrentStage;
                durum.AdvanceTo((GameStage)((int)onceki + 1));
                if (durum.CurrentStage == onceki) break;
            }
            Debug.Log("Gecis kapisi: aşama " + durum.CurrentStage);
        }
    }
}