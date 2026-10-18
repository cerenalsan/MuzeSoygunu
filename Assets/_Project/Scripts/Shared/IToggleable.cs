namespace MuzeSoygunu.Shared
{
    // Aktif/pasif olabilen nesneler (lazerler). IsOn = çalışıyor.
    public interface IToggleable
    {
        bool IsOn { get; }

        void TurnOn();
        void TurnOff();
    }
}