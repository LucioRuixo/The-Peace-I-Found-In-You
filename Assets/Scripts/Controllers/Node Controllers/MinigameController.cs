using System;
using UnityEngine;
using TMPro;
using nullbloq.Noodles;

public class MinigameController : NodeController
{
    public enum Minigame
    {
        Puzzle,
        Cooking,
        Fight
    }

    public override Type NodeType { protected set; get; }

    bool minigameWon;

    public GameObject cooking;
    public GameObject cookingUI,puzzleUI;
    public GameObject gameEndMenu;
    public TextMeshProUGUI gameEndText;

    //public static event Action<int> OnNodeExecutionCompleted;

    void Awake()
    {
        NodeType = typeof(CustomMinigameNode);
    }

    void OnEnable()
    {
        //NodeManager.OnMinigame += Begin;

        CookingMinigameManager.OnGameEnd += EnableGameEndMenu;
        PuzzleController.OnGameEnd += EnableGameEndMenu;
    }

    void OnDisable()
    {
        //NodeManager.OnMinigame -= Begin;

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
        CallNodeExecutionCompletion(index);
    }

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomMinigameNode;

        Begin(node);
    }
}