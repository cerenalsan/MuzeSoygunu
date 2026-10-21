using System;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Llm
{
    // Güvenlik reddetti: ret sayısı artar, sınır aşılırsa oyuncu kaybeder.
    public sealed class DenyAction : IGameAction
    {
        private readonly DenialCounter counter;

        public DenyAction(DenialCounter counter)
        {
            if (counter == null)
            {
                throw new ArgumentNullException(nameof(counter));
            }

            this.counter = counter;
        }

        public string Id => "deny";

        public void Execute(IGameState state)
        {
            if (counter.RegisterDenial())
            {
                state.Lose(LoseReason.GuardRefused);
            }
        }
    }
}
