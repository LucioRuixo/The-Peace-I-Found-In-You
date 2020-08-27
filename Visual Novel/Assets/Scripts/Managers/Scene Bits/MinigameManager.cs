using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public Puzzle puzzle;
    public GameObject minigameUI;
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

        minigameUI.SetActive(true);
    }

    void End()
    {
        minigameUI.SetActive(false);

        nextBit.Execute();
    }

    public void PlayMinigame()
    {
        End();
    }
}