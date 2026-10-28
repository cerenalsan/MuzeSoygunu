using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MuzeSoygunu.Llm
{
    // Boş bir sohbet isteğiyle modeli belleğe yükletir ve bir süre bellekte tutar.
    // Süre LlmSettings.WarmupTimeoutSeconds'tan okunur.
    public sealed class OllamaWarmup : ILlmWarmup
    {
        private const string KeepAlive = "10m";
        private static readonly HttpClient Http = new HttpClient { Timeout = Timeout.InfiniteTimeSpan };

        private readonly LlmSettings settings;

        public OllamaWarmup(LlmSettings settings)
        {
            this.settings = settings;
        }

        public async Task<bool> WarmUpAsync(CancellationToken cancellationToken)
        {
            string url = settings.BaseUrl.TrimEnd('/') + "/api/chat";

            var request = new WarmupRequest
            {
                model = settings.ModelName,
                messages = new EmptyMessage[0],
                stream = false,
                keep_alive = KeepAlive
            };

            string json = JsonUtility.ToJson(request);

            using (var timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                timeoutSource.CancelAfter(TimeSpan.FromSeconds(settings.WarmupTimeoutSeconds));

                try
                {
                    using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                    using (var response = await Http.PostAsync(url, content, timeoutSource.Token))
                    {
                        return response.IsSuccessStatusCode;
                    }
                }
                catch (OperationCanceledException)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        throw;
                    }

                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        [Serializable]
        private class EmptyMessage
        {
            public string role;
            public string content;
        }

        [Serializable]
        private class WarmupRequest
        {
            public string model;
            public EmptyMessage[] messages;
            public bool stream;
            public string keep_alive;
        }
    }
}
