using UnityEngine;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Ceren
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SalterAnahtari : MonoBehaviour, IInteractable
    {
        [SerializeField] private LazerIzgarasi lazerler;
        [SerializeField] private Color cekilmisRenk = new Color(0.2f, 0.9f, 0.3f, 1f);

        public bool Cekildi { get; private set; }

        public string Prompt => Cekildi ? string.Empty : "E ile şalteri çek";

        [ContextMenu("Salteri Cek")]
        public void Interact()
        {
            if (Cekildi) return;
            Cekildi = true;

            lazerler.TurnOff();
            GetComponent<SpriteRenderer>().color = cekilmisRenk;
            Debug.Log("Salter cekildi.");
        }
    }
}