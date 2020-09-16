using UnityEngine;

[CreateAssetMenu(fileName = "New Background", menuName = "Background")]
public class BackgroundSO : ScriptableObject
{
    public BackgroundManager.Background background;

    public Sprite sprite;
}