using Unity.Netcode;
using UnityEngine.Events;

public class PlayerHealth : NetworkBehaviour
{
    private NetworkVariable<float> m_playerHealth = new(100f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server); 
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

    public void TakeDamage(float damage)
    {
        m_playerHealth.Value -= damage;

        CheckHealth();
    }

    private void CheckHealth()
    {
        if(m_playerHealth.Value <= 0)
        {
            GetComponent<NetworkObject>().Despawn();
        }
    }
}
