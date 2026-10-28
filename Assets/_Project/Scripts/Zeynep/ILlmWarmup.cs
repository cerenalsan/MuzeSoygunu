using System.Threading;
using System.Threading.Tasks;

namespace MuzeSoygunu.Llm
{
    // Modeli önceden belleğe yükleten sınıfın sözleşmesi.
    public interface ILlmWarmup
    {
        // Başarılıysa true döner. Hata olursa istisna fırlatmaz, false döner. İptal çağırana bırakılır.
        Task<bool> WarmUpAsync(CancellationToken cancellationToken);
    }
}
