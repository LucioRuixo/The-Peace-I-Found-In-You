using System.Collections.Generic;
using UnityEngine;

public class PuzzleView : MonoBehaviour
{
    PuzzleModel model;

    public float piecePoolSpacing;
    float piecePoolY;

    public Vector2 PieceSize { private set; get; }
    public Vector2 PieceHalfSize { private set; get; }
    public Vector2 BoardHalfSize { private set; get; }

    public GameObject piecePool;
    public SpriteRenderer boardSR;
    [HideInInspector] public SpriteRenderer piecePoolSR;

    void Awake()
    {
        model = GetComponent<PuzzleModel>();

        piecePoolY = piecePool.transform.position.y;

        PieceSize = model.pieceSprites[0].bounds.size;
        PieceHalfSize = new Vector2(PieceSize.x / 2f, PieceSize.y / 2f);
        Vector2 boardSize = new Vector2(PieceSize.x * model.width, PieceSize.y * model.height);
        BoardHalfSize = new Vector2(PieceHalfSize.x * model.width, PieceHalfSize.y * model.height);

        boardSR.size = boardSize;
        piecePoolSR = piecePool.GetComponent<SpriteRenderer>();
    }

    public void ArrangePiecePool(ref List<PuzzlePiece> pool)
    {
        float pieceX = -((PieceSize.x * pool.Count + piecePoolSpacing * (pool.Count - 1)) / 2f) + PieceHalfSize.x;
        Vector2 piecePosition = new Vector2(pieceX, piecePoolY);

        for (int i = 0; i < pool.Count; i++)
        {
            pool[i].transform.position = piecePosition;

            piecePosition.x += PieceSize.x + piecePoolSpacing;
        }
    }
}