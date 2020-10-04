using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{   
    [SerializeField] int totalPlayers;

    int currentPlayer;
    List<Dice> dices;

    [SerializeField] List<GameObject> players;

    // Types of tiles.
    [SerializeField] List<GameObject> homeTiles;
    [SerializeField] List<GameObject> normalTiles;
    [SerializeField] List<GameObject> fTiles;
    [SerializeField] List<GameObject> chanceTiles;
    [SerializeField] GameObject goalTile;

    [SerializeField] Button integrationButton;
    [SerializeField] TMP_InputField integrationInputField;

    GameObject goal;

    private void Start()
    {   
        // Initialize no. of dices
        dices = new List<Dice>(2);
        for(int i = 0; i < 2; i++)
        {
            dices.Add(new Dice());
        }

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
            //Debug.Log("enter");
            // Solve the integration.
            int lower = Mathf.Min(dices[0].CurrentNo, dices[1].CurrentNo);
            int higher = Mathf.Max(dices[0].CurrentNo, dices[1].CurrentNo);

            int ans = Integration.solve(lower, higher);
            Debug.Log("Correct Ans: " + ans);

            int userAns = int.Parse(integrationInputField.text);
            Debug.Log("User Ans: " + userAns);

            // Check if its correct.
            if (ans == userAns)
            {
                // Move player.
                MovePlayer(players[currentPlayer].GetComponent<Player>(), homeTiles[0].transform.position);
                SetNextTurn((currentPlayer + 1) % totalPlayers);
                Debug.Log("Current Player: " + currentPlayer);
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

        if (Input.GetKeyDown("m"))
        {
            MovePlayer(players[currentPlayer].GetComponent<Player>(), homeTiles[1].transform.position);
        }
    }

    private void SetNextTurn(int player)
    {
        currentPlayer = player;
    }

    private void SpawnPlayers()
    {
        // Spawn the available players.
        for(int i = 0; i < totalPlayers; i++)
        {
            players[i].SetActive(true);
        }
    }

    private void MovePlayer(Player player, Vector3 pos)
    {
        Debug.Log("Moving player to " + pos);
        player.move(pos);
    }

    private void RollDices()
    {
        //Debug.Log("Rolling..");

        for(int i = 0; i < 2; i++)
        {
            //Debug.Log(i);
            dices[i].roll();
            //Debug.Log("Rolled");
        }
    }
}
