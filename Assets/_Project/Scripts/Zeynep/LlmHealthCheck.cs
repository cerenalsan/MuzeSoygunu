using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MuzeSoygunu.Llm
{
    // Ollama'nın /api/tags cevabına bakarak sunucunun ve ayarlardaki modelin hazır olduğunu denetler.
    public sealed class LlmHealthCheck : ILlmHealth
    {
        private const int CheckTimeoutSeconds = 5;
        private static readonly HttpClient Http = new HttpClient { Timeout = Timeout.InfiniteTimeSpan };

        private readonly LlmSettings settings;

        public LlmHealthCheck(LlmSettings settings)
        {
            this.settings = settings;
        }

        public async Task<LlmHealthStatus> CheckAsync(CancellationToken cancellationToken)
        {
            string url = settings.BaseUrl.TrimEnd('/') + "/api/tags";

            using (var timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                timeoutSource.CancelAfter(TimeSpan.FromSeconds(CheckTimeoutSeconds));

                try
                {
                    using (var response = await Http.GetAsync(url, timeoutSource.Token))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            return LlmHealthStatus.Unknown;
                        }

                        string body = await response.Content.ReadAsStringAsync();
                        return ContainsModel(body, settings.ModelName)
                            ? LlmHealthStatus.Ready
                            : LlmHealthStatus.ModelNotInstalled;
                    }
                }
                catch (OperationCanceledException)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        throw;
                    }

                    return LlmHealthStatus.ServerUnreachable;
                }
                catch (HttpRequestException)
                {
                    return LlmHealthStatus.ServerUnreachable;
                }
                catch (Exception)
                {
                    return LlmHealthStatus.Unknown;
                }
            }
        }

        // "gemma2:2b" tam eşleşir; etiketsiz bir ad ("gemma2") ":latest" ile eşleşir.
        public static bool ContainsModel(string tagsJson, string modelName)
        {
            if (string.IsNullOrWhiteSpace(tagsJson) || string.IsNullOrWhiteSpace(modelName))
            {
                return false;
            }

            TagsResponse parsed;
            try
            {
                parsed = JsonUtility.FromJson<TagsResponse>(tagsJson);
            }
            catch (Exception)
            {
                return false;
            }

            if (parsed == null || parsed.models == null)
            {
                return false;
            }

            string wanted = modelName.Trim();
            string wantedWithLatest = wanted.Contains(":") ? wanted : wanted + ":latest";

            foreach (TagModel model in parsed.models)
            {
                if (model == null || string.IsNullOrEmpty(model.name))
                {
                    continue;
                }

                if (string.Equals(model.name, wanted, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(model.name, wantedWithLatest, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        [Serializable]
        private class TagModel
        {
            public string name;
        }

        [Serializable]
        private class TagsResponse
        {
            public TagModel[] models;
        }
    }
}
