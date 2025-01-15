using System;
using StarterAssets;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using UnityEngine.EventSystems;
public class ClientPlayerMove : NetworkBehaviour
{
    [SerializeField]
    CharacterController m_CharacterController;
    [SerializeField]
    ThirdPersonController m_ThirdPersonController;
    [SerializeField]
    PlayerInput m_PlayerInput;

    private Vector3 m_moveDirection;
    private Vector2 m_moveInput;
    public float moveSpeed;
    private Rigidbody m_rigidbody;
    private Camera m_playerCamera;

    [SerializeField]
    Transform m_CameraTransform;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_playerCamera = FindAnyObjectByType<Camera>();
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
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_moveInput = context.ReadValue<Vector2>();
        m_moveDirection = new Vector3(m_moveInput.x, 0, m_moveInput.y);
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        if (!IsOwner) return;

        if (m_rigidbody.linearVelocity.magnitude > moveSpeed)
        {
            m_rigidbody.linearVelocity = Vector3.ClampMagnitude(m_rigidbody.linearVelocity, moveSpeed);
        }
        else if (m_rigidbody.linearVelocity.magnitude < 0)
        {
            m_rigidbody.linearVelocity = Vector3.ClampMagnitude(m_rigidbody.linearVelocity, 0);
        }

        Vector3 forward = m_playerCamera.transform.forward;
        Vector3 right = m_playerCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 CameraRelativeMovement = (m_moveDirection.z * forward + m_moveDirection.x * right);

        if (m_moveInput.magnitude == 0)
        {
            m_rigidbody.linearVelocity -= new Vector3(CameraRelativeMovement.x * moveSpeed, 0, CameraRelativeMovement.z * moveSpeed);
        }
        else
        {
            m_rigidbody.linearVelocity += new Vector3(CameraRelativeMovement.x * moveSpeed, m_rigidbody.linearVelocity.y, CameraRelativeMovement.z * moveSpeed);
        }
    }
}