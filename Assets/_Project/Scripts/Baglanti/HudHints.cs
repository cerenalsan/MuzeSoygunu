using TMPro;
using UnityEngine;
using MuzeSoygunu.Llm;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Sandbox
{
    // Ekranın sol üstünde aşamaya göre kısa ipucu gösterir. Oyun açılırken kendiliğinden oluşur,
    // hiçbir sahneye elle eklenmez.
    public class HudHints : MonoBehaviour
    {
        private TextMeshProUGUI label;
        private IGameState state;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            var host = new GameObject("HudHints");
            DontDestroyOnLoad(host);
            host.AddComponent<HudHints>();
        }

        private void Awake()
        {
            var canvasObject = new GameObject("HudHintsCanvas");
            canvasObject.transform.SetParent(transform, false);

            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 50;

            var textObject = new GameObject("HintText");
            textObject.transform.SetParent(canvasObject.transform, false);

            label = textObject.AddComponent<TextMeshProUGUI>();
            label.fontSize = 22;
            label.alignment = TextAlignmentOptions.TopLeft;
            label.raycastTarget = false;
            label.text = string.Empty;

            RectTransform rect = label.rectTransform;
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition = new Vector2(16f, -12f);
            rect.sizeDelta = new Vector2(-32f, 60f);
        }

        private void Update()
        {
            if (state != null)
            {
                return;
            }

            state = MuzeSoygunu.Ceren.GameStateManager.Instance;
            if (state != null)
            {
                state.StageChanged += OnStageChanged;
                Apply(state.CurrentStage);
            }
        }

        private void OnDestroy()
        {
            if (state != null)
            {
                state.StageChanged -= OnStageChanged;
            }
        }

        private void OnStageChanged(GameStage stage)
        {
            Apply(stage);
        }

        private void Apply(GameStage stage)
        {
            label.text = HintTexts.For(stage);
        }
    }
}