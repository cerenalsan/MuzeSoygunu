namespace MuzeSoygunu.Llm
{
    // Modelin doğrulanmış cevabı: oyuncuya gösterilecek metin ve oyun durumunu değiştirecek karar.
    public sealed class NpcReply
    {
        public string Say { get; }
        public NpcAction Action { get; }

        public NpcReply(string say, NpcAction action)
        {
            Say = say;
            Action = action;
        }
    }
}
