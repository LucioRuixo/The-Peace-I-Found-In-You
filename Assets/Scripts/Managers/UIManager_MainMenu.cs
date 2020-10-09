using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager_MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenuFirstSelected = null, creditsScreenFirstSelected = null;
    [SerializeField] GameObject mainMenu = null, creditsScreen = null;
    [SerializeField] TextMeshProUGUI versionText = null;

    void Start()
    {
        versionText.text = "v" + Application.version;
    }

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