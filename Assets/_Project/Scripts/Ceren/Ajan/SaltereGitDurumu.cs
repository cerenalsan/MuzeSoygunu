using UnityEngine;
using UnityEngine.AI;

namespace MuzeSoygunu.Ceren
{
    public class SaltereGitDurumu : IAjanDurumu
    {
        private const float YaklasmaMesafesi = 1f;

        private readonly AjanBeyni beyin;

        public SaltereGitDurumu(AjanBeyni beyin)
        {
            this.beyin = beyin;
        }

        public string Ad => "MoveToSwitch";

        public void Gir()
        {
            beyin.NavAjan.isStopped = false;
            beyin.NavAjan.SetDestination(beyin.Salter.transform.position);
        }

        public void Guncelle()
        {
            NavMeshAgent nav = beyin.NavAjan;
            if (nav.pathPending) return;

            if (nav.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                Debug.LogWarning("[Ajan] Salter icin yol bulunamadi.");
                beyin.DurumDegistir(new OyuncuyuTakipDurumu(beyin));
                return;
            }

            Vector2 yon = ((Vector2)nav.steeringTarget - (Vector2)beyin.transform.position).normalized;
            if (beyin.OnundeMobilEngelVar(yon))
            {
                beyin.DurumDegistir(new EngeleBeklemeDurumu(beyin, yon));
                return;
            }

            float mesafe = Vector2.Distance(beyin.transform.position, beyin.Salter.transform.position);
            if (mesafe <= YaklasmaMesafesi)
                beyin.DurumDegistir(new SalteriCekDurumu(beyin));
        }

        public void Cik()
        {
            beyin.NavAjan.ResetPath();
        }

        public void EmirAlindi() { }
    }
}