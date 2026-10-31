namespace MuzeSoygunu.Ceren
{
    public readonly struct DenemeSonucu
    {
        public string Kosul { get; }
        public int NoktaNo { get; }
        public int DenemeNo { get; }
        public bool Basarili { get; }
        public float SureSaniye { get; }

        public DenemeSonucu(string kosul, int noktaNo, int denemeNo, bool basarili, float sureSaniye)
        {
            Kosul = kosul;
            NoktaNo = noktaNo;
            DenemeNo = denemeNo;
            Basarili = basarili;
            SureSaniye = sureSaniye;
        }
    }
}