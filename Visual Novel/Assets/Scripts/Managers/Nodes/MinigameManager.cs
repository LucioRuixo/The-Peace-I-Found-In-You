using System;
using UnityEngine;
using nullbloq.Noodles;

public class MinigameManager : MonoBehaviour
{
    public enum Minigame
    {
        Puzzle,
        Cooking,
        Fight
    }

    public GameObject cooking;
    public GameObject cookingUI,puzzleUI;
    public GameObject continueButton;

    public static event Action<int> OnNodeExecutionCompleted;

    void OnEnable()
    {
        NoodleManager.OnMinigame += Begin;
        PuzzleController.OnPuzzleCompletion += EnableContinueButton;
    }

    void OnDisable()
    {
        NoodleManager.OnMinigame -= Begin;
        PuzzleController.OnPuzzleCompletion -= EnableContinueButton;
    }

    void Begin(CustomMinigameNode node)
    {
        switch (node.minigame)
        {
            case Minigame.Puzzle:
                puzzleUI.SetActive(true);
                break;
            case Minigame.Cooking:
                cooking.SetActive(true);
                cookingUI.SetActive(true);
                break;
            case Minigame.Fight:
                break;
            default:
                break;
        }

    }

    void EnableContinueButton()
    {
        continueButton.SetActive(true);
    }

    void End()
    {
        cooking.SetActive(false);
        cookingUI.SetActive(false);
        puzzleUI.SetActive(false);

        OnNodeExecutionCompleted(0);
    }

    public void Continue()
    {
        continueButton.SetActive(false);
        End();
    }
}