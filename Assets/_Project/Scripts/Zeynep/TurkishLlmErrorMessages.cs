namespace MuzeSoygunu.Llm
{
    // LlmError türlerine karşılık gelen Türkçe sistem mesajları.
    public sealed class TurkishLlmErrorMessages : ILlmErrorMessages
    {
        public string GetMessage(LlmError error)
        {
            switch (error)
            {
                case LlmError.ServerUnreachable:
                    return "[Sistem] Ollama sunucusuna ulaşılamıyor. Ollama'nın açık olduğundan emin olun.";
                case LlmError.Timeout:
                    return "[Sistem] Model zamanında cevap vermedi. Lütfen tekrar deneyin.";
                case LlmError.ModelNotFound:
                    return "[Sistem] Model bulunamadı. Model adını ve yüklü olup olmadığını kontrol edin.";
                case LlmError.InvalidResponse:
                    return "[Sistem] Modelden anlaşılmayan bir cevap geldi. Lütfen tekrar deneyin.";
                default:
                    return "[Sistem] Bilinmeyen bir hata oluştu.";
            }
        }
    }
}
