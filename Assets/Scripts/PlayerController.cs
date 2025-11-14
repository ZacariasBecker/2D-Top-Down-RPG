using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;

    private PlayerControls playerControls;
    private Vector2 movementInput;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Camera mainCam;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCam = Camera.main;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        ReadMovementInput();
        UpdateAnimations();
        AdjustFacingDirection();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ReadMovementInput()
    {
        movementInput = playerControls.Movement.Move.ReadValue<Vector2>();

        // Evita andar mais rápido na diagonal
        if (movementInput.sqrMagnitude > 1f)
            movementInput = movementInput.normalized;
    }

    private void UpdateAnimations()
    {
        animator.SetFloat("moveX", movementInput.x);
        animator.SetFloat("moveY", movementInput.y);
        animator.SetFloat("speed", movementInput.sqrMagnitude);
    }

    private void Move()
    {
        Vector2 newPos = rb.position + movementInput * (moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    private void AdjustFacingDirection()
    {
        if (mainCam == null)
            return;

        // Mouse via Input System
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 playerScreenPos = mainCam.WorldToScreenPoint(transform.position);

        bool faceLeft = mousePos.x < playerScreenPos.x;

        spriteRenderer.flipX = faceLeft;
    }
}
