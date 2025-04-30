using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class PlayerMovement : MonoBehaviour
{
    private EntityController _playerController;

    private AnimationCurve jumpCurve;

    private bool isJumping = false;

    private float jumpTimer = 0f;

    private float _moveSpeed = 1f;


    [SerializeField]
    private float jumpDuration = 1f;

    [SerializeField]
    private float _baseSpeed = 1f;

    [SerializeField]
    private float _dashSpeed = 3f;

    [SerializeField]
    private float _dashRatio = 3f;

    [SerializeField]
    private KeyCode _forwardKeyCode = KeyCode.W;

    [SerializeField]
    private KeyCode _behindKeyCode = KeyCode.S;

    [SerializeField]
    private KeyCode _rightKeyCode = KeyCode.D;

    [SerializeField]
    private KeyCode _leftKeyCode = KeyCode.A;

    [SerializeField]
    private KeyCode _dashKeyCode = KeyCode.LeftShift;

    [SerializeField]
    private KeyCode _jumpKeyCode = KeyCode.Space;

    private void Start()
    {
        _playerController = GetComponent<EntityController>();
    }

    private void Update()
    {
        int horizontal = Convert.ToInt32(Input.GetKey(_rightKeyCode)) - Convert.ToInt32(Input.GetKey(_leftKeyCode));
        int vertical = Convert.ToInt32(Input.GetKey(_forwardKeyCode)) - Convert.ToInt32(Input.GetKey(_behindKeyCode));
        
        // 正規化
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        _moveSpeed = direction != Vector3.zero && Input.GetKey(_dashKeyCode)
        ? Mathf.Min(_moveSpeed + _dashRatio * Time.deltaTime, _dashSpeed)
        : _baseSpeed;

        if (Input.GetKeyDown(_jumpKeyCode))
        {
            StartJump();
        }

        Vector3 movement = _moveSpeed * direction * Time.deltaTime + JumpUpdate();

        _playerController.Move(movement);
    }

    private void StartJump()
    {
        if (!isJumping) return;
        isJumping = true;
        jumpTimer = 0f;
    }

    private Vector3 JumpUpdate()
    {
        if (isJumping) return Vector3.zero;
        jumpTimer += Time.deltaTime;
        float normalizedTime = jumpTimer / jumpDuration;

        if (normalizedTime >= 1f)
        {
            normalizedTime = 1f;
            isJumping = false;
        }

        return jumpCurve.Evaluate(normalizedTime) * Vector3.up;
    }
}
