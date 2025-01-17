using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : NetworkBehaviour
{
    public float bulletSpeed = 10;

    private Rigidbody m_rb;

    public void Shoot(Vector3 direction)
    {
        m_rb = GetComponent<Rigidbody>();
        m_rb.linearVelocity = direction * bulletSpeed;
    }
}