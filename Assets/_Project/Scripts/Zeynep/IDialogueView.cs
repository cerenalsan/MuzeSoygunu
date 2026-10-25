using System;

namespace MuzeSoygunu.Llm
{
    // Diyalog ekranının sözleşmesi. GuardNpc ekranın nasıl çizildiğini bilmez.
    public interface IDialogueView
    {
        // Oyuncu mesajı gönderdiğinde tetiklenir.
        event Action<string> MessageSubmitted;

        // Oyuncu "Tekrar dene" düğmesine bastığında tetiklenir.
        event Action RetryRequested;

        void ShowPlayerMessage(string text);
        void ShowNpcMessage(string text);
        void ShowSystemMessage(string text);

        // İstek sürerken true: "Düşünüyor..." gösterilir ve gönderme kapanır.
        void SetBusy(bool busy);

        // "Tekrar dene" düğmesini gösterir ya da gizler.
        void SetRetryVisible(bool visible);
    }
}
