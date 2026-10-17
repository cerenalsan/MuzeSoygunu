using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MuzeSoygunu.Llm;

namespace MuzeSoygunu.Sandbox
{
    // Sandbox: yazı kutusundaki metni OllamaClient ile Ollama'ya gönderir ve sonucu gösterir.
    public class LlmSandboxTester : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button sendButton;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private LlmSettings settings;
        [SerializeField, TextArea(2, 5)] private string systemPrompt =
            "Sen bir müzede gece çalışan güvenlik görevlisisin. Kısa ve Türkçe cevap ver.";

        private ILlmClient client;
        private CancellationTokenSource lifetimeCts;

        private void Awake()
        {
            if (settings == null || inputField == null || sendButton == null || statusText == null)
            {
                Debug.LogError("LlmSandboxTester: Inspector alanları eksik, bileşen kapatıldı.");
                enabled = false;
                return;
            }

            client = new OllamaClient(settings);
            lifetimeCts = new CancellationTokenSource();
        }

        private void OnEnable()
        {
            if (sendButton != null)
            {
                sendButton.onClick.AddListener(OnSendClicked);
            }
        }

        private void OnDisable()
        {
            if (sendButton != null)
            {
                sendButton.onClick.RemoveListener(OnSendClicked);
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

        private async void OnSendClicked()
        {
            string message = inputField.text;

            if (string.IsNullOrWhiteSpace(message))
            {
                statusText.text = "Lütfen bir mesaj yazın.";
                return;
            }

            sendButton.interactable = false;
            statusText.text = "Düşünüyor...";

            try
            {
                LlmResult result = await client.SendAsync(systemPrompt, message, lifetimeCts.Token);

                if (result.IsSuccess)
                {
                    statusText.text = result.Content;
                }
                else
                {
                    statusText.text = DescribeError(result.Error);
                    Debug.LogWarning("LLM hatası: " + result.Error + " / " + result.ErrorDetail);
                }
            }
            catch (OperationCanceledException)
            {
                // Sahne kapandı ya da oyun durdu: sessizce çık.
                return;
            }
            finally
            {
                if (sendButton != null)
                {
                    sendButton.interactable = true;
                }
            }
        }

        private static string DescribeError(LlmError error)
        {
            switch (error)
            {
                case LlmError.ServerUnreachable:
                    return "[Sistem] Ollama sunucusuna ulaşılamıyor. Ollama'nın açık olduğundan emin olun.";
                case LlmError.Timeout:
                    return "[Sistem] Model zamanında cevap vermedi. Lütfen tekrar deneyin.";
                case LlmError.ModelNotFound:
                    return "[Sistem] Model bulunamadı. Model adını ve yüklü olup olmadığını kontrol edin.";
                case LlmError.InvalidResponse:
                    return "[Sistem] Modelden anlaşılmayan bir cevap geldi. Lütfen tekrar deneyin.";
                default:
                    return "[Sistem] Bilinmeyen bir hata oluştu.";
            }
        }
    }
}
