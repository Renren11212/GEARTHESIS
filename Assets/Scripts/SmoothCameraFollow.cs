using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.3f; // 追従にかかる時間
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -5);
    
    private Vector3 _currentVelocity = Vector3.zero;

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref _currentVelocity,
            smoothTime
        );

        transform.LookAt(target);
    }
}
