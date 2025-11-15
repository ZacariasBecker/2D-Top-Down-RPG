using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;

    public bool FacingLeft { get; private set; }
    public PlayerControls Controls => playerControls; // expõe PlayerControls para outros scripts

    private PlayerControls playerControls;
    private Vector2 movementInput;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Camera mainCam;

    private void Awake()
    {
        // Garante instância única do Input System
        if (playerControls == null)
            playerControls = new PlayerControls();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCam = Camera.main;
    }

    private void OnEnable()
    {
        if (playerControls == null)
            playerControls = new PlayerControls();
        playerControls.Enable();
    }

    private void OnDisable()
    {
        if (playerControls != null)
            playerControls.Disable();
    }

    private void Update()
    {
        ReadMovementInput();
        UpdateAnimations();
        AdjustFacingDirection();
    }

    private void FixedUpdate() => Move();

    private void ReadMovementInput()
    {
        movementInput = playerControls.Movement.Move.ReadValue<Vector2>();
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
        rb.MovePosition(rb.position + movementInput * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustFacingDirection()
    {
        if (mainCam == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 playerScreenPos = mainCam.WorldToScreenPoint(transform.position);

        FacingLeft = mousePos.x < playerScreenPos.x;
        spriteRenderer.flipX = FacingLeft;
    }
}
