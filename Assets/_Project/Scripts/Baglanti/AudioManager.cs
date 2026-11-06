using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MuzeSoygunu.Llm;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Sandbox
{
    // Ses efektlerini çalar. Oyun açılırken kendiliğinden oluşur. SoundBank bulunamazsa ya da
    // bir ses atanmamışsa sessiz kalır, hata vermez. F8 ile sessize alınır.
    public class AudioManager : MonoBehaviour, ISoundPlayer
    {
        private const string MutedKey = "MuzeSoygunu.Muted";
        private const string BankResourceName = "SoundBank";
        private const float StepInterval = 0.35f;
        private const float MinStepSpeed = 0.5f;     // saniyede birim; altı "duruyor" sayılır
        private const float AlarmMaxSeconds = 3f;    // alarm en fazla bu kadar çalar

        private AudioSource source;      // buton, kazanma, kayıp
        private AudioSource stepSource;  // adım: üst üste binmesin
        private AudioSource alarmSource; // alarm: süresi sınırlı
        private SoundBank bank;
        private IGameState state;
        private Transform playerTransform;
        private Vector3 lastPlayerPosition;
        private bool hasLastPosition;
        private float nextStepTime;
        private float alarmStopTime = -1f;

        public bool Muted
        {
            get { return PlayerPrefs.GetInt(MutedKey, 0) == 1; }
            set { PlayerPrefs.SetInt(MutedKey, value ? 1 : 0); }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            var host = new GameObject("AudioManager");
            DontDestroyOnLoad(host);
            host.AddComponent<AudioManager>();
        }

        private void Awake()
        {
            source = CreateSource();
            stepSource = CreateSource();
            alarmSource = CreateSource();

            bank = Resources.Load<SoundBank>(BankResourceName);
            if (bank == null)
            {
                Debug.LogWarning("AudioManager: Resources/SoundBank bulunamadı, oyun sessiz çalışacak.");
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
            HookButtons();
        }

        private AudioSource CreateSource()
        {
            AudioSource created = gameObject.AddComponent<AudioSource>();
            created.playOnAwake = false;
            return created;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            if (state != null)
            {
                state.StageChanged -= OnStageChanged;
            }
        }

        private void Update()
        {
            if (state == null)
            {
                state = MuzeSoygunu.Ceren.GameStateManager.Instance;
                if (state != null)
                {
                    state.StageChanged += OnStageChanged;
                }
            }

            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.f8Key.wasPressedThisFrame)
            {
                Muted = !Muted;
                if (Muted)
                {
                    StopAll();
                }

                Debug.Log("AudioManager: ses " + (Muted ? "kapalı" : "açık"));
            }

            if (alarmStopTime > 0f && Time.time >= alarmStopTime)
            {
                alarmSource.Stop();
                alarmStopTime = -1f;
            }

            PlayStepsIfMoving();
        }

        public void Play(SoundId id)
        {
            if (Muted || bank == null)
            {
                return;
            }

            AudioClip clip = bank.Get(id);
            if (clip == null)
            {
                return;
            }

            switch (id)
            {
                case SoundId.Step:
                    if (!stepSource.isPlaying)
                    {
                        stepSource.clip = clip;
                        stepSource.Play();
                    }
                    break;
                case SoundId.Alarm:
                    alarmSource.clip = clip;
                    alarmSource.Play();
                    alarmStopTime = Time.time + AlarmMaxSeconds;
                    break;
                default:
                    source.PlayOneShot(clip);
                    break;
            }
        }

        private void StopAll()
        {
            source.Stop();
            stepSource.Stop();
            alarmSource.Stop();
            alarmStopTime = -1f;
        }

        private void OnStageChanged(GameStage stage)
        {
            if (stage == GameStage.Won)
            {
                Play(SoundId.Win);
            }
            else if (stage == GameStage.Lost)
            {
                Play(state.LastLoseReason == LoseReason.GuardAlarm ? SoundId.Alarm : SoundId.Lose);
            }
            else
            {
                // Tekrar dene ya da menüye dönüş: alarm sürüyorsa kes.
                alarmSource.Stop();
                alarmStopTime = -1f;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            playerTransform = null;
            hasLastPosition = false;
            stepSource.Stop();
            HookButtons();
        }

        // Sahnedeki her butona tık sesi ekler (kapalı nesneler dahil).
        private void HookButtons()
        {
            Button[] buttons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (Button button in buttons)
            {
                button.onClick.RemoveListener(OnButtonClicked);
                button.onClick.AddListener(OnButtonClicked);
            }
        }

        private void OnButtonClicked()
        {
            Play(SoundId.ButtonClick);
        }

        // Oyuncunun kareler arasındaki gerçek yer değiştirmesine bakar: duvara dayanıp yürüyemiyorsa
        // ya da duruyorsa adım sesi çalmaz.
        private void PlayStepsIfMoving()
        {
            if (playerTransform == null)
            {
                var player = FindFirstObjectByType<MuzeSoygunu.Ceren.PlayerController>();
                if (player == null)
                {
                    return;
                }

                playerTransform = player.transform;
                hasLastPosition = false;
            }

            Vector3 position = playerTransform.position;
            if (!hasLastPosition)
            {
                lastPlayerPosition = position;
                hasLastPosition = true;
                return;
            }

            float speed = (position - lastPlayerPosition).magnitude / Mathf.Max(Time.deltaTime, 0.0001f);
            lastPlayerPosition = position;

            if (speed > MinStepSpeed && Time.time >= nextStepTime)
            {
                Play(SoundId.Step);
                nextStepTime = Time.time + StepInterval;
            }
        }
    }
}
