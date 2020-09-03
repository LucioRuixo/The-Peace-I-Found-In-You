using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public GameObject minigame;
    SceneBitSO nextBit;

    void OnEnable()
    {
        SceneBitSO.OnMinigame += Begin;
    }

    void OnDisable()
    {
        SceneBitSO.OnMinigame -= Begin;
    }

    void Begin(SceneBitSO.MinigameData data)
    {
        nextBit = data.nextBit;

        minigame.SetActive(true);
    }

    void End()
    {
        minigame.SetActive(false);

        nextBit.Execute();
    }

    public void Continue()
    {
        End();
    }
}