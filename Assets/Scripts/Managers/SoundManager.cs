using UnityEngine;

public class SoundManager : MonoBehaviourSingleton<SoundManager>
{
    public enum Enviroments
    {
        Inicia_Amb_Bosque,
        Inicia_Amb_Campo,
        Inicia_Amb_Casa,
        Inicia_Amb_CasaSeijun,
        Inicia_Amb_GritosYBatalla,
        Inicia_Amb_Noche,
        Inicia_Amb_Pueblo,
        Inicia_Amb_Rio,
        Inicia_Amb_Santuario
    }

    public enum SFXs
    {
        Fx_AbrePuerta,
        Fx_ApretaBoton,
        Fx_Bostezo,
        Fx_BotonDecision,
        Fx_CortesEspada,
        Fx_CorteVerdura,
        Fx_Demonios,
        Fx_ErrarCorte,
        Fx_EspadaDesenvaine,
        Fx_Espadas_Chocando,
        Fx_Fogata,
        Fx_GolpesDePuerta,
        Fx_Grillos,
        Fx_Gritos_Batalla,
        Fx_HachaGolpeMadera,
        Fx_Hojas,
        Fx_Lluvia,
        Fx_PisadasHojas,
        Fx_Respiracion,
        Fx_RevuelveObjetos,
        Fx_RisaMujer,
        Fx_Sonido_Cubiertos,
        Fx_Susurros,
        Fx_YaEstaOscureciendo
    }

    public enum Songs
    {
        Inicia_Creditos,
        Inicia_Juego,
        Inicia_Menu
    }

    [SerializeField] GameObject audioObject;

    public GameObject AudioObject { get { return audioObject; } }

    public void PlayEnviroment(Enviroments enviroment)
    {
        AkSoundEngine.PostEvent(enviroment.ToString(), audioObject);
    }

    public void PlaySFX(SFXs sfx)
    {
        AkSoundEngine.PostEvent(sfx.ToString(), audioObject);
    }

    public void PlaySong(Songs song)
    {
        AkSoundEngine.PostEvent(song.ToString(), audioObject);
    }
}