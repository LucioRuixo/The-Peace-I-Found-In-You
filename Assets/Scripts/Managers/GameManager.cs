using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    void OnEnable()
    {
        NoodleManager.OnNoNoodlesRemaining += GoToMainMenu;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            Application.Quit();
    }

    void OnDisable()
    {
        NoodleManager.OnNoNoodlesRemaining -= GoToMainMenu;
    }

    void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}