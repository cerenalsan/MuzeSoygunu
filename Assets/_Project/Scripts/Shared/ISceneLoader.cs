namespace MuzeSoygunu.Shared
{
    // Oyun aşamasına karşılık gelen sahneyi yükler.
    // Hangi aşamanın hangi sahneye denk geldiğini yalnızca uygulayan sınıf bilir.
    public interface ISceneLoader
    {
        void LoadSceneFor(GameStage stage);
    }
}
