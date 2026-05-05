using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) input.x += 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)  input.x -= 1f;

            bool jumpPressed = Keyboard.current.wKey.wasPressedThisFrame
                            || Keyboard.current.spaceKey.wasPressedThisFrame;

            if (jumpPressed && IsGrounded())
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        transform.position += (Vector3)input.normalized * (moveSpeed * Time.deltaTime);
    }

    bool IsGrounded()
    {
        if (groundCheck != null)
            return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        
        return Physics2D.Raycast(transform.position, Vector2.down, 0.6f, groundLayer).collider != null;
    }
}
