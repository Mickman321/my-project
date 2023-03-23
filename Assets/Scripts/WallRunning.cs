using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    public CharacterController controller;

    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    private bool upwardsRunning;
    private bool downwardsRunning;
    private float horizontalInput;
    private float verticalInput;
    

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [SerializeField]
    private float jumpTimeCounter;
    [SerializeField]
    public float jumpTime;
    private bool isJumping;
    private float jumpForce;
    public float jumpHeight = 10f;

    [Header("References")]
    public Transform orientation;
    private PlayerMovement pm;
    private CharacterController cc;
    private Rigidbody rb;


    Vector3 vector = new Vector3(1, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)
            WallRunningMovement();
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        // Getting Inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        // State 1 - Wallrunning
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround())
        {
            if (!pm.wallrunning)
                StartWallRun();
            // if (Input.GetButton("Jump") && isJumping == true) /* Den här koden ser till så att när spelaren trycker på space och inte håller ner space
            //så blir det ett kortare hopp och den ser också till så att det inte funkar i luften.*/
            /* {


                 if (jumpTimeCounter > 0)
                 {
                     print("continue jump");
                     pm.velocity = Vector3.up * jumpHeight;
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
             }*/
            if (Input.GetButton("Jump"))
            {
                //  velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); 

                print("ground jump");
                isJumping = true;
                jumpTimeCounter = jumpTime;
                pm.velocity = Vector3.up * jumpHeight;


            }
        }

        // State 3 - None
        else
        {
            if (pm.wallrunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;
    }

    private void WallRunningMovement()
    {
       // cc.useGravity = false; //stäng av gravity
        pm.gravity = 0;
       // cc.
        pm.velocity = new Vector3(pm.velocity.x, 0f, pm.velocity.z);

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        // forward force
            rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        // upwards/downwards force
       /* if (upwardsRunning)
            pm.velocity = new Vector3(pm.velocity.x, wallClimbSpeed, pm.velocity.z);
        if (downwardsRunning)
            pm.velocity = new Vector3(pm.velocity.x, -wallClimbSpeed, pm.velocity.z);*/

        // push to wall force
        /*if(Physics.CheckSphere(position,5f,"Wall"))
            ()*/

        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0)) 
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
    }

    private void StopWallRun()
    {
        pm.wallrunning = false;
        pm.gravity = -35f;
    }
}
