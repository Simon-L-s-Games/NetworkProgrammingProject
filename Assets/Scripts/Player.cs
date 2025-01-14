using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private Scoreboard m_scoreboard;

    private int m_killsStat;
    private int m_deathsStat;

    CinemachineVirtualCamera m_playerCamera;

    public override void OnNetworkSpawn()
    {
        Debug.Log("We have connected and spawned");
    }

    private void Start()
    {
        m_scoreboard = FindAnyObjectByType<Scoreboard>();
        m_playerCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
        
        if (IsOwner)
        {
            m_playerCamera.m_Follow = this.transform;
            m_scoreboard.AddPlayerToList(this.OwnerClientId);
        }
    }
}

//m_scoreboard.PlayerUpdateKills(this.OwnerClientId, m_killsStat);
//m_scoreboard.PlayerUpdateDeaths(this.OwnerClientId, m_deathsStat);

//if a player disconnects, remove it from the scoreboard