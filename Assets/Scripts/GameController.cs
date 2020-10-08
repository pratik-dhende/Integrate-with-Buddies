using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{   
    [SerializeField] int totalPlayers;

    int currentPlayer;
    Dice[] dices;
    bool[] f;

    [SerializeField] GameObject[] path;

    [SerializeField] List<GameObject> players;

    // Types of tiles.
    [SerializeField] List<GameObject> homeTiles;
    [SerializeField] List<GameObject> normalTiles;
    [SerializeField] List<GameObject> fTiles;
    [SerializeField] List<GameObject> chanceTiles;
    [SerializeField] GameObject goalTile;

    // Integration related.
    [SerializeField] Button integrationButton;
    [SerializeField] TMP_InputField integrationInputField;

    private void Start()
    {
        f = new bool[totalPlayers];

        //Initialize path.
        path = new GameObject[42];
        SetPath();

        // Initialize no. of dices
        dices = new Dice[2];
        dices[0] = new Dice();
        dices[1] = new Dice();

        // Spawn total players.
        SpawnPlayers();

        // Set a current Player.
        currentPlayer = 0;
    }

    // Main Loop -----------------------------------------------------------------------------------

    private void Update()
    {
        Inputs();
    }

    // Private functions ------------------------------------------------------------------------

    private void Inputs()
    {
        if (Input.GetKeyDown("return") && integrationInputField.text != "")
        {
            // Solve the integration.
            int lower = Mathf.Min(dices[0].CurrentNo, dices[1].CurrentNo);
            int higher = Mathf.Max(dices[0].CurrentNo, dices[1].CurrentNo);

            int ans = Integration.solve(lower, higher);
            //Debug.Log("Correct Ans: " + ans);

            int userAns = int.Parse(integrationInputField.text);
            //Debug.Log("User Ans: " + userAns);

            // Check if its correct.
            if (true)
            {
                Player player = players[currentPlayer].GetComponent<Player>();

                // Get information of next tile.
                int nextTileIndex; // = ((player.currentTile + userAns) + (player.qualified && (player.currentTile == player.QualifyingTile) ? 42 : 24)) % (player.qualified ? 42 : 24);

                if (player.qualified){
                    if (player.currentTile == player.QualifyingTile)
                    {
                        nextTileIndex = player.QualifyingTile;
                    }
                    else
                    {
                        // If he is on normal tiles.
                        // If he is on inner hexagon.
                        nextTileIndex = -1;
                    }
                }
                else
                {
                    nextTileIndex = (player.currentTile + userAns) % 24;
                }
                //Debug.Log("NextTileIndex for " + currentPlayer + ": " + nextTileIndex);

                // Move Player to given pos.
                MovePlayer(player, nextTileIndex);

                Debug.Log("qualified" + (player.qualified));

                // Deciding next turn.
                DecideNextTurn(player);
            }
            else
            {
                Debug.Log("Try Again");
            }
        }

        if (Input.GetKeyDown("r"))
        {
            // Roll dices.
            RollDices();

            Debug.Log("Dice 1: " + dices[0].CurrentNo);
            Debug.Log("Dice 2: " + dices[1].CurrentNo);
        }
    }

    private void SetNextTurn(int player)
    {
        currentPlayer = player;
        //Debug.Log("Current Player: " + currentPlayer);
    }

    private void SpawnPlayers()
    {
        // Spawn the available players.
        for(int i = 0; i < totalPlayers; i++)
        {
            players[i].SetActive(true);
            players[i].GetComponent<Player>().SetPlayerTiles(i);
        }
    }

    private void SetPath()
    {   
        int hTp = currentPlayer;
        int nTp = 0;
        int fTp = 0;
        int cTp = 0;

        for(int i = 0; i < 42;)
        {   
            // Outer hexagon.
            if (cTp < 6  && i < 24)
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

        for(int i = 0; i < 42; i++)
        {
            path[i].GetComponent<Tile>().no = i;
        }
    }

    private void MovePlayer(Player player, int nextTileIndex)
    {
        // Get information of next tile.
        GameObject nextTile = path[nextTileIndex];
        string tileType = nextTile.tag;

        // Free the previous tile.
        Tile currentTile = path[player.currentTile].GetComponent<Tile>();
        currentTile.occupied = false;
        currentTile.currentPlayer = currentPlayer;

        // Move the player to new tile.
        player.move(nextTile.transform.position);

           //--Update the new tile.
        Tile tile = nextTile.GetComponent<Tile>();

           //--Check if it killed someone.
        if (tile.occupied && tile.tag != "Home")
        {
            Player tilePlayer = players[tile.currentPlayer].GetComponent<Player>();
            MovePlayer(tilePlayer, tilePlayer.HomeTile);
            player.qualified = true;
        }

           //--Update tile information.
        tile.UpdateTileAttributes(true, currentPlayer);

        // Update player attributes.
        player.UpdatePlayerAttributes(nextTileIndex, tileType);
    }

    private void RollDices()
    {
        dices[0].roll();
        dices[1].roll();
    }

    // what if all players get at f.
    private void DecideNextTurn(Player player)
     {
        string tileType = player.tileType;
        if (tileType == "Chance")
        {
            Debug.Log("Play Again.");
        }
        else if (tileType == "F")
        {
            f[currentPlayer] = true;
            Debug.Log("Lose Next turn.");
        }
        else if (tileType == "Normal")
        {
            SetNextTurn((currentPlayer + 1) % totalPlayers);

            while (f[currentPlayer])
            {
                SetNextTurn((currentPlayer + 1) % totalPlayers);
            }
        }
        else if (tileType == "Home")
        {
            SetNextTurn((currentPlayer + 1) % totalPlayers);

            while (f[currentPlayer])
            {
                SetNextTurn((currentPlayer + 1) % totalPlayers);
            }
            Debug.Log("Safe");
        }
        else if (tileType == "goal" && player.currentTile == player.GoalTile)
        {
            Debug.Log("Current player won the game.");
        }
    }
}
