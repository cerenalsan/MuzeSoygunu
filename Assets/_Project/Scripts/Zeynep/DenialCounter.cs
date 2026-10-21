namespace MuzeSoygunu.Llm
{
    // NPC'nin kaç kez reddettiğini sayar. Sınır aşılınca oyuncu kaybeder.
    public sealed class DenialCounter
    {
        public int Count { get; private set; }
        public int MaxDenials { get; }

        public DenialCounter(int maxDenials)
        {
            MaxDenials = maxDenials;
        }

        // Sınırı aşan ret (ör. 3 sınırında 4. ret) true döner.
        public bool RegisterDenial()
        {
            Count++;
            return Count > MaxDenials;
        }

        public void Reset()
        {
            Count = 0;
        }
    }
}
