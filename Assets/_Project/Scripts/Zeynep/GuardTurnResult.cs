namespace MuzeSoygunu.Llm
{
    // Bir diyalog turunun sonucu: NPC'nin cevabı ya da oyuncuya gösterilecek sistem mesajı.
    public sealed class GuardTurnResult
    {
        public bool IsSystemMessage { get; }
        public string Text { get; }
        public NpcAction? Action { get; }
        public string TechnicalDetail { get; }

        private GuardTurnResult(bool isSystemMessage, string text, NpcAction? action, string technicalDetail)
        {
            IsSystemMessage = isSystemMessage;
            Text = text;
            Action = action;
            TechnicalDetail = technicalDetail;
        }

        public static GuardTurnResult FromNpc(string say, NpcAction action)
        {
            return new GuardTurnResult(false, say, action, null);
        }

        public static GuardTurnResult FromSystem(string message, string technicalDetail)
        {
            return new GuardTurnResult(true, message, null, technicalDetail);
        }
    }
}
