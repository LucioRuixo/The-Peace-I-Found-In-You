using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleController : MonoBehaviour
{
    PuzzleModel model;
    PuzzleView view;

    public struct Tile
    {
        public bool empty;
        public Vector2Int correctPieceID;
        public Vector2 position;
        public Vector2 halfSize;
        public PuzzlePiece currentPiece;

        public bool Contains(Vector2 checkedPosition)
        {
            return (checkedPosition.x > position.x - halfSize.x && checkedPosition.x < position.x + halfSize.x)
                   &&
                   (checkedPosition.y > position.y - halfSize.y && checkedPosition.y < position.y + halfSize.y);
        }
    }

    int fixedPieces = 0;

    Tile[,] grid;
    List<PuzzlePiece> piecePool;
    List<PuzzlePiece> holdSpace;

    public GameObject piecePrefab;
    public RectTransform canvas;
    public RectTransform boardRect;
    public RectTransform piecePoolRect;
    public RectTransform piecePoolContent;
    public RectTransform holdSpaceRect;
    public RectTransform holdSpaceContent;

    public static event Action OnPuzzleCompletion;

    void Awake()
    {
        model = GetComponent<PuzzleModel>();
        view = GetComponent<PuzzleView>();

        grid = new Tile[model.width, model.height];
        piecePool = new List<PuzzlePiece>();
        holdSpace = new List<PuzzlePiece>();

        float initialPositionX = view.PieceHalfSize.x;
        float initialPositionY = -view.PieceHalfSize.y;

        for (int y = 0; y < model.height; y++)
        {
            float positionY = initialPositionY - view.PieceSize.y * y;

            for (int x = 0; x < model.width; x++)
            {
                grid[x, y].empty = true;

                grid[x, y].position.x = initialPositionX + view.PieceSize.x * x;
                grid[x, y].position.y = positionY;
                grid[x, y].halfSize = view.PieceHalfSize;

                grid[x, y].currentPiece = null;
            }
        }
    }

    void OnEnable()
    {
        PuzzlePiece.OnDragBeginning += SetBoardAsPieceParent;
        PuzzlePiece.OnDragEnd += CheckReleasedPiece;

        GeneratePieces();
    }

    void OnDisable()
    {
        ClearPieces();

        PuzzlePiece.OnDragBeginning -= SetBoardAsPieceParent;
        PuzzlePiece.OnDragEnd -= CheckReleasedPiece;
    }

    void GeneratePieces()
    {
        int pieceNumber = 0;

        for (int y = 0; y < model.height; y++)
        {
            for (int x = 0; x < model.width; x++)
            {
                PuzzlePiece newPiece = Instantiate(piecePrefab, canvas).GetComponent<PuzzlePiece>();
                newPiece.ID = new Vector2Int(x, y);
                newPiece.GetComponent<Image>().sprite = model.pieceSprites[pieceNumber];

                grid[x, y].correctPieceID = newPiece.ID;
                piecePool.Add(newPiece);

                pieceNumber++;
            }
        }

        SufflePiecePool();

        foreach (PuzzlePiece piece in piecePool)
        {
            piece.transform.SetParent(piecePoolContent);
        }
    }

    void ClearPieces()
    {
        for (int y = 0; y < model.height; y++)
        {
            for (int x = 0; x < model.width; x++)
            {
                if (grid[x, y].currentPiece)
                {
                    Destroy(grid[x, y].currentPiece.gameObject);
                    grid[x, y].currentPiece = null;
                }
            }
        }

        foreach (PuzzlePiece piece in piecePool)
        {
            Destroy(piece.gameObject);
        }
        piecePool.Clear();

        foreach (PuzzlePiece piece in holdSpace)
        {
            Destroy(piece.gameObject);
        }
        holdSpace.Clear();
    }

    void SufflePiecePool()
    {
        for (int i = 0; i < piecePool.Count; i++)
        {
            PuzzlePiece auxiliar = piecePool[i];
            int randomIndex = UnityEngine.Random.Range(i, piecePool.Count);
            piecePool[i] = piecePool[randomIndex];
            piecePool[randomIndex] = auxiliar;
        }
    }

    void SetBoardAsPieceParent(Transform piece)
    {
        piece.SetParent(boardRect);
    }

    void CheckReleasedPiece(PuzzlePiece piece, Vector2 pointerPosition)
    {
        Vector2 piecePosition = piece.rect.anchoredPosition;
        bool insideBoard = ((piecePosition.x > 0f) && (piecePosition.x < view.BoardSize.x))
                           &&
                           ((piecePosition.y < 0f) && (piecePosition.y > -view.BoardSize.y));
        
        if (insideBoard)
        {
            for (int y = 0; y < model.height; y++)
            {
                for (int x = 0; x < model.width; x++)
                {
                    if (grid[x, y].Contains(piecePosition) && grid[x, y].empty)
                    {
                        piece.GetComponent<RectTransform>().anchoredPosition = grid[x, y].position;
                        piece.OnBoard = true;
                        grid[piece.CurrentTile.x, piece.CurrentTile.y].empty = true;
                        piece.CurrentTile = new Vector2Int(x, y);

                        if (grid[x, y].correctPieceID == piece.ID)
                        {
                            piece.FixedToBoard = true;
                            fixedPieces++;
                            if (fixedPieces == model.TotalPieces && OnPuzzleCompletion != null)
                                OnPuzzleCompletion();

                            StartCoroutine(piece.Highlight());
                        }

                        grid[x, y].currentPiece = piece;
                        grid[x, y].empty = false;

                        piecePool.Remove(piece);

                        return;
                    }
                }
            }
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(piecePoolRect, piece.rect.position))
        {
            piece.transform.SetParent(piecePoolContent);
            piece.OnBoard = false;
            grid[piece.CurrentTile.x, piece.CurrentTile.y].empty = true;

            piecePool.Add(piece);

            return;
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(holdSpaceRect, piece.rect.position))
        {
            piece.transform.SetParent(holdSpaceContent);
            piece.OnBoard = false;
            grid[piece.CurrentTile.x, piece.CurrentTile.y].empty = true;

            holdSpace.Add(piece);

            return;
        }

        if (piece.OnBoard)
            piece.rect.anchoredPosition = grid[piece.CurrentTile.x, piece.CurrentTile.y].position;
        else
            piece.transform.SetParent(piecePoolContent);
    }

    public List<PuzzlePiece> GetPiecePool()
    {
        return piecePool;
    }
}