namespace MuzeSoygunu.Ceren
{
    public interface IAjanDurumu
    {
        string Ad { get; }
        void Gir();
        void Guncelle();
        void Cik();
        void EmirAlindi();
    }
}