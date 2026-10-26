namespace MuzeSoygunu.Llm
{
    // Bir LLM hata türünü oyuncuya gösterilecek sistem mesajına çevirir.
    // Bu mesajlar NPC'nin ağzından değil, sistemden gelir; asla sahte NPC cevabı değildir.
    public interface ILlmErrorMessages
    {
        string GetMessage(LlmError error);
    }
}
