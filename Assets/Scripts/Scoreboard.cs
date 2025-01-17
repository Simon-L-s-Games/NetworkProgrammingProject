using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public enum StatUpdateType
{
    Kills, Deaths
}

public class Scoreboard : NetworkBehaviour
{
    [SerializeField] private Dictionary<ulong, GameObject> m_playerStatsObjects = new();
    [SerializeField] private Dictionary<ulong, TextMeshProUGUI> m_playerKillsTexts = new();
    [SerializeField] private Dictionary<ulong, TextMeshProUGUI> m_playerDeathsTexts = new();

    [SerializeField] private GameObject m_playerStatsPrefab;
    [SerializeField] private GameObject m_playerListHolder;
    [SerializeField] private GameObject m_scoreboardObject;
    private NetworkManager m_networkManager;

    private int m_killsIndex;
    private int m_deathsIndex;

    public Action<ulong, int> PlayerUpdateDeaths;
    public Action<ulong, int> PlayerUpdateKills;

    

    private void Start()
    {
        m_networkManager = FindAnyObjectByType<NetworkManager>();

        PlayerUpdateDeaths += UpdatePlayerDeathsStat;
        PlayerUpdateKills += UpdatePlayerKillsStat;
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
        m_playerKillsTexts.Add(clientId, playerStatsObject.transform.GetChild(m_killsIndex).GetComponent<TextMeshProUGUI>());
        m_playerDeathsTexts.Add(clientId, playerStatsObject.transform.GetChild(m_deathsIndex).GetComponent<TextMeshProUGUI>());
    }

    private void UpdatePlayerKillsStat(ulong clientId, int kills)
    {
        m_playerKillsTexts[clientId].text = kills.ToString();
    }

    private void UpdatePlayerDeathsStat(ulong clientId, int deaths)
    {
        m_playerDeathsTexts[clientId].text = deaths.ToString();
    }
}