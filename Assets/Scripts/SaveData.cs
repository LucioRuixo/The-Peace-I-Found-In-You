using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    [Serializable]
    public struct BackgroundData
    {
        public BackgroundController.BackgroundType type;
        public BackgroundController.Location location;
        public BackgroundController.Ilustration ilustration;

        public BackgroundData(BackgroundController.BackgroundType _type, BackgroundController.Location _location, BackgroundController.Ilustration _ilustration)
        {
            type = _type;
            location = _location;
            ilustration = _ilustration;
        }
    }

    [Serializable]
    public struct CharacterData
    {
        public int bodyIndex;
        public int armIndex;
        public int headIndex;

        public CharacterManager.CharacterName name;
    }

    [Serializable]
    public struct LogData
    {
        public string lastCharacterToSpeak;
        public string logText;
    }

    [Serializable]
    public struct MusicData
    {
        public bool musicPlaying;

        public MusicController.SongTitle songTitle;
    }

    public bool lastDecisionGood;

    public string currentNodeGUID;

    public int routeSceneIndex;
    public int currentDialogueStripIndex;

    public RouteController.Route currentRoute;
    public FilterController.Filter currentFilter;

    public BackgroundData backgroundData;
    public LogData logData;
    public MusicData musicData;

    public List<CharacterData> charactersInScene;
}