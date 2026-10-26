using UnityEngine;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Ceren
{
    public class YakalanmaTepkisi : MonoBehaviour
    {
        [SerializeField] private GorusKonisi kamera;

        private void OnEnable()
        {
            kamera.OyuncuYakalandi += Yakalandi;
        }

        private void OnDisable()
        {
            kamera.OyuncuYakalandi -= Yakalandi;
        }

        private void Yakalandi()
        {
            Debug.Log("Oyuncu kamera tarafindan yakalandi.");
            GameStateManager.Instance.Lose(LoseReason.CameraCaught);
        }
    }
}