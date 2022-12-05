using UnityEngine;

public enum GameMode
{ 
    Standard = 1,
    DeathRace = 2
}

[CreateAssetMenu(fileName = "Mode_", menuName = "Game Mode", order = 1)]
public class GameModeSO : ScriptableObject
{
    public GameMode mode;
}
