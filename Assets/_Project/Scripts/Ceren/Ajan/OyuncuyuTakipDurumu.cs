using UnityEngine;

namespace MuzeSoygunu.Ceren
{
    public class OyuncuyuTakipDurumu : IAjanDurumu
    {
        private readonly AjanBeyni beyin;

        public OyuncuyuTakipDurumu(AjanBeyni beyin)
        {
            this.beyin = beyin;
        }

        public string Ad => "FollowPlayer";

        public void Gir()
        {
            beyin.NavAjan.isStopped = false;
        }

        public void Guncelle()
        {
            float mesafe = Vector2.Distance(beyin.transform.position, beyin.Oyuncu.position);
            if (mesafe > beyin.TakipMesafesi)
                beyin.NavAjan.SetDestination(beyin.Oyuncu.position);
            else
                beyin.NavAjan.ResetPath();
        }

        public void Cik()
        {
            beyin.NavAjan.ResetPath();
        }

        public void EmirAlindi()
        {
            if (beyin.Salter.Cekildi)
            {
                Debug.Log("[Ajan] Salter zaten cekilmis, emir yoksayildi.");
                return;
            }
            beyin.DurumDegistir(new SaltereGitDurumu(beyin));
        }
    }
}