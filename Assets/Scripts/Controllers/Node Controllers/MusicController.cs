using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nullbloq.Noodles;

public class MusicController : NodeController, ISaveComponent
{
    //public enum SongTitle
    //{
    //    None,
    //    Calm,
    //    Forest,
    //    GoodMemories,
    //    HoshiAndI,
    //    InShadows,
    //    Sachi,
    //    SadPast,
    //    SeijunHome,
    //    Tadao,
    //    Town
    //}

    public override Type NodeType { protected set; get; }

    [SerializeField] float fadeDuration = 3f;

    //[SerializeField] AudioSource channel1 = null;
    //[SerializeField] AudioSource channel2 = null;
    SoundManager soundManager;

    [SerializeField] List<SongSO> songs = null;

    public bool MusicPlaying { private set; get; } = false;
    public SoundManager.Songs CurrentSong { private set; get; } = SoundManager.Songs.None;

    void Awake()
    {
        NodeType = typeof(CustomMusicChangeNode);

        soundManager = SoundManager.Get();
    }

    //void PlaySong(SongTitle songTitle)
    //{
    //    if (songTitle == SongTitle.None) return;
    //
    //    SongSO newSong = null;
    //    foreach (SongSO song in songs)
    //    {
    //        if (song.title == songTitle)
    //        {
    //            newSong = song;
    //            break;
    //        }
    //    }
    //
    //    if (!channel1.isPlaying)
    //    {
    //        StartCoroutine(FadeIn(channel1, newSong.clip));
    //
    //        if (channel2.isPlaying) StartCoroutine(FadeOut(channel2));
    //    }
    //    else
    //    {
    //        StartCoroutine(FadeOut(channel1));
    //        StartCoroutine(FadeIn(channel2, newSong.clip));
    //    }
    //
    //    MusicPlaying = true;
    //    CurrentSong = newSong.title;
    //}

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomMusicChangeNode;

        soundManager.PlaySong(node.songTitle);

        CallNodeExecutionCompletion(0);
    }

    public void SetLoadedData(SaveData loadedData)
    {
        if (loadedData.musicData.musicPlaying)
            soundManager.PlaySong(loadedData.musicData.songTitle);
    }

    //IEnumerator FadeIn(AudioSource source, AudioClip clip)
    //{
    //    if (!clip) yield break;
    //
    //    source.clip = clip;
    //    source.Play();
    //
    //    while (source.volume < 1f)
    //    {
    //        float addedValue = Time.deltaTime / fadeDuration;
    //        source.volume += addedValue;
    //
    //        yield return null;
    //    }
    //}
    //
    //IEnumerator FadeOut(AudioSource source)
    //{
    //    while (source.volume > 0f)
    //    {
    //        float substractedValue = Time.deltaTime / fadeDuration;
    //        source.volume -= substractedValue;
    //
    //        yield return null;
    //    }
    //
    //    source.Stop();
    //}
}