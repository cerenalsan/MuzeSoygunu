using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MuzeSoygunu.Llm
{
    // Diyalog ekranı: sohbet geçmişi, yazı kutusu, gönder ve tekrar dene düğmeleri, "Düşünüyor...". IDialogueView uygular.
    // Bu bileşen panelin kendisinde değil, her zaman açık kalan bir üst nesnede durmalıdır.
    public class DialogueUI : MonoBehaviour, IDialogueView
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text chatText;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button sendButton;
        [SerializeField] private Button retryButton; // isteğe bağlı
        [SerializeField, Min(1)] private int maxLines = 8;

        private readonly List<string> lines = new List<string>();
        private bool busy;
        private bool locked;

        public event Action<string> MessageSubmitted;
        public event Action RetryRequested;

        private void Awake()
        {
            if (panel == null || chatText == null || statusText == null || inputField == null || sendButton == null)
            {
                Debug.LogError("DialogueUI: Inspector alanları eksik, bileşen kapatıldı.");
                enabled = false;
                return;
            }

            if (panel == gameObject)
            {
                Debug.LogError("DialogueUI: panel bu nesnenin kendisi olamaz, ayrı bir alt nesne olmalı.");
                enabled = false;
                return;
            }

            sendButton.onClick.AddListener(Submit);
            inputField.onSubmit.AddListener(OnInputSubmit);

            if (retryButton != null)
            {
                retryButton.onClick.AddListener(OnRetryClicked);
                retryButton.gameObject.SetActive(false);
            }

            chatText.text = string.Empty;
            statusText.text = string.Empty;
            Hide();
        }

        private void OnDestroy()
        {
            if (sendButton != null) sendButton.onClick.RemoveListener(Submit);
            if (inputField != null) inputField.onSubmit.RemoveListener(OnInputSubmit);
            if (retryButton != null) retryButton.onClick.RemoveListener(OnRetryClicked);
        }

        public void Show()
        {
            panel.SetActive(true);
            inputField.ActivateInputField();
        }

        public void Hide()
        {
            panel.SetActive(false);
        }

        // Diyalog bitince yazma ve göndermeyi kapatır.
        public void LockInput()
        {
            locked = true;
            inputField.interactable = false;
            sendButton.interactable = false;
            SetRetryVisible(false);
        }

        public void ShowPlayerMessage(string text)
        {
            AddLine("Sen: " + text);
        }

        public void ShowNpcMessage(string text)
        {
            AddLine("Güvenlik: " + text);
        }

        public void ShowSystemMessage(string text)
        {
            AddLine(text);
        }

        public void SetBusy(bool isBusy)
        {
            busy = isBusy;
            statusText.text = isBusy ? "Düşünüyor..." : string.Empty;

            if (retryButton != null)
            {
                retryButton.interactable = !isBusy;
            }

            if (locked)
            {
                return;
            }

            sendButton.interactable = !isBusy;
            inputField.interactable = !isBusy;

            if (!isBusy && panel.activeInHierarchy)
            {
                inputField.ActivateInputField();
            }
        }

        public void SetRetryVisible(bool visible)
        {
            if (retryButton != null)
            {
                retryButton.gameObject.SetActive(visible && !locked);
            }
        }

        private void OnRetryClicked()
        {
            if (busy || locked)
            {
                return;
            }

            Action handler = RetryRequested;
            if (handler != null)
            {
                handler();
            }
        }

        private void OnInputSubmit(string _)
        {
            Submit();
        }

        private void Submit()
        {
            if (busy || locked)
            {
                return;
            }

            string text = inputField.text;
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            inputField.text = string.Empty;

            Action<string> handler = MessageSubmitted;
            if (handler != null)
            {
                handler(text.Trim());
            }
        }

        private void AddLine(string line)
        {
            lines.Add(line);

            while (lines.Count > maxLines)
            {
                lines.RemoveAt(0);
            }

            chatText.text = string.Join("\n", lines);
        }
    }
}
