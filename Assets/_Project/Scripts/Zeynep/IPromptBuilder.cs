namespace MuzeSoygunu.Llm
{
    // Bir NPC'nin sistem promptunu (kişilik, amaç, kurallar, JSON şeması) ve isteğe bağlı hatırlatmasını üretir.
    public interface IPromptBuilder
    {
        string BuildSystemPrompt();

        // Oyuncunun her mesajının arkasına eklenen kısa hatırlatma. Boş string: hatırlatma yok.
        string BuildReminder();
    }
}
