using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Llm
{
    // Güvenlik görevlisi NPC'nin akışını yönetir: istek, ayrıştırma, eylem, oyun durumu.
    // Hepsi arayüz üzerinden bağlıdır; Unity ya da Ollama'yı doğrudan bilmez.
    public sealed class GuardNpc
    {
        public const string DialogueEndedMessage = "[Sistem] Diyalog sona erdi.";
        public const string TurnLimitMessage = "[Sistem] Güvenlik görevlisi sabrını yitirdi ve seni dışarı bırakmayacak.";

        private readonly ILlmClient client;
        private readonly IPromptBuilder promptBuilder;
        private readonly INpcReplyParser parser;
        private readonly GameActionFactory actionFactory;
        private readonly IGameState gameState;
        private readonly ILlmErrorMessages errorMessages;
        private readonly DialogueSession session;

        public GuardNpc(
            ILlmClient client,
            IPromptBuilder promptBuilder,
            INpcReplyParser parser,
            GameActionFactory actionFactory,
            IGameState gameState,
            ILlmErrorMessages errorMessages,
            DialogueSession session)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (promptBuilder == null) throw new ArgumentNullException(nameof(promptBuilder));
            if (parser == null) throw new ArgumentNullException(nameof(parser));
            if (actionFactory == null) throw new ArgumentNullException(nameof(actionFactory));
            if (gameState == null) throw new ArgumentNullException(nameof(gameState));
            if (errorMessages == null) throw new ArgumentNullException(nameof(errorMessages));
            if (session == null) throw new ArgumentNullException(nameof(session));

            this.client = client;
            this.promptBuilder = promptBuilder;
            this.parser = parser;
            this.actionFactory = actionFactory;
            this.gameState = gameState;
            this.errorMessages = errorMessages;
            this.session = session;
        }

        // Oyuncunun mesajını işler. Hata durumunda istisna fırlatmaz, sistem mesajı döner.
        // Diyalog aşaması bittiyse mesaj işlenmez. Tur sınırı aşılırsa oyuncu kaybeder ve Ollama'ya istek atılmaz.
        // İptal (sahne kapandı) OperationCanceledException olarak çağırana bırakılır.
        public async Task<GuardTurnResult> TalkAsync(string playerMessage, CancellationToken cancellationToken)
        {
            if (gameState.CurrentStage != GameStage.GuardDialogue)
            {
                return GuardTurnResult.FromSystem(DialogueEndedMessage, "Aşama: " + gameState.CurrentStage);
            }

            if (session.IsTurnLimitReached)
            {
                gameState.Lose(LoseReason.GuardRefused);
                return GuardTurnResult.FromSystem(TurnLimitMessage, "Tur sınırı: " + session.PlayerTurns);
            }

            var messages = new List<ChatTurn>(session.Messages);
            messages.Add(new ChatTurn(ChatRole.User, WithReminder(playerMessage)));

            LlmResult result = await client.SendAsync(promptBuilder.BuildSystemPrompt(), messages, cancellationToken);

            if (!result.IsSuccess)
            {
                return GuardTurnResult.FromSystem(errorMessages.GetMessage(result.Error), result.Error + ": " + result.ErrorDetail);
            }

            NpcReply reply;
            string parseError;
            if (!parser.TryParse(result.Content, out reply, out parseError))
            {
                return InvalidResponse(parseError);
            }

            IGameAction action = actionFactory.Create(reply.Action);
            if (action == null)
            {
                return InvalidResponse("Bilinmeyen eylem: " + reply.Action);
            }

            session.Record(playerMessage, result.Content);
            action.Execute(gameState);
            return GuardTurnResult.FromNpc(reply.Say, reply.Action);
        }

        private string WithReminder(string playerMessage)
        {
            string reminder = promptBuilder.BuildReminder();
            return string.IsNullOrEmpty(reminder) ? playerMessage : playerMessage + "\n\n" + reminder;
        }

        private GuardTurnResult InvalidResponse(string detail)
        {
            return GuardTurnResult.FromSystem(errorMessages.GetMessage(LlmError.InvalidResponse), "InvalidResponse: " + detail);
        }
    }
}
