using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;

public class PlayerHealth : NetworkBehaviour
{
    private NetworkVariable<float> m_playerHealth = new(100f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner); 
    public UnityAction<float> OnHealthChanged;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        m_playerHealth.OnValueChanged += OnHealthValueChanged;
    }
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        m_playerHealth.OnValueChanged -= OnHealthValueChanged;
    }

    private void OnHealthValueChanged(float oldValue, float newValue)
    {
        OnHealthChanged?.Invoke(newValue);
    }
}
