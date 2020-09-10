using UnityEngine;

public class PuzzleView : MonoBehaviour
{
    PuzzleModel model;
    PuzzleController controller;

    public float piecePoolSpacing;

    public Vector2 PieceSize { private set; get; }
    public Vector2 PieceHalfSize { private set; get; }
    public Vector2 BoardSize { private set; get; }
    public Vector2 BoardHalfSize { private set; get; }

    public GameObject piecePool;
    public RectTransform boardRect;

    void Awake()
    {
        model = GetComponent<PuzzleModel>();
        controller = GetComponent<PuzzleController>();

        PieceSize = controller.piecePrefab.GetComponent<RectTransform>().rect.size;
        PieceHalfSize = new Vector2(PieceSize.x / 2f, PieceSize.y / 2f);
        BoardSize = new Vector2(PieceSize.x * model.width, PieceSize.y * model.height);
        BoardHalfSize = new Vector2(BoardSize.x / 2f, BoardSize.y / 2f);

        float boardSizeWidth = PieceSize.x * model.width;
        float boardSizeHeight = PieceSize.y * model.height;
        boardRect.sizeDelta = new Vector2(boardSizeWidth, boardSizeHeight);
    }
}