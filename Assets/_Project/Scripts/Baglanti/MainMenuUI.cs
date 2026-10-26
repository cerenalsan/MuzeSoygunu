using UnityEngine;
using UnityEngine.UI;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Sandbox
{
    // Ana menü: Başla oyunu Entrance aşamasına geçirir, Çık uygulamayı kapatır.
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button quitButton;

        private void Awake()
        {
            if (startButton == null || quitButton == null)
            {
                Debug.LogError("MainMenuUI: Inspector alanları eksik, bileşen kapatıldı.");
                enabled = false;
                return;
            }

            startButton.onClick.AddListener(OnStartClicked);
            quitButton.onClick.AddListener(OnQuitClicked);
        }

        private void OnDestroy()
        {
            if (startButton != null) startButton.onClick.RemoveListener(OnStartClicked);
            if (quitButton != null) quitButton.onClick.RemoveListener(OnQuitClicked);
        }

        private void OnStartClicked()
        {
            IGameState state = MuzeSoygunu.Ceren.GameStateManager.Instance;
            if (state == null)
            {
                Debug.LogError("MainMenuUI: GameStateManager.Instance bulunamadı.");
                return;
            }

            if (!state.AdvanceTo(GameStage.Entrance))
            {
                Debug.LogWarning("MainMenuUI: oyun başlatılamadı, aşama: " + state.CurrentStage);
            }
        }

        private void OnQuitClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
