using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("Rayの長さ")]
    private float _raylength = 1.0f;

    [SerializeField, Header("Rayのオフセット")]
    private float _rayOffset = 0.5f;

    [SerializeField, Header("接地レイヤー")]
    private LayerMask _groundLayer;

    public bool IsGrounded()
    {
        return Physics.Raycast(
        transform.position + Vector3.down * _rayOffset,
        Vector3.down,
        _raylength,
        _groundLayer
        );
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    public void Move(Vector3 direction)
    {
        Vector3 newPosition = transform.position + direction;

        transform.position = newPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;

        Gizmos.DrawRay(transform.position + Vector3.down * _rayOffset, Vector3.down * _raylength);
    }
}
