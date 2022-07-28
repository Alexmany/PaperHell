using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Data", menuName = "Game Data")]

[System.Serializable]
public class GameData : ScriptableObject
{
    public int coins;   

    [Space]
    public float highscore;

    [Space]
    public int coffee;
    public int finger;
    public int healthpak;

    [Space]
    public int table_id;
    public int[] table_count;

    [Space]
    public bool[] levels;

    [Space]
    public bool game_ended;    
}

