using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Scoreboard : NetworkBehaviour
{
    [SerializeField] private Dictionary<ulong, GameObject> m_playerStatsObjects = new();
    [SerializeField] private GameObject m_playerStatsPrefab;
    [SerializeField] private GameObject m_playerListHolder;
    [SerializeField] private GameObject m_scoreboardObject;
    private NetworkManager m_networkManager;

    private void Start()
    {
        m_networkManager = FindAnyObjectByType<NetworkManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) m_scoreboardObject.SetActive(!m_scoreboardObject.activeInHierarchy);
    }

    public void AddPlayerToList(ulong clientId)
    {
        GameObject playerStatsObject = Instantiate(m_playerStatsPrefab, m_playerListHolder.transform);
        NetworkObject networkObject = playerStatsObject.GetComponent<NetworkObject>();
        networkObject.Spawn();

        m_playerStatsObjects.Add(clientId, playerStatsObject);
    }

    private void UpdatePlayerStats(ulong clientId, )
    {
        //enum to check what should be updated, switch case depending type
        //index variable per stat so it's easier to read and not type the wrong index
    }
}