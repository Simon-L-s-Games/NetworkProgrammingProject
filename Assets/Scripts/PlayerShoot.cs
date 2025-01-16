using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : NetworkBehaviour
{
    public GameObject bulletPrefab;
    private PlayerCamera playerCamera;
    private Camera cameraComponent;
    [SerializeField] private LayerMask aimColliderLayerMask = new();

    public float shootCooldown;
    private float m_timeSinceLastShot;
    private Vector3 shootDirection;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    private void Update()
    {
        m_timeSinceLastShot += Time.deltaTime;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (m_timeSinceLastShot < shootCooldown) return;
        m_timeSinceLastShot = 0;

        if(cameraComponent == null)
        {
            cameraComponent = FindAnyObjectByType<Camera>();
        }

        Debug.Log(playerCamera);
        Debug.Log(cameraComponent);

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = cameraComponent.ScreenPointToRay(screenCenterPoint);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, 999f))
        {
            shootDirection = hitInfo.point - cameraComponent.transform.position;
            shootDirection.Normalize();
        }

        if (IsOwner)
        {
            SpawnBulletServerRpc();
        }
    }

    [ServerRpc]
    private void SpawnBulletServerRpc()
    {
        if (playerCamera == null)
        {
            playerCamera = GetComponent<PlayerCamera>();
        }

        GameObject bullet = GameObject.Instantiate(bulletPrefab);
        bullet.transform.position = playerCamera.player.transform.position + playerCamera.cameraPlacementInPlayer + playerCamera.transform.forward;
        bullet.GetComponent<Bullet>().Shoot(shootDirection);
        bullet.GetComponent<NetworkObject>().Spawn();
    }
}
