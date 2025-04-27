using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class TestPlayerMovement : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField]
    private float _moveSpeed = 1f; // 移動速度

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal") * Time.deltaTime *_moveSpeed;
        float vertical = Input.GetAxisRaw("Vertical") * Time.deltaTime * _moveSpeed;

        // 方向ベクトルを計算
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        Vector3 movement = direction * _moveSpeed * Time.deltaTime;

        playerController.Move(movement);
    }
}
