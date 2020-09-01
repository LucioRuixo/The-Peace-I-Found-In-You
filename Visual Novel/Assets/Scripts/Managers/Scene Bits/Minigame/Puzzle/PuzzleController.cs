using System.Collections.Generic;
using UnityEngine;

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

    Tile[,] grid;
    List<PuzzlePiece> piecePool;

    public GameObject piecePrefab;
    public Transform board;
    public Transform pieceContainer;

    void OnEnable()
    {
        PuzzlePiece.OnPieceReleased += CheckReleasedPiece;
    }

    void Awake()
    {
        model = GetComponent<PuzzleModel>();
        view = GetComponent<PuzzleView>();

        grid = new Tile[model.width, model.height];
        piecePool = new List<PuzzlePiece>();
    }

    void Start()
    {
        float initialPositionX = -view.BoardHalfSize.x + view.PieceHalfSize.x;
        float initialPositionY = board.position.y + view.BoardHalfSize.y - view.PieceHalfSize.y;

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

    void OnDisable()
    {
        PuzzlePiece.OnPieceReleased -= CheckReleasedPiece;
    }

    void SufflePiecePool()
    {
        for (int i = 0; i < piecePool.Count; i++)
        {
            PuzzlePiece auxiliar = piecePool[i];
            int randomIndex = Random.Range(i, piecePool.Count);
            piecePool[i] = piecePool[randomIndex];
            piecePool[randomIndex] = auxiliar;
        }
    }

    void CheckReleasedPiece(PuzzlePiece piece, Vector2 mousePosition)
    {
        bool insideBoard = ((mousePosition.x > -view.BoardHalfSize.x) && (mousePosition.x < view.BoardHalfSize.x))
                           &&
                           ((mousePosition.y > board.position.y - view.BoardHalfSize.y) && (mousePosition.y < board.position.y + view.BoardHalfSize.y));
        
        if (insideBoard)
        {
            for (int y = 0; y < model.height; y++)
            {
                for (int x = 0; x < model.width; x++)
                {
                    if (grid[x, y].Contains(mousePosition) && grid[x, y].empty)
                    {
                        piece.transform.position = grid[x, y].position;
                        piece.OnBoard = true;
                        grid[piece.CurrentTile.x, piece.CurrentTile.y].empty = true;
                        piece.CurrentTile = new Vector2Int(x, y);
                        if (grid[x, y].correctPieceID == piece.ID)
                        {
                            piece.FixedToBoard = true;
                            StartCoroutine(piece.Highlight());
                        }

                        grid[x, y].currentPiece = piece;
                        grid[x, y].empty = false;

                        piecePool.Remove(piece);
                        view.ArrangePiecePool(ref piecePool);

                        return;
                    }
                }
            }
        }
        else if (view.piecePoolSR.bounds.Contains(mousePosition) && !piecePool.Contains(piece))
        {
            piece.OnBoard = false;
            grid[piece.CurrentTile.x, piece.CurrentTile.y].empty = true;

            piecePool.Add(piece);
            view.ArrangePiecePool(ref piecePool);

            return;
        }

        piece.transform.position = piece.GrabPosition;
    }

    public void GeneratePieces()
    {
        int pieceNumber = 0;

        for (int y = 0; y < model.height; y++)
        {
            for (int x = 0; x < model.width; x++)
            {
                PuzzlePiece newPiece = Instantiate(piecePrefab, pieceContainer).GetComponent<PuzzlePiece>();
                newPiece.ID = new Vector2Int(x, y);
                newPiece.GetComponent<SpriteRenderer>().sprite = model.pieceSprites[pieceNumber];

                grid[x, y].correctPieceID = newPiece.ID;
                piecePool.Add(newPiece);

                pieceNumber++;
            }
        }

        SufflePiecePool();
        view.ArrangePiecePool(ref piecePool);
    }

    public void ClearPieces()
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
    }

    public List<PuzzlePiece> GetPiecePool()
    {
        return piecePool;
    }
}