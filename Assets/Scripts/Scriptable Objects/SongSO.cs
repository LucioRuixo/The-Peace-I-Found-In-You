using UnityEngine;

[CreateAssetMenu(fileName = "New Song", menuName = "Song")]
public class SongSO : ScriptableObject
{
    public SoundManager.Songs title;

    public AudioClip clip;
}