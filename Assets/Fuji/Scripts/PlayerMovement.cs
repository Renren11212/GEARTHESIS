using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class PlayerMovement : MonoBehaviour
{
    private EntityController _playerController;

    [SerializeField]
    private float _moveSpeed = 1f;

    [SerializeField]
    private KeyCode _dashKeyCode = KeyCode.LeftShift;

    private void Start()
    {
        _playerController = GetComponent<EntityController>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if(Input.GetKey(_dashKeyCode))
        {
            
        }
         
        // 正規化
        Vector3 direction = new Vector3(horizontal, 0, vertical);

        Vector3 movement = _moveSpeed * Time.deltaTime * direction;

        _playerController.Move(movement);
    }
}
