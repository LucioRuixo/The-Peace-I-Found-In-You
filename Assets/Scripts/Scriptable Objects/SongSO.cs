using UnityEngine;

[CreateAssetMenu(fileName = "New Song", menuName = "Song")]
public class SongSO : ScriptableObject
{
    public MusicController.SongTitle title;

    public AudioClip clip;
}