

                      // Made by VEOdev //

//                 THANK YOU FOR USING OUR ASSET                //
//     PLEASE RATE THE ASSET AND LEAVE A REVIEW IT HELP A LOT    //



using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

  #region Variables

    #region Inputs Variables

    [Header(">>>>>> Inputs")] //-----------------------------------------------------------------//
    [Space]

    [SerializeField] KeyCode run = KeyCode.LeftControl;
    [SerializeField] KeyCode jump = KeyCode.Space;
    [SerializeField] KeyCode dash = KeyCode.LeftShift;
    [SerializeField] KeyCode climb = KeyCode.Space;
    #endregion

    #region Movement Variables

    [Header(">>>>>> Movement")] //-----------------------------------------------------------------//
    [Space]

    [SerializeField] MovementType movementType = MovementType.Responsive;
    [SerializeField] RotationType rotationType = RotationType.RotateToMovementDirection;   // Chose how you want your character to rotate
    [SerializeField] enum MovementType
    {
        Responsive,                    // No acceleration, Fast responsive movement,
        Smooth,                        // Have small acceleration and decceleration when velocity changed
    }
    [SerializeField] enum RotationType
    {
        RotateToMovementDirection,
        RotateWithMouseClick,
        DoNotRotate,
    }
    public float RunSpeed = 500;
    public float WalkSpeed = 350;
    [SerializeField] [Range(0, 1f)] float airControl = 1; // 1 is full control in the air , 0 you cant control in the air
    [SerializeField] bool AlwaysRun = true;  // Always use run speed
    #endregion

    #region Jumping Variables

    [Header(">>>>>> Jumping")]  //-----------------------------------------------------------------//
    [Space]

    public float MaxJumpForce = 22;              // How High The Player Can Jump
    public int MaxAirJumps = 1;                     // Max Amount Of Air Jumps, Set It To 0 If You Dont Want To Jump In The Air
    public float Gravity = 6;                    // How Fast The Player Will Pulled Down To The Ground, 6 Feels Smooth
    [SerializeField] bool ControlJumpHeight = true; // Stop jumping when release jump key early
    [SerializeField] bool FasterFalling = true; // Get back to the ground from max height faster than from the ground to max height
    #endregion

    #region Dashing Variables

    [Header(">>>>>> Dashing")]//-----------------------------------------------------------------//
    [Space]

    [Range(1, 5f)] public float DashPower = 3;   // It Is A Speed Multiplyer, A Value Of 2 - 3 Is Recommended.
    public float DashDuration = 0.20f;           // Duration Of The Dash In Seconds, Recommended 0.20f.
    public float DashCooldown = 0.5f;            // Duration To Be Able To Dash Again.
    public bool enableDash = true;
    public bool enableAirDash = true;                  // Can Dash In Air ?
    [SerializeField] bool cameraShake = true;    // Shake The Camera While Dashing ? Fancy.
    #endregion

    #region Wall Jump Variables

    [Header(">>>>>> Wall Jump")] //-----------------------------------------------------------------//
    [Space]
    public float wallJumpPower = 25f;
    public float autoJumpDistance = 0.5f;        // Time to get control after wall jumping .. you automatically get control back when hitting the ground
    public bool EnableWallJump = true;

    #endregion

    #region Wall Slide Variables

    [Header(">>>>>> Wall Slide")] //-----------------------------------------------------------------//
    [Space]

    public float wallSlideSpeed = 2;              // Falling speed while sliding in the wall
    public bool EnableWallSlide = true;
    #endregion

    #region Climbing Variables

    [Header(">>>>>> Climbing ")] //-----------------------------------------------------------------//
    [Space]
    public float ClimbingSpeed = 10;
    public bool EnableClimbing = true;
    #endregion

    #region Falling Down Variables
    [Header(">>>>>> Falling ")] //-----------------------------------------------------------------//
    [Space]

    [SerializeField] float maxFallingSpeed = 20;
    [SerializeField] bool limitFallingSpeed = true;
    #endregion

    #region Tags & Layers & Detections
    [Header(">>>>>> Tags & Layers")]

    [SerializeField] LayerMask groundLayer;     // The Layers That Represent The Ground, Any Layer That You Want The Player To Be Able To Jump In
    [SerializeField] LayerMask wallLayer;
    [SerializeField] LayerMask ladderLayer;       // everything you want the player to be able to climb on need to have a ladder Layer.
    [Space]
    [SerializeField] string groundTag = "Ground";
    [SerializeField] string wallTag = "Ground";  // Anything that you want the player to be able to slide on or do a wall jump (you can use the ground tag too)
    [SerializeField] string ladderTag = "Ladder"; // everything you want the player to be able to climb on need to have a ladder Tag.
    [Space]
    [SerializeField] Transform wallDetection;    // empty gameobject in the right side of the collider to detect if the players colliding with the wall
    #endregion

    #region Private Variables
    // Private Variables
    float MoveDirection; // Right > 0 , Left < 0 , Not Moving = 0
    float currentSpeed;  // The current speed of the player in the ground.
    int currentJumps = 0;
    bool canMove = true;
    bool canDash = true;
    bool canClimb = false;
    bool canRotate;
    bool smoothMovement;
    #endregion

    #region Player State
    // Public Variables
    [HideInInspector] public bool DisableInput = false;  // Usefull it when pausing the game for exemple.
    [HideInInspector] public bool isLookingRight; // this is used by the camera manager script to know what direction the character is facing.
    [HideInInspector] public bool isInAir;        // is the character currently in the air or not.
    [HideInInspector] public bool isSliding;      // is the character sticking to the wall / sliding.
    [HideInInspector] public bool isRunning;      // is the character using the running speed.
    [HideInInspector] public bool isStanding;     // is the character in idle state.
    [HideInInspector] public bool isWalking;      // is the character using the wallking speed.
    [HideInInspector] public bool isFalling;      // is the character falling down.
    [HideInInspector] public bool isClimbing;     // is the character climbing in a ladder.
    [HideInInspector] public bool isDashing;      // is the character dashing.
    [HideInInspector] public bool isGrounded;     // is the character feet touching the ground.
    #endregion

    #region Calls
    // Calls
    CameraShake camShake;
    Rigidbody2D unitRigidbody;
    BoxCollider2D unitCollider; // Recommending Box Collider 2D
    #endregion

  #endregion

    // FUNCTIONS // 

    #region Unity Functions
    void Start()
    {
        GetComponenets();
        SetMovementAndRotationType();
    }   
    void Update()
    {
        if (DisableInput)
            return;

        GetMovementInput();
        SetMovementSpeed();
        SetRotation();
        SetMovementState();
        // Juice
        JumpingJuice();
        // INPUTS
        CheckInputs();
    }
    void FixedUpdate()
    {
        Move();
        WallSlide();
        CheckFalling();
        CheckGrounded();
    }
    #endregion

    #region On Start
    void GetComponenets()
    {
        // Calls
        unitRigidbody = GetComponent<Rigidbody2D>();
        unitCollider = GetComponent<BoxCollider2D>();
        camShake = Camera.main.GetComponent<CameraShake>();
    }
    
    void SetMovementAndRotationType()
    {
        // Set Gravity
        unitRigidbody.gravityScale = Gravity;

        // Set Rotation Type
        if (rotationType == RotationType.DoNotRotate)
            canRotate = false;
        else
            canRotate = true;

        // Set Movement Speed
        if (AlwaysRun)
            currentSpeed = RunSpeed;
        else
            currentSpeed = WalkSpeed;


        // Set Movement Type
        switch (movementType)
        {
            case MovementType.Responsive:
                smoothMovement = false;
                break;
            case MovementType.Smooth:
                smoothMovement = true;
                break;
        }
    }
    #endregion // Called On Start Functions

    #region Set Movement
    void GetMovementInput()
    {
        if (smoothMovement) MoveDirection = Input.GetAxis("Horizontal");
        else MoveDirection = (Input.GetAxisRaw("Horizontal"));
    }
    void SetMovementState()
    {
        if (MoveDirection == 0)
        {
            isStanding = true;
            isRunning = false;
            isWalking = false;
        }
        else
        {
            isStanding = false;

            if (currentSpeed == RunSpeed)
            {
                isRunning = true;
                isWalking = false;
            }
            else if (currentSpeed == WalkSpeed)
            {
                isRunning = false;
                isWalking = true;
            }
        }
    }
    void SetMovementSpeed()
    {
        if (!AlwaysRun)
        {
            if (Input.GetKeyDown(run))
                Run();

            if (Input.GetKeyUp(run))
                Walk();
        }
    }
    #endregion // Set Movement

    #region Checks
    void CheckInputs()
    {
        // Jumping Input
        if (Input.GetKeyDown(jump)) Jump();

        // Dashing Input
        if (Input.GetKeyDown(dash)) Dash();

        // Climbing Input
        if (Input.GetKey(climb)) Climb();
    }
    void CheckGrounded()
    {
        // Make sure you set the ground layer to the ground
        RaycastHit2D ray;

        if (transform.rotation.y == 0)
        {
            Vector2 position = new Vector2(unitCollider.bounds.center.x - unitCollider.bounds.extents.x, unitCollider.bounds.min.y);
            ray = Physics2D.Raycast(position, Vector2.down, unitCollider.bounds.extents.y + 0.02f, groundLayer);
        }
        else
        {
            Vector2 position = new Vector2(unitCollider.bounds.center.x + unitCollider.bounds.extents.x, unitCollider.bounds.min.y);
            ray = Physics2D.Raycast(position, Vector2.down, unitCollider.bounds.extents.y + 0.02f, groundLayer);
        }

        if (ray.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    void CheckFalling()
    {
        if (isSliding || isClimbing)
            return;

        if (isGrounded)
        {
            isInAir = false;
            isFalling = false;
        }
        else
        {
            isInAir = true;

            if (unitRigidbody.velocity.y < 0)
            {
                isFalling = true;

                // Fall Animation Trigger                                      (ANIMATION)

                if (limitFallingSpeed) // Limit the fall speed
                {
                    if (unitRigidbody.velocity.y <= -maxFallingSpeed)
                        unitRigidbody.velocity = new Vector3(unitRigidbody.velocity.x, -maxFallingSpeed, unitRigidbody.velocity.y);
                }

            }
            else
                isFalling = false; // Fall Animation Trigger Off               (ANIMATION)
        }
       
    }
    #endregion // Checks

    #region Movement & Rotation
    void Move()
    {
        if (!canMove)
            return;

        if(!isInAir)
            unitRigidbody.velocity = new Vector2(MoveDirection * currentSpeed * Time.fixedDeltaTime, unitRigidbody.velocity.y);
        else
            unitRigidbody.velocity = new Vector2(MoveDirection * (airControl * currentSpeed) * Time.fixedDeltaTime, unitRigidbody.velocity.y);
    } 
    void Walk()
    {
        isRunning = false;
        isWalking = true;
        currentSpeed = WalkSpeed;
    }
    void Run()
    {
        isRunning = true;
        isWalking = false;
        currentSpeed = RunSpeed;
    }
    #endregion  // Movement

    #region Jump
    void Jump()
    {
        if (isClimbing)
            return;

        if (!isSliding)
        {
            if (!isInAir) GroundJump();
            else AirJump();
        }
        else 
        {
            WallJump();
        }
    }
    void GroundJump()
    {
        // Here You add ground Jumping ANIMATION Or the trigger                    (ANIMATION)

        unitRigidbody.velocity = Vector2.up * MaxJumpForce;
        isInAir = true;
        canMove = true;
    }
    void AirJump()
    {
        if (currentJumps >= MaxAirJumps)
            return;

        // Here You add Air Jumping ANIMATION                               (ANIMATION)

        unitRigidbody.gravityScale = Gravity;
        unitRigidbody.velocity = new Vector2(0,0);
        unitRigidbody.velocity = Vector2.up * MaxJumpForce;
        currentJumps++;
    }
    async void WallJump()
    {
        if (!EnableWallJump)
            return;

        unitRigidbody.velocity = Vector2.zero;

        unitRigidbody.AddForce(new Vector2(wallJumpPower / 2 * -MoveDirection, wallJumpPower), ForceMode2D.Impulse);
        DisableInput = true;

        if (canRotate)
        {
            MoveDirection = -MoveDirection;

            if (MoveDirection > 0)
            {
                isLookingRight = true;
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else if (MoveDirection < 0)
            {
                isLookingRight = false;
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
        }


        await Task.Delay((int)(autoJumpDistance * 1000)); // duration after you get control back 

        DisableInput = false;
        MoveDirection = -(MoveDirection);
    }
    void JumpingJuice() // Make Jump Feel Better
    {
        if (isDashing)
            return;


        if (isInAir && !isFalling && !Input.GetKey(jump)) // Stop Jumping when release jump button
        {
            if (ControlJumpHeight)
                unitRigidbody.velocity += Vector2.up * -140 * Time.deltaTime;
        }

        if (FasterFalling && isFalling)
            unitRigidbody.velocity += Vector2.up * -70 * Time.deltaTime;
    }
    #endregion // Jump

    #region Sliding & Climbing
    void WallSlide()
    {
        // Make sure you set the wall layer in anything you want character to be able to do wall jump/slide on

        bool touching = Physics2D.OverlapCircle(wallDetection.position, 0.2f, wallLayer);

        if (touching && !isGrounded && MoveDirection != 0)
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }

        if (!EnableWallSlide)
            return;

        if (isSliding)
            unitRigidbody.velocity = new Vector2(unitRigidbody.velocity.x, Mathf.Clamp(unitRigidbody.velocity.y, -wallSlideSpeed, float.MaxValue));

    }
    void Climb()
    {
        if (!canClimb)
            return;

        bool touching = Physics2D.OverlapCircle(transform.position, 1f, ladderLayer);

        if (touching)
        {
            unitRigidbody.velocity = new Vector2(unitRigidbody.velocity.x, Mathf.Clamp(unitRigidbody.velocity.y, ClimbingSpeed, float.MaxValue));
            isClimbing = true;
            isInAir = false;
            isFalling = false;
        }
        else
        {
            canClimb = false;
            isClimbing = false;
            unitRigidbody.velocity = Vector2.up * 20;
        }
    }
    #endregion // Slide & Climb

    #region Dashing
    void Dash()
    {
        if (!enableDash)
            return;

        if (MoveDirection != 0 && canDash)
        {
            if (!enableAirDash && !isGrounded)
                return;

            StartCoroutine(Dashing());
        }
    }
    void StartDash()
    {
        isDashing = true;
        canDash = false;

        currentSpeed *= DashPower;

        unitRigidbody.gravityScale = 0f; // You can delete this line if you don't want the player to not fall down while dashing
        unitRigidbody.velocity = new Vector2(unitRigidbody.velocity.x, 0);
    }
    void EndDash()
    {
        if (cameraShake) camShake.Shake();

        unitRigidbody.gravityScale = Gravity;
        currentSpeed /= DashPower;
        isDashing = false;
    }
    IEnumerator Dashing()
    {
        StartDash();
        yield return new WaitForSeconds(DashDuration);
        EndDash();
        yield return new WaitForSeconds(DashCooldown - DashDuration);
        canDash = true;
    }
    #endregion // Dash

    #region Rotation
    // Make Player Fasing The Mouse Cursor , Can Be Called On Update , Or When The Player Attacks He Will Turn To The Mouse Direction
    void SetRotation()
    {
        switch (rotationType)
        {
            case RotationType.RotateToMovementDirection:
                RotateToMoveDirection();
                break;
            case RotationType.RotateWithMouseClick:
                RotateToMouse();
                break;
            case RotationType.DoNotRotate:
                break;
        }
    }
    void RotateToMoveDirection()
    {
        if (MoveDirection != 0 && canMove)
        {
            if (MoveDirection > 0)
            {
                if (isLookingRight == false) { } // OPTIONAL ANIMATION TURN RIGHT          (ANIMATION)

                isLookingRight = true;
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else if (MoveDirection < 0)
            {
                if (isLookingRight == true) { } // OPTIONAL ANIMATION TURN LEFT            (ANIMATION)
 
                isLookingRight = false;
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
        }
    }
    void RotateToMouse()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0))
            return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        Vector2 myPos = transform.position;

        Vector2 dir = mousePos - myPos;

        if (dir.x < 0)
        {
            if (isLookingRight == false) { } // OPTIONAL ANIMATION TURN RIGHT              (ANIMATION)

            isLookingRight = false;
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            if (isLookingRight == true) { } // OPTIONAL ANIMATION TURN LEFT                (ANIMATION)

            isLookingRight = true;
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }
    #endregion // Rotation

    #region Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reset Jump Counts When Collide With The Ground
        if (collision.collider.CompareTag(wallTag) || collision.collider.CompareTag(groundTag))
        {
            RaycastHit2D ray;
            ray = Physics2D.Raycast(unitCollider.bounds.center, Vector2.down, unitCollider.bounds.extents.y + 0.02f, groundLayer);

            if (ray.collider != null)
            {
                isInAir = false;
                currentJumps = 0;
                isClimbing = false;
                isFalling = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // On enter ladder
        if (collision.CompareTag(ladderTag))
        {
            canClimb = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // On exit ladder
        if (collision.CompareTag(ladderTag))
        {
            canClimb = false;
            isClimbing = false;
        }
    }
    #endregion // Collisions
}
