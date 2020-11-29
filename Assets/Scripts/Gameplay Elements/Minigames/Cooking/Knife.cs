using UnityEngine;

public class Knife : MonoBehaviour
{
    int collidingFoodAmount = 0;

    bool colliding;

    SoundManager soundManager;

    void Awake()
    {
        soundManager = SoundManager.Get();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            collidingFoodAmount++;
            colliding = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            collidingFoodAmount--;
            colliding = collidingFoodAmount > 0;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Left Click"))
        {
            if (colliding) soundManager.PlaySFX(SoundManager.SFXs.Fx_CorteVerdura);
            else soundManager.PlaySFX(SoundManager.SFXs.Fx_ErrarCorte);
        }
    }
}