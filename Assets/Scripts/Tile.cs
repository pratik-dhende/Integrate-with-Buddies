using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Tile : NetworkBehaviour
{
    [SyncVar]public int no;
    [SyncVar] string type;
    [SyncVar] public bool occupied;
    [SyncVar] int tilePlayers;
    [SyncVar] public int currentPlayerIndex;

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
