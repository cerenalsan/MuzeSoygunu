using UnityEngine;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Llm
{
    // Çıkış kapısı. Kapalıyken engelleyici collider yolu keser, açılınca kapanır.
    public class ExitDoor : MonoBehaviour, IOpenable
    {
        [SerializeField] private Collider2D blocker;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Color closedColor = new Color(0.7f, 0.2f, 0.2f);
        [SerializeField] private Color openColor = new Color(0.2f, 0.7f, 0.3f);

        public bool IsOpen { get; private set; }

        private void Awake()
        {
            Apply();
        }

        public void Open()
        {
            IsOpen = true;
            Apply();
        }

        public void Close()
        {
            IsOpen = false;
            Apply();
        }

        private void Apply()
        {
            if (blocker != null) blocker.enabled = !IsOpen;
            if (sprite != null) sprite.color = IsOpen ? openColor : closedColor;
        }
    }
}
