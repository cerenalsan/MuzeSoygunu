using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace MuzeSoygunu.Ceren
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AjanBeyni : MonoBehaviour
    {
        [SerializeField] private Transform oyuncu;
        [SerializeField] private SalterAnahtari salter;
        [SerializeField] private float takipMesafesi = 2f;

        private NavMeshAgent navAjan;
        private IAjanDurumu mevcutDurum;

        public NavMeshAgent NavAjan => navAjan;
        public Transform Oyuncu => oyuncu;
        public SalterAnahtari Salter => salter;
        public float TakipMesafesi => takipMesafesi;

        private void Awake()
        {
            navAjan = GetComponent<NavMeshAgent>();
            navAjan.updateRotation = false;
            navAjan.updateUpAxis = false;
        }

        private void Start()
        {
            DurumDegistir(new OyuncuyuTakipDurumu(this));
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
                mevcutDurum?.EmirAlindi();

            mevcutDurum?.Guncelle();
        }

        public void DurumDegistir(IAjanDurumu yeniDurum)
        {
            string eski = mevcutDurum != null ? mevcutDurum.Ad : "-";
            mevcutDurum?.Cik();
            mevcutDurum = yeniDurum;
            Debug.Log("[Ajan] " + eski + " -> " + yeniDurum.Ad);
            mevcutDurum.Gir();
        }
    }
}