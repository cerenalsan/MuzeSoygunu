using System.Collections;
using System.IO;
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
        private Vector3 mobilBaslangic;

        private void Awake()
        {
            if (mobilEngel != null) mobilBaslangic = mobilEngel.transform.position;
        }

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
            yield return Kosu("engelsiz", engelsizDenemeSayisi);
            yield return Kosu("mobil", mobilDenemeSayisi);

            string yol = CsvKaydet();
            durumYazisi = "Bitti. CSV: " + yol;
            Debug.Log("[Test] " + durumYazisi);
            calisiyor = false;
        }

        private IEnumerator Kosu(string kosul, int denemeSayisi)
        {
            for (int n = 0; n < baslangicNoktalari.Length; n++)
            {
                for (int d = 1; d <= denemeSayisi; d++)
                {
                    durumYazisi = kosul + " | nokta " + (n + 1) + "/" + baslangicNoktalari.Length
                                  + " | deneme " + d + "/" + denemeSayisi;
                    yield return Dene(kosul, n + 1, d, baslangicNoktalari[n]);
                }
            }
        }

        private IEnumerator Dene(string kosul, int noktaNo, int denemeNo, Transform baslangic)
        {
            if (mobilEngel != null)
            {
                mobilEngel.SetActive(false);
                mobilEngel.transform.position = mobilBaslangic;
                mobilEngel.SetActive(kosul == "mobil");
            }

            salter.Sifirla();
            oyuncu.position = oyuncuNoktasi.position;
            ajan.Sifirla(baslangic.position);

            yield return new WaitForSeconds(1f);
            ajan.EmirVer();

            float gecen = 0f;
            while (!salter.Cekildi && gecen < zamanAsimiSaniye)
            {
                gecen += Time.deltaTime;
                yield return null;
            }

            rapor.Ekle(new DenemeSonucu(kosul, noktaNo, denemeNo, salter.Cekildi, gecen));
        }

        private string CsvKaydet()
        {
            string ad = "ajan-test-" + System.DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".csv";
            string yol = Path.Combine(Application.persistentDataPath, ad);
            File.WriteAllText(yol, rapor.CsvUret());
            return yol;
        }
    }
    }
