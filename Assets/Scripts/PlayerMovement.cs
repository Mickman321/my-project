using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 24f;
    public float walkSpeed;
    public float climbSpeed;
    // public float sprintSpeed;
    //[SerializeField, Range(1, 10)]
    //float sprintSpeed;
    public float jumpHeight = 10f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Vector3 velocity;

    public float gravity = -9.81f;

    public Transform groundCheck;

    public float groundDistance = 0.4f;

    public LayerMask groundMask;

    public bool isGrounded;
    public bool climbing;

    [SerializeField]
    private float jumpTimeCounter;
    [SerializeField]
    public float jumpTime;
    private bool isJumping;
    private float jumpForce;
    public float wallrunSpeed;
    public bool wallrunning;

    // det dem här variablerna gör är att, kolla gravitation och öka velocity, hur mycket distance du är från marken, om spelaren är på marken, att referera till character controller i unity, hur mycket fart spelaren har.
    //Animations skapad av Niljas och scriptade, Används dock inte eftersom jag använder en annan script för animations.
    private bool isMovingForward = false;
    private bool isMovingBackwards = false;

    Animator Am;
    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        climbing,
        wallrunning,
        air,
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


        /* if (Input.GetButtonDown("Jump") && isGrounded)
         {
             //  velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); 

             print("ground jump");
             isJumping = true;
             jumpTimeCounter = jumpTime;
             velocity = Vector3.up * jumpHeight;


         }
        
           if (Input.GetButtonDown("Jump") && isGrounded)
           {
        
       
       
           }

         */

        if (isGrounded == true && Input.GetButtonDown("Jump")) // Den här kollar om spelaren är på marken om den är så ska man kunna man kunna trycka på space för att hoppa.
        {
            print("ground jump");
            isJumping = true;
            jumpTimeCounter = jumpTime;
            velocity = Vector3.up * jumpHeight;
        }

        if (Input.GetButton("Jump") && isJumping == true) /* Den här koden ser till så att när spelaren trycker på space och inte håller ner space
                                                               så blir det ett kortare hopp och den ser också till så att det inte funkar i luften.*/
        {


            if (jumpTimeCounter > 0)
            {
                print("continue jump");
                velocity = Vector3.up * jumpHeight;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                print("nej jump");
                isJumping = false;
            }
        }
        else
        {
            isJumping = false;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }// Det den här koden gör är att få spelaren att gå fram, back, vänster, höger och kolla gravitation

    private void StateHandler()
    {
        // Mode - wallrunning
        if(wallrunning)
        {
            state = MovementState.wallrunning;
            speed = wallrunSpeed;
        }
    }

}


