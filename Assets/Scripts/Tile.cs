using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int no;
    string type;
    public bool occupied;
    public int currentPlayer;

    private void Awake()
    {
        type = gameObject.tag;
        occupied = false;
        currentPlayer = -1;
        no = -1;
    }

    public void UpdateTileAttributes(bool occupied, int currentPlayer)
    {
        this.occupied = occupied;
        this.currentPlayer = currentPlayer;
    }
}
