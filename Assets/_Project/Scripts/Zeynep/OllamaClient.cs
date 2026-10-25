using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MuzeSoygunu.Llm
{
    // Yerel Ollama sunucusuna asenkron istek atan istemci.
    public sealed class OllamaClient : ILlmClient
    {
        // Tek örnek paylaşılır; zaman aşımını kendimiz CancellationToken ile yönetiyoruz.
        private static readonly HttpClient Http = new HttpClient { Timeout = Timeout.InfiniteTimeSpan };

        private readonly LlmSettings settings;

        public OllamaClient(LlmSettings settings)
        {
            this.settings = settings;
        }

        public async Task<LlmResult> SendAsync(string systemPrompt, IReadOnlyList<ChatTurn> messages, CancellationToken cancellationToken)
        {
            string url = settings.BaseUrl.TrimEnd('/') + "/api/chat";
            string requestJson = BuildRequestJson(systemPrompt, messages);

            using (var timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                timeoutSource.CancelAfter(TimeSpan.FromSeconds(settings.TimeoutSeconds));

                try
                {
                    using (var content = new StringContent(requestJson, Encoding.UTF8, "application/json"))
                    using (var response = await Http.PostAsync(url, content, timeoutSource.Token))
                    {
                        string body = await response.Content.ReadAsStringAsync();

                        if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            return LlmResult.Failure(LlmError.ModelNotFound, body);
                        }

                        if (!response.IsSuccessStatusCode)
                        {
                            return LlmResult.Failure(LlmError.InvalidResponse, "HTTP " + (int)response.StatusCode + ": " + body);
                        }

                        return ParseResponse(body);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Oyun iptal ettiyse (sahne değişti) iptali yukarı bırak, süre dolduysa zaman aşımı say.
                    if (cancellationToken.IsCancellationRequested)
                    {
                        throw;
                    }

                    return LlmResult.Failure(LlmError.Timeout, "İstek " + settings.TimeoutSeconds + " saniyede cevaplanmadı.");
                }
                catch (HttpRequestException ex)
                {
                    return LlmResult.Failure(LlmError.ServerUnreachable, ex.Message);
                }
                catch (Exception ex)
                {
                    return LlmResult.Failure(LlmError.InvalidResponse, ex.Message);
                }
            }
        }

        private string BuildRequestJson(string systemPrompt, IReadOnlyList<ChatTurn> messages)
        {
            var list = new ChatMessage[messages.Count + 1];
            list[0] = new ChatMessage { role = "system", content = systemPrompt };

            for (int i = 0; i < messages.Count; i++)
            {
                list[i + 1] = new ChatMessage
                {
                    role = messages[i].Role == ChatRole.User ? "user" : "assistant",
                    content = messages[i].Content
                };
            }

            var request = new ChatRequest
            {
                model = settings.ModelName,
                stream = false,
                format = "json",
                options = new ChatOptions { temperature = settings.Temperature },
                messages = list
            };

            return JsonUtility.ToJson(request);
        }

        private static LlmResult ParseResponse(string body)
        {
            ChatResponse parsed;

            try
            {
                parsed = JsonUtility.FromJson<ChatResponse>(body);
            }
            catch (Exception ex)
            {
                return LlmResult.Failure(LlmError.InvalidResponse, ex.Message);
            }

            if (parsed == null || parsed.message == null || string.IsNullOrWhiteSpace(parsed.message.content))
            {
                return LlmResult.Failure(LlmError.InvalidResponse, "Modelden boş ya da beklenmeyen cevap geldi.");
            }

            return LlmResult.Success(parsed.message.content.Trim());
        }

        [Serializable]
        private class ChatMessage
        {
            public string role;
            public string content;
        }

        [Serializable]
        private class ChatOptions
        {
            public float temperature;
        }

        [Serializable]
        private class ChatRequest
        {
            public string model;
            public ChatMessage[] messages;
            public bool stream;
            public string format;
            public ChatOptions options;
        }

        [Serializable]
        private class ChatResponse
        {
            public ChatMessage message;
        }
    }
}
