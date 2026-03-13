using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private Animator animator;

    private Rigidbody2D rb;

    private Vector2 moveInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        animator.SetBool("IsWalking", isMoving);

        animator.SetFloat("CurrentInputX", moveInput.x);
        animator.SetFloat("CurrentInputY", moveInput.y);

        if (isMoving)
        {
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
    }
}
