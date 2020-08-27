using System;
using UnityEngine;

public class PuzzlePiece : Puzzle
{
    public bool onBoard = false;
    public bool fixedToBoard = false;
    bool grabbed = false;

    Vector2 mouseOffset;

    public Tile currentTile;
    SpriteRenderer spriteRenderer;

    public static event Action<PuzzlePiece> OnPieceReleased;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (!grabbed)
        {
            if (spriteRenderer.bounds.Contains(mousePosition) && Input.GetMouseButtonDown(0))
            {
                mouseOffset = (Vector2)transform.position - mousePosition;
                grabbed = true;

                currentTile.empty = true;
            }
        }
        else
        {
            transform.position = mousePosition + mouseOffset;

            if (Input.GetMouseButtonUp(0))
            {
                if (OnPieceReleased != null) OnPieceReleased(this);
                grabbed = false;
            }
        }
    }
}
