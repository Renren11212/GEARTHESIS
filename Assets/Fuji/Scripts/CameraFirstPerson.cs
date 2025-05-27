using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    private bool controlEnabled = false;
    private float rotationX = 0f;
    private float rotationY = 0f;

    void Update()
    {
        // Cキーでトグル
        if (Input.GetKeyDown(KeyCode.C))
        {
            controlEnabled = !controlEnabled;
            Cursor.lockState = controlEnabled? CursorLockMode.Locked: CursorLockMode.None;
            Cursor.visible = !controlEnabled;
        }

        if (!controlEnabled) return;

        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationY += mouseX;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // 上下回転制限

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }

    void HandleMovement()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) move += transform.forward;
        if (Input.GetKey(KeyCode.S)) move -= transform.forward;
        if (Input.GetKey(KeyCode.A)) move -= transform.right;
        if (Input.GetKey(KeyCode.D)) move += transform.right;
        if (Input.GetKey(KeyCode.Space)) move += transform.up;     
        if (Input.GetKey(KeyCode.LeftShift)) move -= transform.up; 

        transform.position += move.normalized * moveSpeed * Time.deltaTime;
    }
}