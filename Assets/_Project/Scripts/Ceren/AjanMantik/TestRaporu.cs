using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MuzeSoygunu.Ceren
{
    public class TestRaporu
    {
        private readonly List<DenemeSonucu> sonuclar = new List<DenemeSonucu>();

        public IReadOnlyList<DenemeSonucu> Sonuclar => sonuclar;

        public void Ekle(DenemeSonucu sonuc)
        {
            sonuclar.Add(sonuc);
        }

        public string CsvUret()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("kosul,nokta,deneme,basarili,sure_sn");
            foreach (DenemeSonucu s in sonuclar)
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                    "{0},{1},{2},{3},{4:F2}",
                    s.Kosul, s.NoktaNo, s.DenemeNo, s.Basarili ? 1 : 0, s.SureSaniye));
            }
            return sb.ToString();
        }

        public int DenemeSayisi(string kosul, int noktaNo)
        {
            int sayi = 0;
            foreach (DenemeSonucu s in sonuclar)
                if (s.Kosul == kosul && s.NoktaNo == noktaNo) sayi++;
            return sayi;
        }

        public int BasariliSayisi(string kosul, int noktaNo)
        {
            int sayi = 0;
            foreach (DenemeSonucu s in sonuclar)
                if (s.Kosul == kosul && s.NoktaNo == noktaNo && s.Basarili) sayi++;
            return sayi;
        }

        public float BasariOrani(string kosul, int noktaNo)
        {
            int toplam = DenemeSayisi(kosul, noktaNo);
            return toplam == 0 ? 0f : (float)BasariliSayisi(kosul, noktaNo) / toplam;
        }

        // Ortalama sure sadece basarili denemelerden hesaplanir.
        public float OrtalamaSure(string kosul, int noktaNo)
        {
            float toplam = 0f;
            int sayi = 0;
            foreach (DenemeSonucu s in sonuclar)
            {
                if (s.Kosul != kosul || s.NoktaNo != noktaNo || !s.Basarili) continue;
                toplam += s.SureSaniye;
                sayi++;
            }
            return sayi == 0 ? 0f : toplam / sayi;
        }
    }
}