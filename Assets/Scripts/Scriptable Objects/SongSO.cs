using UnityEngine;

[CreateAssetMenu(fileName = "New Song", menuName = "Song")]
public class SongSO : ScriptableObject
{
    public MusicManager.SongTitle title;

    public AudioClip clip;
}