namespace MuzeSoygunu.Shared
{
    // Açılıp kapanabilen geçitler (çıkış kapısı). IsOpen = geçilebilir.
    public interface IOpenable
    {
        bool IsOpen { get; }

        void Open();
        void Close();
    }
}