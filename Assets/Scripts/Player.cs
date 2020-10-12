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
    int innerTile;
    int lastTile;

    // Getters ---------------------------------------------------------------------------------
    public int QualifyingTile { get { return qualifyingTile; } }
    public int InnerTile { get { return innerTile; } }
    public int LastTile { get { return lastTile; } }
    public int HomeTile { get { return homeTile; } }

    // Public functions -----------------------------------------------------------------------
    public void SetPlayerTiles (int homeTileIndex)
    {
        innerTile = 24 + (homeTileIndex * 3);
        qualifyingTile = (homeTileIndex == 0) ? 23 : ((homeTileIndex * 4) + 22) % 23;
        lastTile = (innerTile - 1) > 24 ? (innerTile - 1) : 42;
        currentTile = 4 * homeTileIndex;
        homeTile = 4 * homeTileIndex;

        Debug.Log("Player: " + homeTile / 4);
        Debug.Log("     Inner Tile: " + innerTile);
        Debug.Log("     Qualifying Tile: " + qualifyingTile);
        Debug.Log("     Last Tile: " + lastTile + "\n\n"); 
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
