using UnityEngine;

namespace MuzeSoygunu.Ceren
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MobilEngel : MonoBehaviour
    {
        [SerializeField] private Vector2 uc1 = new Vector2(0f, 9.2f);
        [SerializeField] private Vector2 uc2 = new Vector2(0f, 1.0f);
        [SerializeField] private float hiz = 2f;

        private Rigidbody2D govde;
        private Vector2 hedef;

        private void Awake()
        {
            govde = GetComponent<Rigidbody2D>();
            govde.bodyType = RigidbodyType2D.Kinematic;
            hedef = uc2;
        }

        private void FixedUpdate()
        {
            Vector2 yeni = Vector2.MoveTowards(govde.position, hedef, hiz * Time.fixedDeltaTime);
            govde.MovePosition(yeni);

            if (Vector2.Distance(yeni, hedef) < 0.01f)
                hedef = hedef == uc1 ? uc2 : uc1;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(uc1, uc2);
        }
    }
}