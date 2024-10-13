using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int currentLevel;
    public bool[] levelSucceeded;
    public bool[] shop;
    public int starsCurrency;
    public string[] highscoreTimeEachLv;
    public int[] highscoreStarAmountEachLv;
    //Pfeiltasten beachten
    public char[] keySettings;
    
}
