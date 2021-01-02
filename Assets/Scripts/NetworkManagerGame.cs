using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public struct CreatePegMessage : NetworkMessage
{
    public string purpose;
}

public class NetworkManagerGame : NetworkManager
{
    string[] playerColor = { "Orange", "LG", "Red", "Blue", "Yellow", "DG" };

    GameObject[,] players;
    [SerializeField] GameObject[] path;

    //NetworkControllerGame networkController = new NetworkControllerGame();

    [SerializeField] GameObject[] playerSprites;
    [SerializeField] List<GameObject> homeTiles;
    [SerializeField] List<GameObject> normalTiles;
    [SerializeField] List<GameObject> fTiles;
    [SerializeField] List<GameObject> chanceTiles;
    [SerializeField] GameObject goalTile;

    Dictionary<int, NetworkConnectionToClient> conns;

    // Server Callbacks ----------------------------------------------------------------------------------------

    public override void OnStartServer()
    {
        base.OnStartServer();

        #region SpawnPlayers
        // Register Message on server.
        NetworkServer.RegisterHandler<CreatePegMessage>(OnCreateCharacter);
        #endregion

        #region SetPath
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
        #endregion
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        CreatePegMessage pegMessage = new CreatePegMessage
        {
            purpose = "Create Player"
        };

        conn.Send(pegMessage);
    }

    void OnCreateCharacter(NetworkConnection conn, CreatePegMessage message)
    {
        //int no = numPlayers * 10 + networkController.individualPlayers;

        GameObject gameobject = Instantiate(playerPrefab, homeTiles[numPlayers].transform.position, Quaternion.identity);
        //gameobject.GetComponent<SpriteRenderer>().sprite = playerSprites[numPlayers].GetComponent<SpriteRenderer>().sprite;

        Player player = gameobject.GetComponent<Player>();
        //player.SetPlayerTiles(no / 10, no % 10);
        //player.sprite = numPlayers;

        // call this to use this gameobject as the primary controller
        NetworkServer.AddPlayerForConnection(conn, gameobject);

        Debug.Log($"Total Players: {numPlayers}");
        //networkController.totalPlayers = numPlayers;
    }

    #region NotRequiredForNow
    // Game Controller functions -------------------------------------------------------------
    /*
    public GameObject[] GetPath()
    {
        GameObject[] path = new GameObject[43];

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

        return path;
    }
    */
    #endregion
}
