using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    Scoreboard scoreboard;
    public override void OnNetworkSpawn()
    {
        scoreboard = FindAnyObjectByType<Scoreboard>();
        Debug.Log("We have connected and spawned");
        if (IsOwner)
        {
            scoreboard.AddPlayerToList(this.OwnerClientId);
        }
    }
}
