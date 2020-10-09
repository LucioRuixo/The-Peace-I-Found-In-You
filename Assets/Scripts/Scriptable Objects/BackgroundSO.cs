using UnityEngine;

[CreateAssetMenu(fileName = "New Background", menuName = "Background")]
public class BackgroundSO : ScriptableObject
{
    public BackgroundController.Location location;
    public BackgroundController.Ilustration ilustration;

    public Sprite sprite;
}