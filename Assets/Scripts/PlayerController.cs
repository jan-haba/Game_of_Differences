using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float WalkSpeed = 5.5f;
    [SerializeField] private float RunningSpeed = 9f;
    [SerializeField] private float JumpForce = 8f;

    [SerializeField] private float Gravity = 9.81f;
    [SerializeField] private float LookSensivity = 0.2f;
    [SerializeField] private float LookAngleLimit = 90f;

    private Camera mainCamera;
    private CharacterController characterController;

    private InputAction moveInput;
    private InputAction runInput;
    private InputAction jumpInput;
    private bool jumped = false;

    private float currentMoveSpeed = 0.0f;
    private Vector3 moveDirection = Vector3.zero;
    private float lookAngle = 0.0f;

    private void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();

        moveInput = InputSystem.actions.FindAction("Move");
        runInput = InputSystem.actions.FindAction("Sprint");
        jumpInput = InputSystem.actions.FindAction("Jump");
        jumpInput.started += Jumped;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentMoveSpeed = WalkSpeed;       
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        Vector2 moveVector = moveInput.ReadValue<Vector2>();
        Vector2 mouseDelta = new Vector2(Mouse.current.delta.x.ReadValue(), Mouse.current.delta.y.ReadValue());

        if (!characterController.isGrounded) 
        {
            jumped = false;
        }
        currentMoveSpeed = runInput.IsPressed() ? RunningSpeed : WalkSpeed;

        HandleMovement(moveVector);
        HandleLooking(mouseDelta);
    }

    private void HandleMovement(Vector2 moveVector2) 
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float oldY = moveDirection.y;

        Vector2 newSpeed = new Vector2(moveVector2.y * currentMoveSpeed, moveVector2.x * currentMoveSpeed);

        moveDirection = (forward * newSpeed.x) + (right * newSpeed.y);
        if (jumped && characterController.isGrounded)
            moveDirection.y = JumpForce;
        else
            moveDirection.y = oldY;

        if (!characterController.isGrounded)
            moveDirection.y -= Gravity * Time.deltaTime; 

        characterController.Move(moveDirection * Time.deltaTime);

    }

    private void Jumped(InputAction.CallbackContext ctx) 
    {
        jumped = true;
    }

    private void HandleLooking(Vector2 mouseDelta) 
    {
        lookAngle += -mouseDelta.y * LookSensivity;
        lookAngle = Mathf.Clamp(lookAngle, -LookAngleLimit, LookAngleLimit);

        mainCamera.transform.localRotation = Quaternion.Euler(lookAngle, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseDelta.x * LookSensivity, 0); 
    }

}

