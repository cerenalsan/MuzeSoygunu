using UnityEngine;

namespace MuzeSoygunu.Ceren
{
    public class SalteriCekDurumu : IAjanDurumu
    {
        private const float CekmeSuresi = 1f;

        private readonly AjanBeyni beyin;
        private float baslangic;

        public SalteriCekDurumu(AjanBeyni beyin)
        {
            this.beyin = beyin;
        }

        public string Ad => "PullSwitch";

        public void Gir()
        {
            baslangic = Time.time;
        }

        public void Guncelle()
        {
            if (Time.time - baslangic < CekmeSuresi) return;

            beyin.Salter.Interact();
            beyin.DurumDegistir(new BittiDurumu());
        }

        public void Cik() { }

        public void EmirAlindi() { }
    }
}