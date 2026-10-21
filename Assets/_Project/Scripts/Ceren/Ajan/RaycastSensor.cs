using System.Collections.Generic;
using UnityEngine;

namespace MuzeSoygunu.Ceren
{
    public class RaycastSensor : ISensor
    {
        private static readonly float[] Acilar = { -60f, -30f, 0f, 30f, 60f };

        private readonly Transform kaynak;
        private readonly float menzil;
        private readonly ContactFilter2D filtre;
        private readonly RaycastHit2D[] sonuc = new RaycastHit2D[4];
        private readonly SensorOlcumu[] olcumler = new SensorOlcumu[5];

        public RaycastSensor(Transform kaynak, float menzil)
        {
            this.kaynak = kaynak;
            this.menzil = menzil;
            filtre = new ContactFilter2D { useTriggers = false };
        }

        public IReadOnlyList<SensorOlcumu> Olc(Vector2 yon)
        {
            Vector2 baslangic = kaynak.position;
            for (int i = 0; i < Acilar.Length; i++)
            {
                Vector2 isinYonu = Quaternion.Euler(0f, 0f, Acilar[i]) * yon;
                olcumler[i] = Tara(baslangic, isinYonu, Acilar[i]);
            }
            return olcumler;
        }

        private SensorOlcumu Tara(Vector2 baslangic, Vector2 yon, float aci)
        {
            int sayi = Physics2D.Raycast(baslangic, yon, filtre, sonuc, menzil);
            for (int i = 0; i < sayi; i++)
            {
                Collider2D c = sonuc[i].collider;
                if (c.transform == kaynak) continue;
                return new SensorOlcumu(aci, sonuc[i].distance / menzil, Siniflandir(c));
            }
            return new SensorOlcumu(aci, 1f, AlgilananTur.Yok);
        }

        private static AlgilananTur Siniflandir(Collider2D c)
        {
            if (c.GetComponent<MobilEngel>() != null) return AlgilananTur.MobilEngel;
            if (c.CompareTag("Player")) return AlgilananTur.Oyuncu;
            return AlgilananTur.Statik;
        }
    }
}