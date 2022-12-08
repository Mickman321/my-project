using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float walkSpeed;
    public float sprintSpeed;

    Vector3 velocity;

    public float gravity = -9.81f;

    public Transform groundCheck;

    public float groundDistance = 0.4f;

    public LayerMask groundMask;

    bool isGrounded;



    // det dem h�r variablerna g�r �r att, kolla gravitation och �ka velocity, hur mycket distance du �r fr�n marken, om spelaren �r p� marken, att referera till character controller i unity, hur mycket fart spelaren har.
    //Animations skapad av Niljas och scriptade, Anv�nds dock inte eftersom jag anv�nder en annan script f�r animations.
    private bool isMovingForward = false;
    private bool isMovingBackwards = false;

    Animator Am;
    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        air
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //Debug.DrawRay(groundCheck.position, -Vector3.up * groundDistance);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }// Det den h�r koden g�r �r att f� spelaren att g� fram, back, v�nster, h�ger och kolla gravitation

    private void StateHandler()
    {
        // Mode - Sprinting
       /* if(isGrounded) && Input.GetKey(sprintKey))
        {
        
        }*/
    }

}


