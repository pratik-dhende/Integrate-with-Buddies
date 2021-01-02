using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

[AddComponentMenu("")]
public class NetworkRoomPlayerGame : NetworkRoomPlayer
{
    static readonly ILogger logger = LogFactory.GetLogger(typeof(NetworkRoomPlayerGame));

    public override void OnStartClient()
    {
        if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "OnStartClient {0}", SceneManager.GetActiveScene().path);

        base.OnStartClient();
    }

    public override void OnClientEnterRoom()
    {
        if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "OnClientEnterRoom {0}", SceneManager.GetActiveScene().path);
    }

    public override void OnClientExitRoom()
    {
        if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "OnClientExitRoom {0}", SceneManager.GetActiveScene().path);
    }

    public override void ReadyStateChanged(bool _, bool newReadyState)
    {
        if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "ReadyStateChanged {0}", newReadyState);
    }
}
