using UnityEngine;

public class Food : MonoBehaviour
{
    public float speed;

    Vector3 movement;

    void Awake()
    {
        movement = Vector3.zero;
        movement.y = speed;
    }

    void Update()
    {
        transform.localPosition += movement * Time.deltaTime;
    }
}