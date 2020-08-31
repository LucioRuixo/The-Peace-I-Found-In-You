using System;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public Vector2Int ID { get; set; }

    [HideInInspector] public bool onBoard = false;
    [HideInInspector] public bool fixedToBoard = false;
    bool grabbed = false;

    public Vector2 grabPosition;
    Vector2 mouseOffset;

    public Puzzle.Tile currentTile;
    SpriteRenderer spriteRenderer;

    public static event Action<PuzzlePiece, Vector2> OnPieceReleased;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //if (fixedToBoard) return;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (!grabbed)
        {
            if (spriteRenderer.bounds.Contains(mousePosition) && Input.GetMouseButtonDown(0))
            {
                grabPosition = transform.position;
                mouseOffset = grabPosition - mousePosition;
                grabbed = true;

                currentTile.empty = true;
            }
        }
        else
        {
            transform.position = mousePosition + mouseOffset;

            if (Input.GetMouseButtonUp(0))
            {
                if (OnPieceReleased != null) OnPieceReleased(this, mousePosition);
                grabbed = false;
            }
        }
    }
}