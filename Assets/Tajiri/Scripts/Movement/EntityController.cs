using UnityEngine;

public class EntityController : MonoBehaviour
{
    [Header("接地判定設定")]
    [SerializeField]
    private float _raylength = 1.0f;
    [SerializeField]
    private float _rayOffset = 0.5f;
    [SerializeField]
    private LayerMask _groundLayer;

    [Space(10)]

    [Header("衝突判定設定")]
    [SerializeField]
    private float _colliderRadius = 0.5f;
    [SerializeField]
    private float _colliderHeight = 2.0f;
    [SerializeField]
    private float _colliderOffset = 0.0f;
    [SerializeField]
    private LayerMask _collisionLayer;
    
    /// <summary>
    /// 接地判定
    /// </summary>
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

        if (CheckCollision(newPosition)) return;

        transform.position = newPosition;
    }

    /// <summary>
    /// 衝突判定
    /// </summary>
    private bool CheckCollision(Vector3 targetPosition)
    {
        return Physics.CheckCapsule(
            targetPosition + Vector3.up * _colliderOffset,
            targetPosition + Vector3.up * (_colliderHeight + _colliderOffset),
            _colliderRadius
        );
    }

    /// <summary>
    /// Gizmosの描画
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;

        Gizmos.DrawRay(transform.position + Vector3.down * _rayOffset, Vector3.down * _raylength);

        // カプセルコライダーの可視化
        Gizmos.color = CheckCollision(transform.position) ? Color.red : Color.green;

        Vector3 p1 = transform.position + Vector3.up * _colliderRadius + Vector3.up * _colliderOffset;
        Vector3 p2 = transform.position + Vector3.up * (_colliderHeight - _colliderRadius) + Vector3.up * _colliderOffset;

        Gizmos.DrawWireSphere(p1, _colliderRadius);
        Gizmos.DrawWireSphere(p2, _colliderRadius);
        Gizmos.DrawLine(p1 + Vector3.forward * _colliderRadius, p2 + Vector3.forward * _colliderRadius);
        Gizmos.DrawLine(p1 - Vector3.forward * _colliderRadius, p2 - Vector3.forward * _colliderRadius);
        Gizmos.DrawLine(p1 + Vector3.right * _colliderRadius, p2 + Vector3.right * _colliderRadius);
        Gizmos.DrawLine(p1 - Vector3.right * _colliderRadius, p2 - Vector3.right * _colliderRadius);
    }
}
