using UnityEngine;

[System.Serializable]
public class GameData 
{
    // Keyed by player index (0-3)
    public SerializableDictionary<int, PlayerData> players;

    // Global world state
    //                              id      time
    public SerializableDictionary<string, float> sourcesTime;
    //                              name    questIndex
    public SerializableDictionary<string, uint> charactersDialog;

    public GameData()
    {
        players = new SerializableDictionary<int, PlayerData>();
        sourcesTime = new SerializableDictionary<string, float>();
        charactersDialog = new SerializableDictionary<string, uint>();
    }
}
