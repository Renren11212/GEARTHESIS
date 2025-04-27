using UnityEngine;

public class TestPlayerMovement : MonoBehaviour
{
    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        playerController.Move(new Vector3(horizontal, 0, vertical));
    }
}
