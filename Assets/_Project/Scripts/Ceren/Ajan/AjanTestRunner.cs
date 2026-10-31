using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MuzeSoygunu.Ceren
{
    public class AjanTestRunner : MonoBehaviour
    {
        [SerializeField] private AjanBeyni ajan;
        [SerializeField] private SalterAnahtari salter;
        [SerializeField] private Transform oyuncu;
        [SerializeField] private Transform oyuncuNoktasi;
        [SerializeField] private Transform[] baslangicNoktalari;
        [SerializeField] private GameObject mobilEngel;
        [SerializeField] private int engelsizDenemeSayisi = 10;
        [SerializeField] private int mobilDenemeSayisi = 5;
        [SerializeField] private float zamanAsimiSaniye = 30f;

        private readonly TestRaporu rapor = new TestRaporu();
        private bool calisiyor;
        private string durumYazisi = "F9: testi baslat";

        private void Update()
        {
            if (!calisiyor && Keyboard.current != null && Keyboard.current.f9Key.wasPressedThisFrame)
                StartCoroutine(TestiCalistir());
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 800, 30), durumYazisi);
        }

        private IEnumerator TestiCalistir()
        {
            calisiyor = true;
            durumYazisi = "Test henuz bos";
            yield return null;
            calisiyor = false;
        }
    }
}