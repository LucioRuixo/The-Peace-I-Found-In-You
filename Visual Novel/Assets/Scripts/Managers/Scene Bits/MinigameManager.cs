using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public GameObject minigame;
    public GameObject continueButton;
    SceneBitSO nextBit;

    void OnEnable()
    {
        SceneBitSO.OnMinigame += Begin;
        PuzzleController.OnPuzzleCompletion += EnableContinueButton;
    }

    void OnDisable()
    {
        SceneBitSO.OnMinigame -= Begin;
        PuzzleController.OnPuzzleCompletion -= EnableContinueButton;
    }

    void Begin(SceneBitSO.MinigameData data)
    {
        nextBit = data.nextBit;

        minigame.SetActive(true);
    }

    void EnableContinueButton()
    {
        continueButton.SetActive(true);
    }

    void End()
    {
        minigame.SetActive(false);

        nextBit.Execute();
    }

    public void Continue()
    {
        continueButton.SetActive(false);

        End();
    }
}