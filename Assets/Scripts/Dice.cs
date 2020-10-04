using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice
{
    int currentNo;

    public int CurrentNo { get { return currentNo; } }

    public int roll()
    {
        currentNo = Random.Range(1, 7);
        return currentNo;
    }
}
