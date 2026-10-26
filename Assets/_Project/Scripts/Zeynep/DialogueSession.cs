using System;
using System.Collections.Generic;

namespace MuzeSoygunu.Llm
{
    // Bir diyaloğun geçmişini ve oyuncu tur sayısını tutar. Başarısız istekler geçmişe yazılmaz.
    public sealed class DialogueSession
    {
        // Çift sayı olmalı: geçmiş oyuncu ve NPC mesajı çiftleri hâlinde tutulur.
        public const int DefaultMaxMessages = 6;
        public const int DefaultMaxPlayerTurns = 10;

        private readonly ConversationHistory history;

        public int PlayerTurns { get; private set; }
        public int MaxPlayerTurns { get; }
        public bool IsTurnLimitReached => PlayerTurns >= MaxPlayerTurns;
        public IReadOnlyList<ChatTurn> Messages => history.Turns;

        public DialogueSession(int maxMessages, int maxPlayerTurns)
        {
            if (maxPlayerTurns < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxPlayerTurns));
            }

            history = new ConversationHistory(maxMessages);
            MaxPlayerTurns = maxPlayerTurns;
        }

        public static DialogueSession CreateDefault()
        {
            return new DialogueSession(DefaultMaxMessages, DefaultMaxPlayerTurns);
        }

        // Başarılı bir turu kaydeder: oyuncunun mesajı ve modelin ham cevabı (JSON).
        public void Record(string playerMessage, string npcRawReply)
        {
            history.AddPlayerMessage(playerMessage);
            history.AddNpcMessage(npcRawReply);
            PlayerTurns++;
        }
    }
}
