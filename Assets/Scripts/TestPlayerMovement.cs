using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class TestPlayerMovement : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField]
    private float _moveSpeed = 1f;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime *_moveSpeed;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * _moveSpeed;

        playerController.Move(new Vector3(horizontal, 0, vertical));
    }
}
