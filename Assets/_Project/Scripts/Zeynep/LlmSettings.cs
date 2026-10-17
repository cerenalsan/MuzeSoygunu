using UnityEngine;

namespace MuzeSoygunu.Llm
{
    // Yerel Ollama sunucusuna bağlanmak için gereken ayarlar.
    [CreateAssetMenu(fileName = "LlmSettings", menuName = "MuzeSoygunu/Llm Settings")]
    public class LlmSettings : ScriptableObject
    {
        [SerializeField] private string baseUrl = "http://127.0.0.1:11434";
        [SerializeField] private string modelName = "gemma2:2b";
        [SerializeField, Min(1)] private int timeoutSeconds = 30;
        [SerializeField, Min(1)] private int warmupTimeoutSeconds = 90;
        [SerializeField, Range(0f, 1f)] private float temperature = 0.3f;

        public string BaseUrl => baseUrl;
        public string ModelName => modelName;
        public int TimeoutSeconds => timeoutSeconds;
        public int WarmupTimeoutSeconds => warmupTimeoutSeconds;
        public float Temperature => temperature;
    }
}
