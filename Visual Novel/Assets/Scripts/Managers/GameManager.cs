using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            Application.Quit();
    }
}