using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Sandbox
{
    // Sonuç ekranı: kazandın ya da kaybettin, kayıp nedeni ve yeniden başlatma düğmesi.
    public class ResultScreenUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text reasonText;
        [SerializeField] private Button actionButton;
        [SerializeField] private TMP_Text actionButtonText;

        private IGameState state;

        private void Start()
        {
            if (titleText == null || reasonText == null || actionButton == null || actionButtonText == null)
            {
                Debug.LogError("ResultScreenUI: Inspector alanları eksik, bileşen kapatıldı.");
                enabled = false;
                return;
            }

            state = MuzeSoygunu.Ceren.GameStateManager.Instance;
            if (state == null)
            {
                Debug.LogError("ResultScreenUI: GameStateManager.Instance bulunamadı.");
                titleText.text = "Hata";
                reasonText.text = "Oyun durumu bulunamadı.";
                actionButton.gameObject.SetActive(false);
                return;
            }

            Show();
            actionButton.onClick.AddListener(OnActionClicked);
        }

        private void OnDestroy()
        {
            if (actionButton != null) actionButton.onClick.RemoveListener(OnActionClicked);
        }

        private void Show()
        {
            if (state.CurrentStage == GameStage.Won)
            {
                titleText.text = "Kazandın!";
                reasonText.text = "Güvenliği ikna ettin ve elmasla kaçtın.";
                actionButtonText.text = "Menüye dön";
            }
            else if (state.CurrentStage == GameStage.Lost)
            {
                titleText.text = "Yakalandın!";
                reasonText.text = DescribeReason(state.LastLoseReason);
                actionButtonText.text = "Tekrar dene";
            }
            else
            {
                titleText.text = "Oyun sürüyor";
                reasonText.text = "Bu ekran yalnızca oyun bitince gösterilir. Aşama: " + state.CurrentStage;
                actionButtonText.text = "Menüye dön";
            }
        }

        private void OnActionClicked()
        {
            GameStage target = state.CurrentStage == GameStage.Lost ? GameStage.Entrance : GameStage.MainMenu;

            if (!state.AdvanceTo(target))
            {
                Debug.LogWarning("ResultScreenUI: geçiş reddedildi: " + state.CurrentStage + " -> " + target);
            }
        }

        private static string DescribeReason(LoseReason reason)
        {
            switch (reason)
            {
                case LoseReason.CameraCaught:
                    return "Güvenlik kamerası seni yakaladı.";
                case LoseReason.LaserHit:
                    return "Lazere dokundun.";
                case LoseReason.GuardAlarm:
                    return "Güvenlik görevlisi alarmı çaldı.";
                case LoseReason.GuardRefused:
                    return "Güvenlik görevlisi seni dışarı bırakmadı.";
                default:
                    return "Yakalandın.";
            }
        }
    }
}
