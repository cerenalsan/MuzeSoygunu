using UnityEngine;
using UnityEngine.SceneManagement;
using MuzeSoygunu.Shared;

namespace MuzeSoygunu.Llm
{
    // ISceneLoader uygulaması: aşamaya göre sahneyi yükler, aynı sahne açıksa yeniden yüklemez.
    public sealed class SceneLoader : ISceneLoader
    {
        public void LoadSceneFor(GameStage stage)
        {
            string sceneName = StageSceneMap.SceneFor(stage);

            if (sceneName == null)
            {
                Debug.LogWarning("SceneLoader: bu aşama için sahne tanımlı değil: " + stage);
                return;
            }

            if (SceneManager.GetActiveScene().name == sceneName)
            {
                return;
            }

            SceneManager.LoadScene(sceneName);
        }
    }
}
