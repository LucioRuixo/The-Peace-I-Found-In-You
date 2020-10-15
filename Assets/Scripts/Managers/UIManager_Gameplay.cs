using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_Gameplay : MonoBehaviour
{
    public void Log()
    {
        //por ahora nada che
    }

    public void Save()
    {
        SaveManager.Get().SaveFile();
    }

    public void Exit()
    {
        SceneManager.LoadScene("Main Menu");
    }
}