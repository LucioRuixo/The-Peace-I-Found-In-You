using System;
using Unity.Mathematics;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public struct Tile
    {
        public bool empty;

        public Vector2 position;

        public PuzzlePiece correctPiece;
    }

    protected Vector2Int PieceID { get; set; }

    public int width;
    public int height;

    Vector2 pieceSize;

    public GameObject[,] pieces;
    Tile[,] grid;

    public GameObject piecePrefab;

    void OnEnable()
    {
        PuzzlePiece.OnPieceReleased += FitPieceOnBoard;
    }

    void Start()
    {
        pieceSize = piecePrefab.GetComponent<SpriteRenderer>().sprite.bounds.size;
        //pieces = new Vector2[width * height];
        grid = new Tile[width, height];
        float boardWidth = width * pieceSize.x;
        float boardHeight = height * pieceSize.y;

        Debug.Log(grid == null);

        Vector2 initialPosition = new Vector2(transform.position.x - boardWidth / 2f + pieceSize.x / 2f, transform.position.y + boardHeight / 2f - pieceSize.y / 2f);

        for (int y = 0; y < height; y++)
        {
            float positionY = initialPosition.y - pieceSize.y * y;

            for (int x = 0; x < width; x++)
            {
                grid[x, y].position.x = initialPosition.x + pieceSize.x * x;
                grid[x, y].position.y = positionY;
                grid[x, y].empty = true;
            }
        }
    }

    void OnDrawGizmos()
    {
        //for (int y = 0; y < height; y++)
        //{
        //    for (int x = 0; x < width; x++)
        //    {
        //        Gizmos.DrawWireCube(grid[x, y].position, new Vector3(pieceSize.x, pieceSize.y, 1));
        //    }
        //}
    }

    void OnDisable()
    {
        PuzzlePiece.OnPieceReleased -= FitPieceOnBoard;
    }

    protected void FitPieceOnBoard(PuzzlePiece piece)
    {
        int tileX = -1;
        int tileY = -1;

        Vector2 piecePosition = piece.transform.position;
        Vector2 nearestTilePosition = Vector2.zero;

        Debug.Log(grid == null);
        float distance = Mathf.Abs(grid[0, 0].position.x - piecePosition.x);
        float nextDistance = 0f;
        for (int x = 0; x < width; x++)
        {
            if (x + 1 < width)
            {
                nextDistance = Mathf.Abs(grid[x + 1, 0].position.x - piecePosition.x);

                if (distance < nextDistance)
                {
                    nearestTilePosition.x = grid[x, 0].position.x;
                    tileX = x;
                    break;
                }
            }
            else
            {
                nearestTilePosition.x = grid[x, 0].position.x;
                tileX = x;
                break;
            }

            distance = nextDistance;
        }

        distance = Mathf.Abs(grid[0, 0].position.y - piecePosition.y);
        nextDistance = 0f;
        for (int y = 0; y < height; y++)
        {
            if (y + 1 < width)
            {
                nextDistance = Mathf.Abs(grid[0, y + 1].position.y - piecePosition.y);

                if (distance < nextDistance)
                {
                    nearestTilePosition.y = grid[0, y].position.y;
                    tileY = y;
                    break;
                }
            }
            else
            {
                nearestTilePosition.y = grid[0, y].position.y;
                tileY = y;
                break;
            }

            distance = nextDistance;
        }

        if (tileX > -1 && tileY > -1)
        {
            grid[tileX, tileY].empty = false;

            piece.transform.position = nearestTilePosition;
            piece.onBoard = true;
            piece.currentTile = grid[tileX, tileY];

            if (grid[tileX, tileY].correctPiece == piece) piece.fixedToBoard = true;
        }
    }

    public void GeneratePieces()
    {
        for (int x = 0; x < 2; x++)
        {
            PuzzlePiece newPiece = Instantiate(piecePrefab, new Vector3(x, x, 0f), quaternion.identity, transform).GetComponent<PuzzlePiece>();
            newPiece.PieceID = new Vector2Int(x, 0);
        }
    }
}