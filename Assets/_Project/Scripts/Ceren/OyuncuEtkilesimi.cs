using UnityEngine;
using UnityEngine.InputSystem;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Ceren
{
    public class OyuncuEtkilesimi : MonoBehaviour
    {
        private IInteractable yakin;

        private void OnTriggerEnter2D(Collider2D other)
        {
            IInteractable aday = other.GetComponent<IInteractable>();
            if (aday != null) yakin = aday;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            IInteractable aday = other.GetComponent<IInteractable>();
            if (aday != null && aday == yakin) yakin = null;
        }

        private void Update()
        {
            if (!EtkilesimVar()) return;

            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                yakin.Interact();
        }

        private void OnGUI()
        {
            if (!EtkilesimVar()) return;

            GUIStyle stil = new GUIStyle(GUI.skin.label)
            {
                fontSize = 24,
                alignment = TextAnchor.MiddleCenter
            };
            GUI.Label(new Rect(Screen.width / 2f - 250f, Screen.height - 90f, 500f, 40f), yakin.Prompt, stil);
        }

        private bool EtkilesimVar()
        {
            return yakin != null && !string.IsNullOrEmpty(yakin.Prompt);
        }
    }
}