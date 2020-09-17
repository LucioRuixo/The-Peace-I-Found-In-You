using UnityEngine;

public class KnifeCollider : MonoBehaviour
{
    bool collisioning = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food") collisioning = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Food") collisioning = false;
    }

    void Update()
    {
        if (collisioning && Input.GetButtonDown("Left Click")) Debug.Log("Collision");
    }
}