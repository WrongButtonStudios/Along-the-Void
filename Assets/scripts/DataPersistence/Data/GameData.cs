using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currentLevel;
    public bool[] levelSucceeded;
    public bool[] shop;
    public int starsCurrency;
    public string[] highscoreTimeEachLv;
    public int[] highscoreStarAmountEachLv;
    //Pfeiltasten beachten
    public char[] keySettings;

    public GameData()
    {
        this.currentLevel = 0;
        this.levelSucceeded = new bool[0];
        this.shop = new bool[0];
        this.starsCurrency = new int();
        this.highscoreTimeEachLv = new string[0];
        this.highscoreStarAmountEachLv = new int[0];
        this.keySettings = new char[0];
    }
}
