using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class MovementScript : MonoBehaviour
{
    public float moveSpeed = 5f;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");

    private Rigidbody2D rb;
    private Animator animator;
    private float inputX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        inputX = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) inputX += 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)  inputX -= 1f;

        }

        // Drive animator from raw input so walk anim plays even when blocked by a wall
        if (animator != null)
            animator.SetFloat(SpeedHash, Mathf.Abs(inputX));

        if (inputX != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = inputX > 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    void FixedUpdate()
    {
        // Set velocity instead of moving transform directly so physics handles wall collision without vibrating
        rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);

    }

    void OnDisable()
    {
        if (animator != null)
            animator.SetFloat(SpeedHash, 0f);
    }


}
