using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 12f;
    public float walkSpeed;
    public float sprintSpeed;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

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

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(x, 0f, z).normalized;

       if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);


            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        /*Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);*/

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


