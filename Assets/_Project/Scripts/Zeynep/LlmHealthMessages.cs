namespace MuzeSoygunu.Llm
{
    // Sağlık durumuna karşılık gelen Türkçe sistem mesajları. Hazırsa boş döner.
    public static class LlmHealthMessages
    {
        public static string For(LlmHealthStatus status, string modelName)
        {
            switch (status)
            {
                case LlmHealthStatus.Ready:
                    return string.Empty;
                case LlmHealthStatus.ServerUnreachable:
                    return "[Sistem] Ollama sunucusuna ulaşılamıyor. Ollama'yı açıp 'Tekrar dene'ye bas.";
                case LlmHealthStatus.ModelNotInstalled:
                    return "[Sistem] Model yüklü değil: " + modelName + ". Terminalde 'ollama pull " + modelName + "' çalıştırıp 'Tekrar dene'ye bas.";
                default:
                    return "[Sistem] Ollama durumu denetlenemedi. 'Tekrar dene'ye bas.";
            }
        }
    }
}
