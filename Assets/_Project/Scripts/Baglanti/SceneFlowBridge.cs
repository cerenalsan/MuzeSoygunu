using System.Collections;
using UnityEngine;
using MuzeSoygunu.Llm;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Sandbox
{
    // Oyun aşaması değişince ilgili sahneyi yükler. Oyun açılırken kendiliğinden oluşur,
    // hiçbir sahneye elle eklenmez. Assembly-CSharp'ta durur çünkü GameStateManager.Instance'a erişir.
    public class SceneFlowBridge : MonoBehaviour
    {
        // Sahne değişiminden önce beklenecek süre (diyalogda son cümle okunabilsin diye).
        public static float LoadDelaySeconds { get; set; }

        // true iken aşama değişimleri sahne yüklemez (yalnızca geliştirme kısayolu için).
        public static bool Suppressed { get; set; }

        private ISceneLoader sceneLoader;
        private IGameState state;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            var host = new GameObject("SceneFlowBridge");
            DontDestroyOnLoad(host);
            host.AddComponent<SceneFlowBridge>();
        }

        private void Awake()
        {
            sceneLoader = new SceneLoader();
        }

        private void Update()
        {
            // GameStateManager hazır olana kadar bekle, sonra aboneliği kur ve bu bekleme döngüsünü kapat.
            if (state != null)
            {
                return;
            }

            state = MuzeSoygunu.Ceren.GameStateManager.Instance;
            if (state != null)
            {
                state.StageChanged += OnStageChanged;
                enabled = false;
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
            if (Suppressed)
            {
                return;
            }

            if (LoadDelaySeconds > 0f)
            {
                StartCoroutine(LoadAfterDelay(stage, LoadDelaySeconds));
            }
            else
            {
                sceneLoader.LoadSceneFor(stage);
            }
        }

        private IEnumerator LoadAfterDelay(GameStage stage, float delay)
        {
            yield return new WaitForSeconds(delay);
            sceneLoader.LoadSceneFor(stage);
        }
    }
}
