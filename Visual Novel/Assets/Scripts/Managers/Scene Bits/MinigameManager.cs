using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public GameObject puzzle;
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

        puzzle.SetActive(true);
        minigameUI.SetActive(true);

        puzzle.GetComponent<Puzzle>().GeneratePieces();
    }

    void End()
    {
        minigameUI.SetActive(false);
        puzzle.SetActive(false);

        nextBit.Execute();
    }

    public void PlayMinigame()
    {
        End();
    }
}