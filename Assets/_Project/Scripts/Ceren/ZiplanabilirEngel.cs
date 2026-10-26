using UnityEngine;

namespace MuzeSoygunu.Ceren
{
    [RequireComponent(typeof(Collider2D))]
    public class ZiplanabilirEngel : MonoBehaviour
    {
        public Collider2D Govde { get; private set; }

        private void Awake()
        {
            Govde = GetComponent<Collider2D>();
        }
    }
}