using UnityEngine;

public class ScreenManager : MonoBehaviourSingleton<ScreenManager>
{
    public Vector2 ScreenBounds { private set; get; }
    public Vector2 MinScreenLimits { private set; get; } // Izquierda y abajo
    public Vector2 MaxScreenLimits { private set; get; } // Derecha y arriba

    public override void Awake()
    {
        base.Awake();

        Vector3 position = new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z);
        ScreenBounds = Camera.main.ScreenToWorldPoint(position);
        SetScreenLimits();
    }

    void SetScreenLimits()
    {
        MinScreenLimits = new Vector2(ScreenBounds.x * -1f, ScreenBounds.y);
        MaxScreenLimits = new Vector2(ScreenBounds.x, ScreenBounds.y - -1f);
    }
}