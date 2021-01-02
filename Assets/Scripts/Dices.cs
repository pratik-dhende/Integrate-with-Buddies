using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Dices : NetworkBehaviour
{
    [SyncVar] public int no1;
    [SyncVar] public int no2;

    public void roll()
    {
        no1 = Random.Range(1, 7);
        no2 = Random.Range(1, 7);
    }
}
