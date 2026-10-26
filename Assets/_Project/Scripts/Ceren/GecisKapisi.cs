using UnityEngine;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Ceren
{
    [RequireComponent(typeof(Collider2D))]
    public class GecisKapisi : MonoBehaviour
    {
        [SerializeField] private GameStage hedefAsama = GameStage.LaserRoom;
        [SerializeField] private bool elmasGerekli;

        private bool kullanildi;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (kullanildi || !other.CompareTag("Player")) return;

            IGameState durum = GameStateManager.Instance;
            if (AsamaIlerletici.OyunBitti(durum)) return;
            if (elmasGerekli && !durum.DiamondTaken)
            {
                Debug.Log("Gecis kapisi: once elmasi almalisin.");
                return;
            }

            kullanildi = true;
            AsamaIlerletici.Ilerlet(durum, hedefAsama);
            Debug.Log("Gecis kapisi: aşama " + durum.CurrentStage);
        }
    }
}