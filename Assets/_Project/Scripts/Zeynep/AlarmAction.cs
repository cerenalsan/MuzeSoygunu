using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Llm
{
    // Güvenlik sinirlendi ya da hırsızlığı anladı: alarm çalar, oyuncu kaybeder.
    public sealed class AlarmAction : IGameAction
    {
        public string Id => "alarm";

        public void Execute(IGameState state)
        {
            state.Lose(LoseReason.GuardAlarm);
        }
    }
}
