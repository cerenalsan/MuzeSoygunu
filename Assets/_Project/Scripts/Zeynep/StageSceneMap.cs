using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Llm
{
    // Oyun aşamasına karşılık gelen sahne adını verir. Unity'ye bağlı değildir, birim testle denenebilir.
    public static class StageSceneMap
    {
        public const string Menu = "00_Menu";
        public const string Giris = "01_Giris";
        public const string Lazer = "02_Lazer";
        public const string Cikis = "03_Cikis";
        public const string Sonuc = "04_Sonuc";

        // Tanınmayan bir aşamada null döner.
        public static string SceneFor(GameStage stage)
        {
            switch (stage)
            {
                case GameStage.MainMenu:
                    return Menu;
                case GameStage.Entrance:
                case GameStage.Statue:
                    return Giris;
                case GameStage.LaserRoom:
                case GameStage.Diamond:
                    return Lazer;
                case GameStage.Corridor:
                case GameStage.GuardDialogue:
                    return Cikis;
                case GameStage.Won:
                case GameStage.Lost:
                    return Sonuc;
                default:
                    return null;
            }
        }
    }
}
