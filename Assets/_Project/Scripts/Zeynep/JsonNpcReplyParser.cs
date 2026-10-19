using System;
using UnityEngine;

namespace MuzeSoygunu.Llm
{
    // Modelin JSON cevabını doğrulayıp NpcReply'a çevirir. Geçersiz cevapta istisna fırlatmaz.
    public sealed class JsonNpcReplyParser : INpcReplyParser
    {
        public bool TryParse(string rawModelOutput, out NpcReply reply, out string error)
        {
            reply = null;

            if (string.IsNullOrWhiteSpace(rawModelOutput))
            {
                error = "Cevap boş.";
                return false;
            }

            string json = ExtractJsonObject(rawModelOutput);
            if (json == null)
            {
                error = "Cevapta JSON nesnesi bulunamadı.";
                return false;
            }

            RawReply raw;
            try
            {
                raw = JsonUtility.FromJson<RawReply>(json);
            }
            catch (Exception ex)
            {
                error = "JSON ayrıştırılamadı: " + ex.Message;
                return false;
            }

            if (raw == null || string.IsNullOrWhiteSpace(raw.say))
            {
                error = "\"say\" alanı eksik ya da boş.";
                return false;
            }

            NpcAction action;
            if (!TryMapAction(raw.action, out action))
            {
                error = "Bilinmeyen ya da eksik \"action\" değeri: " + raw.action;
                return false;
            }

            reply = new NpcReply(raw.say.Trim(), action);
            error = null;
            return true;
        }

        // İlk '{' ile son '}' arasını alır; çit işaretlerini ve fazladan metni atar.
        private static string ExtractJsonObject(string text)
        {
            int start = text.IndexOf('{');
            int end = text.LastIndexOf('}');

            if (start < 0 || end <= start)
            {
                return null;
            }

            return text.Substring(start, end - start + 1);
        }

        private static bool TryMapAction(string value, out NpcAction action)
        {
            action = NpcAction.Deny;

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            switch (value.Trim().ToLowerInvariant())
            {
                case "open_door":
                    action = NpcAction.OpenDoor;
                    return true;
                case "deny":
                    action = NpcAction.Deny;
                    return true;
                case "alarm":
                    action = NpcAction.Alarm;
                    return true;
                default:
                    return false;
            }
        }

        [Serializable]
        private class RawReply
        {
            public string say;
            public string action;
        }
    }
}
