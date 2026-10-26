using UnityEngine;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Ceren
{
    public class LazerIzgarasi : MonoBehaviour, IToggleable
    {
        [SerializeField] private LazerIsini[] isinlar;

        public bool IsOn { get; private set; } = true;

        [ContextMenu("Lazerleri Kapat")]
        public void TurnOff()
        {
            IsOn = false;
            foreach (LazerIsini isin in isinlar) isin.TurnOff();
            GameStateManager.Instance.MarkLasersDisabled();
            Debug.Log("Lazerler kapandi.");
        }

        [ContextMenu("Lazerleri Ac")]
        public void TurnOn()
        {
            IsOn = true;
            foreach (LazerIsini isin in isinlar) isin.TurnOn();
            Debug.Log("Lazerler acildi.");
        }
    }
}