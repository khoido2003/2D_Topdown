using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private Animator animator;

    private Rigidbody2D rb;

    private Vector2 moveInput;

    private bool playFootSteps = false;

    [SerializeField]
    private float footstepSpeed = 0.5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (PauseGame.IsGamePaused)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("IsWalking", false);

            StopFootstep();
            return;
        }

        rb.linearVelocity = moveInput * moveSpeed;
        animator.SetBool("IsWalking", rb.linearVelocity.magnitude > 0);

        if (rb.linearVelocity.magnitude > 0 && !playFootSteps)
        {
            StartFootstep();
        }
        else if (rb.linearVelocity.magnitude == 0)
        {
            StopFootstep();
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        animator.SetFloat("CurrentInputX", moveInput.x);
        animator.SetFloat("CurrentInputY", moveInput.y);

        if (isMoving)
        {
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
    }

    private void StopFootstep()
    {
        playFootSteps = false;

        CancelInvoke(nameof(PlayFootstep));
    }

    private void StartFootstep()
    {
        playFootSteps = true;

        InvokeRepeating(nameof(PlayFootstep), 0f, footstepSpeed);
    }

    void PlayFootstep()
    {
        SoundEffectManager.Instance.Play("Footstep", true);
    }
}
