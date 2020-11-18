using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviourSingleton<SceneLoadManager>
{
    [SerializeField] string mainMenuSceneName = "";
    [SerializeField] string gameplaySceneName = "";

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void LoadGameplay()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }
}