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
        //Inicia_Amb_Bosque,
        //Inicia_Amb_Campo,
        //Inicia_Amb_Casa,
        //Inicia_Amb_CasaSeijun,
        //Inicia_Amb_GritosYBatalla,
        //Inicia_Amb_Noche,
        //Inicia_Amb_Pueblo,
        //Inicia_Amb_Rio,
        //Inicia_Amb_Santuario
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
        //Fx_AbrePuerta,
        //Fx_ApretaBoton,
        //Fx_Bostezo,
        //Fx_BotonDecision,
        //Fx_CortesEspada,
        //Fx_CorteVerdura,
        //Fx_Demonios,
        //Fx_ErrarCorte,
        //Fx_EspadaDesenvaine,
        //Fx_Espadas_Chocando,
        //Fx_Fogata,
        //Fx_GolpesDePuerta,
        //Fx_Grillos,
        //Fx_Gritos_Batalla,
        //Fx_HachaGolpeMadera,
        //Fx_Hojas,
        //Fx_Lluvia,
        //Fx_PisadasHojas,
        //Fx_Respiracion,
        //Fx_RevuelveObjetos,
        //Fx_RisaMujer,
        //Fx_Sonido_Cubiertos,
        //Fx_Susurros,
        //Fx_YaEstaOscureciendo
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
        //Inicia_Juego,
        //Inicia_Menu,
        //Inicia_Creditos
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