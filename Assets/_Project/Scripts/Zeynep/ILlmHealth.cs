using System.Threading;
using System.Threading.Tasks;

namespace MuzeSoygunu.Llm
{
    // LLM sunucusunun hazır olup olmadığını denetleyen sınıfın sözleşmesi.
    public interface ILlmHealth
    {
        // Hata durumunda istisna fırlatmaz, durumu döner. İptal çağırana bırakılır.
        Task<LlmHealthStatus> CheckAsync(CancellationToken cancellationToken);
    }
}
