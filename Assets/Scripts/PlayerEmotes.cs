using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerEmotes : NetworkBehaviour
{
    [SerializeField] private float m_emoteLifetime;
    [SerializeField] private GameObject m_emotePrefab;
    [SerializeField] private Vector3 m_emoteLocalPosition;

    private float m_emoteLifetimeTimer;

    private GameObject m_spawnedEmote;
    private bool m_emoteActive = false;

    public void OnEmote(InputAction.CallbackContext context)
    {
        if (IsOwner)
        {
            Debug.Log("Detected another input");
            SpawnEmoteRpc();
        }
    }

    [Rpc(SendTo.Server)]
    private void SpawnEmoteRpc()
    {
        if (m_emoteActive) return;
        m_emoteLifetimeTimer = m_emoteLifetime;

        m_emoteActive = true;
        m_spawnedEmote = GameObject.Instantiate(m_emotePrefab);
        m_spawnedEmote.transform.localPosition = m_emoteLocalPosition;
        m_spawnedEmote.GetComponent<NetworkObject>().Spawn();

        Debug.Log("Spawning emote " + m_emoteActive);

        StartCoroutine(EmoteDespawnTimer());
    }

    private void DespawnEmote()
    {
        m_emoteActive = false;
        m_spawnedEmote.GetComponent<NetworkObject>().Despawn();
        Debug.Log("Despawning emote" + m_emoteActive);
    }

    private IEnumerator EmoteDespawnTimer()
    {
        while(m_emoteLifetimeTimer > 0)
        {
            m_emoteLifetimeTimer -= Time.deltaTime;
            m_spawnedEmote.transform.position = transform.position + m_emoteLocalPosition;

            yield return null;
        }

        DespawnEmote();
    }
}