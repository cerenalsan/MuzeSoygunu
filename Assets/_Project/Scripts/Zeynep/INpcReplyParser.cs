namespace MuzeSoygunu.Llm
{
    // Modelin ham cevabını doğrulayıp NpcReply'a çeviren ayrıştırıcının sözleşmesi.
    public interface INpcReplyParser
    {
        // Başarılıysa true döner ve reply dolar. Geçersiz cevapta istisna fırlatmaz,
        // false döner ve error alanına teknik sebep yazılır.
        bool TryParse(string rawModelOutput, out NpcReply reply, out string error);
    }
}
