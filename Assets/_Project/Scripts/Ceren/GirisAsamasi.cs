using MuzeSoygunu.Sandbox;
using MuzeSoygunu.Shared;
using UnityEngine;

namespace MuzeSoygunu.Ceren
{
    // Sahne menüden gelmeden doğrudan Play ile açıldığında aşamayı bu sahneye uygun değere getirir.
    public class GirisAsamasi : MonoBehaviour
    {
        [SerializeField] private GameStage baslangicAsamasi = GameStage.Entrance;

        private void Start()
        {
            IGameState durum = GameStateManager.Instance;
            if (durum == null || durum.CurrentStage != GameStage.MainMenu)
            {
                return;
            }

            // Aşama yükselirken sahne yükleme tetiklenmesin diye köprü kısa süre kapatılır.
            bool eskiDeger = SceneFlowBridge.Suppressed;
            SceneFlowBridge.Suppressed = true;
            AsamaIlerletici.Ilerlet(durum, baslangicAsamasi);
            SceneFlowBridge.Suppressed = eskiDeger;
        }
    }
}