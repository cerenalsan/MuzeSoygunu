namespace MuzeSoygunu.Llm
{
    // Bir NPC'nin sistem promptunu (kişilik, amaç, kurallar, JSON şeması) üretir.
    public interface IPromptBuilder
    {
        string BuildSystemPrompt();
    }
}
