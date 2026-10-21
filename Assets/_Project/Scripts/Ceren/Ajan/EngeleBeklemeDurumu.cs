using UnityEngine;
using UnityEngine.AI;

namespace MuzeSoygunu.Ceren
{
    public class EngeleBeklemeDurumu : IAjanDurumu
    {
        private const float EnAzBeklemeSuresi = 0.5f;
        private const float GeriCekilmeMesafesi = 1.2f;
        private const float GeriCekilmeAdimi = 1.5f;

        private readonly AjanBeyni beyin;
        private readonly Vector2 yon;
        private float baslangic;

        public EngeleBeklemeDurumu(AjanBeyni beyin, Vector2 yon)
        {
            this.beyin = beyin;
            this.yon = yon;
        }

        public string Ad => "WaitForObstacle";

        public void Gir()
        {
            baslangic = Time.time;
            beyin.NavAjan.isStopped = true;
        }

        public void Guncelle()
        {
            bool engelVar = beyin.OnundeMobilEngelVar(yon);

            if (engelVar && beyin.EnYakinMobilEngelMesafesi < GeriCekilmeMesafesi)
            {
                GeriCekil();
                return;
            }

            beyin.NavAjan.isStopped = true;

            if (Time.time - baslangic < EnAzBeklemeSuresi) return;

            if (!engelVar)
                beyin.DurumDegistir(new SaltereGitDurumu(beyin));
        }

        public void Cik()
        {
            beyin.NavAjan.isStopped = false;
        }

        public void EmirAlindi() { }

        private void GeriCekil()
        {
            Vector3 hedef = beyin.transform.position - (Vector3)(yon * GeriCekilmeAdimi);
            if (NavMesh.SamplePosition(hedef, out NavMeshHit vurus, 1f, NavMesh.AllAreas))
            {
                beyin.NavAjan.isStopped = false;
                beyin.NavAjan.SetDestination(vurus.position);
            }
        }
    }
}