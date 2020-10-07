using System;
using System.Collections.Generic;
using UnityEngine;
using nullbloq.Noodles;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public enum SongTitle
    {
        Calm,
        Forest,
        GoodMemories,
        HoshiAndI,
        InShadows,
        Sachi,
        SadPast,
        SeijunHome,
        Tadao,
        Town
    }

    [SerializeField] float fadeDuration = 3f;

    [SerializeField] AudioSource channel1 = null;
    [SerializeField] AudioSource channel2 = null;

    [SerializeField] List<SongSO> songs = null;
    Dictionary<SongTitle, AudioClip> songDictionary = new Dictionary<SongTitle, AudioClip>();

    public static event Action<int> OnNodeExecutionCompleted;

    void Awake()
    {
        foreach (SongSO song in songs)
        {
            songDictionary.Add(song.title, song.clip);
        }
    }

    void OnEnable()
    {
        NodeManager.OnMusicChange += PlaySong;
    }

    void OnDisable()
    {
        NodeManager.OnMusicChange -= PlaySong;
    }

    void PlaySong(CustomMusicChangeNode node)
    {
        AudioClip clip = null;

        SongTitle key = node.songTitle;
        if (!songDictionary.TryGetValue(key, out clip))
            Debug.LogError("Song not found");

        if (!channel1.isPlaying)
        {
            StartCoroutine(FadeIn(channel1, clip));

            if (channel2.isPlaying) StartCoroutine(FadeOut(channel2));
        }
        else
        {
            StartCoroutine(FadeOut(channel1));
            StartCoroutine(FadeIn(channel2, clip));
        }

        OnNodeExecutionCompleted?.Invoke(0);
    }

    IEnumerator FadeIn(AudioSource source, AudioClip clip)
    {
        if (!clip) yield break;

        source.clip = clip;
        source.Play();

        while (source.volume < 1f)
        {
            float addedValue = Time.deltaTime / fadeDuration;
            source.volume += addedValue;

            yield return null;
        }
    }

    IEnumerator FadeOut(AudioSource source)
    {
        while (source.volume > 0f)
        {
            float substractedValue = Time.deltaTime / fadeDuration;
            source.volume -= substractedValue;

            yield return null;
        }

        source.Stop();
    }
}