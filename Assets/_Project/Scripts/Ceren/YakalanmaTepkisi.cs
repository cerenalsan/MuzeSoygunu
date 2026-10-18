using MuzeSoygunu.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MuzeSoygunu.Ceren
{
    public class YakalanmaTepkisi : MonoBehaviour
    {
        [SerializeField] private GorusKonisi kamera;
        [SerializeField] private float yenidenBaslamaGecikmesi = 1f;

        private void OnEnable()
        {
            if (kamera != null)
            {
                kamera.OyuncuYakalandi += Yakalandi;
            }
        }

        private void OnDisable()
        {
            if (kamera != null)
            {
                kamera.OyuncuYakalandi -= Yakalandi;
            }
        }

        private void Yakalandi()
        {
            GameStateManager.Instance.Lose(LoseReason.CameraCaught);
            Debug.Log("Oyuncu yakalandi, sahne yeniden baslatiliyor.");
            Invoke(nameof(SahneyiBastanBaslat), yenidenBaslamaGecikmesi);
        }

        private void SahneyiBastanBaslat()
        {
            GameStateManager.Instance.AdvanceTo(GameStage.Entrance);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}