using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_Gameplay : MonoBehaviour
{
    [SerializeField] GameObject log = null;

    public void Log()
    {
        log.SetActive(true);
    }

    public void Save()
    {
        SaveManager.Get().SaveFile();
    }

    public void Exit()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void CloseLog()
    {
        log.SetActive(false);
    }
}