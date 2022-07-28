using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue")]
public class Dialog : ScriptableObject
{
    public Dialogue[] sentences;    
}

[System.Serializable]
public class Dialogue
{    
    public string text;
    public Sprite pic;
}