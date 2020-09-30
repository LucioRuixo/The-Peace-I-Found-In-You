using System;
using UnityEngine;
using nullbloq.Noodles;
using TMPro;

public class MinigameManager : MonoBehaviour
{
    public enum Minigame
    {
        Puzzle,
        Cooking,
        Fight
    }

    bool minigameWon;

    public GameObject cooking;
    public GameObject cookingUI,puzzleUI;
    public GameObject gameEndMenu;
    public TextMeshProUGUI gameEndText;

    public static event Action<int> OnNodeExecutionCompleted;

    void OnEnable()
    {
        NoodleManager.OnMinigame += Begin;

        CookingMinigameManager.OnGameEnd += EnableGameEndMenu;
        PuzzleController.OnGameEnd += EnableGameEndMenu;
    }

    void OnDisable()
    {
        NoodleManager.OnMinigame -= Begin;

        CookingMinigameManager.OnGameEnd -= EnableGameEndMenu;
        PuzzleController.OnGameEnd -= EnableGameEndMenu;
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

    void EnableGameEndMenu(bool win)
    {
        minigameWon = win;

        gameEndText.text = win ? "You won!" : "You lost!";
        gameEndMenu.SetActive(true);
    }

    public void End()
    {
        cooking.SetActive(false);
        cookingUI.SetActive(false);
        puzzleUI.SetActive(false);
        gameEndMenu.SetActive(false);

        int index = minigameWon ? 0 : 1;
        OnNodeExecutionCompleted(index);
    }
}