using System;
using System.ComponentModel;
using UnityEngine;

public class player_move : MonoBehaviour
{
    public Transform Shoulders;
    public float TurnSpeed = 0.1f;
    public float MovementSpeed = 5.0f;
    public float JumpHeight = 1.0f;

    public Transform GroundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;
    public float Gravity;
    public Animator AnimController;
    Vector3 velocity;
    public bool isGrounded;
    float moveZ;

    private CharacterController characterController;

    [Header("Zvuky")]
    public AudioSource stepAudio;
    public AudioClip jumpSound;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Pokud AudioSource není ruènì nastavený, zkusíme ho automaticky najít
        if (stepAudio == null)
            stepAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxis("Horizontal") * MovementSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * MovementSpeed * Time.deltaTime;
        bool isMoving = Mathf.Abs(moveX) > 0.01f || Mathf.Abs(moveY) > 0.01f;

        // Zvuk krokù
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

        // Zvuk a animace pro skok
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            moveZ = JumpHeight;
            AnimController.SetBool("jump", true);

            if (jumpSound != null)
                AudioSource.PlayClipAtPoint(jumpSound, transform.position);
        }
        else
        {
            moveZ = 0;
            AnimController.SetBool("jump", false);
        }

        // Animace chùze
        AnimController.SetBool("walk_forward", moveY > 0);
        AnimController.SetBool("walk_back", moveY < 0);
        AnimController.SetBool("walk_right", moveX > 0);
        AnimController.SetBool("walk_left", moveX < 0);

        if (moveY > 0) MovementSpeed = 5.0f;
        if (moveY < 0) MovementSpeed = 2.0f;
        if (moveX != 0 && moveY == 0) MovementSpeed = 3.0f;

        Vector3 move = transform.right * moveX + transform.forward * moveY + transform.up * moveZ;

        if (isMoving)
        {
            Vector3 currentAngles = transform.eulerAngles;
            float targetY = Shoulders.eulerAngles.y;
            float newY = Mathf.LerpAngle(currentAngles.y, targetY, TurnSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(currentAngles.x, newY, currentAngles.z);
        }

        characterController.Move(move);
        velocity.y += Gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
