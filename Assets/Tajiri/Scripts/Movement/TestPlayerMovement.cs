using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class TestPlayerMovement : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _input;

    private EntityController _playerController;

    [SerializeField]
    private float _moveSpeed = 1f;

    private void Start()
    {
        _playerController = GetComponent<EntityController>();

        if (_input == null)
        {
            Debug.LogError("TestPlayerMovement : PlayerInput is not set");
            return;
        }

        _input.RegisterAction(InputActionType.MOVE_FORWARD, MoveForward);
        _input.RegisterAction(InputActionType.MOVE_BACK,    MoveBack);
        _input.RegisterAction(InputActionType.MOVE_RIGHT,   MoveRight);
        _input.RegisterAction(InputActionType.MOVE_LEFT,    MoveLeft);
    }

    /* 正規化してないからゴミだよ！！！ */
    private void MoveForward()
    {
        float deltaMove = Time.deltaTime * _moveSpeed;
        _playerController.Move(new Vector3(0f, 0f, deltaMove));
    }

    private void MoveBack()
    {
        float deltaMove = Time.deltaTime * _moveSpeed;
        _playerController.Move(new Vector3(0f, 0f, -deltaMove));
    }

    private void MoveRight()
    {
        float deltaMove = Time.deltaTime * _moveSpeed;
        _playerController.Move(new Vector3(deltaMove, 0f, 0f));
    }

    private void MoveLeft()
    {
        float deltaMove = Time.deltaTime * _moveSpeed;
        _playerController.Move(new Vector3(-deltaMove, 0f, 0f));
    }
}
