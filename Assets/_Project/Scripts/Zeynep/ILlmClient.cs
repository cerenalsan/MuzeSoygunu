using System.Threading;
using System.Threading.Tasks;

namespace MuzeSoygunu.Llm
{
    // Bir dil modeli sunucusuyla konuşan istemcinin sözleşmesi.
    public interface ILlmClient
    {
        // Sistem promptu ve kullanıcı mesajıyla cevap ister. Hata olunca istisna fırlatmaz,
        // başarısız bir LlmResult döner.
        Task<LlmResult> SendAsync(string systemPrompt, string userMessage, CancellationToken cancellationToken);
    }
}
