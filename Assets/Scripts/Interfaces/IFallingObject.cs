using UnityEngine;

interface IFallingObject
{
    void Initialize(Vector2 position, Quaternion rotation);

    void SetForce(Vector2 force, ForceMode2D mode);

    void FallOffScreen();
}