using System.Collections.Generic;
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
        [SerializeField] private float sensorMenzili = 4f;
        [SerializeField] private float beklemeMesafesi = 2f;
        public float EnYakinMobilEngelMesafesi { get; private set; }
        private NavMeshAgent navAjan;
        private IAjanDurumu mevcutDurum;
        private IReadOnlyList<SensorOlcumu> sonOlcumler;
        private Vector2 sonYon;

        public NavMeshAgent NavAjan => navAjan;
        public Transform Oyuncu => oyuncu;
        public SalterAnahtari Salter => salter;
        public float TakipMesafesi => takipMesafesi;
        public ISensor Sensor { get; private set; }

        private void Awake()
        {
            navAjan = GetComponent<NavMeshAgent>();
            navAjan.updateRotation = false;
            navAjan.updateUpAxis = false;
            Sensor = new RaycastSensor(transform, sensorMenzili);
        }

        private void Start()
        {
            DurumDegistir(new OyuncuyuTakipDurumu(this));
        }

        private void Update()
        {
            if (AsamaIlerletici.OyunBitti(GameStateManager.Instance)) return;

            if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
                EmirVer();

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
        public void EmirVer()
        {
            mevcutDurum?.EmirAlindi();
        }

        public void Sifirla(Vector3 konum)
        {
            navAjan.ResetPath();
            navAjan.Warp(konum);
            DurumDegistir(new OyuncuyuTakipDurumu(this));
        }
        public bool OnundeMobilEngelVar(Vector2 yon)
        {
            sonYon = yon;
            sonOlcumler = Sensor.Olc(yon);

            float enYakin = float.MaxValue;
            for (int i = 0; i < sonOlcumler.Count; i++)
            {
                SensorOlcumu olcum = sonOlcumler[i];
                if (olcum.Tur != AlgilananTur.MobilEngel) continue;

                float metre = olcum.Mesafe * sensorMenzili;
                if (metre < enYakin) enYakin = metre;
            }

            EnYakinMobilEngelMesafesi = enYakin;
            return enYakin <= beklemeMesafesi;
        }

        private void OnDrawGizmos()
        {
            if (sonOlcumler == null) return;

            foreach (SensorOlcumu olcum in sonOlcumler)
            {
                Vector2 isinYonu = Quaternion.Euler(0f, 0f, olcum.Aci) * sonYon;
                Gizmos.color = olcum.Tur == AlgilananTur.Yok ? Color.green : Color.red;
                Gizmos.DrawLine(transform.position, (Vector2)transform.position + isinYonu * olcum.Mesafe * sensorMenzili);
            }
        }
    }
}