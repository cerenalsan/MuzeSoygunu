namespace MuzeSoygunu.Llm
{
    // Ollama sunucusunun ve modelin hazır olup olmadığı.
    public enum LlmHealthStatus
    {
        Ready,
        ServerUnreachable,
        ModelNotInstalled,
        Unknown
    }
}
