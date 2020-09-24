using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool alive = false;
    string posType;
    bool qualified;

    // Public functions -----------------------------------------------------------------------

    public void move(Vector3 pos)
    {
        transform.position = pos;
        // Update player attributes.
    }

    // Private functions -------------------------------------------------------------------------

    private void updatePlayer(bool alive, string posType, bool qualified)
    {
        this.alive = alive;
        this.posType = posType;
        this.qualified = qualified;
    }
}
