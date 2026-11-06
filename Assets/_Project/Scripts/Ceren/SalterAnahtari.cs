using UnityEngine;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Ceren
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SalterAnahtari : MonoBehaviour, IInteractable
    {
        [SerializeField] private LazerIzgarasi lazerler;
        [SerializeField] private Color cekilmisRenk = new Color(0.2f, 0.9f, 0.3f, 1f);
        private Color baslangicRengi;
        public bool Cekildi { get; private set; }
        public string Prompt => string.Empty;
        private void Awake()
        {
            baslangicRengi = GetComponent<SpriteRenderer>().color;
        }
        [ContextMenu("Salteri Cek")]
        public void Interact()
        {
            if (Cekildi) return;
            Cekildi = true;

            lazerler.TurnOff();
            GetComponent<SpriteRenderer>().color = cekilmisRenk;
         
           Debug.Log("Salter cekildi.");

        }
        public void Sifirla()
        {
            Cekildi = false;
            lazerler.TurnOn();
            GetComponent<SpriteRenderer>().color = baslangicRengi;
        }
    }
}