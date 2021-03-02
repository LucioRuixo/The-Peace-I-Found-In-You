using System;
using UnityEngine;

public class SoundManager : MonoBehaviourSingleton<SoundManager>
{
    [Serializable]
    public struct Environment
    {
        public string eventName;
        public Environments environment;
    };

    [Serializable]
    public struct SFX
    {
        public string eventName;
        public SFXs sfx;
    };

    [Serializable]
    public struct Song
    {
        public string stateName;
        public Songs song;
    };

    public enum Environments
    {
        Forest,
        Field,
        ProtagonistHome,
        SeijunHome,
        BattleField,
        Night,
        Village,
        River,
        Sanctuary
    }

    public enum SFXs
    {
        DoorOpening,
        Button,
        Yawn,
        Decision,
        SwordCut,
        FoodCut,
        Demons,
        HitMissed,
        SwordUnsheathed,
        SwordClash,
        Bonfire,
        DoorSmack,
        Crickets,
        BattleScreams,
        AxeHittingWood,
        Leaves,
        Rain,
        StepsOnLeaves,
        Breathing,
        StirringObjects,
        FemaleLaugh,
        Silverware,
        Whispers,
        GettingDark
    }

    public enum Songs
    {
        None,
        Menu,
        Credits,
        Protagonist,
        Hoshi,
        Tadao,
        Minigame
    }

    [SerializeField] string musicInitializationEvent;
    [SerializeField] string musicStateGroupName;

    [SerializeField] GameObject audioObject = null;

    [SerializeField] Environment[] environments;
    [SerializeField] SFX[] sfxs;
    [SerializeField] Song[] songs;

    public bool WwiseInitialized { private set; get; } = false;
    public GameObject AudioObject { get { return audioObject; } }

    void Start()
    {
        WwiseInitialized = true;

        AkSoundEngine.PostEvent(musicInitializationEvent, audioObject);
    }

    public void PlayEnviroment(Environments environment)
    {
        string eventName = null;

        foreach (Environment enviromentObject in environments)
        {
            if (enviromentObject.environment == environment)
            {
                eventName = enviromentObject.eventName;
                break;
            }
        }

        if (eventName != null) AkSoundEngine.PostEvent(eventName, audioObject);
    }

    public void PlaySFX(SFXs sfx)
    {
        string eventName = null;

        foreach (SFX sfxObject in sfxs)
        {
            if (sfxObject.sfx == sfx)
            {
                eventName = sfxObject.eventName;
                break;
            }
        }

        if (eventName != null) AkSoundEngine.PostEvent(eventName, audioObject);

        AkSoundEngine.PostEvent(eventName, audioObject);
    }

    public void PlaySong(Songs song)
    {
        string stateName = null;

        foreach (Song songObject in songs)
        {
            if (songObject.song == song)
            {
                stateName = songObject.stateName;
                break;
            }
        }

        AkSoundEngine.SetState(musicStateGroupName, stateName);
    }
}