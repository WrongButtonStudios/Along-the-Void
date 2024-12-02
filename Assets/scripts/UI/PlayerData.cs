using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    static int[] numberOfFairies;
    static int numberOfGreenFairy = 1;
    static int numberOfRedFairy;
    static int numberOfBlueFairy;
    static int numberOfYellowFairy;
    //static int numberOfFairies = numberOfGreenFairy + numberOfRedFairy + numberOfBlueFairy + numberOfYellowFairy;


    //0=Grün 1=Rot 2=Blau 3=Gelb
    //public PlayerData(Player player)
    //{
    //    numberOfFairies = new int[4];
    //    numberOfFairies[0] = numberOfGreenFairy;
    //    numberOfFairies[1] = numberOfRedFairy;
    //    numberOfFairies[2] = numberOfBlueFairy;
    //    numberOfFairies[3] = numberOfYellowFairy;
    //}




    public void OnGreenFairyBuy()
    {
        numberOfFairies = new int[4];

    }

    public void OnRedFairyBuy()
    {
        numberOfRedFairy++;
    }

    public void OnBlueFairyBuy()
    {
        numberOfBlueFairy++;
    }

    public void OnYellowFairyBuy()
    {
        numberOfYellowFairy++;
    }
}
