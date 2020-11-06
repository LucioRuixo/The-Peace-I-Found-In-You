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
    public MusicData musicData;

    public List<Character> charactersInScene;
}