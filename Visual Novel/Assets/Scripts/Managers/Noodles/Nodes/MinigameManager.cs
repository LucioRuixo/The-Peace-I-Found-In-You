using System;
using UnityEngine;
using nullbloq.Noodles;

public class MinigameManager : MonoBehaviour
{
    public GameObject minigame;
    public GameObject continueButton;
    SceneBitSO nextBit;

    public static event Action<string> OnNodeExecutionCompleted;

    void OnEnable()
    {
        NodeManager.OnMinigame += Begin;
        PuzzleController.OnPuzzleCompletion += EnableContinueButton;
    }

    void OnDisable()
    {
        NodeManager.OnMinigame -= Begin;
        PuzzleController.OnPuzzleCompletion -= EnableContinueButton;
    }

    void Begin(CustomMinigameNode node)
    {
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