namespace MuzeSoygunu.Shared
{
    // Oyuncunun yaklaşıp E tuşuyla etkileşime girebildiği nesneler (şalter, elmas).
    public interface IInteractable
    {
        // Oyuncu yaklaşınca ekranda gösterilecek ipucu metni. Örn: "E ile şalteri çek".
        string Prompt { get; }

        // Oyuncu etkileşime girdiğinde çalışır.
        void Interact();
    }
}
