using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    PlayerHealth playerNetworkHealth;
    public TextMeshProUGUI m_playerHealthText;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        m_playerHealthText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
    }

    private void Update()
    {
        if(playerNetworkHealth != null)
        {
            playerNetworkHealth.OnHealthChanged += OnPlayerHealthChanged;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (playerNetworkHealth != null) return;

        PlayerHealth[] playerHealthCodes = FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None);

        foreach (PlayerHealth p in playerHealthCodes)
        {
            if (p.IsOwner)
            {
                playerNetworkHealth = p;
                break;
            }
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        playerNetworkHealth.OnHealthChanged -= OnPlayerHealthChanged;
    }

    private void OnPlayerHealthChanged(float newHealthValue)
    {
        m_playerHealthText.text = newHealthValue.ToString();
    }
}
