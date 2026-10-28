using System;
using System.Threading;
using UnityEngine;
using MuzeSoygunu.Llm;

namespace MuzeSoygunu.Sandbox
{
    // Sahneye girince modeli arka planda ısıtır. Başarısızlık sessizce geçilir:
    // Ollama sorunlarını oyuncuya sağlık kontrolü ve istek hataları gösterir.
    public class LlmWarmupStarter : MonoBehaviour
    {
        [SerializeField] private LlmSettings settings;

        private CancellationTokenSource lifetimeCts;

        private async void Start()
        {
            if (settings == null)
            {
                Debug.LogWarning("LlmWarmupStarter: Settings atanmamış, ısınma yapılmayacak.");
                return;
            }

            lifetimeCts = new CancellationTokenSource();
            ILlmWarmup warmup = new OllamaWarmup(settings);

            try
            {
                bool ok = await warmup.WarmUpAsync(lifetimeCts.Token);
                Debug.Log("LlmWarmupStarter: ısınma " + (ok ? "tamamlandı" : "başarısız (sessizce geçildi)"));
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void OnDestroy()
        {
            if (lifetimeCts != null)
            {
                lifetimeCts.Cancel();
                lifetimeCts.Dispose();
            }
        }
    }
}
