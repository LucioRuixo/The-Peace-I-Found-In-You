using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager_MainMenu : MonoBehaviour
{
    public GameObject mainMenuFirstSelected, creditsScreenFirstSelected;
    public GameObject mainMenu, creditsScreen;

    public void Play()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void GoToCreditsScreen()
    {
        mainMenu.SetActive(false);
        creditsScreen.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(creditsScreenFirstSelected);
    }

    public void Return()
    {
        creditsScreen.SetActive(false);
        mainMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainMenuFirstSelected);
    }

    public void Quit()
    {
        Application.Quit();
    }
}