using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("Rayの長さ")]
    private float _raylength = 1.0f;

    [SerializeField, Header("Rayのオフセット")]
    private float _rayOffset = 0.5f;
    private string _tag = "Ground";
    private bool _isGrounded = false;

    /// <summary>
    /// プレイヤーが地面に接触しているかどうかを取得します.
    /// </summary>
    /// <remarks>地面に接触している場合はtrue, それ以外はfalse</remarks>
    public bool IsGrounded {get { return _isGrounded; } }

    private void Update()
    {
        _isGrounded = CheckIsGrounded();
    }

    private bool CheckIsGrounded()
    {
        if (Physics.Raycast(transform.position + Vector3.down * _rayOffset, Vector3.down, out RaycastHit hit, _raylength))
        {
            // Groundレイヤーに当たった場合
            if (hit.collider.CompareTag(_tag)) return true;

            else return false;
        }

        // Rayが当たらなかった場合
        else return false;
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
        if (_isGrounded) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;
        
        Gizmos.DrawRay(transform.position + Vector3.down * _rayOffset, Vector3.down * _raylength);
    }
}
