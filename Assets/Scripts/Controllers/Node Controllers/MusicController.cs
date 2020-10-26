using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nullbloq.Noodles;

public class MusicController : NodeController
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

    public override Type NodeType { protected set; get; }

    [SerializeField] float fadeDuration = 3f;

    [SerializeField] AudioSource channel1 = null;
    [SerializeField] AudioSource channel2 = null;

    [SerializeField] List<SongSO> songs = null;

    void Awake()
    {
        NodeType = typeof(CustomMusicChangeNode);
    }

    void PlaySong(CustomMusicChangeNode node)
    {
        AudioClip clip = null;

        foreach (SongSO song in songs)
        {
            if (song.title == node.songTitle)
            {
                clip = song.clip;
                break;
            }
        }

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

        CallNodeExecutionCompletion(0);
    }

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomMusicChangeNode;

        PlaySong(node);
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