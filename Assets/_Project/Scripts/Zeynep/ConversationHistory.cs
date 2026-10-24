using System;
using System.Collections.Generic;

namespace MuzeSoygunu.Llm
{
    // Son N mesajı tutar. Sınır aşılınca en eski mesajlar silinir.
    public sealed class ConversationHistory
    {
        private readonly List<ChatTurn> turns = new List<ChatTurn>();
        private readonly int maxMessages;

        public ConversationHistory(int maxMessages)
        {
            if (maxMessages < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxMessages));
            }

            this.maxMessages = maxMessages;
        }

        public IReadOnlyList<ChatTurn> Turns => turns;

        public void AddPlayerMessage(string text)
        {
            Add(new ChatTurn(ChatRole.User, text));
        }

        public void AddNpcMessage(string text)
        {
            Add(new ChatTurn(ChatRole.Assistant, text));
        }

        public void Clear()
        {
            turns.Clear();
        }

        private void Add(ChatTurn turn)
        {
            turns.Add(turn);

            while (turns.Count > maxMessages)
            {
                turns.RemoveAt(0);
            }
        }
    }
}
