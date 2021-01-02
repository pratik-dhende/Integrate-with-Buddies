using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class Player : NetworkBehaviour
{
    public int no;
    [SyncVar] public int spriteNo;
    public bool alive = false;
    public bool qualified = false;
    [SyncVar(hook = nameof(UpdatedTurnbool))] public bool isTurn;

    public string tileType = "home";
    [SyncVar]public int currentTile;

    [SerializeField] TMP_Text turnNo;
    

    [SerializeField] GameObject[] playerSprites;

    //[SyncVar(hook = nameof(UpdatePlayerSprite))]public int sprite;

    [SyncVar] int homeTile;
    [SyncVar] int qualifyingTile;
    [SyncVar] int innerTile;
    [SyncVar] int lastTile;

    // Getters ---------------------------------------------------------------------------------
    public int QualifyingTile { get { return qualifyingTile; } }
    public int InnerTile { get { return innerTile; } }
    public int LastTile { get { return lastTile; } }
    public int HomeTile { get { return homeTile; } }

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = playerSprites[spriteNo].GetComponent<SpriteRenderer>().sprite;

    }

    // Public functions -----------------------------------------------------------------------
    public void SetPlayerTiles(int playerIndex)
    {
        no = playerIndex * 10;

        innerTile = 24 + (playerIndex * 3);
        qualifyingTile = (playerIndex == 0) ? 23 : ((playerIndex * 4) + 22) % 23;
        lastTile = (innerTile - 1) > 24 ? (innerTile - 1) : 42;
        currentTile = 4 * playerIndex;
        homeTile = 4 * playerIndex;

        Debug.Log("Player: " + homeTile / 4);
        Debug.Log("     Inner Tile: " + innerTile);
        Debug.Log("     Qualifying Tile: " + qualifyingTile);
        Debug.Log("     Last Tile: " + lastTile + "\n\n");

        //Debug.Log("HomeTile: " + currentTile);
    }

    public void SetPlayerTilesLocal(int currentPlayerIndex, int subPlayerIndex)
    {
        no = currentPlayerIndex * 10 + subPlayerIndex;

        innerTile = 24 + (currentPlayerIndex * 3);
        qualifyingTile = (currentPlayerIndex == 0) ? 23 : ((currentPlayerIndex * 4) + 22) % 23;
        lastTile = (innerTile - 1) > 24 ? (innerTile - 1) : 42;
        currentTile = 4 * currentPlayerIndex;
        homeTile = 4 * currentPlayerIndex;

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
    }

    public void UpdatePlayerSprite(int newSprite)
    {
        GetComponent<SpriteRenderer>().sprite = playerSprites[newSprite].GetComponent<SpriteRenderer>().sprite;
        spriteNo = newSprite;
    }

    // Commands -------------------------------------------------------------------------------------------------

    // Rpc --------------------------------------------------------------------------------------------------------
    void UpdatedTurnbool(bool oldb, bool newb)
    {
        if (newb == true)
        {
            turnNo.text = no.ToString();
        }
    }

    // Target ------------------------------------------------------------------------------------------------------
}


