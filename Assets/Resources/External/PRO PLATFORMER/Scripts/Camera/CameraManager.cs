


                                                     // Made by VEOdev //

                              //                THANK YOU FOR USING OUR ASSET                 //
                              //     PLEASE RATE THE ASSET AND LEAVE A REVIEW IT HELP ALOT    //



                   // NOTE : This script need to be attached to the camera parent and not to the camera main //



using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [Header("The Target To Follow")]
    [SerializeField] Transform target; // Target To Follow

    [Header("Properties")]
    [SerializeField] float height = 0f; // The Offset Of The Target In The Y Axis
    [SerializeField] float horizontalSpeed = 3.5f; // The Horizontal Follow Speed : Higher Value = Fast Camera Follow = Less Smothness , Small Value = More Smothness But Slow Camera
    [SerializeField] float verticalSpeed = 7f; // The vertical Follow Speed : recommanded a high value so the player will not go off screen if he is falling long distance
    [Header("Lock The Camera Movement")]
    [SerializeField] bool lockVerticalMovement = false; // If The Camera Should Move In The Y Axis Or Not
    [SerializeField] bool lockHorizontalMovement = false; // If The Camera Should Move In The Y Axis Or Not

    [Header("Camera Follow Type")]
    [SerializeField] MovementType movementType = MovementType.Default; // Chose How You Want The Camera To Follow You
    [SerializeField] float TurnDistance = 5; // How fast you want the camera to look forward

    bool clicked;
    int right = 0;

    Vector3 verticalPos;
    Vector3 horizontalPos;

    public enum MovementType // Chose How You Want The Camera To Follow You
    {
        Default, // Player in the middle
        TurnWithPlayerDirection, // Camera turn more to the direction of the player is facing
        TurnToMouseClick, // Camera turn to the direction of the mouse when clicking
        TurnToMousePosition, // Camera turn more to the direction of the mouse
    }

    private PlayerController player;

    Vector3 NewCameraPos; // Declaring The New Camera Position That We Will Change It In The FollowPlayer Method

    private void Start()
    {
        player = target.GetComponent<PlayerController>();
        transform.position = new Vector3(target.position.x, target.position.y + height , transform.position.z);
    }
    private void FixedUpdate()
    {
        FollowPlayer(); // Calling FollowPlayer In The Fixed Update Because In Some Reasons Doing It In Updates Or LateUpdates Makes It laggy
        // If you feel the camera is lagging try to put this function in LateUpdate or Update (Recommanded Late Update)

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            clicked = true;
    }

    private void LateUpdate()
    {
        SetCameraPosition();
    }


    void FollowPlayer()
    {
        SetCameraMovementType();
        MoveCamera();
    }
    void SetCameraPosition()
    {
        verticalPos = new Vector3(transform.position.x, NewCameraPos.y, transform.position.z);
        horizontalPos = new Vector3(NewCameraPos.x, transform.position.y, transform.position.z);
    }
    void MoveCamera()
    {

        if (!lockHorizontalMovement)
            transform.position = Vector3.Lerp(transform.position, horizontalPos, horizontalSpeed * Time.fixedDeltaTime); // Horizontal Movement

        if (!lockVerticalMovement)
            transform.position = Vector3.Lerp(transform.position, verticalPos, verticalSpeed * Time.fixedDeltaTime); // Vertical Movement
    }


    void SetCameraMovementType()
    {
        switch (movementType)
        {
            case MovementType.Default:
                DefaultMovement();
                break;
            case MovementType.TurnWithPlayerDirection:
                LookForwards();
                break;
            case MovementType.TurnToMousePosition:
                TurnToMouse();
                break;
            case MovementType.TurnToMouseClick:
                TurnToMouseClick();
                break;
        }
    }
    void LookForwards()
    {
        if (player.isLookingRight)
        {

            NewCameraPos = new Vector3(target.position.x + TurnDistance, target.position.y + height, transform.position.z);
        }
        else
        {

            NewCameraPos = new Vector3(target.position.x - TurnDistance, target.position.y + height, transform.position.z);
        }

    }
    void TurnToMouse()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        Vector2 targetPos = target.position;

        Vector2 dir = mousePos - targetPos;

        if (dir.x >= -5 && dir.x <= 5)
        {
            DefaultMovement();
        }
        else
        {
            if (dir.x < 0)
            {

                NewCameraPos = new Vector3(target.position.x - TurnDistance, target.position.y + height, transform.position.z);
            }
            else
            {

                NewCameraPos = new Vector3(target.position.x + TurnDistance, target.position.y + height, transform.position.z);
            }
        }
            
    }
    void TurnToMouseClick()
    {
        if (clicked)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            Vector2 targetPos = target.position;

            Vector2 dir = mousePos - targetPos;

            if (dir.x >= -5 && dir.x <= 5)
            {
                right = 0;
            }
            else
            {
                if (dir.x < 0)
                {
                    right = -1;
                }
                else
                {
                    right = 1;
                }
            }

            clicked = false;
        }


        if (right == -1)
        {

            NewCameraPos = new Vector3(target.position.x - TurnDistance, target.position.y + height, transform.position.z);
        }
        else if (right == 1)
        {

            NewCameraPos = new Vector3(target.position.x + TurnDistance, target.position.y + height, transform.position.z);
        }
        else if (right == 0)
        {
            DefaultMovement();
        }

    }
    void DefaultMovement()
    {
        NewCameraPos = new Vector3(target.position.x, target.position.y + height, transform.position.z);
    }
    
}
