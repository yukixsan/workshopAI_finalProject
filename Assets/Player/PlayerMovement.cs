using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    private Vector2 move, mouseLook, joystickLook;
    private Vector3 rotationTarget;
    public Vector3 lookDirection;
    public bool isOnPc;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }
    public void OnJoystickLook(InputAction.CallbackContext context)
    {
        joystickLook = context.ReadValue<Vector2>();
    }

    void Update()
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

    private void HandleMouseAim()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseLook);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            rotationTarget = hit.point;
            lookDirection = rotationTarget - transform.position;
            lookDirection.y = 0; // Keep rotation on the horizontal plane

            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), rotationSpeed);
            }
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
            transform.Translate(movement * speed * Time.deltaTime, Space.World);

            // Only rotate with movement when not aiming
            if (!isOnPc || joystickLook.sqrMagnitude < 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
