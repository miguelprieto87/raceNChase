using UnityEngine;

[System.Serializable]
public struct GameSettings
{
    [Header("Global Settings")]
    public byte maxPlayers;
    public GameMode gameMode;

    [Header("Standard Settings")]
    public byte timeToRaceStart;
    public byte numberOfLaps;
}
