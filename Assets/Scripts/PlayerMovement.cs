using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : NetworkBehaviour
{
    Dices dices = new Dices();
    int individualPlayers;

    [SerializeField] GameObject canvas;

    [SerializeField] TMP_InputField integrationInputField;
    [SyncVar(hook = nameof(UpdateInputText))] string inputText;

    [SerializeField] List<GameObject> homeTiles;
    [SerializeField] List<GameObject> normalTiles;
    [SerializeField] List<GameObject> fTiles;
    [SerializeField] List<GameObject> chanceTiles;
    [SerializeField] GameObject goalTile;

    [SerializeField]
    GameObject[] path;

    [SerializeField] TMP_Text dice1No;
    [SerializeField] TMP_Text dice2No;

    private void Start()
    {
        SetPath();
        if (!GetComponent<Player>().isTurn)
        {
            canvas.SetActive(false);
        }
    }

    private void Update()
    {

        inputText = integrationInputField.text;
        //Debug.Log("Input Text: " + inputText);
        if (!isLocalPlayer) { return; }
        //if (!GetComponent<Player>().isTurn) { return; };

        Inputs();
    }

    // Game Specific functions ------------------------------------------------------------------------------

    void Inputs()
    {
        if (Input.GetKeyDown("return") && GetComponent<Player>().isTurn)
        {
            Debug.Log("Enter");
            Debug.Log("Input Text: " + inputText);
            // Solve the integration.
            int lower = Mathf.Min(dices.no1, dices.no2);
            int higher = Mathf.Max(dices.no1, dices.no2);

            int ans = Integration.solve(lower, higher);
            //Debug.Log("Correct Ans: " + ans);

            int userAns = int.Parse(integrationInputField.text);
            Debug.Log("User Ans: " + integrationInputField.text);

            DecideNextTileIndex(gameObject.GetComponent<Player>(), userAns);

        }

        if (Input.GetKeyDown("r"))
        {
            // Roll dices.
            CmdRoll();
        }
    }

    private void SetPath()
    {
        path = new GameObject[43];
        int hTp = 0;
        int nTp = 0;
        int fTp = 0;
        int cTp = 0;

        for (int i = 0; i < 42;)
        {
            // Outer hexagon.
            if (cTp < 6 && i < 24)
            {   
                path[i] = homeTiles[hTp++];
                path[i + 1] = normalTiles[nTp++];
                path[i + 2] = normalTiles[nTp++];
                path[i + 3] = chanceTiles[cTp++];
                i += 4;
            }
            // Inner hexagon.
            else
            {
                path[i] = normalTiles[nTp++];
                path[i + 1] = normalTiles[nTp++];
                path[i + 2] = fTiles[fTp++];
                i += 3;
            }
        }

        path[42] = goalTile;

        for (int i = 0; i < 43; i++)
        {
            path[i].GetComponent<Tile>().no = i;
        }
    }
    #region NotRequiredforNow

    private void DecideNextTileIndex(Player player, int userAns)
    {
        int nextTileIndex;

        if (player.qualified)
        {
            // Outer hexagon
            if (player.currentTile < 24)
            {
                int dstQualify = (24 - (player.currentTile - player.QualifyingTile)) % 24;
                if (dstQualify < userAns)
                {
                    nextTileIndex = player.InnerTile + userAns - (dstQualify + 1);
                    //Debug.Log("Qualifying tile for player: " + currentPlayerIndex + "is " + nextTileIndex);
                }
                else
                {
                    nextTileIndex = (player.currentTile + userAns) % 24;
                }
            }
            // Inner hexagon.
            else
            {
                nextTileIndex = (player.currentTile + userAns - 24) % 18 + 24;
                // Win condition.
                if (nextTileIndex - 1 == player.LastTile || (player.no % 10 == 0 && nextTileIndex - 1 == 23))
                {
                    nextTileIndex = 42;
                }
            }
        }
        else
        {
            nextTileIndex = (player.currentTile + userAns) % 24;
        }

        Debug.Log("Next Tile Index: " + nextTileIndex);

        // Move Player
        CmdMovePlayer(gameObject, nextTileIndex);

        // SetNextTurn
        //CmdSetNextTurn();
    }

    

    //private void CmdSetNextTurn(int currentPlayerIndex, int totalPlayers)
    //{
    //    currentPlayerIndex = (currentPlayerIndex + 1) % totalPlayers;
    //}
    #endregion

    //Commands ------------------------------------------------------------------------------------------
    [Command]
    void CmdRoll()
    {
        dices.roll();

        RpcshowDicesRolls(dices.no1, dices.no2);
    }

    [Command]
    void CmdMovePlayer(GameObject target, int nextTileIndex)
    {
        RpcMovePlayer(nextTileIndex);
    }

    [Command]
    void CmdMoveTilePlayer(GameObject tile, int tileIndex)
    {
        NetworkConnection conn = NetworkServer.connections[tile.GetComponent<Tile>().currentPlayerIndex];
        //TargetMovePlayer(conn, gameObject.GetComponent<Player>().HomeTile);
    }

    //Rpc ---------------------------------------------------------------------------------------------------------
    [ClientRpc]
    void RpcshowDicesRolls(int no1, int no2)
    {
        Debug.Log($"Dice1: {no1}");
        Debug.Log($"Dice2: {no2}");
        dice1No.text = no1.ToString();
        dice2No.text = no2.ToString();
    }

    //Target ------------------------------------------------------------------------------------------------
    [ClientRpc]
    private void RpcMovePlayer(int nextTileIndex)
    {
        Debug.Log("Moving Player....");
        Player player = gameObject.GetComponent<Player>();
        // Get information of next tile.
        GameObject nextTile = path[nextTileIndex];
        string tileType = nextTile.tag;

        // Free the previous tile.
        Tile currentTile = path[player.currentTile].GetComponent<Tile>();
        currentTile.occupied = false;
        currentTile.currentPlayerIndex = -1;

        // Move the player to new tile.
        player.move(nextTile.transform.position);

        //--Update the new tile.
        Tile tile = nextTile.GetComponent<Tile>();

        //Debug.Log("tile.occupied: " + tile.occupied);
        //Debug.Log("tile.tag: " + tile.tag);
        //Debug.Log("is Same: " + tile.currentPlayerIndex);

        //--Check if it killed someone.
        if (tile.occupied && tile.tag != "Home" && ((int)(tile.currentPlayerIndex / 10) != player.no))
        {
            //Player tilePlayer = players[(int)(tile.currentPlayerIndex / 10), tile.currentPlayerIndex % 10].GetComponent<Player>();
            //TargetMovePlayer(tilePlayer, tilePlayer.HomeTile);

            //CmdMoveTilePlayer(nextTile, player.HomeTile);

            // Qualify them to go into inner hexagon.
            player.qualified = true;
        }

        //--Update tile information.
        tile.UpdateTileAttributes(true, player.no);

        // Update player attributes.
        player.UpdatePlayerAttributes(nextTileIndex, tileType);

        Debug.Log(transform.name + " moved");
    }


    // Server Debugging functions ---------------------------------------------------------------------------------
    void UpdateInputText(string oldInput, string newInput)
    {
        //Debug.Log($"New Input: {newInput}");
    }
}
