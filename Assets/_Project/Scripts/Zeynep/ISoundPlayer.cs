namespace MuzeSoygunu.Llm
{
    // Ses efekti çalan sınıfın sözleşmesi. Ses bulunamazsa ya da kapalıysa hata vermez, sessiz kalır.
    public interface ISoundPlayer
    {
        bool Muted { get; set; }
        void Play(SoundId id);
    }
}
