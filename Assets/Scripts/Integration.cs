using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Integration
{
    public static int solve(int lower, int higher)
    {
        Debug.Log("Lower: " + lower);
        Debug.Log("Higher: " + higher);

        int ans = (int)(((Mathf.Pow(higher, 2) / 2) - (3 * higher / 2)) - ((Mathf.Pow(lower, 2) / 2) - (3 * lower / 2)));
        return ans;
    } 
}
