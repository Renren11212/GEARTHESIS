using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class TestPlayerMovement : MonoBehaviour
{
    private EntityController _playerController;

    [SerializeField]
    private float _moveSpeed = 1f;

    private void Start()
    {
        _playerController = GetComponent<EntityController>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal") * Time.deltaTime *_moveSpeed;
        float vertical = Input.GetAxisRaw("Vertical") * Time.deltaTime * _moveSpeed;

        // 正規化
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        Vector3 movement = _moveSpeed * Time.deltaTime * direction;

        _playerController.Move(movement);
    }
}
