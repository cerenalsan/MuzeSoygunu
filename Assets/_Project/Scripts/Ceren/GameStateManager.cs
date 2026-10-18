using MuzeSoygunu.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MuzeSoygunu.Ceren
{
    public class GameStateManager : MonoBehaviour, IGameState
    {
        public static IGameState Instance { get; private set; }

        // Hangi aşamadan hangisine geçilebilir.
        private static readonly Dictionary<GameStage, GameStage[]> AllowedNext =
            new Dictionary<GameStage, GameStage[]>
            {
                { GameStage.MainMenu,      new[] { GameStage.Entrance } },
                { GameStage.Entrance,      new[] { GameStage.Statue } },
                { GameStage.Statue,        new[] { GameStage.LaserRoom } },
                { GameStage.LaserRoom,     new[] { GameStage.Diamond } },
                { GameStage.Diamond,       new[] { GameStage.Corridor } },
                { GameStage.Corridor,      new[] { GameStage.GuardDialogue } },
                { GameStage.GuardDialogue, new GameStage[0] },
                { GameStage.Won,           new[] { GameStage.MainMenu } },
                { GameStage.Lost,          new[] { GameStage.Entrance, GameStage.MainMenu } }
            };

        public GameStage CurrentStage { get; private set; } = GameStage.MainMenu;
        public LoseReason LastLoseReason { get; private set; } = LoseReason.None;
        public bool LasersDisabled { get; private set; }
        public bool DiamondTaken { get; private set; }

        public event Action<GameStage> StageChanged;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            if (Instance != null) return;
            new GameObject("GameStateManager").AddComponent<GameStateManager>();
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (ReferenceEquals(Instance, this)) Instance = null;
        }

        public bool AdvanceTo(GameStage next)
        {
            if (!AllowedNext[CurrentStage].Contains(next))
            {
                Debug.LogWarning($"Izin verilmeyen gecis: {CurrentStage} -> {next}");
                return false;
            }

            if (next == GameStage.Entrance || next == GameStage.MainMenu)
            {
                ResetProgress();
            }

            SetStage(next);
            return true;
        }

        public void MarkLasersDisabled() => LasersDisabled = true;

        public void MarkDiamondTaken() => DiamondTaken = true;

        public void Win()
        {
            if (CurrentStage != GameStage.GuardDialogue)
            {
                Debug.LogWarning($"Win sadece GuardDialogue asamasinda cagrilabilir, simdiki: {CurrentStage}");
                return;
            }
            SetStage(GameStage.Won);
        }

        public void Lose(LoseReason reason)
        {
            if (CurrentStage == GameStage.Won || CurrentStage == GameStage.Lost || CurrentStage == GameStage.MainMenu)
            {
                Debug.LogWarning($"Lose bu asamada cagrilamaz: {CurrentStage}");
                return;
            }
            LastLoseReason = reason;
            SetStage(GameStage.Lost);
        }

        private void ResetProgress()
        {
            LasersDisabled = false;
            DiamondTaken = false;
            LastLoseReason = LoseReason.None;
        }

        private void SetStage(GameStage stage)
        {
            CurrentStage = stage;
            StageChanged?.Invoke(stage);
        }
    }
}