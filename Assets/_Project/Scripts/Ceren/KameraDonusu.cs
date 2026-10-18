using UnityEngine;

namespace MuzeSoygunu.Ceren
{
    public class KameraDonusu : MonoBehaviour
    {
        [SerializeField] private float donusAcisi = 45f;
        [SerializeField] private float hiz = 30f;

        private float baslangicAcisi;

        private void Start()
        {
            baslangicAcisi = transform.eulerAngles.z;
        }

        private void Update()
        {
            float sapma = Mathf.PingPong(Time.time * hiz, donusAcisi * 2f) - donusAcisi;
            transform.rotation = Quaternion.Euler(0f, 0f, baslangicAcisi + sapma);
        }
    }
}