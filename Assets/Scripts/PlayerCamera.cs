using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Unity.Netcode;

public class PlayerCamera : NetworkBehaviour
{
    public Transform player;
    private Transform playerCameraTransform;

    public Vector3 cameraPlacementInPlayer;
    private Vector2 m_lookDirection;

    private float cameraVerticalRotation;
    public float mouseSensitivty = 100f;

    public void OnLook(InputAction.CallbackContext context)
    {
        m_lookDirection = context.ReadValue<Vector2>();
        m_lookDirection /= 10 * mouseSensitivty;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        enabled = IsClient;
        if (!IsOwner)
        {
            enabled = false;
            return;
        }

        //player = GetComponent<Transform>();
        playerCameraTransform = FindAnyObjectByType<Camera>().transform;
        playerCameraTransform.parent = this.transform;
        playerCameraTransform.localPosition = cameraPlacementInPlayer;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        cameraVerticalRotation -= m_lookDirection.y;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90, 90);
        playerCameraTransform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        player.Rotate(Vector3.up * m_lookDirection.x);
    }
}
