using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MuzeSoygunu.Llm
{
    // Bir dil modeli sunucusuyla konuşan istemcinin sözleşmesi.
    public interface ILlmClient
    {
        // Sistem promptu ve konuşma geçmişiyle cevap ister (son mesaj oyuncunun yeni mesajıdır).
        // Hata olunca istisna fırlatmaz, başarısız bir LlmResult döner.
        Task<LlmResult> SendAsync(string systemPrompt, IReadOnlyList<ChatTurn> messages, CancellationToken cancellationToken);
    }
}
