using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Llm
{
    // Oyun aşamasına göre ekranda gösterilecek kısa ipucu. Boş string: ipucu yok.
    public static class HintTexts
    {
        public static string For(GameStage stage)
        {
            switch (stage)
            {
                case GameStage.Entrance:
                case GameStage.Statue:
                    return "WASD: yürü   Space: zıpla";
                case GameStage.LaserRoom:
                    return "WASD: yürü   Q: ajana emir ver   E: etkileşim";
                case GameStage.Diamond:
                    return "E: elması al";
                case GameStage.Corridor:
                    return "WASD: yürü   Güvenliğe yaklaş";
                default:
                    return string.Empty;
            }
        }
    }
}