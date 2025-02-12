using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private Scoreboard m_scoreboard;

    private int m_killsStat;
    private int m_deathsStat;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Debug.Log("We have connected and spawned");
    }

    private void Start()
    {
        m_scoreboard = FindAnyObjectByType<Scoreboard>();
        
        if (IsOwner)
        {
            m_scoreboard.AddPlayerToList(this.OwnerClientId);
        }
    }
}

//m_scoreboard.PlayerUpdateKills(this.OwnerClientId, m_killsStat);
//m_scoreboard.PlayerUpdateDeaths(this.OwnerClientId, m_deathsStat);

//if a player disconnects, remove it from the scoreboard


//To do: player moving, sprinting, crouching and jumping