using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public GameObject puzzleGO;
    public GameObject minigameUI;
    PuzzleController puzzle;
    SceneBitSO nextBit;

    void OnEnable()
    {
        SceneBitSO.OnMinigame += Begin;
    }

    void Awake()
    {
        puzzle = puzzleGO.GetComponent<PuzzleController>();
    }

    void OnDisable()
    {
        SceneBitSO.OnMinigame -= Begin;
    }

    void Begin(SceneBitSO.MinigameData data)
    {
        nextBit = data.nextBit;

        puzzleGO.SetActive(true);
        minigameUI.SetActive(true);

        puzzle.GeneratePieces();
    }

    void End()
    {
        puzzle.ClearPieces();

        minigameUI.SetActive(false);
        puzzleGO.SetActive(false);

        nextBit.Execute();
    }

    public void Continue()
    {
        End();
    }
}