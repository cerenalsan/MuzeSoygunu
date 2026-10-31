namespace MuzeSoygunu.Ceren
{
    public enum AlgilananTur
    {
        Yok,
        Statik,
        MobilEngel,
        Oyuncu
    }

    public readonly struct SensorOlcumu
    {
        public float Aci { get; }
        public float Mesafe { get; }
        public AlgilananTur Tur { get; }

        public SensorOlcumu(float aci, float mesafe, AlgilananTur tur)
        {
            Aci = aci;
            Mesafe = mesafe;
            Tur = tur;
        }
    }
}