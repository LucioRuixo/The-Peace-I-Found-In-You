using UnityEngine;

[CreateAssetMenu(fileName = "New Background", menuName = "Background")]
public class BackgroundSO : ScriptableObject
{
    public BackgroundManager.Location location;
    public BackgroundManager.Ilustration ilustration;

    public Sprite sprite;
}