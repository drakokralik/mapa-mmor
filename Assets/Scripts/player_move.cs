using System;
using UnityEngine;
using Mirror;

public class player_move : NetworkBehaviour
{
    public Transform Shoulders;
    public Transform CameraTransform;
    public float TurnSpeed = 5.0f;
    public float MovementSpeed = 5.0f;
    public float JumpHeight = 1.0f;

    public Transform GroundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;
    public float Gravity = -9.81f;
    public Animator AnimController;
    Vector3 velocity;
    public bool isGrounded;
    float moveZ;

    private CharacterController characterController;

    [Header("Zvuky")]
    public AudioSource stepAudio;
    public AudioClip jumpSound;

    [HideInInspector]
    public bool CanMove;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (stepAudio == null)
            stepAudio = GetComponent<AudioSource>();

        if (!isLocalPlayer && CameraTransform != null)
        {
            CameraTransform.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!isLocalPlayer || !CanMove) return;

        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxis("Horizontal") * MovementSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * MovementSpeed * Time.deltaTime;
        bool isMoving = Mathf.Abs(moveX) > 0.01f || Mathf.Abs(moveY) > 0.01f;

        if (isMoving && isGrounded)
        {
            if (!stepAudio.isPlaying)
                stepAudio.Play();
        }
        else
        {
            if (stepAudio.isPlaying)
                stepAudio.Stop();
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            AnimController.SetBool("jump", true);

            if (jumpSound != null)
                AudioSource.PlayClipAtPoint(jumpSound, transform.position);
        }
        else
        {
            AnimController.SetBool("jump", false);
        }

        AnimController.SetBool("walk_forward", moveY > 0);
        AnimController.SetBool("walk_back", moveY < 0);
        AnimController.SetBool("walk_right", moveX > 0);
        AnimController.SetBool("walk_left", moveX < 0);

        if (CameraTransform != null)
        {
            Vector3 cameraForward = CameraTransform.forward;
            cameraForward.y = 0;
            if (cameraForward.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, TurnSpeed * Time.deltaTime);
            }
        }

        Vector3 move = transform.right * moveX + transform.forward * moveY;
        velocity.y += Gravity * Time.deltaTime;

        characterController.Move(move);
        characterController.Move(velocity * Time.deltaTime);
    }
}
