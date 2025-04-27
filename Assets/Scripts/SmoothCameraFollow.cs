using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothTime = 0.3f;
    [SerializeField] private Vector3 _offset = new Vector3(0, 2, -5);
    
    private Vector3 _currentVelocity = Vector3.zero;

    private void Start()
    {
        if (_target == null)
        {
            Debug.LogError("Target not set for SmoothCameraFollow.");

            // Disable the camera if no target is set
            enabled = false;
        }
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = _target.position + _offset;

        // using SmoothDamp for a smoother transition
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref _currentVelocity,
            _smoothTime
        );

        // Look at the target
        transform.LookAt(_target);
    }
}
