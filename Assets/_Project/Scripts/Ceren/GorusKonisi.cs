using System;
using UnityEngine;

namespace MuzeSoygunu.Ceren
{
    public class GorusKonisi : MonoBehaviour
    {
        [SerializeField] private Transform oyuncu;
        [SerializeField] private float menzil = 12f;
        [SerializeField] private float yariAci = 15f;

        public event Action OyuncuYakalandi;
        public float Menzil => menzil;
        public float YariAci => yariAci;
        private bool yakalandi;

        private void Update()
        {
            if (yakalandi || oyuncu == null)
            {
                return;
            }

            if (OyuncuyuGoruyorMu())
            {
                yakalandi = true;
                Debug.Log("Kamera oyuncuyu gordu.");
                OyuncuYakalandi?.Invoke();
            }
        }

        private bool OyuncuyuGoruyorMu()
        {
            Vector2 kameraKonumu = transform.position;
            Vector2 oyuncuyaYon = (Vector2)oyuncu.position - kameraKonumu;
            float mesafe = oyuncuyaYon.magnitude;

            if (mesafe > menzil)
            {
                return false;
            }

            float aci = Vector2.Angle(transform.right, oyuncuyaYon);
            if (aci > yariAci)
            {
                return false;
            }

            RaycastHit2D vurus = Physics2D.Raycast(kameraKonumu, oyuncuyaYon.normalized, mesafe);
            return vurus.collider != null && vurus.collider.transform == oyuncu;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 sol = Quaternion.Euler(0f, 0f, yariAci) * transform.right * menzil;
            Vector3 sag = Quaternion.Euler(0f, 0f, -yariAci) * transform.right * menzil;
            Gizmos.DrawRay(transform.position, sol);
            Gizmos.DrawRay(transform.position, sag);
        }
    }
}