using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerLocomotion : MonoBehaviour
{
    CharacterController characterController;
    Transform playerContainer, cameraContainer;

    public float speed = 6.0f;
    public float sprintSpeed = 8.0f;
    public float jumpSpeed = 10f;
    public float crouchSpeed = 3.0f;
    public float mouseSensitivity = 0.5f;
    public float gravity = 20.0f;
    public float lookUpClamp = -30f;
    public float lookDownClamp = 60f;

    private Vector3 moveDirection = Vector3.zero;
    float rotateX, rotateY;

    PlayerInput playerInput;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction sprintAction;
    InputAction crouchAction;
    InputAction lookAction;
    InputAction previousAction;
    InputAction nextAction;

    void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();

        var map = playerInput.currentActionMap;

        moveAction = map.FindAction("Move", true);
        jumpAction = map.FindAction("Jump", true);
        sprintAction = map.FindAction("Sprint", true);
        crouchAction = map.FindAction("Crouch", true);
        lookAction = map.FindAction("Look", true);
        previousAction = map.FindAction("Previous", true);
        nextAction = map.FindAction("Next", true);
    }


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        GameManager.ResetGame();

        characterController = GetComponent<CharacterController>();
        SetCurrentCamera();
    }

    void Update()
    {
        if (!MenuControls.IsGamePaused)
        {
            Locomotion();
            RotateAndLook();
            PerspectiveCheck();
            SwapWeapon();
        }
    }

    void SetCurrentCamera()
    {
        SwitchPerspective switchPerspective = GetComponent<SwitchPerspective>();
        if (switchPerspective.GetPerspective() == SwitchPerspective.Perspective.First)
        {
            playerContainer = gameObject.transform.Find("Container1P");
            cameraContainer = playerContainer.transform.Find("Camera1PContainer");
        }
        else
        {
            playerContainer = gameObject.transform.Find("Container3P");
            cameraContainer = playerContainer.transform.Find("Camera3PContainer");
        }

    }

    void Locomotion()
    {
        
        if (characterController.isGrounded) // When grounded, set y-axis to zero (to ignore it)
        {
            Vector2 move = moveAction.ReadValue<Vector2>();
            moveDirection = new Vector3(move.x, 0f, move.y);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (jumpAction.IsPressed())
            {
                
                moveDirection.y = jumpSpeed;
                
            }
            if (sprintAction.IsPressed())
            {
                moveDirection.x *= sprintSpeed;
                moveDirection.z *= sprintSpeed;


            }
            if (crouchAction.IsPressed())
            {
                characterController.height = 0.65f;
                characterController.center = new Vector3(0f, 0.5f, 0f);
                
                moveDirection /= crouchSpeed;
            }
            else //if crouch unpressed
            {
                characterController.height = 2f;
                characterController.center = new Vector3(0f, 1f, 0f);
            }
        }
        else
        {
            Vector2 move = moveAction.ReadValue<Vector2>();
            moveDirection = new Vector3(move.x, moveDirection.y, move.y);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.x *= speed;
            moveDirection.z *= speed;
            if (sprintAction.IsPressed())
            {
                moveDirection.x *= speed;
                moveDirection.z *= speed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);

    }

    void RotateAndLook()
    {
        Vector2 look = lookAction.ReadValue<Vector2>();

        rotateX = look.x * mouseSensitivity;
        rotateY -= look.y * mouseSensitivity;

        rotateY = Mathf.Clamp(rotateY, lookUpClamp, lookDownClamp);

        transform.Rotate(0f, rotateX, 0f);

        cameraContainer.transform.localRotation = Quaternion.Euler(rotateY, 0f, 0f);
    }

    void PerspectiveCheck()
    {
        if (previousAction.WasPressedThisFrame())
        {
            SwitchPerspective switchPerspective = GetComponent<SwitchPerspective>();

            if (switchPerspective != null)
            {
                if (switchPerspective.GetPerspective() == SwitchPerspective.Perspective.First)
                {
                    switchPerspective.SetPerspective(SwitchPerspective.Perspective.Third);
                }
                else
                {
                    switchPerspective.SetPerspective(SwitchPerspective.Perspective.First);
                }

                SetCurrentCamera();
            }
        }
    }

    void SwapWeapon()
    {
        if (nextAction.WasPressedThisFrame())
        { 
            SwitchWeapon switchWeapon = GetComponent<SwitchWeapon>();

            if (switchWeapon != null)
            {
                if (switchWeapon.GetWeapon() == SwitchWeapon.Weapon.Gun)
                {
                    switchWeapon.SetWeapon(SwitchWeapon.Weapon.Bow);
                    gameObject.GetComponent<PlayerGunAttack>().enabled = false;
                }
                else
                {
                    switchWeapon.SetWeapon(SwitchWeapon.Weapon.Gun);
                    gameObject.GetComponent<PlayerBowAttack>().enabled = false;
                }
            }
        }
    }
}