using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool alive = false;
    public bool qualified = false;

    public string tileType = "home";
    public int currentTile;

    int homeTile;
    int qualifyingTile;
    int goalTile;

    // Getters ---------------------------------------------------------------------------------
    public int QualifyingTile { get { return qualifyingTile; } }
    public int GoalTile { get { return goalTile; } }
    public int HomeTile { get { return homeTile; } }

    // Public functions -----------------------------------------------------------------------
    public void SetPlayerTiles (int homeTileIndex)
    {
        qualifyingTile = ((homeTileIndex * 4) + 24) % 24;
        goalTile = (homeTileIndex + 42) % 42;
        currentTile = 4 * homeTileIndex;
        homeTile = 4 * homeTileIndex;
        //Debug.Log("HomeTile: " + currentTile);
    }

    public void move(Vector3 pos)
    {
        transform.position = pos;
        // Update player attributes.
    }

    public void UpdatePlayerAttributes(int currentTile, string tileType, bool alive = false)
    {
        this.currentTile = currentTile;
        this.tileType = tileType;
        this.alive = alive;
        this.qualified = qualified;
    }
}
