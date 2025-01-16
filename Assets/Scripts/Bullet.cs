using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : NetworkBehaviour
{
    public float bulletSpeed = 10;
    public float bulletDamage = 20;

    public float bulletLifetime;
    private float m_bulletLifetimeTimer;

    private Rigidbody m_rb;

    private void Update()
    {
        m_bulletLifetimeTimer += Time.deltaTime;

        if(m_bulletLifetimeTimer > bulletLifetime && gameObject.activeInHierarchy && IsOwner)
        {
            DespawnBulletServerRpc();
        }
    }

    public void Shoot(Vector3 direction)
    {
        m_rb = GetComponent<Rigidbody>();
        m_rb.linearVelocity = direction * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && IsOwner)
        {
            other.GetComponent<PlayerHealth>().TakeDamage(bulletDamage);
            DespawnBulletServerRpc();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.activeInHierarchy && IsOwner)
        {
            DespawnBulletServerRpc();
        }
    }

    [ServerRpc]
    private void DespawnBulletServerRpc()
    {
        GetComponent<NetworkObject>().Despawn();
    }
}