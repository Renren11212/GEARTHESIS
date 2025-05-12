using UnityEngine;
using Cysharp.Threading.Tasks;

public class DirectionalAttackBase : MonoBehaviour, IGameAction
{
    [SerializeField]
    private PlayerInput _input;

    // デフォルトの入力タイプ
    public InputPressType DefaultInputType => InputPressType.INSTANT;
    // ランタイムで変更可能
    public InputPressType CurrentInputType { get; set; }
    // デフォルトのキー入力
    public KeyCode DefaultKeyCode => KeyCode.Mouse0;
    // ランタイムで変更可能
    public KeyCode CurrentKeyCode { get; set; }
    public bool CanExecute() => true;

    [Header("攻撃判定ベジェ曲線設定")]
    [SerializeField] private Vector3 _point0; // 始点
    [SerializeField] private Vector3 _point1; // 制御点
    [SerializeField] private Vector3 _point2; // 終点

    private Vector3 GetLocalPoint(Vector3 point) => transform.position + transform.rotation * point;
    private Vector3 _localPoint0 => GetLocalPoint(_point0);
    private Vector3 _localPoint1 => GetLocalPoint(_point1);
    private Vector3 _localPoint2 => GetLocalPoint(_point2);

    [Header("攻撃設定")]
    [SerializeField] private float _attackDuration = 0.5f;
    [SerializeField] private float _raycastInterval = 0.05f;
    [SerializeField] private float _rayLength = 1.0f;
    [SerializeField] private LayerMask _layerMask;

    private void Awake()
    {
        CurrentInputType = DefaultInputType;
        CurrentKeyCode = DefaultKeyCode;

        if (_input == null)
        {
            Debug.LogError("PlayerInput is not assigned.");
            return;
        }

        _input.RegisterAction(this)
;    }

    public void Execute()
    {
        Attack().Forget();
    }

    private async UniTaskVoid Attack()
    {
        float elapsed = 0f;

        while (elapsed < _attackDuration)
        {
            float t = elapsed / _attackDuration;

            Vector3 point = GetQuadraticBezierPoint(_localPoint0, _localPoint1, _localPoint2, t);
            Vector3 next = GetQuadraticBezierPoint(_localPoint0, _localPoint1, _localPoint2, t + 0.01f);

            Vector3 direction = (next - point).normalized;

            if (Physics.Raycast(point, direction, out RaycastHit hit, _rayLength, _layerMask))
            {
                Debug.Log($"Hit: {hit.collider.name}");
                // ダメージ処理など

                return;
            }

            elapsed += _raycastInterval;
            await UniTask.Delay(System.TimeSpan.FromSeconds(_raycastInterval));
        }
    }

    private Vector3 GetQuadraticBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        return Vector3.Lerp(a, b, t);
    }

    #if UNITY_EDITOR
    // シーンビューで可視化
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 prevPoint = _localPoint0;
        for (int i = 1; i <= 20; i++)
        {
            float t = i / 20f;
            Vector3 point = GetQuadraticBezierPoint(_localPoint0, _localPoint1, _localPoint2, t);
            Gizmos.DrawLine(prevPoint, point);
            prevPoint = point;
        }

        // 制御点に球表示
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_localPoint0, 0.05f);
        Gizmos.DrawSphere(_localPoint2, 0.05f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_localPoint1, 0.05f);
    }
    #endif
}
