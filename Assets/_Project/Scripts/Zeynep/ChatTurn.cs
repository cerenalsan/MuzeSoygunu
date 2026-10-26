namespace MuzeSoygunu.Llm
{
    public enum ChatRole
    {
        User,
        Assistant
    }

    // Konuşmadaki tek bir mesaj.
    public sealed class ChatTurn
    {
        public ChatRole Role { get; }
        public string Content { get; }

        public ChatTurn(ChatRole role, string content)
        {
            Role = role;
            Content = content;
        }
    }
}
