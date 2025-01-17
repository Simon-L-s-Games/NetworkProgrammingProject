using UnityEngine;
using Unity.Netcode;

public class BulletServerCollisionCheck : NetworkBehaviour
{
    public float bulletDamage = 20;

    public override void OnNetworkSpawn()
   {
        base.OnNetworkSpawn();
        if (!IsServer)
        {
            enabled = false;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player") && IsOwner)
    //    {
    //        other.GetComponent<PlayerHealth>().TakeDamage(bulletDamage);
    //        GetComponent<NetworkObject>().Despawn();
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && IsOwner)
        {
            collision.gameObject.GetComponentInParent<PlayerHealth>().TakeDamage(bulletDamage);
            GetComponent<NetworkObject>().Despawn();
        }
        else if (collision.gameObject.activeInHierarchy && IsOwner)
        {
            GetComponent<NetworkObject>().Despawn();
        }
    }
}
