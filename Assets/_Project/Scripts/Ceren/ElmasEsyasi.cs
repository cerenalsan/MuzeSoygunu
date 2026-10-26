using UnityEngine;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Ceren
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ElmasEsyasi : MonoBehaviour, IInteractable
    {
        private bool alindi;

        public string Prompt => alindi ? string.Empty : "E ile elmasi al";

        public void Interact()
        {
            if (alindi) return;
            if (AsamaIlerletici.OyunBitti(GameStateManager.Instance)) return;
            alindi = true;

            IGameState durum = GameStateManager.Instance;
            AsamaIlerletici.Ilerlet(durum, GameStage.Diamond);
            durum.MarkDiamondTaken();

            GetComponent<SpriteRenderer>().enabled = false;
            Debug.Log("Elmas alindi, aşama: " + durum.CurrentStage);
        }
    }
}