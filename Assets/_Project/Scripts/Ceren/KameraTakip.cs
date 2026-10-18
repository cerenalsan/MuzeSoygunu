using UnityEngine;

namespace MuzeSoygunu
{
    public class KameraTakip : MonoBehaviour
    {
        [SerializeField] private Transform hedef;
        [SerializeField] private float yumusatmaSuresi = 0.15f;

        private Vector3 hiz;

        private void LateUpdate()
        {
            if (hedef == null)
            {
                return;
            }

            Vector3 istenenKonum = new Vector3(
                hedef.position.x,
                hedef.position.y,
                transform.position.z);

            transform.position = Vector3.SmoothDamp(
                transform.position,
                istenenKonum,
                ref hiz,
                yumusatmaSuresi);
        }
    }
}