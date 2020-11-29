using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int no;
    string type;
    public bool occupied;
    int tilePlayers;
    public int currentPlayerIndex;

    public int TilePlayers { get { return tilePlayers; } }

    private void Awake()
    {
        type = gameObject.tag;
        occupied = false;
        currentPlayerIndex = -1;
        no = -1;
        tilePlayers = 0;
    }

    public void UpdateTileAttributes(bool occupied, int currentPlayerIndex)
    {
        this.occupied = occupied;
        this.currentPlayerIndex = currentPlayerIndex;

        tilePlayers = this.occupied ? tilePlayers + 1 : -1;
    }
}
