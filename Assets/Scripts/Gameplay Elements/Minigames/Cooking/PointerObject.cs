using UnityEngine;

public class PointerObject : MonoBehaviour
{
    void Update()
    {
        Vector2 position = Vector2.zero;
        position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = position;
    }
}