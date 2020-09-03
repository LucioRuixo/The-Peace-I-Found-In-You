using System.Collections.Generic;
using UnityEngine;

public class PuzzleView : MonoBehaviour
{
    PuzzleModel model;
    PuzzleController controller;

    public float piecePoolSpacing;

    public Vector2 PieceSize { private set; get; }
    public Vector2 PieceHalfSize { private set; get; }
    public Vector2 BoardHalfSize { private set; get; }

    public GameObject piecePool;
    public RectTransform boardRect;

    void Awake()
    {
        model = GetComponent<PuzzleModel>();
        controller = GetComponent<PuzzleController>();

        PieceSize = controller.piecePrefab.GetComponent<RectTransform>().sizeDelta;
        PieceHalfSize = new Vector2(PieceSize.x / 2f, PieceSize.y / 2f);
        BoardHalfSize = new Vector2(PieceHalfSize.x * model.width, PieceHalfSize.y * model.height);

        float boardSizeWidth = PieceSize.x * model.width;
        float boardSizeHeight = PieceSize.y * model.height;
        boardRect.sizeDelta = new Vector2(boardSizeWidth, boardSizeHeight);
    }

    //public void ArrangePiecePool(ref List<PuzzlePiece> pool)
    //{
    //    float pieceX = -((PieceSize.x * pool.Count + piecePoolSpacing * (pool.Count - 1)) / 2f) + PieceHalfSize.x;
    //    Vector2 piecePosition = new Vector2(pieceX, piecePoolY);
    //
    //    for (int i = 0; i < pool.Count; i++)
    //    {
    //        pool[i].transform.position = piecePosition;
    //
    //        piecePosition.x += PieceSize.x + piecePoolSpacing;
    //    }
    //}
}