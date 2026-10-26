using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using MuzeSoygunu.Llm;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Sandbox
{
    // Çıkış koridorunda güvenlikle diyaloğu yönetir. Assembly-CSharp'ta durur çünkü
    // GameStateManager.Instance ve Ceren'in oyuncu bileşenlerine buradan erişilir.
    [RequireComponent(typeof(Collider2D))]
    public class GuardDialogueController : MonoBehaviour
    {
        [SerializeField] private DialogueUI dialogueUI;
        [SerializeField] private ExitDoor exitDoor;
        [SerializeField] private LlmSettings settings;
        [SerializeField, Min(1)] private int maxDenials = 3;
        [SerializeField, Min(0f)] private float sceneChangeDelaySeconds = 3f;

        private IGameState state;
        private IDialogueView view;
        private ILlmHealth health;
        private GuardNpc guard;
        private CancellationTokenSource lifetimeCts;
        private string lastMessage;
        private bool started;
        private bool finished;
        private bool busy;

        private void Start()
        {
            if (dialogueUI == null || exitDoor == null || settings == null)
            {
                Debug.LogError("GuardDialogueController: Inspector alanları eksik, bileşen kapatıldı.");
                enabled = false;
                return;
            }

            if (!GetComponent<Collider2D>().isTrigger)
            {
                Debug.LogWarning("GuardDialogueController: Collider2D 'Is Trigger' işaretli olmalı.");
            }

            state = MuzeSoygunu.Ceren.GameStateManager.Instance;
            if (state == null)
            {
                Debug.LogError("GuardDialogueController: GameStateManager.Instance bulunamadı.");
                enabled = false;
                return;
            }

#if UNITY_EDITOR
            DevShortcut(state);
#endif

            health = new LlmHealthCheck(settings);

            guard = new GuardNpc(
                new OllamaClient(settings),
                new GuardPromptBuilder(),
                new JsonNpcReplyParser(),
                new GameActionFactory(exitDoor, new DenialCounter(maxDenials)),
                state,
                new TurkishLlmErrorMessages(),
                DialogueSession.CreateDefault());

            view = dialogueUI;
            view.MessageSubmitted += OnMessageSubmitted;
            view.RetryRequested += OnRetryRequested;
            lifetimeCts = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            SceneFlowBridge.LoadDelaySeconds = 0f;

            if (view != null)
            {
                view.MessageSubmitted -= OnMessageSubmitted;
                view.RetryRequested -= OnRetryRequested;
            }

            if (lifetimeCts != null)
            {
                lifetimeCts.Cancel();
                lifetimeCts.Dispose();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (started || !enabled || !other.CompareTag("Player"))
            {
                return;
            }

            BeginDialogue();
        }

        private async void BeginDialogue()
        {
            if (state.CurrentStage == GameStage.Corridor)
            {
                if (!state.AdvanceTo(GameStage.GuardDialogue))
                {
                    Debug.LogWarning("GuardDialogueController: GuardDialogue aşamasına geçilemedi.");
                    return;
                }
            }
            else if (state.CurrentStage != GameStage.GuardDialogue)
            {
                Debug.LogWarning("GuardDialogueController: diyalog bu aşamada başlamaz: " + state.CurrentStage);
                return;
            }

            started = true;
            FreezePlayer();
            SceneFlowBridge.LoadDelaySeconds = sceneChangeDelaySeconds;
            dialogueUI.Show();
            view.ShowSystemMessage("[Sistem] Güvenlik görevlisi yolunu kesti. Ona bir şey yaz.");

            await CheckHealthAsync(false);
        }

        // Ollama hazır değilse oyuncuya Türkçe sistem mesajı ve "Tekrar dene" düğmesi gösterir.
        private async Task CheckHealthAsync(bool announceReady)
        {
            busy = true;

            try
            {
                LlmHealthStatus status = await health.CheckAsync(lifetimeCts.Token);

                if (status == LlmHealthStatus.Ready)
                {
                    view.SetRetryVisible(false);
                    if (announceReady)
                    {
                        view.ShowSystemMessage("[Sistem] Ollama hazır.");
                    }
                }
                else
                {
                    view.ShowSystemMessage(LlmHealthMessages.For(status, settings.ModelName));
                    view.SetRetryVisible(true);
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
            finally
            {
                busy = false;
            }
        }

        // PlayerController, ZiplamaYetenegi ve OyuncuEtkilesimi klavyeyi doğrudan okuduğu için
        // yazı yazarken karakter yürümesin, zıplamasın, etkileşime girmesin.
        // Ceren'in dosyalarına dokunmadan bileşenler kapatılır ve hız sıfırlanır.
        private static void FreezePlayer()
        {
            var player = FindFirstObjectByType<MuzeSoygunu.Ceren.PlayerController>();
            if (player == null)
            {
                return;
            }

            player.enabled = false;

            var jump = player.GetComponent<MuzeSoygunu.Ceren.ZiplamaYetenegi>();
            if (jump != null)
            {
                jump.enabled = false;
            }

            var interaction = player.GetComponent<MuzeSoygunu.Ceren.OyuncuEtkilesimi>();
            if (interaction != null)
            {
                interaction.enabled = false;
            }

            var body = player.GetComponent<Rigidbody2D>();
            if (body != null)
            {
                body.linearVelocity = Vector2.zero;
            }
        }

        private async void OnMessageSubmitted(string text)
        {
            if (finished)
            {
                return;
            }

            lastMessage = text;
            view.ShowPlayerMessage(text);
            await TalkAsync(text);
        }

        // Başarısız son mesajı yeniden gönderir; henüz hiç mesaj yoksa sağlık kontrolünü tekrarlar.
        private async void OnRetryRequested()
        {
            if (finished || busy)
            {
                return;
            }

            if (lastMessage == null)
            {
                await CheckHealthAsync(true);
                return;
            }

            await TalkAsync(lastMessage);
        }

        private async Task TalkAsync(string text)
        {
            busy = true;
            view.SetBusy(true);

            try
            {
                GuardTurnResult turn = await guard.TalkAsync(text, lifetimeCts.Token);

                if (turn.IsSystemMessage)
                {
                    view.ShowSystemMessage(turn.Text);
                    view.SetRetryVisible(true);
                    Debug.LogWarning("Sistem mesajı: " + turn.TechnicalDetail);
                }
                else
                {
                    view.ShowNpcMessage(turn.Text);
                    view.SetRetryVisible(false);
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
            finally
            {
                busy = false;

                if (this != null && dialogueUI != null)
                {
                    view.SetBusy(false);

                    if (state.CurrentStage == GameStage.Won || state.CurrentStage == GameStage.Lost)
                    {
                        finished = true;
                        dialogueUI.LockInput();
                        view.ShowSystemMessage("[Sistem] Sonuç ekranına geçiliyor...");
                    }
                }
            }
        }

#if UNITY_EDITOR
        // Yalnızca editörde: sahne doğrudan Play'de denenirken (oyun MainMenu aşamasındayken)
        // aşamaları sessizce Corridor'a ilerletir. Build'de çalışmaz.
        private static void DevShortcut(IGameState s)
        {
            if (s.CurrentStage != GameStage.MainMenu)
            {
                return;
            }

            GameStage[] path =
            {
                GameStage.Entrance, GameStage.Statue, GameStage.LaserRoom,
                GameStage.Diamond, GameStage.Corridor
            };

            SceneFlowBridge.Suppressed = true;
            foreach (GameStage stage in path)
            {
                if (!s.AdvanceTo(stage))
                {
                    break;
                }
            }
            SceneFlowBridge.Suppressed = false;
        }
#endif
    }
}
