using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public struct Tile
    {
        public bool empty;
        public Vector2 position;
        public Vector2 halfSize;
        public PuzzlePiece currentPiece;
        public PuzzlePiece correctPiece;

        public bool Contains(Vector2 checkedPosition)
        {
            return (checkedPosition.x > position.x - halfSize.x && checkedPosition.x < position.x + halfSize.x)
                   &&
                   (checkedPosition.y > position.y - halfSize.y && checkedPosition.y < position.y + halfSize.y);
        }
    }

    public int width;
    public int height;
    int pieceAmount;

    public float piecePoolSpacing;
    float piecePoolY;

    Vector2 pieceSize;
    Vector2 pieceHalfSize;
    Vector2 boardHalfSize;

    Tile[,] grid;
    List<PuzzlePiece> piecePool;

    public GameObject piecePrefab;
    public GameObject piecePoolGO;
    public Transform pieceContainer;
    public SpriteRenderer boardSR;
    SpriteRenderer piecePoolSR;

    void OnEnable()
    {
        PuzzlePiece.OnPieceReleased += CheckReleasedPiece;
    }

    void Awake()
    {
        pieceAmount = width * height;

        piecePoolY = piecePoolGO.transform.position.y;

        pieceSize = piecePrefab.GetComponent<SpriteRenderer>().sprite.bounds.size;
        pieceHalfSize = new Vector2(pieceSize.x / 2f, pieceSize.y / 2f);
        boardHalfSize = new Vector2(pieceHalfSize.x * width, pieceHalfSize.y * height);

        grid = new Tile[width, height];
        piecePool = new List<PuzzlePiece>();

        Vector2 boardSize = new Vector2(pieceSize.x * width, pieceSize.y * height);
        boardSR.size = boardSize;
        piecePoolSR = piecePoolGO.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Vector2 initialPosition = new Vector2(transform.position.x - boardHalfSize.x + pieceHalfSize.x, transform.position.y + boardHalfSize.y - pieceHalfSize.y);

        for (int y = 0; y < height; y++)
        {
            float positionY = initialPosition.y - pieceSize.y * y;

            for (int x = 0; x < width; x++)
            {
                grid[x, y].position.x = initialPosition.x + pieceSize.x * x;
                grid[x, y].position.y = positionY;
                grid[x, y].halfSize = pieceHalfSize;
                grid[x, y].empty = true;
            }
        }
    }

    void OnDrawGizmos()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Gizmos.DrawWireCube(grid[x, y].position, new Vector3(pieceSize.x, pieceSize.y, 1));
            }
        }
    }

    void OnDisable()
    {
        PuzzlePiece.OnPieceReleased -= CheckReleasedPiece;
    }

    void ArrangePiecePool()
    {
        float pieceX = -((pieceSize.x * piecePool.Count + piecePoolSpacing * (piecePool.Count - 1)) / 2f) + pieceHalfSize.x;
        Vector2 piecePosition = new Vector2(pieceX, piecePoolY);

        for (int i = 0; i < piecePool.Count; i++)
        {
            piecePool[i].transform.position = piecePosition;

            piecePosition.x += pieceSize.x + piecePoolSpacing;
        }
    }

    void CheckReleasedPiece(PuzzlePiece piece, Vector2 mousePosition)
    {
        Debug.Log(piece.currentTile.empty);

        bool insideBoard = (mousePosition.x > transform.position.x - boardHalfSize.x && mousePosition.x < transform.position.x + boardHalfSize.x)
                           &&
                           (mousePosition.y > transform.position.y - boardHalfSize.y && mousePosition.y < transform.position.y + boardHalfSize.y);

        if (insideBoard)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (grid[x, y].Contains(mousePosition) && grid[x, y].empty)
                    {
                        piece.transform.position = grid[x, y].position;
                        piece.onBoard = true;
                        piece.currentTile = grid[x, y];
                        if (grid[x, y].correctPiece == piece) piece.fixedToBoard = true;

                        grid[x, y].currentPiece = piece;
                        grid[x, y].empty = false;

                        piecePool.Remove(piece);
                        ArrangePiecePool();

                        return;
                    }
                }
            }
        }
        else if (piecePoolSR.bounds.Contains(mousePosition) && !piecePool.Contains(piece))
        {
            piece.onBoard = false;
            piece.currentTile.empty = true;

            piecePool.Add(piece);
            ArrangePiecePool();

            return;
        }

        piece.transform.position = piece.grabPosition;
    }

    //void FitPieceOnTile(PuzzlePiece piece, Tile tile)
    //{
    //    int tileX = -1;
    //    int tileY = -1;
    //    
    //    Vector2 piecePosition = piece.transform.position;
    //    Vector2 nearestTilePosition = Vector2.zero;
    //    
    //    float distance = Mathf.Abs(grid[0, 0].position.x - piecePosition.x);
    //    float nextDistance = 0f;
    //    for (int x = 0; x < width; x++)
    //    {
    //        if (x + 1 < width)
    //        {
    //            nextDistance = Mathf.Abs(grid[x + 1, 0].position.x - piecePosition.x);
    //    
    //            if (distance < nextDistance)
    //            {
    //                nearestTilePosition.x = grid[x, 0].position.x;
    //                tileX = x;
    //                break;
    //            }
    //        }
    //        else
    //        {
    //            nearestTilePosition.x = grid[x, 0].position.x;
    //            tileX = x;
    //            break;
    //        }
    //    
    //        distance = nextDistance;
    //    }
    //    
    //    distance = Mathf.Abs(grid[0, 0].position.y - piecePosition.y);
    //    nextDistance = 0f;
    //    for (int y = 0; y < height; y++)
    //    {
    //        if (y + 1 < width)
    //        {
    //            nextDistance = Mathf.Abs(grid[0, y + 1].position.y - piecePosition.y);
    //    
    //            if (distance < nextDistance)
    //            {
    //                nearestTilePosition.y = grid[0, y].position.y;
    //                tileY = y;
    //                break;
    //            }
    //        }
    //        else
    //        {
    //            nearestTilePosition.y = grid[0, y].position.y;
    //            tileY = y;
    //            break;
    //        }
    //    
    //        distance = nextDistance;
    //    }
    //    
    //    if (tileX > -1 && tileY > -1)
    //    {
    //        grid[tileX, tileY].empty = false;
    //    
    //        piece.transform.position = nearestTilePosition;
    //        piece.onBoard = true;
    //        piece.currentTile = grid[tileX, tileY];
    //    
    //        if (grid[tileX, tileY].correctPiece == piece) piece.fixedToBoard = true;
    //    }
    //}

    public void GeneratePieces()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                PuzzlePiece newPiece = Instantiate(piecePrefab, pieceContainer).GetComponent<PuzzlePiece>();
                newPiece.ID = new Vector2Int(x, y);
                piecePool.Add(newPiece);
            }
        }

        ArrangePiecePool();
    }
}