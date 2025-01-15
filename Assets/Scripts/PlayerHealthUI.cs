using UnityEngine;
using Unity.Netcode;

public class PlayerHealthUI : MonoBehaviour
{
    PlayerHealth playerNetworkHealth;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnDestroy()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        playerNetworkHealth = FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None)[0];
        playerNetworkHealth.OnHealthChanged += OnPlayerHealthChanged;
    }

    private void OnClientDisconnected(ulong clientId)
    {
        playerNetworkHealth.OnHealthChanged -= OnPlayerHealthChanged;
    }

    private void OnPlayerHealthChanged(float newHealthValue)
    {
        //update the text "Health: " + newHealthValue
    }
}
