using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MuzeSoygunu.Ceren
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class ZiplamaYetenegi : MonoBehaviour
    {
        [SerializeField] private float ziplamaSuresi = 0.8f;
        [SerializeField] private Color havadaRengi = new Color(1f, 1f, 0.6f, 1f);

        private Collider2D oyuncuGovdesi;
        private SpriteRenderer gorsel;
        private Color normalRenk;
        private bool havada;

        private void Awake()
        {
            oyuncuGovdesi = GetComponent<Collider2D>();
            gorsel = GetComponent<SpriteRenderer>();
            normalRenk = gorsel.color;
        }

        private void Update()
        {
            if (havada)
            {
                return;
            }

            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                StartCoroutine(Zipla());
            }
        }

        private IEnumerator Zipla()
        {
            havada = true;
            gorsel.color = havadaRengi;

            ZiplanabilirEngel[] engeller =
                FindObjectsByType<ZiplanabilirEngel>(FindObjectsSortMode.None);

            CarpismayiAyarla(engeller, true);
            yield return new WaitForSeconds(ziplamaSuresi);

            while (BirEngelinIcindeMi(engeller))
            {
                yield return null;
            }

            CarpismayiAyarla(engeller, false);
            gorsel.color = normalRenk;
            havada = false;
        }

        private void CarpismayiAyarla(ZiplanabilirEngel[] engeller, bool yoksay)
        {
            foreach (ZiplanabilirEngel engel in engeller)
            {
                Physics2D.IgnoreCollision(oyuncuGovdesi, engel.Govde, yoksay);
            }
        }

        private bool BirEngelinIcindeMi(ZiplanabilirEngel[] engeller)
        {
            foreach (ZiplanabilirEngel engel in engeller)
            {
                if (engel.Govde.Distance(oyuncuGovdesi).isOverlapped)
                {
                    return true;
                }
            }
            return false;
        }
    }
}