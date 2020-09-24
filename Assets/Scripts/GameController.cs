using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    int totalPlayers;

    Player currentPlayer;

    List<Player> players;

    // Types of tiles.
    List<GameObject> homeTiles;
    List<GameObject> normalTiles;
    List<GameObject> fTiles;
    List<GameObject> chanceTiles;

    GameObject goal;

    private void Start()
    {   
        spawnPlayers();
        // Set a current Player.
    }

    // Main Loop -----------------------------------------------------------------------------------

    private void Update()
    {
        // Solve the integration.

        // Check if its correct.

        // If correct.
            // Move player.
        // Else
            // Try again.

        // Set next turn.
    }

    // Private functions ------------------------------------------------------------------------

    private void spawnPlayers()
    {
        // Spawn the available players.
    }

    private void movePlayer(Player player, Vector3 pos)
    {   
        player.move(pos);
    }
}
