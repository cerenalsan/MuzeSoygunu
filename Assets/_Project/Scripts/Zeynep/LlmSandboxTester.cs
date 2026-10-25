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
        private ILlmErrorMessages errorMessages;
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
            errorMessages = new TurkishLlmErrorMessages();
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
                LlmResult result = await client.SendAsync(systemPrompt, new[] { new ChatTurn(ChatRole.User, message) }, lifetimeCts.Token);

                if (result.IsSuccess)
                {
                    statusText.text = result.Content;
                }
                else
                {
                    statusText.text = errorMessages.GetMessage(result.Error);
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
    }
}
