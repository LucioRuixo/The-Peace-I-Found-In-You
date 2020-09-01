using System.Collections.Generic;
using UnityEngine;

public class PuzzleModel : MonoBehaviour
{
    public int PieceAmount { private set; get; }

    public int width;
    public int height;

    public List<Sprite> pieceSprites;

    void Awake()
    {
        PieceAmount = width * height;
    }
}