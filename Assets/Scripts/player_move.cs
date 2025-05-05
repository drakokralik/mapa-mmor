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

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float moveX = Input.GetAxis("Horizontal") * MovementSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * MovementSpeed * Time.deltaTime;

        if (moveY > 0)
        {
            AnimController.SetBool("walk_forward", true);
            MovementSpeed = 5.0f;
        }
        else if (moveY == 0)
        {
            AnimController.SetBool("walk_forward", false);
        }

        if (moveY < 0)
        {
            AnimController.SetBool("walk_back", true);
            MovementSpeed = 2.0f;
        }
        else if (moveY == 0)
        {
            AnimController.SetBool("walk_back", false);
        }


        if (moveX > 0)
        {
            AnimController.SetBool("walk_right", true);
            MovementSpeed = 3.0f;
        }
        else if (moveX == 0)
        {
            AnimController.SetBool("walk_right", false);
        }

        if (moveX < 0)
        {
            AnimController.SetBool("walk_left", true);
            MovementSpeed = 3.0f;
        }
        else if (moveX == 0)
        {
            AnimController.SetBool("walk_left", false);
        }



        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            moveZ = JumpHeight;
            AnimController.SetBool("jump", true);
        }
        else
        {
            moveZ = 0;
            AnimController.SetBool("jump", false);
        }


        Vector3 move = transform.right * moveX + transform.forward * moveY + transform.up * moveZ;


        if (moveX != 0 || moveY != 0)
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
