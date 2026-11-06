using UnityEngine;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Ceren
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class LazerIsini : MonoBehaviour, IToggleable
    {
        private Collider2D govde;
        private SpriteRenderer[] gorseller;

        public bool IsOn { get; private set; } = true;

        private void Awake()
        {
            govde = GetComponent<Collider2D>();
            gorseller = GetComponentsInChildren<SpriteRenderer>();
            Uygula();
        }

        public void TurnOn()
        {
            IsOn = true;
            Uygula();
        }

        public void TurnOff()
        {
            IsOn = false;
            Uygula();
        }

        private void Uygula()
        {
            govde.enabled = IsOn;
            foreach (SpriteRenderer g in gorseller) g.enabled = IsOn;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!IsOn || !other.CompareTag("Player")) return;
            if (GameStateManager.Instance.CurrentStage == GameStage.Lost) return;

            Debug.Log("Lazer oyuncuya dokundu.");
            GameStateManager.Instance.Lose(LoseReason.LaserHit);
        }
    }
}