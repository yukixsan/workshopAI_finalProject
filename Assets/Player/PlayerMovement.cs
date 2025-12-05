using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    private Vector2 move, mouseLook, joystickLook;
    private Vector3 rotationTarget;
    public Vector3 lookDirection;
    public bool isOnPc;

    [Header("Knockback Settings")]
    public float knockbackForce = 10f; // Force applied during knockback
    public float knockbackDuration = 0.2f; // Duration of knockback

    private Rigidbody rb; // Player's Rigidbody
    private PlayerHealth playerHealth; // Reference to PlayerHealth
    [SerializeField] private bool isKnockedBack = false;

    [Header("ANIMATIONS")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject hitBoxIcon;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerHealth = GetComponent<PlayerHealth>();

    }
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    public void OnFocus(InputAction.CallbackContext context)
    {
        if (context.performed) // When Shift is pressed
        {
            speed -= 7f;
            hitBoxIcon.SetActive(true);
        }
        else if (context.canceled) // When Shift is released
        {
            speed += 7f;
            hitBoxIcon.SetActive(false);
        }
    }
    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }
    public void OnJoystickLook(InputAction.CallbackContext context)
    {
        joystickLook = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if(!isKnockedBack) 
        {
            if (isOnPc)
            {
               HandleMouseAim();
            }
            else
            {
                HandleJoystickAim();
            }

            MovePlayer();
        }


        
            hitBoxIcon.transform.rotation = Quaternion.identity;
        
    }

    private void HandleMouseAim()
    {
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        Ray ray = Camera.main.ScreenPointToRay(mouseLook);
        if (groundPlane.Raycast(ray, out float enter))
        {
            // Calculate the hit point on the plane
            Vector3 hitPoint = ray.GetPoint(enter);

            // Calculate the direction from the player to the mouse position
            lookDirection = hitPoint - transform.position;

            // Ignore height differences
            lookDirection.y = 0;

            // Handle small distances by defaulting to current forward direction
            if (lookDirection.sqrMagnitude < 0.01f)
            {
                lookDirection = transform.forward;
            }

            // Rotate the player to face the direction
            transform.forward = lookDirection;
        }
    }

    

    private void HandleJoystickAim()
    {
        if (joystickLook.sqrMagnitude > 0.1f)
        {
            Vector3 aimDirection = new Vector3(joystickLook.x, 0, joystickLook.y);
            Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void MovePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);
        if (movement.sqrMagnitude > 0.1f)
        {
            //transform.Translate(movement * speed * Time.deltaTime, Space.World);
            
            rb.linearVelocity = movement * speed ;

            // Only rotate with movement when not aiming
            if (!isOnPc || joystickLook.sqrMagnitude < 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        //Handle Animation
        animator.SetBool("isMoving", movement.sqrMagnitude > 0.1f);
        //Handle blend layer
        if (move.x != 0f && move.y == 0)
        {
            animator.SetLayerWeight(1, 1);
        }
        else if (move.x != 0f && move.y != 0)
        {
            float dotProduct = Vector3.Dot(movement.normalized, Vector3.forward);
            float tangentWeight = Mathf.Abs(dotProduct) < 0.7f ? 0.85f : 1f; // Blend weight based on angle
            animator.SetLayerWeight(1, tangentWeight);
        }
        else
        {
            animator.SetLayerWeight(1, 0f);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            print("collide with enemy");

            Vector3 knockbackDirection = (transform.position - collision.transform.position).normalized;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.VelocityChange);

            //transform.DOShakePosition(0.2f, 1f, 7, 20);

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10);
                Debug.Log($"Player took {10} damage!");
            }
            StartCoroutine(HandleKnockback());

        }
    }
    private System.Collections.IEnumerator HandleKnockback()
    {
        isKnockedBack = true; // Disable input
        yield return new WaitForSeconds(knockbackDuration + 0.2f); // Wait for knockback to complete
        rb.linearVelocity = Vector3.zero;
        isKnockedBack = false; // Re-enable input
    }

    
}
