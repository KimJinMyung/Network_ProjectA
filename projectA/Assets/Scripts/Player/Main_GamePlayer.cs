using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Main_GamePlayer : NetworkBehaviour
{
    private void Update()
    {
        Debug.Log(this.isLocalPlayer);
    }
}
