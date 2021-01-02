using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] int totalPlayers;
    [SerializeField] int individualPlayers;

    [SerializeField] int currentPlayerIndex;
    //[SerializeField]int subPlayerIndex;
    int[] subPlayerIndices;
    Dices dices;
    bool[,] f;

    [SerializeField] GameObject[] path;

    [SerializeField] GameObject[] playerPrefabs;
    GameObject[,] players;

    // Types of tiles.
    [SerializeField] List<GameObject> homeTiles;
    [SerializeField] List<GameObject> normalTiles;
    [SerializeField] List<GameObject> fTiles;
    [SerializeField] List<GameObject> chanceTiles;
    [SerializeField] GameObject goalTile;

    // Integration related.
    [SerializeField] Button integrationButton;
    [SerializeField] TMP_InputField integrationInputField;

    bool gameOver = false;

    private void Start()
    {
        f = new bool[totalPlayers, individualPlayers];

        //Initialize path.
        path = new GameObject[43];
        SetPath();

        //Initialize players.
        players = new GameObject[totalPlayers, individualPlayers];
        subPlayerIndices = new int[totalPlayers];
        for (int i = 0; i < totalPlayers; i++)
        {
            subPlayerIndices[i] = 0;
        }

        // Initialize no. of dices
        dices = new Dices();

        // Spawn total players.
        SpawnPlayers();

        // Set a current Player.
        currentPlayerIndex = 0;
    }

    // Main Loop -----------------------------------------------------------------------------------

    private void Update()
    {
        if (!gameOver)
        {
            Inputs();
        }
    }

    // Private functions ------------------------------------------------------------------------

    private void Inputs()
    {
        if (Input.GetKeyDown("return") && integrationInputField.text != "")
        {
            // Solve the integration.
            int lower = Mathf.Min(dices.no1, dices.no2);
            int higher = Mathf.Max(dices.no1, dices.no2);

            //int ans = Integration.solve(lower, higher);
            //Debug.Log("Correct Ans: " + ans);

            int userAns = int.Parse(integrationInputField.text);
            //Debug.Log("User Ans: " + userAns);

            // Check if its correct.
            if (true)
            {
                Player player = players[currentPlayerIndex, subPlayerIndices[currentPlayerIndex]].GetComponent<Player>();

                // Filter Player.
                player = FilterPlayer(player);

                int nextTileIndex = DecideNextTileIndex(player, userAns);

                // Move Player to given pos.
                MovePlayer(player, nextTileIndex);

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

            //Debug.Log("Dice 1: " + dices[0].CurrentNo);
            //Debug.Log("Dice 2: " + dices[1].CurrentNo);
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Pressed.");
            RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);

            if (hitInfo)
            {
                //Debug.Log(hitInfo.transform.gameObject.tag);
                if (hitInfo.transform.gameObject.tag == currentPlayerIndex.ToString() + "Peg")
                {
                    GameObject currentPlayer = hitInfo.transform.gameObject;

                    // When a peg reaches goal tile it can't be moved.
                    if (currentPlayer.GetComponent<Player>().tileType != "Goal")
                    {
                        subPlayerIndices[currentPlayerIndex] = currentPlayer.GetComponent<Player>().no % 10;
                    }
                    //Debug.Log("Player chosen: " + (currentPlayerIndex * 10 + subPlayerIndices[currentPlayerIndex]));
                }
            }
        }
    }

    private void SetNextTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % totalPlayers;

        if (f[currentPlayerIndex, subPlayerIndices[currentPlayerIndex]])
        {
            f[currentPlayerIndex, subPlayerIndices[currentPlayerIndex]] = false;
            Debug.Log(currentPlayerIndex + " is flagged");
            SetNextTurn();
        }

    }

    private void SpawnPlayers()
    {
        // Spawn the available players.
        for (int i = 0; i < totalPlayers; i++)
        {
            for (int j = 0; j < individualPlayers; j++)
            {
                players[i, j] = Instantiate(playerPrefabs[i], playerPrefabs[i].transform.position, Quaternion.identity);
                players[i, j].GetComponent<Player>().SetPlayerTilesLocal(i, j);
                players[i, j].SetActive(true);
            }
            //players[i].SetActive(true);
            //players[i].GetComponent<Player>().SetPlayerTiles(i);
        }
    }

    private void SetPath()
    {
        int hTp = currentPlayerIndex;
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

    private int DecideNextTileIndex(Player player, int userAns)
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
                if (nextTileIndex - 1 == player.LastTile || (currentPlayerIndex == 0 && nextTileIndex - 1 == 23))
                {
                    nextTileIndex = 42;
                }
            }
        }
        else
        {
            nextTileIndex = (player.currentTile + userAns) % 24;
        }

        return nextTileIndex;
    }

    private Player FilterPlayer(Player player)
    {
        if (player.tileType == "Goal")
        {
            Player filteredPlayer;

            subPlayerIndices[currentPlayerIndex] = (subPlayerIndices[currentPlayerIndex] + 1) % individualPlayers;
            filteredPlayer = FilterPlayer(players[currentPlayerIndex, subPlayerIndices[currentPlayerIndex]].GetComponent<Player>());

            return filteredPlayer;
        }

        return player;
    }

    private void MovePlayer(Player player, int nextTileIndex)
    {
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
        if (tile.occupied && tile.tag != "Home" && ((int)(tile.currentPlayerIndex / 10) != currentPlayerIndex))
        {
            Player tilePlayer = players[(int)(tile.currentPlayerIndex / 10), tile.currentPlayerIndex % 10].GetComponent<Player>();
            MovePlayer(tilePlayer, tilePlayer.HomeTile);

            // Qualify them to go into inner hexagon.
            for (int i = 0; i < individualPlayers; i++)
            {
                players[currentPlayerIndex, i].GetComponent<Player>().qualified = true;
            }
        }

        //--Update tile information.
        tile.UpdateTileAttributes(true, currentPlayerIndex * 10 + subPlayerIndices[currentPlayerIndex]);

        // Update player attributes.
        player.UpdatePlayerAttributes(nextTileIndex, tileType);
    }

    private void RollDices()
    {
        dices.roll();
    }

    private void DecideNextTurn(Player player)
    {
        string tileType = player.tileType;
        if (tileType == "Chance")
        {
            //Debug.Log("Play Again.");
        }
        else if (tileType == "F")
        {
            f[currentPlayerIndex, subPlayerIndices[currentPlayerIndex]] = true;

            SetNextTurn();
            Debug.Log("Current Player: " + currentPlayerIndex);

            //Debug.Log("Lose Next turn.");
        }
        else if (tileType == "Normal")
        {
            SetNextTurn();
            Debug.Log("Current Player: " + currentPlayerIndex);
        }
        else if (tileType == "Home")
        {
            SetNextTurn();
            Debug.Log("Current Player: " + currentPlayerIndex);
            //Debug.Log("Safe");
        }
        else if (tileType == "Goal" && player.currentTile == player.LastTile && goalTile.GetComponent<Tile>().TilePlayers == individualPlayers)
        {
            Debug.Log("Current player won the game.");
            gameOver = true;
        }
    }
}
