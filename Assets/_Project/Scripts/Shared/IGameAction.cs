namespace MuzeSoygunu.Shared
{
    // LLM'in verdiği bir kararı oyun durumuna uygulayan eylem.
    public interface IGameAction
    {
        // JSON'daki action değeriyle aynı: "open_door", "deny", "alarm".
        string Id { get; }

        // Eylemi oyun durumuna uygular.
        void Execute(IGameState state);
    }
}