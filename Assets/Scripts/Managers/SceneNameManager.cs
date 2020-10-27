using UnityEngine;

public class SceneNameManager : MonoBehaviourSingleton<SceneNameManager>
{
    [SerializeField] string mainMenuSceneName;
    public string MainMenu { get { return mainMenuSceneName; } }

    [SerializeField] string gameplaySceneName;
    public string Gameplay { get { return gameplaySceneName; } }
}