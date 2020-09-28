using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    void OnEnable()
    {
        NoodleManager.OnGameFinished += GoToMainMenu;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            Application.Quit();
    }

    void OnDisable()
    {
        NoodleManager.OnGameFinished -= GoToMainMenu;
    }

    void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}