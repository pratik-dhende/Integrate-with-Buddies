using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Integration
{
    public static int solve(int lower, int higher)
    {
        //Debug.Log("Lower: " + lower);
        //Debug.Log("Higher: " + higher);

        int ans = (int)((higher * (higher - 3) / 2) - (lower * (lower - 3) / 2));
        return ans;
    } 
}
