using UnityEngine;

public class SoundManager_MainMenu : MonoBehaviour
{
    SoundManager soundManager;

    void OnEnable()
    {
        UIManager_MainMenu.OnMainScreen += PlayMenuSong;
        UIManager_MainMenu.OnCreditsScreen += PlayCreditsSong;
    }

    void Awake()
    {
        soundManager = SoundManager.Get();
    }

    void PlayMenuSong()
    {
        soundManager.PlaySong(SoundManager.Songs.Inicia_Menu);
    }

    void PlayCreditsSong()
    {
        soundManager.PlaySong(SoundManager.Songs.Inicia_Creditos);
    }
}