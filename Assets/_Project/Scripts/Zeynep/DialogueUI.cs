using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MuzeSoygunu.Llm
{
    public class DialogueUI : MonoBehaviour, IDialogueView
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text chatText;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button sendButton;
        [SerializeField] private Button retryButton;
        [SerializeField] private ScrollRect chatScroll;
        [SerializeField, Min(1)] private int maxLines = 40;

        private readonly List<string> lines = new List<string>();
        private bool busy;
        private bool locked;
        private int lastDots;

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

        private void Update()
        {
            if (!busy) return;

            int dots = (int)(Time.unscaledTime * 3f) % 3 + 1;
            if (dots != lastDots)
            {
                lastDots = dots;
                statusText.text = "Düşünüyor" + new string('.', dots);
            }
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
            lastDots = 0;
            statusText.text = isBusy ? "Düşünüyor." : string.Empty;

            if (retryButton != null)
            {
                retryButton.interactable = !isBusy;
            }

            if (locked) return;

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
            if (busy || locked) return;
            RetryRequested?.Invoke();
        }

        private void OnInputSubmit(string _)
        {
            Submit();
        }

        private void Submit()
        {
            if (busy || locked) return;

            string text = inputField.text;
            if (string.IsNullOrWhiteSpace(text)) return;

            inputField.text = string.Empty;
            MessageSubmitted?.Invoke(text.Trim());
        }

        private void AddLine(string line)
        {
            lines.Add(line);
            while (lines.Count > maxLines)
            {
                lines.RemoveAt(0);
            }

            chatText.text = string.Join("\n", lines);

            if (chatScroll != null && isActiveAndEnabled)
            {
                StartCoroutine(ScrollToBottom());
            }
        }

        private IEnumerator ScrollToBottom()
        {
            yield return null;
            Canvas.ForceUpdateCanvases();
            if (chatScroll != null)
            {
                chatScroll.verticalNormalizedPosition = 0f;
            }
        }
    }
}