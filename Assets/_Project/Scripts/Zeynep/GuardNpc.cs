using System;
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

        private readonly ILlmClient client;
        private readonly IPromptBuilder promptBuilder;
        private readonly INpcReplyParser parser;
        private readonly GameActionFactory actionFactory;
        private readonly IGameState gameState;
        private readonly ILlmErrorMessages errorMessages;

        public GuardNpc(
            ILlmClient client,
            IPromptBuilder promptBuilder,
            INpcReplyParser parser,
            GameActionFactory actionFactory,
            IGameState gameState,
            ILlmErrorMessages errorMessages)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (promptBuilder == null) throw new ArgumentNullException(nameof(promptBuilder));
            if (parser == null) throw new ArgumentNullException(nameof(parser));
            if (actionFactory == null) throw new ArgumentNullException(nameof(actionFactory));
            if (gameState == null) throw new ArgumentNullException(nameof(gameState));
            if (errorMessages == null) throw new ArgumentNullException(nameof(errorMessages));

            this.client = client;
            this.promptBuilder = promptBuilder;
            this.parser = parser;
            this.actionFactory = actionFactory;
            this.gameState = gameState;
            this.errorMessages = errorMessages;
        }

        // Oyuncunun mesajını işler. Hata durumunda istisna fırlatmaz, sistem mesajı döner.
        // Diyalog aşaması bittiyse (kazanıldı ya da kaybedildi) mesaj işlenmez, Ollama'ya istek atılmaz.
        // İptal (sahne kapandı) OperationCanceledException olarak çağırana bırakılır.
        public async Task<GuardTurnResult> TalkAsync(string playerMessage, CancellationToken cancellationToken)
        {
            if (gameState.CurrentStage != GameStage.GuardDialogue)
            {
                return GuardTurnResult.FromSystem(DialogueEndedMessage, "Aşama: " + gameState.CurrentStage);
            }

            LlmResult result = await client.SendAsync(promptBuilder.BuildSystemPrompt(), playerMessage, cancellationToken);

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

            action.Execute(gameState);
            return GuardTurnResult.FromNpc(reply.Say, reply.Action);
        }

        private GuardTurnResult InvalidResponse(string detail)
        {
            return GuardTurnResult.FromSystem(errorMessages.GetMessage(LlmError.InvalidResponse), "InvalidResponse: " + detail);
        }
    }
}
