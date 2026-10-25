using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MuzeSoygunu.Llm;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Sandbox
{
    // Sandbox: yazı kutusundan GuardNpc'ye mesaj gönderir, sonucu ve oyun aşamasını gösterir.
    // Assembly-CSharp'ta durur çünkü GameStateManager.Instance'a buradan erişilir.
    public class GuardSandboxBridge : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button sendButton;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private LlmSettings settings;
        [SerializeField, Min(1)] private int maxDenials = 3;

        private GuardNpc guard;
        private DenialCounter denialCounter;
        private SandboxDoor door;
        private CancellationTokenSource lifetimeCts;

        // Test için kapının yapay uygulaması. Gerçek ExitDoor 25 Ekim'de yazılacak.
        private sealed class SandboxDoor : IOpenable
        {
            public bool IsOpen { get; private set; }
            public void Open() { IsOpen = true; }
            public void Close() { IsOpen = false; }
        }

        private void Start()
        {
            if (settings == null || inputField == null || sendButton == null || statusText == null)
            {
                Debug.LogError("GuardSandboxBridge: Inspector alanları eksik, bileşen kapatıldı.");
                enabled = false;
                return;
            }

            IGameState state = GameStateManager_Instance();
            if (state == null)
            {
                Debug.LogError("GuardSandboxBridge: GameStateManager.Instance bulunamadı.");
                enabled = false;
                return;
            }

            AdvanceToGuardDialogue(state);

            denialCounter = new DenialCounter(maxDenials);
            door = new SandboxDoor();

            guard = new GuardNpc(
                new OllamaClient(settings),
                new GuardPromptBuilder(),
                new JsonNpcReplyParser(),
                new GameActionFactory(door, denialCounter),
                state,
                new TurkishLlmErrorMessages(),
                DialogueSession.CreateDefault());

            lifetimeCts = new CancellationTokenSource();
            sendButton.onClick.AddListener(OnSendClicked);
            statusText.text = "Güvenlik görevlisiyle konuş. Aşama: " + state.CurrentStage;
        }

        private static IGameState GameStateManager_Instance()
        {
            return MuzeSoygunu.Ceren.GameStateManager.Instance;
        }

        // Sandbox'ta oyunu menüden güvenlik diyaloğuna sırayla ilerletir.
        private static void AdvanceToGuardDialogue(IGameState state)
        {
            GameStage[] path =
            {
                GameStage.Entrance, GameStage.Statue, GameStage.LaserRoom,
                GameStage.Diamond, GameStage.Corridor, GameStage.GuardDialogue
            };

            foreach (GameStage stage in path)
            {
                if (state.CurrentStage == stage) continue;
                if (!state.AdvanceTo(stage))
                {
                    Debug.LogWarning("GuardSandboxBridge: aşama ilerletilemedi: " + stage);
                    return;
                }
            }
        }

        private void OnDestroy()
        {
            if (sendButton != null) sendButton.onClick.RemoveListener(OnSendClicked);
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
                GuardTurnResult turn = await guard.TalkAsync(message, lifetimeCts.Token);
                IGameState state = GameStateManager_Instance();

                if (turn.IsSystemMessage)
                {
                    statusText.text = turn.Text;
                    Debug.LogWarning("Sistem mesajı: " + turn.TechnicalDetail);
                }
                else
                {
                    statusText.text = turn.Text
                        + "\n[action: " + turn.Action + " | aşama: " + state.CurrentStage
                        + " | ret: " + denialCounter.Count + "/" + denialCounter.MaxDenials
                        + " | kapı açık: " + door.IsOpen + "]";
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
            finally
            {
                if (sendButton != null) sendButton.interactable = true;
            }
        }
    }
}
